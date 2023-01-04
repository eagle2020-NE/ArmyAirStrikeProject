using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputAxis
{
    public enum AxisState
    {
        None,
        TowardPositive,
        TowardNegative,
        TowardPositiveFromNone,
        TowardNegativeFromNone,
        UnderGravityPositive,
        UnderGravityNegative,
    }

    protected Settings.InputAxis _setting;
    private AxisState _previousState;
    private AxisState _lastFrameState;
    private float _lastFrameValue;

    public InputAxisType Type { get; private set; }
    public bool Active { get; private set; }
    public AxisState State { get; private set; }
    public float Value { get; private set; }

    public InputAxis(Settings.InputAxis setting)
    {
        _setting = setting;
        Type = _setting.type;
        Active = false;
        State = AxisState.None;
        _previousState = AxisState.None;
        _lastFrameState = AxisState.None;
        Value = 0.0f;
        _lastFrameValue = 0.0f;
    }

    public void Tick()
    {
        Value = AxisValue();

        if (!IsInputting())
        {
            if (!Biased(_setting.dead))
            {
                State = AxisState.None;
            }
            else if (Value > _lastFrameValue)
            {
                if (_lastFrameState != AxisState.None) State = AxisState.UnderGravityNegative;
            }
            else if (Value < _lastFrameValue)
            {
                if (_lastFrameState != AxisState.None) State = AxisState.UnderGravityPositive;
            }
        }
        else
        {
            if (!Biased(_setting.threshold))
            {
                //if (PreviousState == AxisState.None || State == AxisState.None) State = AxisState.None;
            }
            else if (Value > _lastFrameValue)
            {
                if (_previousState == AxisState.None || _lastFrameState == AxisState.None) State = AxisState.TowardPositiveFromNone;
                else State = AxisState.TowardPositive;
            }
            else if (Value < _lastFrameValue)
            {
                if (_previousState == AxisState.None || _lastFrameState == AxisState.None) State = AxisState.TowardNegativeFromNone;
                else State = AxisState.TowardNegative;
            }
        }

        Active = State != AxisState.None;
    }

    public void LateTick()
    {
        if (State != _lastFrameState)
        {
            _previousState = _lastFrameState;
        }

        _lastFrameValue = Value;
        _lastFrameState = State;
    }

    public virtual void Dispose()
    {
        _setting = null;
    }

    public bool Biased(float threshold)
    {
        var mid = (_setting.min + _setting.max) / 2.0f;
        return Mathf.Abs(Value - mid) > threshold;
    }

    protected abstract float AxisValue();
    protected abstract bool IsInputting();
}
