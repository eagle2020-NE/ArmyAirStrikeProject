using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Net;
using System.Globalization;


public class TimeManager : MonoBehaviour
{

    
    public float TimeFramePassed = 0;
    public int WeeklyCoinRewardAmount = 5000;

    private Animator connectionText_Anim;
    private float CounterSaveTime = 0;
    private float CounterBuildConnection;
    public static TimeManager Instance;

    public DateTime currentTime;


    public GameObject connectionErrorPnl;

    public GameObject dailyGiftBtn;
    public Text remainTime_To_DailyGift_txt;
    public GameObject DailyLockIcon;

    public GameObject WeeklyGiftBtn;
    public GameObject WeeklyLockIcon;
    public Text remainTime_To_WeeklyGift_txt;

    public GameObject connectionText;



    // singleton
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }

        //FindRequirment();

        //BuildConnection();
        //print("_______________________________");
    }

    public void FindRequirment()
    {
        //// Daily reguired Object
        //connectionErrorPnl = GameObject.Find("Internet_Connection_pnl");
        //connectionText_Anim = GameObject.Find("Errortxt").GetComponent<Animator>();
        //connectionText = GameObject.Find("Errortxt");
        //connectionErrorPnl.SetActive(false);

        //dailyGiftBtn = GameObject.Find("Daily_Prize_Btn");

        //remainTime_To_DailyGift_txt = GameObject.Find("DailyTimeToOpen").GetComponent<Text>();
        //DailyLockIcon = GameObject.Find("LockDailyPrize");

        //// weekly
        //WeeklyGiftBtn = GameObject.Find("Week_Prize_Btn");
        //WeeklyLockIcon = GameObject.Find("LockWeekPrize");
        //remainTime_To_WeeklyGift_txt = GameObject.Find("Weekly_TimeToOpen").GetComponent<Text>();
        // Daily reguired Object
        connectionErrorPnl = PrizingDetails.Instance.connectionErrorPnl;
        
        connectionText = PrizingDetails.Instance.connectionText;
        connectionText_Anim = connectionText.gameObject.GetComponent<Animator>();
        connectionErrorPnl.SetActive(false);

        dailyGiftBtn = PrizingDetails.Instance.dailyGiftBtn;

        remainTime_To_DailyGift_txt = PrizingDetails.Instance.remainTime_To_DailyGift_txt;
        DailyLockIcon = PrizingDetails.Instance.DailyLockIcon;

        // weekly
        WeeklyGiftBtn = PrizingDetails.Instance.WeeklyGiftBtn;
        WeeklyLockIcon = PrizingDetails.Instance.WeeklyLockIcon;
        remainTime_To_WeeklyGift_txt = PrizingDetails.Instance.remainTime_To_WeeklyGift_txt;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;

    }



    // Start is called before the first frame update
    void Start()
    {
        

        DontDestroyOnLoad(gameObject);
        if (PlayerPrefs.HasKey("TimePassedinSecond"))
        {
            TimeFramePassed = PlayerPrefs.GetFloat("TimePassedinSecond");
        }

        BuildConnection();
        
    }

    IEnumerator GetTimeFromGoogle()
    {
        WWW www = new WWW("http://google.com");
        yield return www;
        if (www.error != null)
        {
            connectionErrorPnl.SetActive(true);
            StartCoroutine(DeactiveErrorTextAnim());
            //print("NNNNNNNNNNNNNNNNNNNNNNNNNNNN");
        }
        else
        {
            connectionErrorPnl.SetActive(false);
            // Get Time From Server when we have internet Connection
            print("MMMMMMMMMMMMMMMMMMMMMMMMMMMMM");
            var Response = WebRequest.Create("http://www.google.com").GetResponse();
            yield return Response;

            currentTime = DateTime.ParseExact
                (Response.Headers["date"], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);

            // check can we give gift to player
            if (PlayerPrefs.HasKey("LastTimePlayerGotDailyPrize") == false)
            {
                //GivePrizeToPlayer();
                remainTime_To_DailyGift_txt.text = "Open";
                DailyLockIcon.SetActive(false);
                dailyGiftBtn.GetComponent<Button>().interactable = true;
            }
            else
            {
                // check if 1 day is already passed

                DateTime LastTimePrizeGaveToPlayer = Convert.ToDateTime(PlayerPrefs.GetString("LastTimePlayerGotDailyPrize"));
                print(currentTime.Subtract(LastTimePrizeGaveToPlayer).TotalDays);

                if (currentTime.Subtract(LastTimePrizeGaveToPlayer).TotalDays > 1)
                {
                    //GivePrizeToPlayer();
                    remainTime_To_DailyGift_txt.text = "Open";
                    DailyLockIcon.SetActive(false);
                    dailyGiftBtn.GetComponent<Button>().interactable = true;
                }
                else
                {
                    remainTimeToGift();
                    DailyLockIcon.SetActive(true);
                    dailyGiftBtn.GetComponent<Button>().interactable = false;
                }

            }


            // check 7 day AchiveMent

            if (PlayerPrefs.HasKey("LastTimePlayerGotWeeklyPrize") == false)
            {
                //GivePrizeToPlayer();
                remainTime_To_WeeklyGift_txt.text = "Open";
                WeeklyLockIcon.SetActive(false);
                WeeklyGiftBtn.GetComponent<Button>().interactable = true;
            }
            else
            {
                // check if 7 day is already passed

                DateTime LastTimePrizeGaveToPlayer = Convert.ToDateTime(PlayerPrefs.GetString("LastTimePlayerGotWeeklyPrize"));
                print(currentTime.Subtract(LastTimePrizeGaveToPlayer).TotalDays);

                if (currentTime.Subtract(LastTimePrizeGaveToPlayer).TotalDays > 7)
                {

                    remainTime_To_WeeklyGift_txt.text = "Open";
                    WeeklyLockIcon.SetActive(false);
                    WeeklyGiftBtn.GetComponent<Button>().interactable = true;
                }
                else
                {
                    RemainDayToGift();
                    WeeklyLockIcon.SetActive(true);
                    WeeklyGiftBtn.GetComponent<Button>().interactable = false;
                }

            }


        }
    }

    
    IEnumerator DeactiveErrorTextAnim()
    {
        yield return new WaitForSeconds(2.5f);
        //connectionText_Anim.enabled = false;
        connectionText.transform.rotation = Quaternion.Euler(Vector3.zero);
    }



    public void GiveDailyPrizeToPlayer()
    {


        print("we gave player a prize");

        // save time that we gave prize to player
        PlayerPrefs.SetString("LastTimePlayerGotDailyPrize", currentTime.ToString());
        DailyLockIcon.SetActive(true);
        dailyGiftBtn.GetComponent<Button>().interactable = false;
        remainTimeToGift();
    } 
    public void remainTimeToGift()
    {
        DateTime lastPrizeTime = Convert.ToDateTime(PlayerPrefs.GetString("LastTimePlayerGotDailyPrize"));
        var passedTime = currentTime.Subtract(lastPrizeTime).TotalHours;
        int Hour_remaintime = 23 - (int)passedTime;
        float M_remainTime = (float)(passedTime - (int)passedTime);
        int Minute_remainTime = (int)(59 - M_remainTime * 60);
        remainTime_To_DailyGift_txt.text = Hour_remaintime + "h" + Minute_remainTime + "m";
    }


    public void GiveWeeklyPrizeToPlayer()
    {

        print("we gave player a prize");



        // save time that we gave prize to player
        PlayerPrefs.SetString("LastTimePlayerGotWeeklyPrize", currentTime.ToString());
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + WeeklyCoinRewardAmount);
        FindObjectOfType<TimeShowToPlayer>().SetCoinText();
        WeeklyGiftBtn.gameObject.transform.GetComponent<Button>().interactable = false;
        WeeklyLockIcon.SetActive(true);
        RemainDayToGift();
    }
    public void RemainDayToGift()
    {
        DateTime lastPrizeTime = Convert.ToDateTime(PlayerPrefs.GetString("LastTimePlayerGotWeeklyPrize"));
        var passedTime = currentTime.Subtract(lastPrizeTime).TotalDays;
        int Day_remaintime = 7 - (int)passedTime;
        print(passedTime + "  " + Day_remaintime);
        remainTime_To_WeeklyGift_txt.text = Day_remaintime + "day";
    }



    public void BuildConnection()
    {
        StartCoroutine(GetTimeFromGoogle());
        //connectionErrorPnl.SetActive(false);
    }
    
    public void ExitPrizeScene()
    {
        //SceneManager.LoadScene("Shop");
        //Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        TimeFramePassed += Time.deltaTime;
        CounterSaveTime += Time.deltaTime;
        if (CounterSaveTime > 3)
        {
            PlayerPrefs.SetFloat("TimePassedinSecond", TimeFramePassed);
            CounterSaveTime = 0;
        }
        
        if (Input.GetKey(KeyCode.G))
        {
            SceneManager.LoadScene("Gift");
            //BuildConnection();
        }
        
    }

}
