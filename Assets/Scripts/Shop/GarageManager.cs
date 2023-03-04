using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StrikeKit;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Globalization;
using System.Net;

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

    [Header("Fighters Level Upgrade Item dependencies")]
    [SerializeField] private Text currentLevelText;
    [SerializeField] private Text addedArmorText;
    [SerializeField] private Text addedSpeedText;
    [SerializeField] private Text currentArmorText;
    [SerializeField] private Text currentSpeedText;
    [SerializeField] private Text goldNeedText;
    [SerializeField] private Text timeToUP;
    [SerializeField] private Text UPTimeText;
    [SerializeField] private GameObject  levelupTimingPanel;
    [SerializeField] private GameObject  internetErrorPanel;
    [SerializeField] private GameObject  planeLevelUpPanel;
    [SerializeField] private GameObject  waitingPanel;


    [SerializeField] private GameObject  GetLevelUpPanel;
    [SerializeField] private Text nextLevelText;


    public void _resetAllPanel()
    {
        waitingPanel.SetActive(false);
        planeLevelUpPanel.SetActive(false);
        internetErrorPanel.SetActive(false);
        levelupTimingPanel.SetActive(false);
        GetLevelUpPanel.SetActive(false);
    }

    public DateTime currentTime;


    private int currentLevel;
    private int upTime;
    public void UpgradePlaneLevel()
    {
        PlayerPrefs.SetInt("upgrageLevel" + currentShowingPlaneNum, 1);
        PlayerPrefs.SetInt("lastLevelUpgrading", currentLevel);
        PlayerPrefs.SetString("LastTimePlane" + currentShowingPlaneNum + "update", currentTime.ToString());
        //print("current time " + currentTime.ToString());
        // upgrade health , maxSpeed 
        int upgradeGoldNeed = planesData.planesDetails[currentShowingPlaneNum].levelUpgrade[currentLevel].goldsNeed;
        upTime = planesData.planesDetails[currentShowingPlaneNum].levelUpgrade[currentLevel].timeToUpgrade * 60;
        if (gold >= upgradeGoldNeed)
        {
            GoldTransactions(false, upgradeGoldNeed);
            levelupTimingPanel.SetActive(true);
            planeLevelUpPanel.SetActive(false);
            internetErrorPanel.SetActive(false);
            //SetCurrentLevelUpTime();
            StartCoroutine(SetCurrentLevelUpTime());
            //UpgradeOperation();
        }
        else
        {
            AdButton.SetActive(true);
            buyButton.SetActive(false);
        }

    }

    private void Update()
    {
        

    }

    // each 1 s  callback
    IEnumerator SetCurrentLevelUpTime()
    {
        print("current plane name : " + planeToLoad.name);

        if (upTime <= 0)
        {
            //UpgradeOperation();

            //levelupTimingPanel.SetActive(false);
            //planeLevelUpPanel.SetActive(false);
            //GetLevelUpPanel.SetActive(true);
            //nextLevelText.text = (currentLevel + 2).ToString();
            if (PlayerPrefs.GetInt(planeToLoad.name + "done") == 1)
            {
                levelupTimingPanel.SetActive(false);
                planeLevelUpPanel.SetActive(false);
                GetLevelUpPanel.SetActive(true);
                nextLevelText.text = (currentLevel + 2).ToString();
            }


        }
        else
        {
            int hours = upTime / 3600;
            int minutes = (upTime - (hours * 3600)) / 60;
            int seconds = upTime - (minutes * 60) - (hours * 3600);
            //print("UpTime : " + upTime);

            timeToUP.text = hours + " H : " + minutes + " M : " + seconds + " S";
        }

        yield return new WaitForSeconds(1);
        upTime--;
        if(upTime >= 0)
        {
            StartCoroutine(SetCurrentLevelUpTime());
        }
        


    }

    

    public void CheckCurrentPlaneLevelUpgrade()
    {
        upTime = planesData.planesDetails[currentShowingPlaneNum].levelUpgrade[currentLevel].timeToUpgrade * 60;
        var Response = WebRequest.Create("http://www.google.com").GetResponse();

        currentTime = DateTime.ParseExact
                (Response.Headers["date"], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);

        DateTime lastPrizeTime = Convert.ToDateTime(PlayerPrefs.GetString("LastTimePlane" + currentShowingPlaneNum + "update"));
        //print("last upgrade time : " + lastPrizeTime);
        var passedTime = currentTime.Subtract(lastPrizeTime).TotalSeconds;
        print("passed time : " + passedTime + " lastprizetime : " + lastPrizeTime);
        upTime -= (int)passedTime;


        //print("upgrageLevel : " + PlayerPrefs.GetInt("upgrageLevel" + currentShowingPlaneNum));
        if (isOnline && !levelupTimingPanel.activeInHierarchy &&
            PlayerPrefs.GetInt("upgrageLevel" + currentShowingPlaneNum) == 1 && PlayerPrefs.GetInt("lastLevelUpgrading") == currentLevel)
        {
            
            levelupTimingPanel.SetActive(true);
            planeLevelUpPanel.SetActive(false);
            internetErrorPanel.SetActive(false);
            waitingPanel.SetActive(false);
            PlaneParrent(true);
            StartCoroutine(SetCurrentLevelUpTime());
            print("XXXXXXXXXXXXXXX");
            
        }
        if(isOnline && PlayerPrefs.GetInt("upgrageLevel" + currentShowingPlaneNum) == 0)
        {
            UpgradePlaneLevelUI();
            print("____________");

        }
        if(isOnline && upTime < 0)
        {
            PlayerPrefs.SetInt(planeToLoad.name + "done", 1);
            StartCoroutine(SetCurrentLevelUpTime());
            //levelupTimingPanel.SetActive(false);
            //planeLevelUpPanel.SetActive(false);
            //GetLevelUpPanel.SetActive(true);
            //nextLevelText.text = (currentLevel + 2).ToString();
            print("KKKKKKKKKKKK");
        }
        if (!isOnline)
        {
            print("OOOOOOOOOOOOO");
            waitingPanel.SetActive(false);
            //PlaneParrent(true);
            internetErrorPanel.SetActive(true);
            levelupTimingPanel.SetActive(false);
            planeLevelUpPanel.SetActive(false);
        }

    }

    void PlaneParrent(bool isactive)
    {
        CreatedPlaneParent.gameObject.SetActive(isactive);
        JetCharText.gameObject.SetActive(isactive);
        JetNametxt.gameObject.SetActive(isactive);
    }
    public void UpgradeOperation()
    {
        levelupTimingPanel.SetActive(false);
        planeLevelUpPanel.SetActive(true);
        GetLevelUpPanel.SetActive(false);
        PlayerPrefs.SetInt("upgrageLevel" + currentShowingPlaneNum, 0);
        PlayerPrefs.SetInt(planeToLoad.name, 0);
        planesData.planesDetails[currentShowingPlaneNum].planeCurrentHealth += planesData.planesDetails[currentShowingPlaneNum].levelUpgrade[currentLevel].healthIncrease;
        planesData.planesDetails[currentShowingPlaneNum].planeCurrentSpeed += planesData.planesDetails[currentShowingPlaneNum].levelUpgrade[currentLevel].SpeedIncrease;
        planesData.planesDetails[currentShowingPlaneNum].levelUpgrade[currentLevel].isUpgrade = true;
        UpgradePlaneLevelUI();
    }
    public void UpgradePlaneLevelUI()
    {
        internetErrorPanel.SetActive(false);
        levelupTimingPanel.SetActive(false);
        planeLevelUpPanel.SetActive(true);
        waitingPanel.SetActive(false);
        PlaneParrent(true);

        // upgrade ui
        for (int i = 0; i < planesData.planesDetails[currentShowingPlaneNum].levelUpgrade.Length; i++)
        {
            if (!planesData.planesDetails[currentShowingPlaneNum].levelUpgrade[i].isUpgrade)
            {
                goldNeedText.text = planesData.planesDetails[currentShowingPlaneNum].levelUpgrade[i].goldsNeed.ToString();
                currentLevel = i;
                currentLevelText.text = (i + 1).ToString();
                addedArmorText.text = planesData.planesDetails[currentShowingPlaneNum].levelUpgrade[i].healthIncrease.ToString();
                addedSpeedText.text = planesData.planesDetails[currentShowingPlaneNum].levelUpgrade[i].SpeedIncrease.ToString();
                currentArmorText.text = planesData.planesDetails[currentShowingPlaneNum].planeCurrentHealth.ToString();
                currentSpeedText.text = (planesData.planesDetails[currentShowingPlaneNum].planeCurrentSpeed + 1000).ToString();
                UPTimeText.text = planesData.planesDetails[currentShowingPlaneNum].levelUpgrade[i].timeToUpgrade + " min";
                return;
            }
        }
        
        
    }

    private bool isOnline;

    public void Refreshing()
    {
        waitingPanel.SetActive(true);
        PlaneParrent(false);
        StartCoroutine(BuildConnection());
        //UpgradePlaneLevelUI();
        //CheckCurrentPlaneLevelUpgrade();
    }
    IEnumerator BuildConnection()
    {
        WWW www = new WWW("http://google.com");
        yield return www;
        if(www.error == null)
        {
            isOnline = true;
            var Response = WebRequest.Create("http://www.google.com").GetResponse();
            yield return Response;
            currentTime = DateTime.ParseExact
                    (Response.Headers["date"], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
            
            UpgradePlaneLevelUI();
            CheckCurrentPlaneLevelUpgrade();
        }
        else
        {
            //StartCoroutine(BuildConnection());
            isOnline = false;
            CheckCurrentPlaneLevelUpgrade();
            //StartCoroutine(BuildConnection());
        }

        print("isOnline : " + isOnline);
    }

    private void Awake()
    {
        //StartCoroutine(BuildConnection());
        soundManagers = FindObjectsOfType<SoundManager>(true);
        foreach(var sm in soundManagers)
        {
            sm.Initialize();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //UpgradePlaneLevelUI();
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
        _resetAllPanel();
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
        Refreshing();
        JetCharText.text = planesData.planesDetails[currentShowingPlaneNum].planeCharacteristic;
        
        //UpgradePlaneLevelUI();
        //CheckCurrentPlaneLevelUpgrade();
    }
}


public enum UpgradingType
{
    Rocket,
    Gun,
    None
}
