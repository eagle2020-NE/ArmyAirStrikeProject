using UnityEngine;

abstract class BaseControlSurfaceData : IControlSurfaceData
{
    protected Settings.ControlSurface ControlSurfaceSettings;
    protected bool LeftSided;

    protected BaseControlSurfaceData(Settings.ControlSurface controlSurfaceSettings, bool leftSided)
    {
        ControlSurfaceSettings = controlSurfaceSettings;
        LeftSided = leftSided;
    }

    public abstract float IncidenceAngle { get; }
    public abstract float DihedralAngle { get; }
    public abstract float Area { get; }
    public abstract AnimationCurve LiftCurve { get; }
    public abstract AnimationCurve DragCurve { get; }
}