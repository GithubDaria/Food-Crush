using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelGoalData : ScriptableObject
{
    public List<string> goals = new List<string>();
    public List<int> amountgoal = new List<int>();
    public int MovesCount;
}
