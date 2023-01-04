using UnityEngine;

namespace IRIAF.TheLegendOfEagles.Systems.Weapon
{
    public class ScrollingTrail : MonoBehaviour
    {
        public float speed;
        public float trailFactor;
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
            //if (this.gameObject.GetComponentInParent<Rigidbody>().velocity.magnitude < 50 && _trail.time > 0)
            //{
            //    _trail.time -= Time.deltaTime;

            //}
            //else if (this.gameObject.GetComponentInParent<Rigidbody>().velocity.magnitude > 50 && _trail.time < 0.3f)
            //{
            //    _trail.time += Time.deltaTime;
            //}
            _trail.time = 1.5e-3f * this.gameObject.GetComponentInParent<Rigidbody>().velocity.magnitude * trailFactor;
            if(this.gameObject.GetComponentInParent<Rigidbody>().velocity.magnitude <= 10)
            {
                _trail.time = 0;
            }
        }
    }
}