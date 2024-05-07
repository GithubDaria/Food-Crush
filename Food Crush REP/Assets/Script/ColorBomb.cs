using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorBomb : MonoBehaviour, IDots
{
    private Dots ThisDot;
    private Dots OtherDot;
    public FindMatches findMatches;
    public Board board;
    void Start()
    {
        findMatches = this.GetComponent<Dots>().findMatches;
        board = this.GetComponent<Dots>().board;
    }
    public IEnumerator IsMatched(Dots currentdot, Dots otherdot)
    {
        ThisDot = currentdot;
        OtherDot = otherdot;
        currentdot.isMatched = true;
        yield return StartCoroutine(ColorBombMatchFinder(currentdot, otherdot));
    }
    IEnumerator ColorBombMatchFinder(Dots currentdot, Dots otherdot)
    {
        if (otherdot != null)
        {
            if (!otherdot.isBomb)
            {
                board.SecretScore += 30;

                Debug.Log("ColorMatch");
                MatchPiecesOfColor(otherdot.tag);
                findMatches.CurentMatches.Add(ThisDot.gameObject);
                board.DestroyMatches();
            }
            else
            {
                if (otherdot.isColorBomb)
                {
                board.SecretScore += 100;

                    ColorBombMatchedWithColorBomb();
                }
                else if (otherdot.isRowBomb || otherdot.isCollumnBomb)
                {
                    board.SecretScore += 50;

                    yield return StartCoroutine(ColorBombMatchedWithArrow(currentdot, otherdot));
                }
                else if (otherdot.isLTBomb)
                {
                    board.SecretScore += 55;

                    yield return StartCoroutine(ColorBombMatchedWithLTBomb(currentdot, otherdot));
                }
            }
        }
        else
        {

            MatchPiecesOfColor(ColorBombOtherDotIsNull());
        }
    }
    public void MatchPiecesOfColor(string color)
    {
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.AllDots[i, j] != null)
                {
                    if (board.AllDots[i, j].CompareTag(color))
                    {
                        board.AllDots[i, j].GetComponent<Dots>().isMatched = true;
                    }
                }
            }
        }
    }
    public string ColorBombOtherDotIsNull()
    {
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.AllDots[i, j] != null)
                {
                    if (board.AllDots[i, j].GetComponent<Dots>().isBomb == false)
                    {
                        return board.AllDots[i, j].tag;
                    }
                }
            }
        }
        return "Orange Dot";
    }
    IEnumerator ColorBombMatchedWithArrow(Dots currentdot, Dots otherdot)
    {
        List<GameObject> dotsColor = new List<GameObject>();
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.AllDots[i, j] != null && !board.AllDots[i, j].GetComponent<Dots>().isBomb)
                {
                    bool randomBool = UnityEngine.Random.Range(0, 2) == 0;
                    if (UnityEngine.Random.Range(0, 10) < 1)
                    {
                        if (randomBool)
                        {
                            yield return new WaitForSeconds(0.2f);
                            board.AllDots[i, j].GetComponent<Dots>().normaldot.MakeColumnBomb();
                            dotsColor.Add(board.AllDots[i, j]);
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.2f);
                            dotsColor.Add(board.AllDots[i, j]);
                            board.AllDots[i, j].GetComponent<Dots>().normaldot.MakeRowBomb();
                        }
                    }
                }
            }
        }
        yield return StartCoroutine(SetToMatchedDotsPieces(dotsColor));

    }
    IEnumerator ColorBombMatchedWithLTBomb(Dots currentdot, Dots otherdot)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.AllDots[i, j] != null && !board.AllDots[i, j].GetComponent<Dots>().isBomb)
                {
                    bool randomBool = UnityEngine.Random.Range(0, 2) == 0;
                    if (UnityEngine.Random.Range(0, 10) < 1)
                    {
                        yield return new WaitForSeconds(0.2f);
                        board.AllDots[i, j].GetComponent<Dots>().normaldot.MakeLTBomb();
                        dots.Add(board.AllDots[i, j]);
                    }
                }
            }
        }
        yield return StartCoroutine(SetToMatchedDotsPieces(dots));
    }
    public void ColorBombMatchedWithColorBomb()
    {
        List<GameObject> dots = new List<GameObject>(board.AllDots.Cast<GameObject>());
        StartCoroutine(SetToMatchedDotsPieces(dots));
    }
    IEnumerator SetToMatchedDotsPieces(List<GameObject> dotsColor)
    {
        findMatches.AddMatchedByBombs(dotsColor);
        yield return new WaitForSeconds(0.3f);
        Debug.Log("Dots Color" + dotsColor.Count.ToString());
        for (int i = 0; i != dotsColor.Count; i++)
        {
            dotsColor[i].GetComponent<Dots>().SetMatchTrue();
            Debug.Log("Dots - " + i.ToString());
        }
        dotsColor = null;
        findMatches.CurentMatches.Add(ThisDot.gameObject);
        Destroy(OtherDot.gameObject);
    }
}
