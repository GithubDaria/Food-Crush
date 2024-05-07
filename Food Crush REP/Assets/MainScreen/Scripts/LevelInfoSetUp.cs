using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelInfoSetUp : MonoBehaviour
{
    public TextMeshProUGUI TextScore;
    public Image[] ListOfStars;
    public TextMeshProUGUI LevelNUmber;
    public Sprite EmptyStar;
    public Sprite FullStar;
    public Button PlayButton;
    public CheckLevelsDone MainLevelScript;
    private int LevelNum;
    public void SetUpTheLevelInfoPanel(int levelnum, int score, int stars, bool passed)
    {
        TextScore.text = "Score: \n" + score.ToString();
        LevelNUmber.text = "Level " + (levelnum + 1).ToString();
        LevelNum = levelnum;
        SetStarsSprite(stars);
    }
    private void SetStarsSprite(int staramount)
    {
        for (int i = 0; i < staramount; i++)
        {
            ListOfStars[i].sprite = FullStar;
        }
    }
    public void LoadLevel()
    {
        //Debug.Log("Level NUmber" + GiveLevelNumber());
        MainLevelScript.ChangeClickedLevel(LevelNum);
        SceneManager.LoadScene(1);
    }
}
