using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StrikeKit;
using UnityEngine.UI;

public class EnemyHealthBarManager : MonoBehaviour
{
    public Image fillBar;

    public DamageManager ownerDamageManager;

    // Start is called before the first frame update
    void Start()
    {
        fillBar.fillAmount = 1;
    }

    public void SetEnemyHealthBar()
    {
        float factor = (float)ownerDamageManager.HP / (float)ownerDamageManager.HPmax;

        fillBar.fillAmount = factor;
    }
    // Update is called once per frame
    void Update()
    {
        if (ownerDamageManager.checkhealthbar)
        {

            SetEnemyHealthBar();
            ownerDamageManager.checkhealthbar = false;
        }
        
    }
}
