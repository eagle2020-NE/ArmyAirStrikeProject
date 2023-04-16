using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTimer;
using TMPro;
public class TimerToPlayThisLevel : MonoBehaviour
{

    [SerializeField] private float TimeToPlay;
    [SerializeField] private TextMeshProUGUI TimeToPlayText;

    Timer timer;

    void Start()
    {
        timer = Timer.Register(TimeToPlay, () => Debug.Log("Hello World"));
    }

    private void Update()
    {
        //TimeToPlayText.text = timer.GetTimeRemaining()
        TimeToPlayText.text = "Time : " + ((int)timer.GetTimeRemaining());
    }
}
