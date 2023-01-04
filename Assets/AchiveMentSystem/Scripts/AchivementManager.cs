using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;


public enum AchivementType
{
    killedEnemy,
    getCoin,
}
public class AchivementManager : MonoBehaviour
{

    public int firstRocketsCount;

    public AchivementTask _achivementPrefab;
    public Transform _achivementParent;
    List<AchivementTask> _achivementTasks = new List<AchivementTask>();

    public int RocketReward;
    public int KilledEnemyRewardFactor;
    public int FreeRocket;
    public int FreeCoin;
    public int CoinFactor;


    public List<DefineAchivement> achivementsList;
    [SerializeField]private List<DefineAchivement> _createdAchivementsList;

    void Awake()
    {
        if (PlayerPrefs.GetInt("giveFirstRockets") != 1)
        {
            PlayerPrefs.SetInt("NormalRocket", firstRocketsCount);
            PlayerPrefs.SetInt("giveFirstRockets", 1);
        }
        //_createdAchivementsList = new List<DefineAchivement>(20);

        //print("___________________________________");

        //for (int i= PlayerPrefs.GetInt("lastCoinFactor"); i > 0 ; i--)
        //{
        //    AchivementTask _achivement = Instantiate(_achivementPrefab, _achivementParent);

        //    // add to list
        //    _achivementTasks.Add(_achivement);

        //    _achivement.Init("Obtain " + (i) * 100 + " Cion", (i) * 100 / 2, AchivementType.getCoin, (i) * 100 / 2, RewardType.Coin);
        //    CheckUnlockAndDoneAchivement();
        //    CheckEnemiesKilled();
        //    CheckCoinsAdd();
        //    if (PlayerPrefs.GetInt("GetAchivement" + _achivement.name) == 1)
        //    {
        //        Destroy(_achivement.gameObject);
        //    }

        //}

        //for (int i = 0; i < achivementsList.Count; i++)
        //{
        //    AchivementTask _achivement = Instantiate(_achivementPrefab, _achivementParent);
        //    _achivementTasks.Add(_achivement);
        //    _achivement.Init(achivementsList[i].achivementText, achivementsList[i].achivementAmount, achivementsList[i].achiveType, achivementsList[i].RewardCount, achivementsList[i].rewardType);

        //}

        checkCreatedAchivement();
        FreeAchivementGenerator();
        foreach (var achivement in _achivementTasks)
        {
            if (achivement.achiveType == AchivementType.killedEnemy && PlayerPrefs.GetInt("KilledEnemiesCount") <= achivement.achiveAmount && PlayerPrefs.GetInt("KilledEnemiesCount") != 0)
            {
                achivement._achivementAmountTxt.text = PlayerPrefs.GetInt("KilledEnemiesCount") + "/" + achivement.achiveAmount;
                achivement.FillBar.fillAmount = (float)PlayerPrefs.GetInt("KilledEnemiesCount") / (float)achivement.achiveAmount;
            }
            if (achivement.FillBar.fillAmount == 1 && !achivement.UnLockedAchivement)
            {
                achivement.UnLock();
            }
        }
        foreach (var achivement in _achivementTasks)
        {
            if (achivement.achiveType == AchivementType.getCoin && PlayerPrefs.GetInt("Coins") <= achivement.achiveAmount && PlayerPrefs.GetInt("Coins") != 0)
            {
                achivement._achivementAmountTxt.text = PlayerPrefs.GetInt("Coins") + "/" + achivement.achiveAmount;
                achivement.FillBar.fillAmount = (float)PlayerPrefs.GetInt("Coins") / (float)achivement.achiveAmount;
            }
            if (achivement.FillBar.fillAmount == 1 && !achivement.UnLockedAchivement)
            {
                achivement.UnLock();
            }
        }
        //CheckEnemiesKilled();
        //CheckCoinsAdd();
        //CheckUnlockAndDoneAchivement();

    }

    private void Update()
    {

    }

    
    public void CheckEnemiesKilled()
    {
        RealTimeKillAchivementGenerator(CoinFactor);
        foreach (var achivement in _achivementTasks)
        {
            if (achivement.achiveType == AchivementType.killedEnemy && PlayerPrefs.GetInt("KilledEnemiesCount") <= achivement.achiveAmount && PlayerPrefs.GetInt("KilledEnemiesCount") != 0)
            {
                achivement._achivementAmountTxt.text = PlayerPrefs.GetInt("KilledEnemiesCount") + "/" + achivement.achiveAmount;
                achivement.FillBar.fillAmount = (float)PlayerPrefs.GetInt("KilledEnemiesCount") / (float)achivement.achiveAmount;
            }
            if (achivement.FillBar.fillAmount == 1 && !achivement.UnLockedAchivement)
            {
                achivement.UnLock();
            }
        }
        CheckCoinsAdd();
    }

    public void CheckCoinsAdd()
    {
        //RealTimeCoinAchivementGenerator();
        foreach (var achivement in _achivementTasks)
        {
            if (achivement.achiveType == AchivementType.getCoin && PlayerPrefs.GetInt("Coins") <= achivement.achiveAmount && PlayerPrefs.GetInt("Coins") != 0)
            {
                achivement._achivementAmountTxt.text = PlayerPrefs.GetInt("Coins") + "/" + achivement.achiveAmount;
                achivement.FillBar.fillAmount = (float)PlayerPrefs.GetInt("Coins") / (float)achivement.achiveAmount;
            }
            if(achivement.FillBar.fillAmount == 1 && !achivement.UnLockedAchivement)
            {
                achivement.UnLock();
            }
        }

        
    }

    public void CheckUnlockAndDoneAchivement()
    {
        for (int i = 0; i < achivementsList.Count; i++)
        {
            var _achivement = _achivementTasks.FirstOrDefault(x => x.name == achivementsList[i].achivementText);
            if (PlayerPrefs.GetInt("GetAchivement" + achivementsList[i].achivementText) == 1)
            {
                _achivement.UnLock();
                _achivement.TaskDone();
                //print(_achivement.name + " Done");
            }
            else if (PlayerPrefs.GetInt("UnLockedAchivement" + achivementsList[i].achivementText) == 1)
            {
                _achivement.UnLock();
                //print(_achivement.name + " unlock");
            }
        }
    }


    public void FreeAchivementGenerator()
    {
        if (PlayerPrefs.GetInt("FREE Coin") != 1)
        {
            // create  achivement prefab
            AchivementTask _achivement = Instantiate(_achivementPrefab, _achivementParent);

            // add to list
            _achivementTasks.Add(_achivement);

            _achivement.Init("FREE Coin", 0, AchivementType.getCoin, FreeCoin, RewardType.Coin);

        }

        if (PlayerPrefs.GetInt("FREE Rocket") != 1)
        {
            // create  achivement prefab
            AchivementTask _achivement = Instantiate(_achivementPrefab, _achivementParent);

            // add to list
            _achivementTasks.Add(_achivement);

            _achivement.Init("FREE Rocket", 0, AchivementType.killedEnemy, FreeRocket, RewardType.Rocket);
        }

    }

    public void checkCreatedAchivement()
    {
        // for enemy kill
        int EnemyFactor = PlayerPrefs.GetInt("KilledEnemiesCount") / KilledEnemyRewardFactor;
        for(int i = EnemyFactor; i > 0; i--)
        {
            if (PlayerPrefs.GetInt("Kill " + i * KilledEnemyRewardFactor + " Enemis for coin") != 1)
            {
                // instantiate achivement
                AchivementTask _achivement = Instantiate(_achivementPrefab, _achivementParent);

                // add to list
                _achivementTasks.Add(_achivement);

                // init achivement
                _achivement.Init("Kill " + i * KilledEnemyRewardFactor + " Enemis for coin", i * KilledEnemyRewardFactor, AchivementType.killedEnemy,
                    ((PlayerPrefs.GetInt("KilledEnemiesCount") / KilledEnemyRewardFactor) * CoinFactor), RewardType.Coin);
            }
            if (PlayerPrefs.GetInt("Kill " + i * KilledEnemyRewardFactor + " Enemis for rocket") != 1)
            {
                // instantiate achivement
                AchivementTask _achivement = Instantiate(_achivementPrefab, _achivementParent);

                // add to list
                _achivementTasks.Add(_achivement);

                // init achivement
                _achivement.Init("Kill " + i * KilledEnemyRewardFactor + " Enemis for rocket", i * KilledEnemyRewardFactor, AchivementType.killedEnemy,
                    RocketReward, RewardType.Rocket);
            }
        }



        //// for coin obtain
        //int coinFactor = PlayerPrefs.GetInt("Coins") / 1000;
        //for (int i = coinFactor; i > 0; i--)
        //{
        //    if (PlayerPrefs.GetInt("Obtain " + i * 1000 + " Cions for coin") != 1)
        //    {
        //        // instantiate achivement
        //        AchivementTask _achivement = Instantiate(_achivementPrefab, _achivementParent);

        //        // add to list
        //        _achivementTasks.Add(_achivement);

        //        // init achivement
        //        _achivement.Init("Obtain " + i * 1000 + " Cions for coin", i * 1000, AchivementType.getCoin,
        //            i * 1000, RewardType.Coin);
        //    }
        //    if (PlayerPrefs.GetInt("Obtain " + i * 1000 + " Cions for rocket") != 1)
        //    {
        //        // instantiate achivement
        //        AchivementTask _achivement = Instantiate(_achivementPrefab, _achivementParent);

        //        // add to list
        //        _achivementTasks.Add(_achivement);

        //        // init achivement
        //        _achivement.Init("Obtain " + i * 1000 + " Cions for rocket", i * 1000, AchivementType.getCoin,
        //            10, RewardType.Rocket);
        //    }
        //}
    }

    public void RealTimeKillAchivementGenerator(int CoinFactor)
    {

        // kill _ coin reward
        if (PlayerPrefs.GetInt("KilledEnemiesCount") % KilledEnemyRewardFactor == 0)
        {
            
            // instantiate achivement
            AchivementTask _achivement = Instantiate(_achivementPrefab, _achivementParent);

            // add to list
            _achivementTasks.Add(_achivement);

            // init achivement
            _achivement.Init("Kill " + PlayerPrefs.GetInt("KilledEnemiesCount") + " Enemis for coin", PlayerPrefs.GetInt("KilledEnemiesCount"), AchivementType.killedEnemy,
                ((PlayerPrefs.GetInt("KilledEnemiesCount") / KilledEnemyRewardFactor) * CoinFactor), RewardType.Coin);


        }

        // kill _ rocket reward
        if (PlayerPrefs.GetInt("KilledEnemiesCount") % KilledEnemyRewardFactor == 0)
        {

            // instantiate achivement
            AchivementTask _achivement = Instantiate(_achivementPrefab, _achivementParent);

            // add to list
            _achivementTasks.Add(_achivement);

            // init achivement
            _achivement.Init("Kill " + PlayerPrefs.GetInt("KilledEnemiesCount") + " Enemis for rocket", PlayerPrefs.GetInt("KilledEnemiesCount"), AchivementType.killedEnemy,
                RocketReward, RewardType.Rocket);


        }

        PlayerPrefs.SetInt("previousCoins", PlayerPrefs.GetInt("Coins"));
    }
    //public void RealTimeCoinAchivementGenerator()
    //{


    //    // coin _ coin reward
    //    if (PlayerPrefs.GetInt("Coins") % 1000 == 0 && PlayerPrefs.GetInt("Coins") != 0)
    //    {
    //        // instantiate achivement
    //        AchivementTask _achivement = Instantiate(_achivementPrefab, _achivementParent);

    //        // add to list
    //        _achivementTasks.Add(_achivement);

    //        // init achivement
    //        _achivement.Init("Obtain " + PlayerPrefs.GetInt("Coins") + " Cions for coin", PlayerPrefs.GetInt("Coins"), AchivementType.getCoin,
    //            PlayerPrefs.GetInt("Coins"), RewardType.Coin);


    //    }

    //    // coin _ rocket reward
    //    if (PlayerPrefs.GetInt("Coins") % 1000 == 0 && PlayerPrefs.GetInt("Coins") != 0)
    //    {
    //        // instantiate achivement
    //        AchivementTask _achivement = Instantiate(_achivementPrefab, _achivementParent);

    //        // add to list
    //        _achivementTasks.Add(_achivement);

    //        // init achivement
    //        _achivement.Init("Obtain " + PlayerPrefs.GetInt("Coins") + " Cions for rocket", PlayerPrefs.GetInt("Coins"), AchivementType.getCoin,
    //            PlayerPrefs.GetInt("Coins") / 1000, RewardType.Rocket);


    //    }

    //    PlayerPrefs.SetInt("previousCoins", PlayerPrefs.GetInt("Coins"));
    //}


    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}

[System.Serializable]
public class DefineAchivement
{
    public string achivementText;
    public int achivementAmount;
    public AchivementType achiveType;

    public RewardType rewardType;
    public int RewardCount;

    public bool Achived;
}

public enum RewardType
{
    Coin,
    Rocket,

}