using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class CoinsManager : MonoBehaviour
{
    public int Coins;
    public GameEvent onCoinsAdd;

    private void Start()
    {

        Coins = PlayerPrefs.GetInt("Coins");
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Coins += 100;
            PlayerPrefs.SetInt("Coins", Coins);
            //print($"Achive {Coins} Coins");
            onCoinsAdd.Raise();
        }
    }
}
