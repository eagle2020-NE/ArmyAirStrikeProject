using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class EnemyKiller : MonoBehaviour
{
    public int KilledEnemiesCount;
    public GameEvent onEnemiesKilled;

    private void Start()
    {
        KilledEnemiesCount = PlayerPrefs.GetInt("KilledEnemiesCount");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            KilledEnemiesCount++;
            PlayerPrefs.SetInt("KilledEnemiesCount", KilledEnemiesCount);
            //print($"Kill {KilledEnemiesCount} Enemies");
            onEnemiesKilled.Raise();
        }
    }
}
