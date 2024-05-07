using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PassingLevel : MonoBehaviour
{
    public bool passedNum;
    public int scoreNum;
    public int starsNum;

    public static bool passed = false;
    public static int score = 0;
    public static int stars = 0;
    public LevelGoalData levelGoalData;
    private string TypeTag;
    private int AmountGoal;
    private int CurrentAmount;
    public TextMeshProUGUI ScoreText;
    private TextMeshProUGUI GoalAmountText;
    public TextMeshProUGUI MoveText;
    private Image goalsprite;
    public List<SpriteRenderer> AllElementsSprite;
    public GameObject ScorePrefab;
    private GameObject InstaPrefab;
    public Transform GridTasks;
    public int Moves;
    public GameObject LockedScren;
    public bool PlayerWon;

    public TextMeshProUGUI PlayerGameResult;
    private void Start()
    {
        for(int i = 0; i < levelGoalData.goals.Count; i++)
        {
            Debug.Log(levelGoalData.goals[i] + " - " + levelGoalData.amountgoal[i]);
            InstaPrefab = Instantiate(ScorePrefab, transform.parent = GridTasks);
        }
        if(levelGoalData.goals.Count < 1)
        {
            InstaPrefab = Instantiate(ScorePrefab, transform.parent = GridTasks);
            TypeTag = "Blue Dot";
            AmountGoal = 2;
        }
        else
        {
            TypeTag = levelGoalData.goals[0];
            AmountGoal = levelGoalData.amountgoal[0];

        }
        //Moves = levelGoalData.MovesCount;
        Moves = 20;
        CurrentAmount = AmountGoal;
        goalsprite = InstaPrefab.transform.GetChild(0).GetComponent<Image>();
        GoalAmountText = goalsprite.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        GoalAmountText.text = AmountGoal.ToString();
        SpriteRenderer sprgoal = GetSpriteForGoal();
        goalsprite.sprite = sprgoal.sprite;
        MoveText.text = Moves.ToString();
        /*goalsprite = sprgoal.color;*/
    }
    public void FinishLevel()
    {
       passed = passedNum;
       score = scoreNum;
       stars = starsNum;

    SceneManager.LoadScene(0);

    }
    private SpriteRenderer GetSpriteForGoal()
    {
        foreach (var item in AllElementsSprite)
        {
            if (item.tag == TypeTag)
            {
                Debug.Log("Got it ");
                return item;
            }
        }
        return null;
    }
    public void CheckMatchesForGoal(List<GameObject> matchedDots)
    {
        Debug.Log("Dots to matched and sent " + matchedDots.Count.ToString());

        foreach (GameObject go in matchedDots)
        {
            if (go != null)
            {
                if (go.tag == TypeTag)
                {
                    CurrentAmount--;
                    UpdateScoreGoals();
                }
            }
        }
    }
    public void UpdateScoreFromSecret(int secterscore)
    {
        scoreNum = secterscore;
        ScoreText.text= "Score - " + scoreNum.ToString();
    }
    private void UpdateScoreGoals()
    {
        if (CurrentAmount >= 0)
        {
            GoalAmountText.text = CurrentAmount.ToString();
        }
        else
        {
            GameIsOver();
        }
    }
    public void MoveMade()
    {
        Moves--;
        if(Moves == 0)
        {
            MoveText.text = "0";
            GameIsOver();
            return;
        }
        MoveText.text = Moves.ToString();
    }
    public void GameIsOver()
    {
        if (CurrentAmount < 1)
        {
            //win
            Debug.Log("Win");
            PlayerWon = true;

        }
        else
        {
            //lose
            PlayerWon = false;

            Debug.Log("Lose");
        }
        LockedScren.SetActive(true);
    }

}
