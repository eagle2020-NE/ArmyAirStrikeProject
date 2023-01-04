using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeShowToPlayer : MonoBehaviour
{
    public Text TimeText;
    public Text Coins;
    public static TimeShowToPlayer Instance;


    private float CounterSaveTime;
    // Start is called before the first frame update

    private void Awake()
    {
        //if (null == Instance)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Destroy(this);
        //}
        
    }
    void Start()
    {
        Coins.text = " Coins : " + PlayerPrefs.GetInt("Coins");
        //TimeManager.Instance.FindRequirment();
        TimeManager.Instance.BuildConnection();

        //print("__________________________________");
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan TimeConvert = TimeSpan.FromSeconds((double)(TimeManager.Instance.TimeFramePassed));
        TimeText.text = TimeConvert.Hours + "h:" + TimeConvert.Minutes + "m:" + TimeConvert.Seconds + "s";


        
    }

    public void SetCoinText()
    {
        Coins.text = "Coins: " + PlayerPrefs.GetInt("Coins");
    }
    
}
