
public class AngledWing : AircraftControlSurface
{
    public override ControlSurfaceType type => ControlSurfaceType.AngledWing;

    private void Awake()
    {
        _controlSurfaceData = new AngledWingData(Resolver.Instance.settings.controlSurface, LeftSided);
    }
}