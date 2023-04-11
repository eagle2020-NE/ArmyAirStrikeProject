using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonLevelController : MonoBehaviour
{
    #region Public Variable
    public Text txtLevelNumber;
    public GameObject LockImage;
    //public GameObject coinRepo;
    public AudioSource audioSrc;
    public AudioClip clipOpen;
    public AudioClip clipLock;
    //public CoinsRepository coinRepo;
    #endregion

    #region Private Variable
    private bool Locked;
    private int num;
    //private CoinsRepository coinRepo;
    #endregion

    #region Public Method
    public void Init(int number,bool isLocked)
    {
        Locked = isLocked;
        if(isLocked)
        {
            LockImage.SetActive(true);
            txtLevelNumber.gameObject.SetActive(false);
        }
        else
        {
            LockImage.SetActive(false);
            txtLevelNumber.gameObject.SetActive(true);
            txtLevelNumber.text = (number + 1).ToString();
            num = number;
        }
    }

    public void Click()
    {
        //coinRepo.Save();
        if (Locked == false)
        {
            audioSrc.PlayOneShot(clipOpen);
            //string LevelNumber = "Level " + num;
            //SceneManager.LoadScene(LevelNumber);
            StartCoroutine(AfterSound());
        }
        else
        {
            audioSrc.PlayOneShot(clipLock);
        }
    }
    #endregion

    #region Private Method

    private void Start()
    {
        //coinRepo = FindObjectOfType<CoinsRepository>();
    }
    private IEnumerator AfterSound()
    {
        yield return new WaitForSeconds(clipOpen.length);
        string LevelNumber = "Level" + num;
        SceneManager.LoadScene(LevelNumber);
    }

    private void OnApplicationQuit()
    {
        //coinRepo.Save();
    }
    #endregion
}
