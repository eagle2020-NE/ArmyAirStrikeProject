using UnityEngine;

class FlapData : BaseControlSurfaceData
{
    public FlapData(Settings.ControlSurface controlSurfaceSettings, bool leftSided) : base(controlSurfaceSettings, leftSided)
    {
    }

    public override float IncidenceAngle => (LeftSided ? -1f : 1f) * ControlSurfaceSettings.flapIncidenceAngle;
    public override float DihedralAngle => (LeftSided ? -1f : 1f) * ControlSurfaceSettings.flapDihedralAngle;
    public override float Area => ControlSurfaceSettings.flapArea;
    public override AnimationCurve LiftCurve => ControlSurfaceSettings.nonRudderLiftCoefficientCurve;
    public override AnimationCurve DragCurve => ControlSurfaceSettings.nonRudderDragCoefficientCurve;
}