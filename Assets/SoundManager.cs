using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    Image image;

    public Text text;
    public AudioMixer master;
    bool state;
    public string exposedParameter;
    public string playerPrefs;
    private void Start()
    {
        
        Initialize();
    }

    public void Initialize()
    {
        image = transform.GetComponent<Image>();

        //if (!PlayerPrefs.HasKey(playerPrefs))
        //{

        //}
        if (PlayerPrefs.GetInt(playerPrefs) == 0)
        {
            state = true;
            AudioExtensions.SetVolumeToOne(master, exposedParameter, 0);
            //PlayerPrefs.SetInt(playerPrefs, 1);
            text.text = "ON";
            image.color = Color.green;
            

        }
        else
        {
            AudioExtensions.SetVolumeToZero(master, exposedParameter, 0);
            //PlayerPrefs.SetInt(playerPrefs, 0);
            state = false;

            text.text = "OFF";
            image.color = Color.red;
        }
        print(PlayerPrefs.GetInt(playerPrefs) + "  ____");
        //ToggleMusic();
    }

    public void ToggleMusic()
    {
        if(state)
        {
            AudioExtensions.SetVolumeToZero(master, exposedParameter, 0);
            PlayerPrefs.SetInt(playerPrefs, 1);
            state = false;
            
            text.text = "OFF";
            image.color = Color.red;
        }
        else
        {
            AudioExtensions.SetVolumeToOne(master, exposedParameter, 0);
            PlayerPrefs.SetInt(playerPrefs, 0);
            state = true;
            
            text.text = "ON";
            image.color = Color.green;
        }
        print(state + " " + text.text);
    }
    

}
