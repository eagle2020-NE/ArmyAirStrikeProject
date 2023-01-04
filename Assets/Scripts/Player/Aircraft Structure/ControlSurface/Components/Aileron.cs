
public class Aileron : AircraftControlSurface
{
    public override ControlSurfaceType type => ControlSurfaceType.Aileron;

    private void Awake()
    {
        _controlSurfaceData = new AileronData(Resolver.Instance.settings.controlSurface, LeftSided);
    }
}
