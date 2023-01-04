using UnityEngine;
public abstract class AircraftControlSurface : MonoBehaviour
{
    const float DENSITY = 1.225f;
    protected IControlSurfaceData _controlSurfaceData;
    private Rigidbody targetRigidbody;

    public virtual ControlSurfaceType type { get; protected set; }
    protected bool LeftSided => transform.localPosition.x < 0;

    public Vector3 ChordNormalInWorld
    {
        get
        {
            var incidenceAngleRad = _controlSurfaceData.IncidenceAngle * Mathf.Deg2Rad;
            var dihedralAngleRad = _controlSurfaceData.DihedralAngle * Mathf.Deg2Rad;

            var chordNormal = new Vector3(
                x: Mathf.Cos(f: incidenceAngleRad) * Mathf.Sin(f: dihedralAngleRad),
                y: Mathf.Cos(f: incidenceAngleRad) * Mathf.Cos(f: dihedralAngleRad),
                z: -Mathf.Sin(f: incidenceAngleRad));


            return transform.TransformDirection(direction: chordNormal).normalized;
        }
    }
    private Vector3 CGRelativeCoord => transform.position - targetRigidbody.worldCenterOfMass;

    public void CalcLiftAndDrag(out Vector3 lift, out Vector3 drag, Rigidbody rigidbody)
    {
        //todo consider moving this to Construct
        targetRigidbody = rigidbody;

        var velocity = targetRigidbody.velocity + Vector3.Cross(lhs: targetRigidbody.angularVelocity, rhs: CGRelativeCoord);
        var speed = velocity.magnitude;

        var dragDir = -velocity.normalized;
        var normalToPlane = Vector3.Cross(lhs: dragDir, rhs: ChordNormalInWorld).normalized;
        var liftDir = Vector3.Cross(lhs: normalToPlane, rhs: dragDir).normalized;

        var tmp = Vector3.Dot(lhs: dragDir, rhs: ChordNormalInWorld);
        tmp = Mathf.Clamp(value: tmp, min: -1f, max: 1f);
        var aoa = Mathf.Asin(f: tmp) * Mathf.Rad2Deg;

        var dragCoefficient = CalcDragCoefficient(aoa);
        var liftCoefficient = CalcLiftCoefficient(aoa);
        var coefficient = 0.5f * DENSITY * speed * speed * _controlSurfaceData.Area; //unit: N

        lift = coefficient * liftCoefficient * liftDir;
        drag = coefficient * dragCoefficient * dragDir;
    }

    private float CalcLiftCoefficient(float aoa)
    {
        if (type == ControlSurfaceType.Rudder)
        {
            var coefficient = _controlSurfaceData.LiftCurve.Evaluate(Mathf.Abs(aoa));
            return (aoa > 0.0f) ? coefficient : -coefficient;
        }
        else
        {
            return _controlSurfaceData.LiftCurve.Evaluate(aoa);
        }
    }

    private float CalcDragCoefficient(float aoa)
    {
        if (type == ControlSurfaceType.Rudder)
        {
            var coefficient = _controlSurfaceData.DragCurve.Evaluate(Mathf.Abs(aoa));
            return (aoa > 0.0f) ? coefficient : -coefficient;
        }
        else
        {
            return _controlSurfaceData.DragCurve.Evaluate(aoa);
        }
    }
}

