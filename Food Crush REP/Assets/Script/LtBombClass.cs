using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LtBombClass : MonoBehaviour, IDots
{
    private Dots Currentdot;
    public FindMatches findMatches;
    public Board board;
    void Start()
    {
        findMatches = this.GetComponent<Dots>().findMatches;
        board = this.GetComponent<Dots>().board;
    }
    public IEnumerator IsMatched(Dots currentdot, Dots otherdot)
    {
        Currentdot = currentdot;
        Currentdot.isMatched = true;
        if (otherdot != null)
        {
            if (!otherdot.isBomb)
            {
                board.SecretScore += 20;

                GetLTPieces(currentdot.column, currentdot.row, 1);
            }
            else if (otherdot.isCollumnBomb || otherdot.isRowBomb)
            {
                board.SecretScore += 13;
                IDots dotsComponent = otherdot.GetComponent<IDots>();
                if (dotsComponent != null)
                {
                    dotsComponent.IsMatched(otherdot, currentdot);
                }
            }
            else if (otherdot.isLTBomb)
            {
                board.SecretScore += 30;
                otherdot.isMatched = true;
                GetLTPieces(currentdot.column, currentdot.row, 2);
            }
            else if (otherdot.isColorBomb)
            {
                otherdot.GetComponent<ColorBomb>().IsMatched(otherdot, currentdot);
            }
            Destroy(otherdot.gameObject);
        }
        else
        {
            GetLTPieces(currentdot.column, currentdot.row, 1);
        }

        yield return null;
    }
    public void GetLTPieces(int column, int row, int RangeOfrowcollumn)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = column - RangeOfrowcollumn; i <= column + RangeOfrowcollumn; i++)
        {
            for (int j = row - RangeOfrowcollumn; j <= row + RangeOfrowcollumn; j++)
            {
                if (i >= 0 && i < board.width && j >= 0 && j < board.height)
                {
                    board.AllDots[i, j].GetComponent<Dots>().SetMatchTrue();
                    dots.Add(board.AllDots[i, j]);
                }
            }
        }
        Debug.Log("Dots to goal " + dots.Count.ToString());
        findMatches.AddMatchedByBombs(dots);
        dots.Clear();
    }
}
