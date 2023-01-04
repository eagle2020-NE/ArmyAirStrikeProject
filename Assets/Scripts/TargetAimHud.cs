using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hudNavigation;
using StrikeKit;

public class TargetAimHud : MonoBehaviour
{

    public float AimScales = 1.0f;
    public Transform player;
    public Renderer ren;

    // aims prefabs
    public GameObject Aim;
    public GameObject Aimw;
    public GameObject Lock;

    public float lockTime;
    public float lockDist;

    public Vector2 ponintonscreen;

    public DamageManager damageManager;

    #region private variables

    Camera mainCamera;

    public Transform TGUI;
    public GameObject UIAimw;
    public GameObject UIAim;
    public GameObject UILock;
    public RectTransform CanvasRect;
    float timer = 0.0f;
    float dist;
    bool visible;
    #endregion

    private void Awake()
    {
        visible = false;
        //TGUI = GameObject.Find("navigation Radar canvas").transform;
        TGUI = FindObjectOfType<HudNavigationCanvas>().transform;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        //CanvasRect = GameObject.Find("navigation Radar canvas").GetComponent<RectTransform>();
        CanvasRect = FindObjectOfType<HudNavigationCanvas>().GetComponent<RectTransform>();

        damageManager = transform.GetComponent<DamageManager>();
    }

    private void Start()
    {
        UIAimw = Instantiate(Aimw, TGUI);
        UIAimw.transform.GetComponent<EnemyHealthBarManager>().ownerDamageManager = this.gameObject.transform.GetComponent<DamageManager>();
        UIAim = Instantiate(Aim, TGUI);
        UILock = Instantiate(Lock, TGUI);

        UIAim.transform.localScale = Vector3.one * AimScales;
        UIAimw.transform.localScale = Vector3.one * AimScales;
        UIAimw.transform.localScale = Vector3.one * AimScales;

        UIAim.SetActive(false);
        UIAimw.SetActive(false);
        UIAimw.SetActive(false);
    }

    private void Update()
    {
        //print("ren.isVisible : " + ren.isVisible);

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
        Vector2 NewscreenPoint = WorldObject_ScreenPosition;

        if (player == null)
        {
            player = Resolver.Instance.playerTrans;
        }

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > HudNavigationSystem.Instance.radarRadius)
        {
            damageManager.HP = 0;
            print("__________________dist destroy");
        }

        //print("left bottom : " + TargetRectRef.instance.wcor[0]);
        
        if (ren.isVisible)
        {
            ponintonscreen = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            
            if (ponintonscreen.x > TargetRectRef.instance.wcor[0].x && ponintonscreen.x < TargetRectRef.instance.wcor[2].x  &&
                ponintonscreen.y > TargetRectRef.instance.wcor[0].y && ponintonscreen.y < TargetRectRef.instance.wcor[1].y)
            {
                //print("innnnnnnnn  " + ponintonscreen);
            }
            else
            {
                timer = 0;
                visible = false;
                //print("outttttttttttt" + ponintonscreen);
                UIAim.SetActive(false);
                UILock.SetActive(false);
                UIAimw.SetActive(true);

                UIAimw.transform.localPosition = NewscreenPoint;

                if (player.GetComponent<Aircraft>().Target == transform)
                    player.GetComponent<Aircraft>().Target = null;

                return;
            }
            visible = true;
            UIAim.transform.localPosition = NewscreenPoint;
            UIAimw.transform.localPosition = NewscreenPoint;
            UILock.transform.localPosition = NewscreenPoint;

            timer += Time.deltaTime;
            float seconds = timer % 60;

            

            dist = Vector3.Distance(transform.position, player.position);

            //print("dist : " + dist);

            if (seconds > lockTime && dist < lockDist)
            {
                player.GetComponent<Aircraft>().Target = transform;
                UIAim.SetActive(false);
                UIAimw.SetActive(true);
                UILock.SetActive(true);
            }
            else if (dist < lockDist)
            {
                UIAim.SetActive(true);
                UIAimw.SetActive(true);
                UILock.SetActive(false);
            }
        }
        else
        {
            timer = 0;
            visible = false;
            UIAim.SetActive(false);
            UIAimw.SetActive(false);
            UILock.SetActive(false);
        }


        //print(damageManager.HP);
        if (damageManager.HP <= 0)
        {
            Destroy(UIAim);
            Destroy(UIAimw);
            Destroy(UILock);
            Destroy(this.gameObject);
        }

        //time += Time.deltaTime;
        //if (time > 10)
        //{
        //    Destroy(UIAim);
        //    Destroy(UIAimw);
        //    Destroy(UILock);
        //}
    }
    float time = 0;

}
