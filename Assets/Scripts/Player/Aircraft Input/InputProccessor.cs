using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProccessor
{
    private InputStructure _inputStructure;
    private int _axesCount;

    public InputProccessor(InputStructure inputStructure)
    {
        _inputStructure = inputStructure;

        _axesCount = _inputStructure.Axes.Length;
    }

    public void Tick()
    {
        for (var i = 0; i < _axesCount; i++)
        {
            _inputStructure.Axes[i].Tick();
        }

    }

    public void LateTick()
    {
        for (var i = 0; i < _axesCount; i++)
        {
            _inputStructure.Axes[i].LateTick();
        }
    }

    public void Dispose()
    {
        _inputStructure = null;
    }
}
