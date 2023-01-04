using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StrikeKit;
using TMPro;
using UnityEngine.SceneManagement;

public class GarageManager : MonoBehaviour
{
    
    public RuntimeAnimatorController rotationController;
    public RuntimeAnimatorController blendTreeAnim;
    //public GameObject currentSelectedPlaneForGame;
    //public StandardBannerAd standardBannerAd;
    public PlanesData planesData;
    public GamesData gameData;
    public Transform CreatedPlaneParent;
    public int currentShowingPlaneNum;
    public int curSelectedPlaneNumForGame;
    public GameObject lastCreatedPlane;
    public GameObject leftArrowObject;
    public GameObject rightArrowObject;
    public GameObject canvasPlaneEffect;

    


    public int gold;



    [Header("Buy Button UI")]
    public GameObject buyButton;
    public Text planePriceText;

    [Header("Ad Button UI")]
    public GameObject AdButton;

    [Header("Select Button UI")]
    public GameObject selectButton;

    [Header("Selected  UI")]
    public GameObject isSelectedUI;
    public GameObject CostumizeButton;


    [Header("Player's Golld UI")]
    public Text playerCoinsText;

    [Header("Customization Panel")]
    public GameObject CustomizationOptionsPanel;

    [Header("Upgrade Section Panel")]
    public GameObject upgradeSectionPanel;

    [Header("Rocket Upgrade Section Panel")]
    public GameObject rocketUpgradeSectionPanel;

    [Header("Gun Upgrade Section Panel")]
    public GameObject gunUpgradeSectionPanel;

    [Header("Upgrading Type")]
    public UpgradingType upgradingType;

    [Header("Rocket Upgrade Item dependencies")]
    public GameObject sampleRocketItem_Image;
    public GameObject rocketBuybtn;
    public GameObject rocketAdbtn;

    [Header("Gun Upgrade Item dependencies")]
    public GameObject sampleGunItem_Image;
    public GameObject gunBuybtn;
    public GameObject gunAdbtn;

    public TextMeshProUGUI JetNametxt;
    public TextMeshProUGUI JetCharText;
    public SoundManager[] soundManagers;
    private void Awake()
    {
        soundManagers = FindObjectsOfType<SoundManager>(true);
        foreach(var sm in soundManagers)
        {
            sm.Initialize();
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        //standardBannerAd.RequestAndShow();
        SetPlayerBasicValue();
        LoadPlayerBasicValue();
        currentShowingPlaneNum = PlayerPrefs.GetInt("curSelectedPlaneNumForGame");
        CreatePlane();
        print("curSelectedPlaneNumForGame" + PlayerPrefs.GetInt("curSelectedPlaneNumForGame"));
        JetNametxt.gameObject.SetActive(true);

        //if (currentSelectedPlaneForGame == null)
        //{
        //    curSelectedPlaneNumForGame = PlayerPrefs.GetInt("curSelectedPlaneNumForGame");
        //    currentSelectedPlaneForGame = Resources.Load("Planes/" + curSelectedPlaneNumForGame + "/Prefab/" + curSelectedPlaneNumForGame) as GameObject;
        //    print("___________________");
        //}



    }

    
    /// <summary>
    /// First Time Given Gifts to Player
    /// </summary>
    public void SetPlayerBasicValue()
    {
        if (PlayerPrefs.GetInt("basicValuesGiven") == 0)
        {
            // Give Firt Time Gold
            GoldTransactions(true, gameData.baseGold);
            PlayerPrefs.SetInt("curSelectedPlaneNumForGame", 0);

            PlayerPrefs.SetInt("basicValuesGiven", 1);
            
        }
    }

    /// <summary>
    /// Load Player Basic Value From Memory
    /// </summary>
    public void LoadPlayerBasicValue()
    {
        gold = PlayerPrefs.GetInt("Coins");
        playerCoinsText.text = gold.ToString();
    }

    /// <summary>
    /// Give Gold or Not
    /// </summary>
    public void GoldTransactions(bool give, int amount)
    {
        if (give)
        {
            int currentGold = PlayerPrefs.GetInt("Coins");
            currentGold += amount;

            PlayerPrefs.SetInt("Coins", currentGold);
            gold = currentGold;
        }
        else
        {
            int currentGold = PlayerPrefs.GetInt("Coins");
            currentGold -= amount;

            PlayerPrefs.SetInt("Coins", currentGold);
            gold = currentGold;
        }


        //gold = PlayerPrefs.GetInt("goldsAmount");
        playerCoinsText.text = gold.ToString();
    }



    public void CreateNextPlane()
    {
        currentShowingPlaneNum++;
        print(currentShowingPlaneNum);
        CreatePlane();
    }

    public void CreatePrePlane()
    {
        currentShowingPlaneNum--;
        print(currentShowingPlaneNum);
        CreatePlane();
    }


    GameObject planeToLoad;
    public void CreatePlane()
    {
        if(lastCreatedPlane != null)
        {
            //PrepareAircraftForGame(lastCreatedPlane);
            Destroy(lastCreatedPlane);
        }


        planeToLoad = Resources.Load("Planes/" + currentShowingPlaneNum + "/Prefab/" + currentShowingPlaneNum) as GameObject;
        PrepareAircraftForShop(planeToLoad);
        GameObject thisPlane = Instantiate(planeToLoad, CreatedPlaneParent);
        //PrepareAircraftForShop(thisPlane);



        JetNametxt.text = "<" + planesData.planesDetails[currentShowingPlaneNum].planeName + ">";

        lastCreatedPlane = thisPlane;


        Check_UI_Button();

        CheckPlaneUpgrade();

        Check_UI_Arrows();

    }


    public void PrepareAircraftForShop(GameObject currentPlaneObj)
    {
        currentPlaneObj.GetComponentInChildren<Colliders>(true).gameObject.SetActive(false);
        Rigidbody currentRigidBody = currentPlaneObj.transform.GetComponent<Rigidbody>();
        currentRigidBody.isKinematic = true;

        Animator currentAnimator = currentPlaneObj.transform.GetComponent<Animator>();
        //currentAnimator.enabled = true;
        currentAnimator.runtimeAnimatorController = rotationController;

        Aircraft currentAircraft = currentPlaneObj.transform.GetComponent<Aircraft>();
        currentAircraft.enabled = false;
        currentAircraft.inShop = true;

        AudioSource currentAudioSource = currentPlaneObj.transform.GetComponent<AudioSource>();
        currentAudioSource.enabled = false;

        WeaponController currentWeaponController = currentPlaneObj.transform.GetComponent<WeaponController>();
        currentWeaponController.enabled = false;

        DamageManager currentDamageManager = currentPlaneObj.transform.GetComponent<DamageManager>();
        currentDamageManager.enabled = false;

        FlightOnHit currentFlightOnHit = currentPlaneObj.transform.GetComponent<FlightOnHit>();
        currentFlightOnHit.enabled = false;

        PlaneCode currentPlaneCode = currentPlaneObj.transform.GetComponent<PlaneCode>();
        currentPlaneCode.enabled = true;

    }

    public void PrepareAircraftForGame(GameObject currentPlaneObj)
    {
        currentPlaneObj.transform.GetComponentInChildren<Colliders>(true).gameObject.SetActive(true);

        Rigidbody currentRigidBody = currentPlaneObj.transform.GetComponent<Rigidbody>();
        currentRigidBody.isKinematic = false;

        Animator currentAnimator = currentPlaneObj.transform.GetComponent<Animator>();
        //currentAnimator.enabled = false;
        currentAnimator.runtimeAnimatorController = blendTreeAnim;

        Aircraft currentAircraft = currentPlaneObj.transform.GetComponent<Aircraft>();
        currentAircraft.enabled = true;
        currentAircraft.inShop = false;
        print("XXXXXXXXXXXXXX");

        AudioSource currentAudioSource = currentPlaneObj.transform.GetComponent<AudioSource>();
        currentAudioSource.enabled = true;

        WeaponController currentWeaponController = currentPlaneObj.transform.GetComponent<WeaponController>();
        currentWeaponController.enabled = true;

        DamageManager currentDamageManager = currentPlaneObj.transform.GetComponent<DamageManager>();
        currentDamageManager.enabled = true;

        FlightOnHit currentFlightOnHit = currentPlaneObj.transform.GetComponent<FlightOnHit>();
        currentFlightOnHit.enabled = true;

        PlaneCode currentPlaneCode = currentPlaneObj.transform.GetComponent<PlaneCode>();
        currentPlaneCode.enabled = false;

    }

    /// <summary>
    /// Check First and Last UI Arrow for Active/Deactive
    /// </summary>
    public void Check_UI_Arrows()
    {
        if(currentShowingPlaneNum == 0)
        {
            leftArrowObject.SetActive(false);
        }
        else
        {
            leftArrowObject.SetActive(true);
        }

        if (currentShowingPlaneNum == 1)
        {
            rightArrowObject.SetActive(false);
        }
        else
        {
            rightArrowObject.SetActive(true);
        }

    }

    public void Check_UI_Button()
    {
        bool NeedToBuythisPlane = PlayerPrefs.GetInt("PlaneSaled" + currentShowingPlaneNum) == 0;
        if (NeedToBuythisPlane && !planesData.planesDetails[currentShowingPlaneNum].isFreePlane)    
        {
            selectButton.SetActive(false);
            isSelectedUI.SetActive(false);
            CostumizeButton.SetActive(false);
            buyButton.SetActive(true);
            planePriceText.text = planesData.planesDetails[currentShowingPlaneNum].BuyGold.ToString();
        }
        else if (PlayerPrefs.GetInt("curSelectedPlaneNumForGame") == currentShowingPlaneNum)
        {
            selectButton.SetActive(false);
            buyButton.SetActive(false);
            AdButton.SetActive(false);
            isSelectedUI.SetActive(true);
            CostumizeButton.SetActive(true);
            CheckPlaneUpgrade();
        }
        else
        {
            buyButton.SetActive(false);
            AdButton.SetActive(false);
            isSelectedUI.SetActive(false);
            CostumizeButton.SetActive(false);
            selectButton.SetActive(true);
            CheckPlaneUpgrade();
        }
    }

    public void CheckPlaneUpgrade()
    {

        // For Rocket Upgrade
        int RocketUpgradeNumber = planesData.planesDetails[currentShowingPlaneNum].upgradeDetails[0].goldsNeed.Length;
        for (int i = 0; i < RocketUpgradeNumber; i++)
        {
            if (PlayerPrefs.GetInt("SelectedPlaneRocket" + currentShowingPlaneNum + "RocketBought" + i) == 1 || i ==0)
            {
                lastCreatedPlane.GetComponent<PlaneCode>().Rockets[i].SetActive(true);
            }
            else
            {
                lastCreatedPlane.GetComponent<PlaneCode>().Rockets[i].SetActive(false);
            }
            
        }
        // For Gun Upgrade
        int GunUpgradeNumber = planesData.planesDetails[currentShowingPlaneNum].upgradeDetails[1].goldsNeed.Length;
        for (int i = 0; i < GunUpgradeNumber; i++)
        {
            if (i == 0 || PlayerPrefs.GetInt("SelectedPlaneGun" + currentShowingPlaneNum + "GunBought" + i) == 1)
            {
                lastCreatedPlane.GetComponent<PlaneCode>().BulletLaunchers[i].SetActive(true);
            }
            else
            {
                lastCreatedPlane.GetComponent<PlaneCode>().BulletLaunchers[i].SetActive(false);
            }
        }
    }

    public void BuyPlane()
    {
        int buyGoldNeed = planesData.planesDetails[currentShowingPlaneNum].BuyGold;
        if (gold >= buyGoldNeed)
        {
            GoldTransactions(false, buyGoldNeed);
            PlayerPrefs.SetInt("PlaneSaled" + currentShowingPlaneNum, 1);
            Check_UI_Button();
        }
        else
        {
            AdButton.SetActive(true);
            buyButton.SetActive(false);
        }
    }


    public void SelectCurrentPlaneForGame()
    {
        curSelectedPlaneNumForGame = currentShowingPlaneNum;
        PlayerPrefs.SetInt("curSelectedPlaneNumForGame", currentShowingPlaneNum);
        //currentSelectedPlaneForGame = Resources.Load("Planes/" + curSelectedPlaneNumForGame + "/Prefab/" + curSelectedPlaneNumForGame) as GameObject;
        //currentSelectedPlaneForGame = Instantiate(planeToLoad, CreatedPlaneParent);
        
        Check_UI_Button();
    }



    public void StartRocketCustomization()
    {
        upgradingType = UpgradingType.Rocket;
        StartCustomization();
    }



    public void StartGunCustomization()
    {
        upgradingType = UpgradingType.Gun;
        StartCustomization();
    }

    private GameObject[] RocketItems;
    private GameObject[] GunItems;





    public void StartCustomization()
    {
        CustomizationOptionsPanel.SetActive(false);

        lastCreatedPlane.GetComponent<Animator>().enabled = false;
        canvasPlaneEffect.SetActive(false);

        upgradeSectionPanel.SetActive(true);

        switch (upgradingType)
        {
            case UpgradingType.Rocket:
                lastCreatedPlane.GetComponent<PlaneCode>().RocketCameras[0].SetActive(true);
                rocketUpgradeSectionPanel.SetActive(true);
                int rocketUpNum = planesData.planesDetails[currentShowingPlaneNum].upgradeDetails[0].goldsNeed.Length;
                RocketItems = new GameObject[rocketUpNum];
                for (int i = 0; i < rocketUpNum; i++)
                {
                    GameObject rocketItemObject = Instantiate(sampleRocketItem_Image, sampleRocketItem_Image.transform.parent) as GameObject;
                    rocketItemObject.SetActive(true);

                    rocketItemObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Planes/" + currentShowingPlaneNum + "/Rockets/" + currentShowingPlaneNum);
                    rocketItemObject.GetComponentInChildren<Text>().text = "Rocket " + (i + 1);

                    int rand = i;
                    rocketItemObject.GetComponent<Button>().onClick.AddListener(delegate { ChangeRocketPreview(rand); });
 
                    RocketItems[i] = rocketItemObject as GameObject;
                }
                break;

            case UpgradingType.Gun:
                lastCreatedPlane.GetComponent<PlaneCode>().GunCameras[0].SetActive(true);
                gunUpgradeSectionPanel.SetActive(true);
                int gunUpNum = planesData.planesDetails[currentShowingPlaneNum].upgradeDetails[1].goldsNeed.Length;
                GunItems = new GameObject[gunUpNum];
                for (int i = 0; i < gunUpNum; i++)
                {
                    GameObject gunItemObject = Instantiate(sampleGunItem_Image, sampleGunItem_Image.transform.parent) as GameObject;
                    gunItemObject.SetActive(true);
                    gunItemObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Planes/" + currentShowingPlaneNum + "/Guns/" + currentShowingPlaneNum);
                    int rand = i;
                    gunItemObject.GetComponent<Button>().onClick.AddListener(delegate { ChangeGunPreview(rand); });
                    gunItemObject.GetComponentInChildren<Text>().text = "Gun " + (i + 1);
                    GunItems[i] = gunItemObject as GameObject;
                }
                break;
        }



    }


    public void EndCustomization()
    {
        CustomizationOptionsPanel.SetActive(true);
        lastCreatedPlane.GetComponent<Animator>().enabled = true;
        canvasPlaneEffect.SetActive(true);
        upgradeSectionPanel.SetActive(false);

        switch (upgradingType)
        {
            case UpgradingType.Rocket:
                lastCreatedPlane.GetComponent<PlaneCode>().RocketCameras[selectedRocketToBuyNum].SetActive(false);
                rocketUpgradeSectionPanel.SetActive(false);
                foreach (GameObject rocketItem in RocketItems)
                    Destroy(rocketItem);
                CheckPlaneUpgrade();
                rocketBuybtn.SetActive(false);
                break;

            case UpgradingType.Gun:
                lastCreatedPlane.GetComponent<PlaneCode>().GunCameras[selectedGunToBuyNum].SetActive(false);
                gunUpgradeSectionPanel.SetActive(false);
                foreach (GameObject gunItem in GunItems)
                    Destroy(gunItem);
                CheckPlaneUpgrade();
                gunBuybtn.SetActive(false);
                break;
        }

        upgradingType = UpgradingType.None;

    }



    /// <summary>
    /// Rocket Upgrade
    /// </summary>
    private int selectedRocketToBuyNum;
    public void ChangeRocketPreview(int selectedRocket)
    {
        selectedRocketToBuyNum = selectedRocket;
        bool bought = PlayerPrefs.GetInt("SelectedPlaneRocket" + currentShowingPlaneNum + "RocketBought" + selectedRocketToBuyNum) == 1; // when buy
        rocketAdbtn.SetActive(false);
        rocketBuybtn.gameObject.SetActive(selectedRocket != 0 && !bought);
        
        lastCreatedPlane.GetComponent<PlaneCode>().ActiveRocketOnThisPlane(selectedRocket);
        rocketBuybtn.GetComponentInChildren<Text>().text = 
            planesData.planesDetails[currentShowingPlaneNum].upgradeDetails[0].goldsNeed[selectedRocket].ToString();
    }
    public void BuyThisRocket()
    {
        int goldNeed = planesData.planesDetails[currentShowingPlaneNum].upgradeDetails[0].goldsNeed[selectedRocketToBuyNum];//selectedRocketToBuyNum = 1;
        print(selectedRocketToBuyNum);
        if (gold >= goldNeed)
        {
            PlayerPrefs.SetInt("LauncherCount" + currentShowingPlaneNum, selectedRocketToBuyNum);
            GoldTransactions(false, goldNeed);
            PlayerPrefs.SetInt("SelectedPlaneRocket" + currentShowingPlaneNum, selectedRocketToBuyNum);
            PlayerPrefs.SetInt("SelectedPlaneRocket" + currentShowingPlaneNum + "RocketBought" + selectedRocketToBuyNum , 1);
            //EndCustomization();
            CheckPlaneUpgrade();
            rocketBuybtn.SetActive(false);
        }
        else
        {
            rocketAdbtn.SetActive(true);
            rocketBuybtn.SetActive(false);
        }
    }


    /// <summary>
    /// Gun Upgrade
    /// </summary>
    private int selectedGunToBuyNum;
    public void ChangeGunPreview(int selectedGun)
    {
        selectedGunToBuyNum = selectedGun;
        bool bought = PlayerPrefs.GetInt("SelectedPlaneGun" + currentShowingPlaneNum + "GunBought" + selectedGunToBuyNum) == 1;
        gunAdbtn.SetActive(false);
        gunBuybtn.gameObject.SetActive(selectedGun != 0 && !bought);
        
        lastCreatedPlane.GetComponent<PlaneCode>().ActiveGunOnThisPlane(selectedGun);
        gunBuybtn.GetComponentInChildren<Text>().text = 
            planesData.planesDetails[currentShowingPlaneNum].upgradeDetails[1].goldsNeed[selectedGun].ToString();
    }
    public void BuyThisGun()
    {
        int goldNeed = planesData.planesDetails[currentShowingPlaneNum].upgradeDetails[1].goldsNeed[selectedGunToBuyNum]; // 0 1 2
        if (gold >= goldNeed)
        {
            PlayerPrefs.SetInt("GunCount" + currentShowingPlaneNum, selectedGunToBuyNum);
            GoldTransactions(false, goldNeed);
            PlayerPrefs.SetInt("SelectedPlaneGun" + currentShowingPlaneNum, selectedGunToBuyNum);
            PlayerPrefs.SetInt("SelectedPlaneGun" + currentShowingPlaneNum + "GunBought" + selectedGunToBuyNum, 1);
            //EndCustomization();
            CheckPlaneUpgrade();
            gunBuybtn.SetActive(false);
        }
        else
        {
            gunAdbtn.SetActive(true);
            gunBuybtn.SetActive(false);
        }
    }

    public void ChangeScene(string name)
    {
        GameObject thisplane = Resources.Load("Planes/" + PlayerPrefs.GetInt("curSelectedPlaneNumForGame")
            + "/Prefab/" + PlayerPrefs.GetInt("curSelectedPlaneNumForGame")) as GameObject;
        print("changeScene 0  " + PlayerPrefs.GetInt("curSelectedPlaneNumForGame"));
        PrepareAircraftForGame(thisplane);
        print("changeScene 1  " + thisplane.transform.GetComponentInChildren<Colliders>().gameObject.activeInHierarchy);
        //StartCoroutine(Delay(name));
        SceneManager.LoadScene(name);
    }
    IEnumerator Delay(string name)
    {
        yield return new WaitForSeconds(0);
        SceneManager.LoadScene(name);
    }


    public void SetJetCharacter()
    {
        JetCharText.text = planesData.planesDetails[currentShowingPlaneNum].planeCharacteristic;
    }
}


public enum UpgradingType
{
    Rocket,
    Gun,
    None
}
