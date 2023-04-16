using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StrikeKit;
using UnityEngine.SceneManagement;

public class HudManager : MonoBehaviour
{
    public static HudManager Instance { get; private set; }
    public Image PlayerHealthBar;
    public TextMeshProUGUI PlayerHealthText;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI HighScore;
    public TextMeshProUGUI Coin;
    public TextMeshProUGUI RocketNum;
    public Text PlayerSpeedtxt;
    public Text PlayerHeigthtxt;

    public DamageManager playerDamageManager;

    public GameObject LosePanel;
    public GameObject AchivementPanel;

    public int KilledEnemiesCount;
    public int _coin;

    public Image RocketLocked;

    private PauseButtonAnimation pauseButton;
    private void Awake()
    {
        pauseButton = FindObjectOfType<PauseButtonAnimation>();

        KilledEnemiesCount = 0;
        Coin.text = " Coins : " + PlayerPrefs.GetInt("Coins");
        
        Score.text = "Killed : " + KilledEnemiesCount;
        HighScore.text = "TOP : " + PlayerPrefs.GetInt("Record");

        if (null == Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        RocketNum.text = " Rockets : " + PlayerPrefs.GetInt("NormalRocket");
    }


    private void Update()
    {
        if (playerDamageManager != null)
        {
            PlayerSpeedtxt.text = (int)(playerDamageManager.gameObject.GetComponent<Rigidbody>().velocity.magnitude +
                Resolver.Instance._planesData.planesDetails[PlayerPrefs.GetInt("curSelectedPlaneNumForGame")].planeCurrentSpeed + 1000)
                + " m/s";
            PlayerHeigthtxt.text = (int)(playerDamageManager.gameObject.transform.position.y) + " m";
        }
        
    }
    public void ScoreAndCoin()
    {
        KilledEnemiesCount++;
        Score.text = "Killed : " + KilledEnemiesCount;
        //_coin = PlayerPrefs.GetInt("Coins") + 50;
        //PlayerPrefs.SetInt("Coins", _coin);
        Coin.text = " Coins : " + PlayerPrefs.GetInt("Coins");
        
    }

    public void SetRocketText()
    {
        RocketNum.text = " Rockets : " + PlayerPrefs.GetInt("NormalRocket");
    }

    public void SetPlayerHealthBar()
    {
        if (playerDamageManager.HP < 0)
            playerDamageManager.HP = 0;

        PlayerHealthBar.fillAmount = (float)playerDamageManager.HP / (float)playerDamageManager.HPmax;
        PlayerHealthText.text = ((int)(PlayerHealthBar.fillAmount * 100)).ToString();
        if (playerDamageManager.HP < playerDamageManager.HPmax / 2)
        {
            PlayerHealthBar.color = Color.red;
        }
        if (playerDamageManager.HP <= 0)
        {
            LosePanel.SetActive(true);
        }

    }
    public void AchivementCanvas()
    {
        AchivementPanel.SetActive(true);
    }
    public void ResumeGame()
    {
        SceneManager.LoadScene("EndLessGamePlay");
        Time.timeScale = 1;
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LockedEnemyRocketAlarm()
    {
        if(!RocketLocked.gameObject.activeInHierarchy)
        {
            RocketLocked.gameObject.SetActive(true);
            Invoke("DeactiveAlarm", 2);
        }
        

    }
    void DeactiveAlarm()
    {
        RocketLocked.gameObject.SetActive(false);
    }

    public void PausePanelExit()
    {
        pauseButton.Animate();
    }

    public void Shop()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Shop");
    }
}
