using UnityEngine;

class ElevatorData : BaseControlSurfaceData
{
    public ElevatorData(Settings.ControlSurface controlSurfaceSettings, bool leftSided) : base(controlSurfaceSettings, leftSided)
    {
    }

    public override float IncidenceAngle => (LeftSided ? -1f : 1f) * ControlSurfaceSettings.elevatorIncidenceAngle;
    public override float DihedralAngle => (LeftSided ? -1f : 1f) * ControlSurfaceSettings.elevatorDihedralAngle;
    public override float Area => ControlSurfaceSettings.elevatorArea;
    public override AnimationCurve LiftCurve => ControlSurfaceSettings.nonRudderLiftCoefficientCurve;
    public override AnimationCurve DragCurve => ControlSurfaceSettings.nonRudderDragCoefficientCurve;
}