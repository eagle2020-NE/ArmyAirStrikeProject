using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeLevel : MonoBehaviour
{

    [SerializeField] private LevelRepository levelRepo;
    [SerializeField] private Text LevelNm;

    private void Start()
    {
        LevelNm.text = (SceneManager.GetActiveScene().buildIndex - 1).ToString();
    }

    public void ChangeCurrentLevel()
    {
        levelRepo.OpenNextLevel(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("Level" + (SceneManager.GetActiveScene().buildIndex - 1));
    }

    public void OpenLevelManager()
    {
        SceneManager.LoadScene("LevelManager");
    }
}
