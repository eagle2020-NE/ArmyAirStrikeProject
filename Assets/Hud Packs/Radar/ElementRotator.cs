using DG.Tweening;
using UnityEngine;

public class ElementRotator : MonoBehaviour
{
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    } 

    void Start()
    {
        _rectTransform.DOLocalRotate(-360f * Vector3.forward, 3f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}
