using System;
using UnityEngine;

[Serializable]
public partial class Settings 
{

    public InputAxis[] inputAxis;
    public ControlSurface controlSurface;
    public Engine engine;
    public Flight flight;

    [Serializable]
    public class InputAxis
    {
        [Header("General")]
        public InputAxisType type = InputAxisType.None;
        public float min = -1.0f;
        public float max = 1.0f;
        public float threshold = 0.05f;
        public float dead = 0.01f;

        [Header("Mobile")]
        public JoystickIdentifier joystickIdentifier = JoystickIdentifier.None;
        public JoystickDirection joystickDirection = JoystickDirection.None;

    }

    [Serializable]
    public class ControlSurface
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

    [Serializable]
    public class Engine
    {
        [Tooltip("Maximum thrust generated by single engine in Newton(N)")]
        public float maxThrustMagnitude = 120000.0f;
        public AnimationCurve engineThrottle;
    }

    [Serializable]
    public class Flight
    {
        public float maxRollAngle = 75f;
        public float rollSpeed = 180f;
        public AnimationCurve rollTowardExtreme;
        public float maxPitchAngle = 75f;
        public float pitchSpeed = 50f;
        public float yawSpeed = 40f;
        public float maxSpeed = 40f;
    }
}
