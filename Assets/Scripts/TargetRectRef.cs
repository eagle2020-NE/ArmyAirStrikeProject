using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRectRef : MonoBehaviour
{
    public static TargetRectRef instance;

    private void Awake()
    {
        instance = this;
    }


    RectTransform rec;
    public Vector3[] wcor = new Vector3[4];

    void Start()
    {
        rec = GetComponent<RectTransform>();
    }

    void Update()
    {
        rec.GetWorldCorners(wcor);
    }
}
