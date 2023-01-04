using System;
using UnityEngine;

[Serializable]
public class ControlSurfaceSettings
{
    [Space]
    [Header("[Non Rudder Flight Characteristic]")]
    [Space]
    public AnimationCurve nonRudderLiftCoefficientCurve;
    public AnimationCurve nonRudderDragCoefficientCurve;
    [Space]
    [Header("[Rudder Flight Characteristic]")]
    [Space]
    public AnimationCurve rudderLiftCoefficientCurve;
    public AnimationCurve rudderDragCoefficientCurve;
    [Space]
    [Header("[Control Surface Specs]")]
    [Space]
    public float flapIncidenceAngle = 3.0f;
    public float flapDihedralAngle = 0.0f;
    public float flapArea = 10.0f;
    [Space]
    public float aileronIncidenceAngle = 3.0f;
    public float aileronDihedralAngle = 0.0f;
    public float aileronArea = 6.4f;
    [Space]
    public float angledTrailIncidenceAngle = 3.0f;
    public float angledTrailDihedralAngle = -24.0f;
    public float angledTrailArea = 6.0f;
    [Space]
    public float elevatorIncidenceAngle = 0.0f;
    public float elevatorDihedralAngle = 12.9f;
    public float elevatorArea = 5.0f;
    [Space]
    public float rudderIncidenceAngle = 0.0f;
    public float rudderDihedralAngle = 90.0f;
    public float rudderArea = 7.0f;
    [Space]
    public float fuselageIncidenceAngle = 0.0f;
    public float fuselageDihedralAngle = 0.0f;
    public float fuselageArea = 13.5f;
}
