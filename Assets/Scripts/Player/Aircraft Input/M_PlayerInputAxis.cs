using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class M_PlayerInputAxis : InputAxis
{
    private VirtualJoystick _joystick = null;
    //private VirtualJoystick _joystick = DependencyResolver.Instance.Joysticks[0];

    public M_PlayerInputAxis(Settings.InputAxis setting) : base(setting)
    {
        _joystick = Resolver.Instance.Joysticks.Single(j => j.identifier == _setting.joystickIdentifier);
    }

    protected override float AxisValue()
    {
        if (_joystick != null && _setting.joystickDirection != JoystickDirection.None)
        {
            return MathHelper.Scale(_joystick.GetValue(_setting.joystickDirection), -1, 1, _setting.min, _setting.max);
        }

        return 0.0f;
    }

    protected override bool IsInputting()
    {
        if (_joystick != null && _setting.joystickDirection != JoystickDirection.None)
        {
            return _joystick.BeingDragged;
        }

        return false;
    }
}
