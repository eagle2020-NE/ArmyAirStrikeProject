

public class PlaneRudder : AircraftControlSurface
{
    public override ControlSurfaceType type => ControlSurfaceType.Rudder;

    private void Awake()
    {
        _controlSurfaceData = new RudderData(Resolver.Instance.settings.controlSurface, LeftSided);
    }
}