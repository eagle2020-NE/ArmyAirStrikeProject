using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlanesData" , menuName = "PlanesData")]
public class PlanesData : ScriptableObject
{
    public PlanesDetails[] planesDetails;



}


[System.Serializable]
public class PlanesDetails
{
    public string planeName;
    public string planeCharacteristic;
    public int planeNumber;
    public int BuyGold;
    public bool isFreePlane;
    [Space(10)]

    public int numberOfRockets;
    public int numberOfBullets;

    [Space(10)]

    public int planeCurrentHealth;
    public int planeCurrentSpeed;

    [Space(10)]
    public UpgradeDetails[] upgradeDetails;
    [Space(10)]
    public LevelUpgrade[] levelUpgrade;
}

[System.Serializable]
public class UpgradeDetails
{
    public UpgradeType upgradeType;
    public int[] goldsNeed;
}

[System.Serializable]
public class LevelUpgrade
{
    public string Level;
    public int goldsNeed;
    public int healthIncrease;
    public int SpeedIncrease;
    public int timeToUpgrade;
    public bool isUpgrade;
    
}




public enum UpgradeType
{
    Rocket,
    Gun,
    Level
}
