using UnityEngine;
using UnityEngine.EventSystems;

public class LeftJoyStickDradArea : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] protected RectTransform _background = null;
    [SerializeField] protected Vector3 initBackgroundPos;

    public void OnStartDrag(PointerEventData eventData)
    {
        print("start drag");
    }
    public void OnDrag(PointerEventData eventData)
    {
        print(" drag");
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        print("end drag");
    }
}
