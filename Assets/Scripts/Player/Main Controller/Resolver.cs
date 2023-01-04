using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using hudNavigation;
using StrikeKit;
using UnityEngine.UI;

public class Resolver : MonoBehaviour
{
    public static Resolver Instance { get; private set; }

    public Transform playerTrans;
    public WeaponController PlayerWeaponController;

    [SerializeField] private Settings _settings = null;
    public Settings settings => _settings;
    public VirtualJoystick[] Joysticks { get; private set; }

    public CinemachineVirtualCamera VirtualCamera;

    public CinemachineVirtualCamera FlipCamera;

    public RuntimeAnimatorController RightAnim;
    public RuntimeAnimatorController LeftAnim;
    public RuntimeAnimatorController swipeRightAnim;

    public GameObject PlayerPrefab;

    public Camera Camera;

    [SerializeField] private RectTransform _targetLock = null;

    public RectTransform TargetLock => _targetLock;



    public HudNavigationSystem navigationSystem { get; private set; }

    public Animator playerAnim;

    private void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        Resolve();
    }
    private void Start()
    {
        //Resolve();
    }
    private void Update()
    {

        //if (FlipCamera == null)
        //{
        //    FlipCamera = playerTrans.gameObject.GetComponentInChildren<CinemachineVirtualCamera>(true);
        //}
    }
    public void Resolve()
    {
        
        Joysticks = FindObjectsOfType<VirtualJoystick>();
        Camera = FindObjectOfType<Camera>();
        navigationSystem = FindObjectOfType<HudNavigationSystem>();
        //CheckPlaneUpgrade();
        
    }


    //void CheckPlaneUpgrade()
    //{
    //    // for rocket
    //    for (int i = 0; i < PlayerPrefs.GetInt("LauncherCount") + 1; i++)
    //    {
    //        PlayerWeaponController.WeaponLists[i + 3].gameObject.SetActive(true);
    //    }
    //    //  for gun
    //    for (int i = 0; i < PlayerPrefs.GetInt("GunCount") + 1; i++)
    //    {
    //        PlayerWeaponController.WeaponLists[i].gameObject.SetActive(true);
    //    }
    //}
}