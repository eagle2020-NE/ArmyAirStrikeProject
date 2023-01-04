using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlightController
{
    private Settings.Flight _flightSettings;
    private Settings.Engine _engineSettings;
    private Rigidbody _rigidBody;
    private Transform _transform;
    private IReadOnlyList<AircraftEngine> _engines;
    private IReadOnlyList<AircraftWheel> _wheels;
    private IReadOnlyList<AircraftControlSurface> _surfaces;
    private AircraftFlightState _flightState;
    private InputAxis _pitchAxis;
    private InputAxis _rollAxis;
    private InputAxis _yawAxis;
    private InputAxis _throttleAxis;
    ParticleSystem.EmissionModule WEmissionLeft;
    ParticleSystem.EmissionModule WEmissionRight;

    public FlightController(Settings.Flight flightSettings, Settings.Engine engineSettings)
    {
        _flightSettings = flightSettings;
        _engineSettings = engineSettings;
    }

    public void Initialize(Transform transform, Rigidbody rigidbody, AircraftStructure structure, InputStructure inputStructure, AircraftFlightState flightState,
        ParticleSystem.EmissionModule windEmissionLeft, ParticleSystem.EmissionModule windEmmisionRight)
    {
        WEmissionLeft = windEmissionLeft;
        WEmissionRight = windEmmisionRight;

        _flightState = flightState;

        _surfaces = structure.Surfaces;
        _engines = structure.Engines;
        _wheels = structure.Wheels;
        _rigidBody = rigidbody;
        _transform = transform;

        _rigidBody.drag = Mathf.Epsilon;
        _rigidBody.centerOfMass = new Vector3(0.0f, 0.0f, 0.0f);

        _pitchAxis = inputStructure.Axes.Single(a => a.Type == InputAxisType.Pitch);
        _rollAxis = inputStructure.Axes.Single(a => a.Type == InputAxisType.Roll);
        _yawAxis = inputStructure.Axes.Single(a => a.Type == InputAxisType.Yaw);
        _throttleAxis = inputStructure.Axes.Single(a => a.Type == InputAxisType.Throttle);

        for (var i = 0; i < _engines.Count; i++)
        {
            _engines[i].Init(_engineSettings);
        }
    }

    public void FixedTick()
    {
        var totalLift = Vector3.zero;
        var totalDrag = Vector3.zero;
        var totalThrust = Vector3.zero;

        for (var i = 0; i < _engines.Count; i++)
        {
            totalThrust += _engines[i].CalcThrust(_throttleAxis.Value, _transform.forward);
        }
        //Debug.Log("totalThrust : " + totalThrust);
        //Debug.Log("_throttleAxis : " + _throttleAxis.Value);
        //Debug.Log("exp curve : " + _engineSettings.engineThrottle.Evaluate(_throttleAxis.Value));

        for (var i = 0; i < _surfaces.Count; i++)
        {
            _surfaces[i].CalcLiftAndDrag(out var lift, out var drag, _rigidBody);

            totalLift += lift;
            totalDrag += drag;
        }

        Debug.DrawLine(_transform.position, _transform.position + (totalThrust + totalLift + totalDrag + _rigidBody.mass * Physics.gravity) / 10000f, color: Color.green, Time.fixedDeltaTime);
        Debug.DrawLine(_transform.position, _transform.position + (totalLift) / 10000f, color: Color.cyan, Time.fixedDeltaTime);
        Debug.DrawLine(_transform.position, _transform.position + _rigidBody.velocity, color: Color.red, Time.fixedDeltaTime);

        _rigidBody.AddForce((totalThrust + totalLift + totalDrag), ForceMode.Force);

        //Debug.Log("speed : " + _rigidBody.velocity.magnitude);
        //Debug.Log("total drag : " + totalDrag);
        //Debug.Log("total Force : " + (totalThrust + totalLift + totalDrag).magnitude);

        if (_rigidBody.velocity.magnitude > 5.0f)
        {
            CalibrateVelocity();
        }
        //CalibrateVelocity();
        //Debug.Log("yaw : " + _yawAxis.Value + "   pitch : " + _pitchAxis.Value + " roll : " + _rollAxis.Value);
        ApplyPlayerInput();
    }

    #region Custom Methods
    private void ApplyPlayerInput()
    {
        var targetRotation = _rigidBody.rotation;

        if (_yawAxis.Active)  // when inputAxes.Type != None , Active is true
        {
            targetRotation = Yaw(targetRotation, _yawAxis);
        }

        if (_pitchAxis.Active && !_flightState.Grounded)
        {
            targetRotation = Pitch(targetRotation, _pitchAxis);
        }

        if (_rollAxis.Active && !_flightState.Grounded)
        {
            targetRotation = Roll(targetRotation, _rollAxis, _pitchAxis);
        }
        
        _rigidBody.MoveRotation(Quaternion.Lerp(_rigidBody.rotation, targetRotation, 0.5f));
    }

    private Quaternion Yaw(Quaternion rotation, InputAxis yawAxis)
    {
        var angles = rotation.eulerAngles;

        if (angles.y > 180)
        {
            angles.y -= 360f;
        }
        else if (angles.y < -180)
        {
            angles.y += 360f;
        }

        angles.y += _flightSettings.yawSpeed * yawAxis.Value * Time.fixedDeltaTime;

        return Quaternion.Euler(angles.x, angles.y, angles.z);
    }

    private Quaternion Pitch(Quaternion rotation, InputAxis pitchAxis)
    {
        var angles = rotation.eulerAngles;
        
        //Debug.Log("___________ angle x" + _transform.localEulerAngles.x);

        if (angles.x > 180)
        {
            angles.x -= 360f;
        }
        else if (angles.x < -180)
        {
            angles.x += 360f;
        }

        angles.x = Mathf.Clamp(angles.x + _flightSettings.pitchSpeed * pitchAxis.Value * Time.fixedDeltaTime, -_flightSettings.maxPitchAngle, _flightSettings.maxPitchAngle);

        return Quaternion.Euler(angles.x, angles.y, angles.z);
    }
    float timer;
    private Quaternion Roll(Quaternion rotation, InputAxis rollAxis, InputAxis pitchAxis)
    {
        var angles = rotation.eulerAngles;

        angles.z = _flightSettings.maxRollAngle * _flightSettings.rollTowardExtreme.Evaluate(rollAxis.Value);

        //Debug.Log("roll input : " + rollAxis.Value);
        if (angles.y > 180)
        {
            angles.y -= 360f;
        }
        else if (angles.y < -180)
        {
            angles.y += 360f;
        }

        //if (rollAxis.Value >= 0.95f)
        //{
        //    timer += Time.deltaTime;
        //    if (timer > 2)
        //    {
        //        Resolver.Instance.playerAnim.enabled = true;
        //        Resolver.Instance.playerAnim.SetBool("animRight", true);
        //    }

        //}
        //else if (rollAxis.Value > 0 && rollAxis.Value < 0.95f)
        //{
        //    timer = 0;
        //    Resolver.Instance.playerAnim.SetBool("animRight", false);
        //    Resolver.Instance.playerAnim.enabled = false;
        //}
        //else if (rollAxis.Value <= -0.95f)
        //{
        //    timer += Time.deltaTime;
        //    if (timer > 2)
        //    {
        //        Resolver.Instance.playerAnim.enabled = true;
        //        Resolver.Instance.playerAnim.SetBool("animLeft", true);
        //    }
        //}
        //else if (rollAxis.Value < 0 && rollAxis.Value > -0.95f)
        //{
        //    timer = 0;
        //    Resolver.Instance.playerAnim.SetBool("animLeft", false);
        //    Resolver.Instance.playerAnim.enabled = false;
        //}
        //WEmissionLeft.rateOverTime = Mathf.Abs(angles.z) * 100;
        //WEmissionRight.rateOverTime = Mathf.Abs(angles.z) * 100;

        angles.y += .2f * _flightSettings.rollSpeed * rollAxis.Value * Time.fixedDeltaTime;

        //if (pitchAxis.State == InputAxis.AxisState.UnderGravityNegative ||
        //    pitchAxis.State == InputAxis.AxisState.UnderGravityPositive ||
        //    pitchAxis.State == InputAxis.AxisState.None)
        //{
        //    angles.x = Mathf.LerpAngle(angles.x, 0.0f, 2f * Time.fixedDeltaTime);
        //}

        return Quaternion.Euler(angles.x, angles.y, angles.z);
    }

    private void CalibrateVelocity()
    {
        var forwardSpeed = Vector3.Dot(_rigidBody.velocity, _transform.forward);
        //Debug.Log("forwardSpeed : " + forwardSpeed + " velocity : " + _rigidBody.velocity.magnitude + " throttle : " + _throttleAxis.Value);

        if (forwardSpeed <= 0f)
        {
            return;
        }

        forwardSpeed = Mathf.Lerp(forwardSpeed, _rigidBody.velocity.magnitude, 20f * Time.fixedDeltaTime);

        forwardSpeed = Mathf.Clamp(forwardSpeed, 0, Resolver.Instance.settings.flight.maxSpeed * _throttleAxis.Value);

        _rigidBody.velocity = Vector3.Lerp(_rigidBody.velocity, forwardSpeed * _transform.forward, 10f * Time.fixedDeltaTime);
        
        //Debug.Log("forewardSpeed : " + forwardSpeed + "____velocity : " + _rigidBody.velocity.magnitude);
    }

    #endregion

    public void Dispose()
    {
        _flightSettings = null;
        _rigidBody = null;
        _transform = null;
        _engines = null;
        _wheels = null;
        _surfaces = null;
        _flightState = null;
        _pitchAxis = null;
        _rollAxis = null;
        _yawAxis = null;
        _throttleAxis = null;
    }
}