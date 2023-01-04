using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCode : MonoBehaviour
{
    public GameObject MainCameraObject;
    public GameObject[] BulletLaunchers;
    public GameObject[] Rockets;
    public GameObject[] Bullets;

    [Header("Cameras")]
    public GameObject[] RocketCameras;
    public GameObject[] GunCameras;

    // Start is called before the first frame update
    void Start()
    {
        MainCameraObject = GameObject.Find("MainPlaneCamera");
    }

    
    public void ActiveRocketOnThisPlane(int RocketNum)
    {
        for (int i = 0; i < Rockets.Length; i++)
        {
            Rockets[i].SetActive(i == RocketNum);
            RocketCameras[i].SetActive(i == RocketNum);
        }
    }


    public void ActiveGunOnThisPlane(int GunNum)
    {
        for (int i = 0; i < BulletLaunchers.Length; i++)
        {
            BulletLaunchers[i].SetActive(i == GunNum);
            GunCameras[i].SetActive(i == GunNum);
        }
    }
}
