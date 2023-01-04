using Cinemachine;
using System.Linq;
using UnityEngine;

public class CameraController
{
    private CinemachineVirtualCamera _vCamera;
    public CinemachineTransposer _liveCameraTransposer;
    private CinemachineComposer _liveCameraComposer;

    public Aircraft _target;

    private InputAxis _targetPitchAxis;
    private InputAxis _targetRollAxis;
    private InputAxis _targetYawAxis;
    private InputAxis _targetThrottleAxis;

    public CameraController(CinemachineVirtualCamera vCamera)
    {
        _vCamera = vCamera;
    }

    public void Initialize()
    {
        _liveCameraTransposer = _vCamera.GetCinemachineComponent<CinemachineTransposer>();
        _liveCameraComposer = _vCamera.GetCinemachineComponent<CinemachineComposer>();

        _liveCameraTransposer.m_AngularDampingMode = CinemachineTransposer.AngularDampingMode.Euler;
        _liveCameraTransposer.m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetNoRoll;

        _liveCameraComposer.m_HorizontalDamping = 0f;
        _liveCameraComposer.m_VerticalDamping =0.5f;

        _liveCameraTransposer.m_XDamping = 1f;
        _liveCameraTransposer.m_YDamping = 0.5f;

        _liveCameraTransposer.m_PitchDamping = 0.5f;
        _liveCameraTransposer.m_RollDamping = 0.5f;
        _liveCameraTransposer.m_YawDamping = 5f;
    }

    public void SetTarget(Aircraft primaryTarget)
    {
        if (null == primaryTarget)
        {
            Reset();
            return;
        }

        _target = primaryTarget;

        _vCamera.Follow = _target.directorManual.vCamFollowTarget;
        _vCamera.LookAt = _target.directorManual.vCamLookAtTarget;

        var axes = primaryTarget.InputStructure.Axes;
        _targetPitchAxis = axes.Single(a => a.Type == InputAxisType.Pitch);
        _targetRollAxis = axes.Single(a => a.Type == InputAxisType.Roll);
        _targetYawAxis = axes.Single(a => a.Type == InputAxisType.Yaw);
        _targetThrottleAxis = axes.Single(a => a.Type == InputAxisType.Throttle);

        // TODO Consider add a accessor for animator component
        //_SDC.m_AnimatedTarget=_target.Animator
    }

    public void Reset()
    {
        _targetPitchAxis = null;
        _targetRollAxis = null;
        _targetYawAxis = null;
        _targetThrottleAxis = null;
        _target = null;
    }

    public void Tick()
    {
        if (_target == null)
        {
            return;
        }

        //Debug.Log("pitch input : " + _targetPitchAxis.Value + "    roll input  :  " + _targetRollAxis.Value + "   throthle input  :  " + _targetThrottleAxis.Value);

        var roll = _targetRollAxis.Value;
        var pitch = _targetPitchAxis.Value;
        var yaw = _targetYawAxis.Value;
        var throttle = _targetThrottleAxis.Value;

        //Debug.Log("****************" + throttle);

        var rollScaled = MathHelper.Scale(roll, -1f, 1f, 0f, 1f);
        var pitchScaled = MathHelper.Scale(pitch, -1f, 1f, 0f, 1f);


        var rollAbs = Mathf.Abs(roll);
        var pitchAbs = Mathf.Abs(pitch);
        var yawAbs = Mathf.Abs(yaw);


        var position = Vector3.Lerp(_target.directorManual.Right.position, _target.directorManual.Left.position, rollScaled);
        position += _target.transform.up * Mathf.Lerp(_target.directorManual.Up.localPosition.y, _target.directorManual.Down.localPosition.y, pitchScaled);
        _target.directorManual.vCamFollowTarget.position = position;

        _vCamera.m_Lens.Dutch = Mathf.Lerp(-10.0f, 10.0f, rollScaled);


        //
        //_liveCameraTransposer.m_XDamping = Mathf.Lerp(0f, .6f, rollAbs);
        //_liveCameraTransposer.m_YDamping = Mathf.Lerp(0f, 6f, pitchAbs);
        //
        //Debug.Log("throttle : " + throttle);

        _liveCameraTransposer.m_ZDamping = Mathf.Lerp(0f, 0.2f, throttle);
        //_liveCameraTransposer.m_ZDamping = 0;

        //
        //_liveCameraTransposer.m_PitchDamping = Mathf.Lerp(0.1f, 30f, pitchAbs);
        //_liveCameraTransposer.m_RollDamping = Mathf.Lerp(0.1f, 15f, rollAbs);
        //_liveCameraTransposer.m_YawDamping = Mathf.Lerp(0.1f, .2f, yawAbs);
        //


        _liveCameraComposer.m_ScreenX = Mathf.Lerp(0.6f, 0.4f, rollScaled);

        //
        //_liveCameraComposer.m_ScreenY = Mathf.Lerp(0.8f, 0.35f, pitchScaled);
    }

    
}