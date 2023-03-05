using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;
using TMPro;

public class FiringButtonsManager : MonoBehaviour
{

    Aircraft Player;

    bool CanFireBullet;
    bool CantCheck;
    float Bullettimer = 0;
    bool fillBullet;


    //public int firstRocketsCount;
    
    [Header("Bullet UI")]
    public float TimeForFiringBullet;
    public float CoolDownTimeForFiringBullet;
    public Image BulletImage;

    [Header("Rocket UI")]
    public Image[] RocketImages;
    public float CoolDownTimeForFiringRocket;
    public int LauncherCapacity;
    int shootingRocketNum;
    public int rocketNumCanShoot;
    public float RocketFireRate;
    public TextMeshProUGUI rocketCanShoot;
    bool RocketLaunched;
    bool canLaunchRocket;
    float RocketTimer;


    private void Start()
    {
        //if (PlayerPrefs.GetInt("giveFirstRockets") != 1)
        //{
        //    PlayerPrefs.SetInt("NormalRocket", firstRocketsCount);
        //    PlayerPrefs.SetInt("giveFirstRockets", 1);
        //}
        rocketNumCanShoot = Resolver.Instance._planesData.planesDetails[PlayerPrefs.GetInt("curSelectedPlaneNumForGame")].numberOfRockets;
        //rocketNumCanShoot = PlayerPrefs.GetInt("LauncherCount" + PlayerPrefs.GetInt("curSelectedPlaneNumForGame")) + 1;
        rocketCanShoot.text = rocketNumCanShoot.ToString();

        print("rocketNumCanShoot : " + rocketNumCanShoot);
        

        canLaunchRocket = true;
        CantCheck = false;

        GunNumCanShoot = Resolver.Instance._planesData.planesDetails[PlayerPrefs.GetInt("curSelectedPlaneNumForGame")].numberOfBullets;
        //GunNumCanShoot = PlayerPrefs.GetInt("GunCount" + PlayerPrefs.GetInt("curSelectedPlaneNumForGame")) + 1;
        GunCanShootTMpro.text = GunNumCanShoot.ToString();

        print("GunNumCanShoot : " + GunNumCanShoot);
    }
    
    private void Update()
    {
        if (Player == null && !CantCheck)
            Player = Resolver.Instance.playerTrans.GetComponent<Aircraft>();

        // bullet firing and UI functions
        CheckBulletUI();


        // Check Rocket UIs
        CheckRocketUI();

    }





    // check Rocket UI 
    private void CheckRocketUI()
    {
        if (RocketLaunched)
        {
            RocketTimer += Time.deltaTime;
            for (int i = 0; i < rocketNumCanShoot; i++)
            {
                if (RocketImages[i].fillAmount < 1)
                {
                    RocketImages[i].fillAmount += Time.deltaTime / CoolDownTimeForFiringRocket;
                }
                if (RocketImages[i].fillAmount == 1)
                {
                    canLaunchRocket = true;
                }
            }
            


            //if (RocketTimer > CoolDownTimeForFiringRocket)
            //{
            //    RocketTimer = 0;
            //    canLaunchRocket = true;
            //}
        }
    }

    // Rocket  launch
    public void _launchRocket()
    {
        if (shootingRocketNum >= rocketNumCanShoot)
        {
            shootingRocketNum = 0;
        }

        print(shootingRocketNum);
        if (PlayerPrefs.GetInt("NormalRocket") > 0 && RocketImages[shootingRocketNum].fillAmount == 1)// && canLaunchRocket)
        {
            //StartCoroutine(DelaydforLaunch());
            Resolver.Instance.PlayerWeaponController.LaunchWeapon(3 + shootingRocketNum);
            PlayerPrefs.SetInt("NormalRocket", PlayerPrefs.GetInt("NormalRocket") - 1);
            HudManager.Instance.SetRocketText();
            RocketImages[shootingRocketNum].fillAmount = 0;
            shootingRocketNum++;
            RocketLaunched = true;
            canLaunchRocket = false;

        }

        
        

        
    }

    private IEnumerator DelaydforLaunch()
    {
        for (int i = 0; i < rocketNumCanShoot; i++)
        {
            Resolver.Instance.PlayerWeaponController.LaunchWeapon(3 + i);
            PlayerPrefs.SetInt("NormalRocket", PlayerPrefs.GetInt("NormalRocket") - 1);
            HudManager.Instance.SetRocketText();
            RocketImages[shootingRocketNum].fillAmount = 0;
            shootingRocketNum++;
            RocketLaunched = true;
            canLaunchRocket = false;
            // delay

            yield return new WaitForSeconds(RocketFireRate);
        }

        
    }

    public void ChangeRocketNumCanShoot()
    {

        //if (rocketNumCanShoot == 2)
        //{
        //    rocketNumCanShoot = 1;
        //}
        //else
        //{
        //    rocketNumCanShoot = 2;
        //}
        //rocketNumCanShoot = PlayerPrefs.GetInt("LauncherCount");
        rocketCanShoot.text = rocketNumCanShoot.ToString();
    }

    // bullet firing and UI functions
    private void CheckBulletUI()
    {
        if (CanFireBullet && !fillBullet)
        {
            print("fill amount" + BulletImage.fillAmount);

            // firing bullet
            if (GunNumCanShoot == 2)
            {
                Resolver.Instance.PlayerWeaponController.LaunchWeapon(1);
                Resolver.Instance.PlayerWeaponController.LaunchWeapon(2);
            }
            else
            {
                for (int i = 0; i < GunNumCanShoot; i++)
                {
                    Resolver.Instance.PlayerWeaponController.LaunchWeapon(i);
                }
                
            }



            Bullettimer += Time.deltaTime;
            BulletImage.fillAmount -= Time.deltaTime / TimeForFiringBullet;

            if (Bullettimer > TimeForFiringBullet)
            {

                CanFireBullet = false;
                Bullettimer = 0;

            }
        }
        if (!CanFireBullet && BulletImage.fillAmount != 1)
        {
            BulletImage.fillAmount += Time.deltaTime / CoolDownTimeForFiringBullet;
            fillBullet = true;
            //if (BulletImage.fillAmount == 1)
            //{
            //    fillBullet = false;
            //}
        }
        if (CanFireBullet && BulletImage.fillAmount > 0.05f)
        {
            fillBullet = false;
        }
        if (CanFireBullet && BulletImage.fillAmount <= 0.05f)
        {
            BulletImage.fillAmount += Time.deltaTime / CoolDownTimeForFiringBullet;
            fillBullet = true;
        }

    }


    // call by bullet ui button in scene
    public void FiringBullet(bool canShoot)
    {
        CanFireBullet = canShoot;
    }

    int GunNumCanShoot;
    public TextMeshProUGUI GunCanShootTMpro;
    public void ChangeGunNumCanShoot()
    {
        GunNumCanShoot++;

        if (GunNumCanShoot == 4)
        {
            GunNumCanShoot = 1;
        }
        GunCanShootTMpro.text = GunNumCanShoot.ToString();

        
    }
    // check is player destroied or not
    public void CheckPlayerExist()
    {
        CantCheck = true;
    }

}
