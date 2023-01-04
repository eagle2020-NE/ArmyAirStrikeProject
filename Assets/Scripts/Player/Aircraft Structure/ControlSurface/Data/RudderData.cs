using UnityEngine;
class RudderData : BaseControlSurfaceData
{
    public RudderData(Settings.ControlSurface controlSurfaceSettings, bool leftSided) : base(controlSurfaceSettings, leftSided)
    {
    }

    public override float IncidenceAngle => (LeftSided ? -1f : 1f) * ControlSurfaceSettings.rudderIncidenceAngle;
    public override float DihedralAngle => (LeftSided ? -1f : 1f) * ControlSurfaceSettings.rudderDihedralAngle;
    public override float Area => ControlSurfaceSettings.rudderArea;
    public override AnimationCurve LiftCurve => ControlSurfaceSettings.rudderLiftCoefficientCurve;
    public override AnimationCurve DragCurve => ControlSurfaceSettings.rudderDragCoefficientCurve;
}
