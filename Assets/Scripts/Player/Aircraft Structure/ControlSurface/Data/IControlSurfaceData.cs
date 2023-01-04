using UnityEngine;

public interface IControlSurfaceData
{
    float IncidenceAngle { get; }
    float DihedralAngle { get; }
    float Area { get; }
    AnimationCurve LiftCurve { get; }
    AnimationCurve DragCurve { get; }
}