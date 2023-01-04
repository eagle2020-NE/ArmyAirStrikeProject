using UnityEngine;

class AngledWingData : BaseControlSurfaceData
{
    public AngledWingData(Settings.ControlSurface controlSurfaceSettings, bool leftSided) : base(controlSurfaceSettings, leftSided)
    {
    }

    public override float IncidenceAngle => (LeftSided ? -1f : 1f) * ControlSurfaceSettings.angledTrailIncidenceAngle;
    public override float DihedralAngle => (LeftSided ? -1f : 1f) * ControlSurfaceSettings.angledTrailDihedralAngle;
    public override float Area => ControlSurfaceSettings.angledTrailArea;
    public override AnimationCurve LiftCurve => ControlSurfaceSettings.nonRudderLiftCoefficientCurve;
    public override AnimationCurve DragCurve => ControlSurfaceSettings.nonRudderDragCoefficientCurve;
}