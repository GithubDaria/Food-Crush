using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RowArrowBomb : MonoBehaviour, IDots
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
                board.SecretScore += 15;

                GetRowPieces(currentdot.row);
            }
            else if (otherdot.isCollumnBomb || otherdot.isRowBomb)
            {
                board.SecretScore += 20;
                currentdot.row = otherdot.row;
                currentdot.column = otherdot.column;
                otherdot.isMatched = true;
                GetRowPieces(currentdot.row);
                GetColumnPieces(currentdot.column);
                Destroy(otherdot.gameObject);
            }
            else if (otherdot.isLTBomb)
            {
                board.SecretScore += 25;
                otherdot.isMatched = true;
                LtToArrowMove(currentdot.row);
                Destroy(otherdot.gameObject);
            }
            else if (otherdot.isColorBomb)
            {
                board.SecretScore += 50;
                otherdot.GetComponent<ColorBomb>().IsMatched(otherdot, currentdot);
                Destroy(otherdot.gameObject);
            }
        }
        else
        {
            GetRowPieces(currentdot.column);
        }
        yield return null;
        //board.DestroyMatches();
    }
    private void LtToArrowMove(int row)
    {
        for (int i = row - 1; i <= row + 1; i++)
        {
            if (i >= 0 && i < board.height)
            {
                GetRowPieces(i);
            }
        }
    }

    public void GetRowPieces(int row)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.width; i++)
        {
            if (board.AllDots[i, row] != null)
            {
                //Debug.Log("Collumn - " + i.ToString() + "Row - " + row);

                board.AllDots[i, row].GetComponent<Dots>().SetMatchTrue();
                dots.Add(board.AllDots[i, row]);

            }
        }
        findMatches.AddMatchedByBombs(dots);
        dots.Clear();
    }
    public void GetColumnPieces(int column)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.height; i++)
        {
            if (board.AllDots[column, i] != null)
            {
                //Debug.Log("Collumn - " + column.ToString() + "Row - " + i);
                board.AllDots[column, i].GetComponent<Dots>().SetMatchTrue();
                dots.Add(board.AllDots[column, i]);
            }
        }
        findMatches.AddMatchedByBombs(dots);
        dots.Clear();
    }

}
