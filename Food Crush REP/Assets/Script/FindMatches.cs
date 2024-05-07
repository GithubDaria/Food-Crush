using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.IK;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> CurentMatches;
    public List<GameObject> AllMatchedDots;
    //List<GameObject> ColorDots = new List<GameObject>();

    void Start()
    {
        board = FindObjectOfType<Board>();
    }
    public void StartFindAllMatchesCoroutine()
    {
        StartCoroutine(FindAllMatches());
    }
    public IEnumerator FindAllMatches()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject CurentDot = board.AllDots[i, j];
                if (CurentDot != null)
                {
                    Dots CurentDotDot = CurentDot.GetComponent<Dots>();

                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject LeftDot = board.AllDots[i - 1, j];
                        GameObject RightDot = board.AllDots[i + 1, j];

                        if (LeftDot != null && RightDot != null)
                        {
                            Dots rightDotDot = RightDot.GetComponent<Dots>();
                            Dots leftDotDot = LeftDot.GetComponent<Dots>();
                            if (LeftDot.tag == CurentDot.tag && RightDot.tag == CurentDot.tag)
                            {
                                /*CurentMatches.Union(IsRowBomb(leftDotDot, CurentDotDot, rightDotDot));
                                CurentMatches.Union(IsColumnBomb(leftDotDot, CurentDotDot, rightDotDot));
                                CurentMatches.Union(IsLTBomb(leftDotDot, CurentDotDot, rightDotDot));*/
                                GetNearbyPieces(LeftDot, CurentDot, RightDot);
                            }
                        }
                    }

                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject UpDot = board.AllDots[i, j + 1];
                        GameObject DownDot = board.AllDots[i, j - 1];
                        if (UpDot != null && DownDot != null)
                        {
                            Dots upDotDot = UpDot.GetComponent<Dots>();
                            Dots dowDotDot = DownDot.GetComponent<Dots>();
                            if (UpDot.tag == CurentDot.tag && DownDot.tag == CurentDot.tag)
                            {
                                /* CurentMatches.Union(IsColumnBomb(upDotDot, CurentDotDot, dowDotDot));
                                 CurentMatches.Union(IsRowBomb(upDotDot, CurentDotDot, dowDotDot));
                                 CurentMatches.Union(IsLTBomb(upDotDot, CurentDotDot, dowDotDot));*/
                                GetNearbyPieces(UpDot, CurentDot, DownDot);
                            }
                        }
                    }
                    /*board.CheackToMakeBombs();*/
                }
            }
        }
    }
    private void GetNearbyPieces(GameObject dot1, GameObject dot2, GameObject dot3)
    {
        AddToListAndMatch(dot1);
        AddToListAndMatch(dot2);
        AddToListAndMatch(dot3);
    }
    private void AddToListAndMatch(GameObject dot)
    {
        if (!CurentMatches.Contains(dot))
        {
            CurentMatches.Add(dot);
            AllMatchedDots.Add(dot);
        }
        dot.GetComponent<Dots>().isMatched = true;
    }
    public void AddMatchedByBombs(List<GameObject> otherMatchedDots)
    {
        //List<GameObject> AllMatchedDots = new List<GameObject>(AllMatchedDots.Union(otherMatchedDots));
        AllMatchedDots.AddRange(otherMatchedDots);
        Debug.Log("Dots to matched " + AllMatchedDots.Count.ToString());
    }
}
