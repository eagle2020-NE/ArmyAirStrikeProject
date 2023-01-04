

public class Flap : AircraftControlSurface
{
    public override ControlSurfaceType type => ControlSurfaceType.Flap;

    private void Awake()
    {
        _controlSurfaceData = new FlapData(Resolver.Instance.settings.controlSurface, LeftSided);
    }
}