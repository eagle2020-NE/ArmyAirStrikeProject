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
    public UpgradeDetails[] upgradeDetails;
}

[System.Serializable]
public class UpgradeDetails
{
    public UpgradeType upgradeType;
    public int[] goldsNeed;
}

public enum UpgradeType
{
    Rocket,
    Gun
}
