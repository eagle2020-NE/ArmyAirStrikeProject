using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputStructure
{
    private int _axesCount;
    public InputAxis[] Axes { get; private set; }


    public InputStructure()
    {
        if (GameObject.FindObjectOfType<Resolver>())
        {
            _axesCount = Resolver.Instance.settings.inputAxis.Length;
            Axes = new InputAxis[_axesCount];
            for (var i = 0; i < _axesCount; i++)
            {
                Axes[i] = new M_PlayerInputAxis(Resolver.Instance.settings.inputAxis[i]);
            }
        }
        
    }


    public void Dispose()
    {
        for (var i = 0; i < _axesCount; i++)
        {
            Axes[i].Dispose();
        }
        Axes = null;
    }
}
