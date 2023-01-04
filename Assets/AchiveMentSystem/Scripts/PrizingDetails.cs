using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PrizingDetails : MonoBehaviour
{
    public GameObject connectionErrorPnl;

    public GameObject dailyGiftBtn;
    public Text remainTime_To_DailyGift_txt;
    public GameObject DailyLockIcon;

    public GameObject WeeklyGiftBtn;
    public GameObject WeeklyLockIcon;
    public Text remainTime_To_WeeklyGift_txt;

    public GameObject connectionText;

    public static PrizingDetails Instance;
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
        TimeManager.Instance.FindRequirment();
    }
}
