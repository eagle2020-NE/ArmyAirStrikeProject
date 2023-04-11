using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRepository : MonoBehaviour
{
    #region Public Variable
    [Range(1, 10)]
    public int LevelCount;
    #endregion

    #region Private Variable
    private int LockedLevelIndex;
    #endregion

    #region Constant Variable
    private const string RepositoryName = "levelRepository";
    #endregion

    #region Public Method
    public bool IsLocked(int i)    // Level (i) is Locked or Not ?
    {
        string[] s = RetriveFromRepoToArrey();
        if(s[i] == "1")
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void OpenLevel(int i)   // Open Level (i) 
    {
        string[] s = RetriveFromRepoToArrey();
        s[i] = "1";
        string newS = ConvertToString(s);
        SaveRepo(newS);
    }

    public void OpenNextLevel(int thisLevelIndex)   //Find first Locked Level = (0) { isLock == true} , Then Open it
    {
        //string[] s = RetriveFromRepoToArrey();
        //for (int i = 0; i < s.Length; i++)          // find first 0 in levelArrey. for example (index = 2)
        //{
        //    if (s[i] == "0")
        //    {
        //        LockedLevelIndex = i;
        //        break;
        //    }
        //}
        //OpenLevel(LockedLevelIndex);
        OpenLevel(thisLevelIndex -1);
        
    }

    public bool[] RetriveAllLevel()   // return all level locked State. [false,false,true,true,true,true,...] => show that Level 1 & 2 is Open.
    {
        bool[] levelArrey = new bool[LevelCount];
        string[] levels = new string[LevelCount];
        levels = RetriveFromRepoToArrey();
        for(int i = 0; i < LevelCount; i++)
        {
            if(levels[i] == "1")
            {
                levelArrey[i] = false;
            }
            else
            {
                levelArrey[i] = true;
            }
        }
        return levelArrey;
    }
    #endregion

    #region Private Method
    private void Awake()
    { 
        //Init();
    }
    private string[] RetriveFromRepoToArrey()  // [1,0,0,0,0,0,0,0,0,0]
    {
        string levels = PlayerPrefs.GetString(RepositoryName);
        //Debug.Log(levels);
        return levels.Split('-');
    }

    public void Init()
    {
        if (PlayerPrefs.HasKey(RepositoryName) == false)
        {
            string s = "1-0-0-0-0-0-0-0-0-0";
            SaveRepo(s);
        }
    }

    private string ConvertToString(string[] s)     //  s : [1,1,0,0]  ==> newS : "1-1-0-0"
    {
        string newS = "";
        for (int i = 0; i < s.Length; i++)        // s.Length = 4
        {
            newS += s[i];
            if (i < s.Length - 1)                 // i < 3
            {
                newS += "-";
            }
        }
        return newS;
    }
    private void SaveRepo(string s)
    {
        PlayerPrefs.SetString(RepositoryName, s);
    }
    #endregion
}
