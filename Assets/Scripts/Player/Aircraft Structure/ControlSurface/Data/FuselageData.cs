using UnityEngine;

class FuselageData : BaseControlSurfaceData
{
    public FuselageData(Settings.ControlSurface controlSurfaceSettings, bool leftSided) : base(controlSurfaceSettings, leftSided)
    {
    }

    public override float IncidenceAngle => (LeftSided ? -1f : 1f) * ControlSurfaceSettings.fuselageIncidenceAngle;
    public override float DihedralAngle => (LeftSided ? -1f : 1f) * ControlSurfaceSettings.fuselageDihedralAngle;
    public override float Area => ControlSurfaceSettings.fuselageArea;
    public override AnimationCurve LiftCurve => ControlSurfaceSettings.nonRudderLiftCoefficientCurve;
    public override AnimationCurve DragCurve => ControlSurfaceSettings.nonRudderDragCoefficientCurve;
}