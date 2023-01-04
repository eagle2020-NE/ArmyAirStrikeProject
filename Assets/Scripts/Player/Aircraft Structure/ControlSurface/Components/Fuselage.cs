

public class Fuselage : AircraftControlSurface
{
    public override ControlSurfaceType type => ControlSurfaceType.Fuselage;

    private void Awake()
    {
        _controlSurfaceData = new FuselageData(Resolver.Instance.settings.controlSurface, LeftSided);
    }
}