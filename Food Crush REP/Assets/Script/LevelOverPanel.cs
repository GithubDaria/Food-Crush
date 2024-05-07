using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class LevelOverPanel : MonoBehaviour
{
    public Image[] ListOfStars;
    public Sprite EmptyStar;
    public Sprite FullStar;
    public TextMeshProUGUI TextScoreText;
    public TextMeshProUGUI PlayerResutlText;
    public PassingLevel passedleveldata;
    private int ScoreHolder;
    public static bool passed;
    public static int score;
    public static int stars;

    private void Start()
    {
         ScoreHolder = passedleveldata.scoreNum;

        if (passedleveldata.Moves > 0)
        {
            ScoreHolder += passedleveldata.Moves * 10;
        }
        score = ScoreHolder;
        TextScoreText.text = "Score: " + ScoreHolder.ToString();

        if (SetMainTextResutl(passedleveldata.PlayerWon))
        {
            SetStarsSprite();

        }

    }
    private void SetStarsSprite()
    {
        int startamount = CountStarts();
        stars = startamount;
        for (int i = 0; i < startamount; i++)
        {
            ListOfStars[i].sprite = FullStar;
        }
    }
    public void ExitTheLevel()
    {
        //passedleveldata.FinishLevel();
        SceneManager.LoadScene(0);
    }
    public bool SetMainTextResutl(bool res)
    {
        if (res)
        {
            PlayerResutlText.text = "Winner";
            passed = true;
            return true;
        }
        else
        {
            PlayerResutlText.text = "Loser";
            passed = false;
            return false;
        }

    }
    private int CountStarts()
    {
        int stars = 0;
        if (ScoreHolder > 10)
        {
            stars++;
        }
        if(ScoreHolder > 20)
        {
            stars++;
        }
        if (ScoreHolder > 50)
        {
            stars++;
        }
        return stars;
    }
}
