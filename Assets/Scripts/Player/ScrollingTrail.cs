using UnityEngine;

public class ScrollingTrail : MonoBehaviour
{
    public float speed;
    private TrailRenderer _trail;
    private Material _material;

    private void Awake()
    {
        _trail = GetComponent<TrailRenderer>();
        _material = _trail.material;
    }

    private void Update()
    {
        var offset = _material.mainTextureOffset;

        offset.x -= Time.deltaTime * speed / 60.0f;

        if (offset.x < -1f)
        {
            offset.x = 1f;
        }

        _material.mainTextureOffset = offset;
    }
}