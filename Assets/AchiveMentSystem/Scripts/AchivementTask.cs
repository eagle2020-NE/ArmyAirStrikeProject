using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchivementTask : MonoBehaviour
{
    public TextMeshProUGUI _achivementTxt;
    public TextMeshProUGUI _achivementAmountTxt;

    public TextMeshProUGUI rewardText;

    public Image FillBar;

    [HideInInspector]
    public bool UnLockedAchivement;

    public AchivementType achiveType;
    public int achiveAmount;

    public RewardType rewardType;
    public int rewardAmount;

    public void Init(string name , int amount , AchivementType achiveType , int reward, RewardType rewardType)
    {
        achiveAmount = amount;
        this.achiveType = achiveType;

        this.rewardType = rewardType;
        rewardAmount = reward;
        
        if (this.rewardType == RewardType.Coin)
        {
            rewardText.text = rewardAmount + " Coins";
        }
        else if (this.rewardType == RewardType.Rocket)
        {
            rewardText.text = rewardAmount + " Rockets";
        }

        _achivementTxt.text = name;
        gameObject.name = name;

        if (this.achiveType == AchivementType.killedEnemy && achiveAmount != 0)
        {
            if (PlayerPrefs.GetInt("KilledEnemiesCount") >= achiveAmount )
            {
                _achivementAmountTxt.text = achiveAmount + "/" + achiveAmount;
                FillBar.fillAmount = (float)achiveAmount / (float)achiveAmount;
            }
            else
            {
                _achivementAmountTxt.text = PlayerPrefs.GetInt("KilledEnemiesCount") + "/" + achiveAmount;
                FillBar.fillAmount = (float)PlayerPrefs.GetInt("KilledEnemiesCount") / (float)achiveAmount;
            }
            
        }
        else if (this.achiveType == AchivementType.getCoin && achiveAmount != 0)
        {
            if (PlayerPrefs.GetInt("Coins") >= achiveAmount )
            {
                _achivementAmountTxt.text = achiveAmount + "/" + achiveAmount;
                FillBar.fillAmount = (float)achiveAmount / (float)achiveAmount;
            }
            else
            {
                _achivementAmountTxt.text = PlayerPrefs.GetInt("Coins") + "/" + achiveAmount;
                FillBar.fillAmount = (float)PlayerPrefs.GetInt("Coins") / (float)achiveAmount;
            }
            
        }

        
        if(achiveAmount == 0)
        {
            FillBar.fillAmount = 1f;
            _achivementAmountTxt.text = "";
            //print(achiveAmount + " ______");
        }
        
    }

    public void UnLock()
    {
        UnLockedAchivement = true;
        GetComponent<Image>().color = Color.cyan;
        transform.SetAsFirstSibling();
        PlayerPrefs.SetInt("UnLockedAchivement" + gameObject.name, 1);
    }

    public void TaskDone()
    {
        if (UnLockedAchivement)
        {
            if (rewardType == RewardType.Coin)
            {
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + rewardAmount);
            }
            
            
            //print(gameObject.name + " DONE");
            PlayerPrefs.SetInt("GetAchivement" + gameObject.name, 1);

            if (achiveAmount == 0)
            {
                PlayerPrefs.SetInt(gameObject.name, 1);
            }
            else
            {
                PlayerPrefs.SetInt(gameObject.name, 1);
            }
            if (rewardType == RewardType.Rocket)
            {
                PlayerPrefs.SetInt("NormalRocket", rewardAmount + PlayerPrefs.GetInt("NormalRocket"));
            }


            HudManager.Instance.ScoreAndCoin();
            HudManager.Instance.SetRocketText();

            Destroy(this.gameObject);
            //GetComponent<Button>().interactable = false;
            //GetComponent<Image>().color = Color.yellow;
            //transform.SetAsLastSibling();
            
        }
        
    }

}
