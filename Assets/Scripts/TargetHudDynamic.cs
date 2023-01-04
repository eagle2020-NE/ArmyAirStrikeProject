using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetHudDynamic : MonoBehaviour
{
    public float horizontalFactor = 1000;
    public float verticalFactor = 1000;
    public float _smoothFactor = 2;

    public Vector2 xConstrain;
    public Vector2 yConstrain;

    private InputAxis _targetPitchAxis;
    private InputAxis _targetRollAxis;

    private Aircraft _target;
    public RectTransform rect;
    Vector2 centerPos;
    Vector3 psitionCenter;
    private void Start()
    {
        _target = FindObjectOfType<Aircraft>();
        SetTarget(_target);
    }

    private void Update()
    {
        if (_target == null)
        {
            _target = FindObjectOfType<Aircraft>();
            SetTarget(_target);
        }

        Tick();


    }
    public void SetTarget(Aircraft primaryTarget)
    {
        if (null == primaryTarget)
        {
            Reset();
            return;
        }
        rect = GetComponent<RectTransform>();
        centerPos = rect.anchoredPosition;
        psitionCenter = rect.localPosition;
        //_target = primaryTarget;


        var axes = primaryTarget.InputStructure.Axes;
        _targetPitchAxis = axes.Single(a => a.Type == InputAxisType.Pitch);
        _targetRollAxis = axes.Single(a => a.Type == InputAxisType.Roll);


        // TODO Consider add a accessor for animator component
        //_SDC.m_AnimatedTarget=_target.Animator
    }

    public void Reset()
    {
        _targetPitchAxis = null;
        _targetRollAxis = null;

    }

    public void Tick()
    {
        if (null == _target)
        {
            return;
        }


        var rollScaled = _targetRollAxis.Value;
        var pitchScaled = _targetPitchAxis.Value;
        //print("hor : " + rollScaled + "  ver : " + pitchScaled);
        print("______ : " + rect.anchoredPosition);
        //var rollScaled = MathHelper.Scale(roll, -1f, 1f, 0f, 1f);
        //var pitchScaled = MathHelper.Scale(pitch, -1f, 1f, 0f, 1f);
        
        if (Mathf.Abs(rollScaled) != 0 || Mathf.Abs(pitchScaled) != 1)
        {
            //if (rollScaled > 0) rollScaled = 1;
            //else rollScaled = -1;
            pitchScaled *= -1;

            rect.anchoredPosition = Vector2.Lerp(centerPos,
                new Vector2(horizontalFactor * rollScaled, verticalFactor * pitchScaled),
                _smoothFactor * Time.deltaTime);

            //    rect.localPosition = Vector2.Lerp(psitionCenter, rect.localPosition + new Vector3(horizontalFactor * rollScaled, verticalFactor * pitchScaled), _smoothFactor * Time.deltaTime);

            //    if(rect.localPosition.x < xConstrain.x)
            //    {
            //        rect.localPosition = new Vector3(
            //            xConstrain.x,
            //            rect.localPosition.y,
            //            rect.localPosition.z
            //            );

            //    }
            //    else if (rect.localPosition.x > xConstrain.y)
            //    {
            //        rect.localPosition = new Vector3(
            //            xConstrain.y,
            //            rect.localPosition.y,
            //            rect.localPosition.z
            //            );
            //    }
            //    if (rect.localPosition.y < yConstrain.x)
            //    {
            //        rect.localPosition = new Vector3(
            //            rect.localPosition.x,
            //            yConstrain.x,
            //            rect.localPosition.z
            //            );
            //    }
            //    else if (rect.localPosition.y > yConstrain.y)
            //    {
            //        rect.localPosition = new Vector3(
            //            rect.localPosition.x,
            //            yConstrain.y,
            //            rect.localPosition.z
            //            );
            //    }

            //    rect.localPosition = new Vector3(
            //        Mathf.Clamp(rect.localPosition.x, xConstrain.x, xConstrain.y),
            //        Mathf.Clamp(rect.localPosition.y, yConstrain.x, yConstrain.y),
            //        rect.localPosition.z
            //        );
        }
        else
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, centerPos, 1.5f * Time.deltaTime);
            //rect.localPosition = Vector3.Lerp(rect.localPosition, psitionCenter, _smoothFactor * Time.deltaTime);
        }
        
    }
}
