using UnityEngine;

public class AircraftWheel : MonoBehaviour
{
    private WheelCollider _col;

    private void Awake()
    {
        _col = GetComponentInChildren<WheelCollider>(true);
    }

    private void Start()
    {
        _col.motorTorque = Mathf.Epsilon;
        //_col.forceAppPointDistance = 1.5f;
        //_col.suspensionDistance = .1f;
    }
}