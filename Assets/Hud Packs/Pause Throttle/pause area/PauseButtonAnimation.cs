
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonAnimation : MonoBehaviour
{
    [SerializeField] Sprite _offGraphic = null;
    [SerializeField] Sprite _onGraphic = null;
    [SerializeField] Image image = null;
    [SerializeField] GameObject pausePnl = null;



    private void Start()
    {
        image.sprite = _offGraphic;
        image.SetNativeSize();
        //pausePnl = GameObject.FindObjectOfType<PausePanelVisual>(true).gameObject;
    }

    public void Animate()
    {
        var onState = image.sprite == _onGraphic ? true : false;

        image.sprite = onState ? _offGraphic : _onGraphic;
        if (onState)
        {
            Time.timeScale = 1;
            pausePnl.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            pausePnl.SetActive(true);
        }
        image.SetNativeSize();
    }
}