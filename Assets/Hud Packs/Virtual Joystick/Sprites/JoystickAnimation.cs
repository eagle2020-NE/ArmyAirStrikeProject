
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VirtualJoystick))]
public class JoystickAnimation : MonoBehaviour
{
    //[SerializeField] private Image _placeHolderImage = null;
    [SerializeField] private Image _staticActivationImage = null;
    [SerializeField] private Image _dynamicBladeImage = null;
    [SerializeField] [Range(0f, 1f)] private float fadeTime = 0.03f;
    [SerializeField] private float _dynamicBladMaxRotationSpeed = 100f;

    private VirtualJoystick _joystick = null;
    //private IEntity _player = null;

    private void Awake()
    {
        _joystick = GetComponent<VirtualJoystick>();
    }

    private void Start()
    {
        _staticActivationImage.color = SetAlpha(_staticActivationImage.color, 0f, false, fadeTime);
        _dynamicBladeImage.color = SetAlpha(_dynamicBladeImage.color, 0f, false, fadeTime);
    }

    private void Update()
    {
        if (_joystick.BeingDragged)
        {
            _staticActivationImage.color = SetAlpha(_staticActivationImage.color, 1f, true, fadeTime);
            _dynamicBladeImage.color = SetAlpha(_dynamicBladeImage.color, 1f, true, fadeTime);
        }
        else
        {
            _staticActivationImage.color = SetAlpha(_staticActivationImage.color, 0f, true, fadeTime);
            _dynamicBladeImage.color = SetAlpha(_dynamicBladeImage.color, 0f, true, fadeTime);
        }

        _dynamicBladeImage.rectTransform.Rotate(Vector3.forward, -_dynamicBladMaxRotationSpeed * _joystick.GetValue(JoystickDirection.Horizontal) * Time.deltaTime);

        //if (null == _player)
        //{
        //    if (DependencyResolver.Instance.Aircrafts.Values.Any(a => a.Type == EntityType.Player))
        //    {
        //        _player = DependencyResolver.Instance.Aircrafts.Values.Single(a => a.Type == EntityType.Player) ?? null;
        //    }

        //    return;
        //}

        //todo Add player related stuff
    }

    private Color SetAlpha(Color color, float targetAlpha, bool lerp, float time)
    {
        if (color.a == targetAlpha)
        {
            return color;
        }

        color.a = lerp ? Mathf.Lerp(color.a, targetAlpha, time) : targetAlpha;
        return color;
    }
}