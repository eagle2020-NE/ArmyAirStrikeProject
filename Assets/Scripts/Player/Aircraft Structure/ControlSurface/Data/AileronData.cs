using UnityEngine;

class AileronData : BaseControlSurfaceData
{
    public AileronData(Settings.ControlSurface controlSurfaceSettings, bool leftSided) : base(controlSurfaceSettings, leftSided)
    {
    }

    public override float IncidenceAngle => (LeftSided ? -1f : 1f) * ControlSurfaceSettings.aileronIncidenceAngle;
    public override float DihedralAngle => (LeftSided ? -1f : 1f) * ControlSurfaceSettings.aileronDihedralAngle;
    public override float Area => ControlSurfaceSettings.aileronArea;
    public override AnimationCurve LiftCurve => ControlSurfaceSettings.nonRudderLiftCoefficientCurve;
    public override AnimationCurve DragCurve => ControlSurfaceSettings.nonRudderDragCoefficientCurve;
}