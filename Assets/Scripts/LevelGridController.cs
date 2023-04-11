using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGridController : MonoBehaviour
{
    #region Public Variable
    public GameObject LevelButtonPrefab;
    //public CoinsRepository CoinRepo;
    public AudioSource audioSrc;
    public AudioClip clip;
    #endregion

    #region Private Variable
    [SerializeField]
    private LevelRepository levelRepo;
    #endregion

    #region Public Method
    public void MainMenu()
    {
        //CoinRepo.Save();
        audioSrc.PlayOneShot(clip);
        SceneManager.LoadScene("MainMenu");
        //Debug.Log("coinRepo" + CoinRepo.Get());
    }
    #endregion

    #region Private Method
    private void Awake()
    {
        levelRepo.Init();
        //CoinRepo.Save();
    }
    private void Start()
    {
        bool[] levels = levelRepo.RetriveAllLevel();
        for (int i = 0; i < levelRepo.LevelCount; i++)
        {
            GameObject btn = Instantiate(LevelButtonPrefab, transform.position, Quaternion.identity) as GameObject;
            btn.transform.SetParent(transform);
            btn.transform.localScale = Vector3.one;
            btn.GetComponent<ButtonLevelController>().Init(i, levels[i]);
        }
    }
    private void OnApplicationQuit()
    {
        //CoinRepo.Save();
    }
    #endregion
}
