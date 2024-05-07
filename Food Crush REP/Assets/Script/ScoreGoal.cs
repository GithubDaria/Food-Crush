using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGoal : MonoBehaviour
{
    public string TypeTag;
    public int AmountGoal;
    private int CurrentAmount;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI GoalAmountText;
    public Image goalsprite;
    public List<SpriteRenderer> AllElementsSprite;
    // Start is called before the first frame update
    void Start()
    {
        //DotScipt = FindObjectOfType<Dots>();
        TypeTag = "Green Dot";
        AmountGoal = 10;
        CurrentAmount = AmountGoal;
        GoalAmountText.text = AmountGoal.ToString();
        SpriteRenderer sprgoal = GetSpriteForGoal();
        goalsprite.sprite = sprgoal.sprite;
        goalsprite.color = sprgoal.color;
        //goalsprite.sprite = DotScipt.
    }
    private SpriteRenderer GetSpriteForGoal()
    {
        foreach(var item in AllElementsSprite)
        {
            if(item.tag == TypeTag)
            {
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
    private void UpdateScoreGoals()
    {
        if(CurrentAmount >= 0)
        {
            GoalAmountText.text = CurrentAmount.ToString();
        }
    }
}
