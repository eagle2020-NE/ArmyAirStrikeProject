using System;
using UnityEngine;

public class AircraftEngine : MonoBehaviour
{
    private Settings.Engine _settings;

    public void Init(Settings.Engine settings)
    {
        _settings = settings;
    }

    public Vector3 CalcThrust(float throttle, Vector3 forward)
    {
        return _settings.maxThrustMagnitude * _settings.engineThrottle.Evaluate(throttle) * forward;


    }
}