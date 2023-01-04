
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class SoundButtonManager : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private JoystickIdentifier _identifier;
    [SerializeField] protected RectTransform _background = null;
    [SerializeField] protected Vector3 initBackgroundPos;
    [SerializeField] private RectTransform _handle = null;
    [SerializeField] private float _deadZone = 0.1f;
    [SerializeField] private float _horizontalSensitivity = 1f;
    [SerializeField] private float _verticalSensitivity = 1f;
    [SerializeField] private float _elasticity = 1.9f;
    [SerializeField] private bool _invertVertical = false;
    [SerializeField] private bool _invertHorizontal = false;
    [SerializeField] private bool _constraintHorizontal = false;
    [SerializeField] private bool _constraintVertical = false;

    [SerializeField] private bool canStatic;

    private float _radius;
    private float _horizontalValue;
    private float _verticalValue;


    public JoystickIdentifier identifier => _identifier;
    public bool BeingDragged { get; private set; }

    public AudioMixer audioMixer;

    private void Start()
    {
        
        
        var sizeDelta = _background.sizeDelta;
        _radius = Mathf.Max(sizeDelta.x, sizeDelta.y) / 2f;
        initBackgroundPos = _background.position;
    }

    public float smothFactorToZero;
    private void Update()
    {
        //DetectTouch();


        if (!BeingDragged && !canStatic)
        {
            _handle.anchoredPosition = Vector2.Lerp(_handle.anchoredPosition, Vector2.zero, smothFactorToZero * Time.deltaTime);
        }

        var handleNormalPos = _handle.anchoredPosition / _radius;

        if (!_constraintHorizontal)
        {
            _horizontalValue = Mathf.Lerp(_horizontalValue, Mathf.Abs(handleNormalPos.x) > _deadZone ? handleNormalPos.x : 0f, _horizontalSensitivity * Time.deltaTime);

        }

        if (!_constraintVertical)
        {
            _verticalValue = Mathf.Lerp(_verticalValue, Mathf.Abs(handleNormalPos.y) > _deadZone ? handleNormalPos.y : 0f, _verticalSensitivity * Time.deltaTime);

            audioMixer.SetFloat("Sound", ValueToVolume(GetValue(JoystickDirection.Vertical), 20, 0));
            print(ValueToVolume(GetValue(JoystickDirection.Vertical), 20, 0));

                
        }
        
    }

    public float GetValue(JoystickDirection direction)
    {
        if (_constraintVertical)
        {
            Debug.Assert(direction == JoystickDirection.Horizontal);
        }

        if (_constraintHorizontal)
        {
            Debug.Assert(direction == JoystickDirection.Vertical);
        }

        if (direction == JoystickDirection.Horizontal)
        {
            return (_invertHorizontal ? 0f : 1f) * _horizontalValue;
        }

        return (_invertVertical ? 0f : 1f) * _verticalValue;
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("onDrag");
        BeingDragged = true;

        var touchPos = eventData.position;

        var relativePos = touchPos - (Vector2)_background.position;
        var targetPos = Mathf.Clamp(relativePos.magnitude, 0, _elasticity * _radius) * relativePos.normalized;

        if (_constraintHorizontal)
        {
            targetPos.x = 0f;
        }

        if (_constraintVertical)
        {
            targetPos.y = 0f;
        }

        _handle.anchoredPosition = Vector2.Lerp(_handle.anchoredPosition, targetPos, 10f * Time.deltaTime);

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        BeingDragged = false;
    }

    float _zeroVolume;
    private float ValueToVolume(float value, float maxVolume , float zeroVolume)
    {
        _zeroVolume = zeroVolume;
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * (maxVolume - _zeroVolume) / 4f + maxVolume;

    }
    void DetectTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && _identifier == JoystickIdentifier.Left)
            {
                if (touch.position.x < Screen.width / 2)
                {

                    _background.position = touch.position;

                }

            }
            else if (touch.phase == TouchPhase.Moved && _identifier == JoystickIdentifier.Left && touch.position.x < Screen.width / 2)
            {
                BeingDragged = true;

                var touchPos = touch.position;

                var relativePos = touchPos - (Vector2)_background.position;
                var targetPos = Mathf.Clamp(relativePos.magnitude, 0, _elasticity * _radius) * relativePos.normalized;

                if (_constraintHorizontal)
                {
                    targetPos.x = 0f;
                }

                if (_constraintVertical)
                {
                    targetPos.y = 0f;
                }

                _handle.anchoredPosition = Vector2.Lerp(_handle.anchoredPosition, targetPos, 10f * Time.deltaTime);
            }
            if (touch.phase == TouchPhase.Ended && _identifier == JoystickIdentifier.Left)
            {


                _background.position = initBackgroundPos;
                BeingDragged = false;
            }
        }
    }
}