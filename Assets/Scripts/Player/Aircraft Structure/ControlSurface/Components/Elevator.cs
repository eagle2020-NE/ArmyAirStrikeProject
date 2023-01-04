public class Elevator : AircraftControlSurface
{
    public override ControlSurfaceType type => ControlSurfaceType.Elevator;

    private void Awake()
    {
        _controlSurfaceData = new ElevatorData(Resolver.Instance.settings.controlSurface, LeftSided);
    }
}