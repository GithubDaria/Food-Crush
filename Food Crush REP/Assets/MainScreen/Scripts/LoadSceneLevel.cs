using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LoadSceneLevel : MonoBehaviour
{
    [SerializeField] int LevelNumber;
    //[SerializeField] int LevelNumber2;
    public int num = 0;
    public GameObject LevelLocked;
    public GameObject Stars;
    public int Score;
    public int Moves;
    public int StarsNumber;
    public Sprite EmptyStar;
    public Sprite FullStar;
    public Image[] ListOfStars;
    public bool Passed = false;
    public LevelInfoSetUp LevelInfoScript;
    public LevelGoalData levelgoaldata;
    [SerializeField] List<string> levelgoallist;
    [SerializeField] List<int> levelgoalamountlists;

    public void SetThelevelValue(int score, int stars, int levelnumber, int moves, bool passed)
    {
        LevelNumber = levelnumber;
        Score = score;
        Passed = passed;
        StarsNumber = stars;
        Moves = moves;
        LevelLocked.SetActive(false);
        Stars.SetActive(true);
        SetStarsAmount(StarsNumber);
        
    }
    public void SetStarsAmount(int amount)
    {
        int i = 0;
        for (i = 0; i < amount; i++)
        {
            ListOfStars[i].sprite = FullStar;
        }
        while(i< ListOfStars.Count())
        {
            ListOfStars[i].sprite = EmptyStar;
            i++;
        }
    }
    public void CallFunction()
    {
        levelgoaldata.goals = levelgoallist;
        levelgoaldata.amountgoal = levelgoalamountlists;
        levelgoaldata.MovesCount = Moves;
        LevelInfoScript.SetUpTheLevelInfoPanel(LevelNumber, Score, StarsNumber, Passed);
    }
}
