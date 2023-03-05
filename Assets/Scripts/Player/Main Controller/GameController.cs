using StrikeKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hudNavigation;
using TMPro;

public class GameController : MonoBehaviour
{
    private  CameraController _cameraController = null;
    public Transform playertrans;

    //public TextMeshProUGUI TestAndroid;

    private HudManager hudManage;
    private GameObject _player;

    private  HudNavigationSystem _navigationSystem = null;

    [System.Obsolete]
    private void Start()
    {
#if UNITY_EDITOR

        Debug.logger.logEnabled = true;
#else
        Debug.logger.logEnabled = false;
#endif
        //print("GameController Start() ");
        //Debug.LogWarning("game controller start");
        
        _player =  Instantiate(Resources.Load("Planes/" + PlayerPrefs.GetInt("curSelectedPlaneNumForGame") + "/Prefab/" + PlayerPrefs.GetInt("curSelectedPlaneNumForGame")) as GameObject);
        _player.transform.position = playertrans.position;

       


        _navigationSystem = Resolver.Instance.navigationSystem;
        _navigationSystem.SetPlayer(_player.transform);

        



        Resolver.Instance.playerTrans = _player.transform;
        Resolver.Instance.PlayerWeaponController = _player.GetComponent<WeaponController>();
        CheckPlaneUpgrade();



        Resolver.Instance.playerAnim = _player.GetComponent<Animator>();


        hudManage = FindObjectOfType<HudManager>();
        hudManage.playerDamageManager = _player.GetComponent<DamageManager>();
        hudManage.SetPlayerHealthBar();

        if (Resolver.Instance.VirtualCamera.gameObject.activeInHierarchy)
        {
            _cameraController = new CameraController(Resolver.Instance.VirtualCamera);
            _cameraController.Initialize();
            //TestAndroid.text = _cameraController._liveCameraTransposer.m_YawDamping.ToString();
            Aircraft cameraTarget = _player.transform.GetComponent<Aircraft>();
            //TestAndroid.text = cameraTarget.rb.mass.ToString();
            _cameraController.SetTarget(cameraTarget);
            //TestAndroid.text = _cameraController._target.gameObject.name;
        }


    }

    private void Update()
    {

        if (Resolver.Instance.VirtualCamera.gameObject.activeInHierarchy)
        {
            _cameraController.Tick();
        }
            
        //if (_cameraController._target == null)
        //{
        //    _cameraController.SetTarget(_player.GetComponent<Aircraft>());
        //    TestAndroid.text = Resolver.Instance.VirtualCamera.Follow.gameObject.name.ToString();
        //}
    }

    void CheckPlaneUpgrade()
    {
        // for rocket
        print(Resolver.Instance._planesData.planesDetails[PlayerPrefs.GetInt("curSelectedPlaneNumForGame")].numberOfRockets);
        for (int i = 0; i < Resolver.Instance._planesData.planesDetails[PlayerPrefs.GetInt("curSelectedPlaneNumForGame")].numberOfRockets; i++)
        {
            Resolver.Instance.PlayerWeaponController.WeaponLists[i + 3].gameObject.SetActive(true);
        }
        //  for gun
        for (int i = 0; i < Resolver.Instance._planesData.planesDetails[PlayerPrefs.GetInt("curSelectedPlaneNumForGame")].numberOfBullets; i++)
        {
            Resolver.Instance.PlayerWeaponController.WeaponLists[i].gameObject.SetActive(true);
        }
        print("DDDDDDDDDDDDDDDDDDDDDDDDDDDD");
    }
}
