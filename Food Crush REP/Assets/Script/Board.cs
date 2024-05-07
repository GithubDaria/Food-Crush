using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum GameState
{
    wait,
    move
}

public class Board : MonoBehaviour
{
    private FindMatches findMatches;
    public GameState CurentState = GameState.move;
    public int width;
    public int height;
    public int OffsetPos = 20;
    public GameObject tilePrefab;
    private BackGroundTile[,] Tiles;
    public GameObject[,] AllDots;
    private PassingLevel scoregoal;
    public AudioScript audioPlayer;
    public Dots currentDot;
    public GameObject[] Dots;
    public int SecretScore = 0;
    public int NumberOfSameCurrntDotTag;
    private bool FinishedSetUp;
    public bool DeadLock;
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        scoregoal = FindObjectOfType<PassingLevel>();
        audioPlayer = FindObjectOfType<AudioScript>();
        Tiles = new BackGroundTile[width, height];
        AllDots = new GameObject[width, height];
        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPos = new Vector2(i, j + OffsetPos);
                GameObject backGroundTile = Instantiate(tilePrefab, tempPos, Quaternion.identity) as GameObject;
                backGroundTile.transform.parent = this.transform;
                backGroundTile.name = "( " + i + " ," + j + " )";
                int dotToUse = Random.Range(0, Dots.Length);
                int maxIterations = 0;
                while (MatchesAt(i, j, Dots[dotToUse]) && maxIterations < 100)
                {
                    dotToUse = Random.Range(0, Dots.Length);
                    maxIterations++;
                }
                maxIterations = 0;
                GameObject dot = Instantiate(Dots[dotToUse], tempPos, Quaternion.identity);
                dot.GetComponent<Dots>().row = j;
                dot.GetComponent<Dots>().column = i;
                dot.transform.parent = this.transform;
                dot.name = "( " + i + " ," + j + " )";
                AllDots[i, j] = dot;
            }
        }
    }
    private bool MatchesAt(int column, int row, GameObject dot)
    {
        if (column > 1 && row > 1)
        {
            if (AllDots[column - 1, row].tag == dot.tag && AllDots[column - 2, row].tag == dot.tag)
            {
                return true;
            }
            if (AllDots[column, row - 1].tag == dot.tag && AllDots[column, row - 2].tag == dot.tag)
            {
                return true;
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (AllDots[column, row - 1].tag == dot.tag && AllDots[column, row - 2].tag == dot.tag)
                    return true;
            }
            if (column > 1)
            {
                if (AllDots[column - 1, row].tag == dot.tag && AllDots[column - 2, row].tag == dot.tag)
                    return true;
            }
        }


        return false;
    }
    public void CheackToMakeBombs()
    {
        if (currentDot == null)
        {
            currentDot = CheckWhichPieceMovedForBomb(findMatches.CurentMatches, "null");
            //currentDot.SwipeDirection = 2;
            //currentDot.otherDot = CheckWhichPieceMovedForBomb(findMatches.CurentMatches, "null").gameObject;
        }
        NumberOfSameCurrntDotTag = CountAmountOfSameTag(findMatches.CurentMatches, currentDot.tag);
        Debug.Log("SameTagAmount = " + NumberOfSameCurrntDotTag.ToString());
        switch (findMatches.CurentMatches.Count)
        {
            case 4:
                {
                    MakeRowArrowOrCollumn();
                    SecretScore+= 4;
                    audioPlayer.BombCreationAudio();

                    break;
                }
            case 5:
                {
                    MakeColorBombOrLtBomb();
                    SecretScore += 10;
                    audioPlayer.BombCreationAudio();

                    break;
                }
            case 7:
                {
                    MakeRowArrowOrCollumn();
                    SecretScore += 4;
                    audioPlayer.BombCreationAudio();

                    break;
                }   
            case 8:
                {
                    if (!MakeRowArrowOrCollumn())
                    {
                        MakeColorBombOrLtBomb();
                        SecretScore += 10;
                    }
                    SecretScore += 4;
                    audioPlayer.BombCreationAudio();

                    break;
                }
            case 9:
                {
                    if (MakeRowArrowOrCollumn() == false)
                    {
                        MakeColorBombOrLtBomb();
                        SecretScore += 10;
                    }
                    audioPlayer.BombCreationAudio();

                    SecretScore += 4;
                    break;
                }
            case 10:
                {
                    MakeColorBombOrLtBomb();
                    SecretScore += 10;
                    audioPlayer.BombCreationAudio();

                    break;

                }
        }
    }
    public Dots CheckWhichPieceMovedForBomb(List<GameObject> dots, string DotTag)
    {
        foreach (GameObject gb in dots)
        {
            Dots gbdot = gb.GetComponent<Dots>();
            if(gbdot.tag != DotTag)
            {
                if (gbdot.row != gbdot.previousRow)
                {
                    return gbdot;
                }
            }
        }
        return null;
    }
    public int CountAmountOfSameTag(List<GameObject> dots, string DotTag)
    {
        int NumberOfSameTag = 0;
        foreach (GameObject gb in dots)
        {
            Dots gbdot = gb.GetComponent<Dots>();
            if (gbdot.tag == DotTag)
            {
                NumberOfSameTag++;
            }
        }
        return NumberOfSameTag;
    }
    private bool ColumbOrRowTry(bool ColorBombSpecial)
    {
        int numberHorizontal = 0;
        int numberVertical = 0;
        Dots firstPiece = findMatches.CurentMatches[0].GetComponent<Dots>();
        if (firstPiece != null)
        {
            foreach (GameObject curentPiece in findMatches.CurentMatches)
            {
                Dots dot = curentPiece.GetComponent<Dots>();
                if (dot.row == firstPiece.row)
                {
                    numberHorizontal++;
                }
                if (dot.column == firstPiece.column)
                {
                    numberVertical++;
                }


            }
        }
        if(findMatches.CurentMatches.Count == 5)
        {
            return(numberVertical == 5 || numberHorizontal == 5); //ColorBomb 
        }
        else if(ColorBombSpecial)
        {
            Debug.Log("Horizon = " + numberHorizontal.ToString());
            Debug.Log("Vertical = " + numberVertical.ToString());
            /*return (numberHorizontal >=8 || numberVertical >=8);*/ //colorbomb
            return (numberVertical == 5 || numberHorizontal == 5);
        }
        return(numberVertical > numberHorizontal); //collunm arrow
    }
    private bool MakeRowArrowOrCollumn() //4 //7 //8 //9
    {
        if(NumberOfSameCurrntDotTag == 4) //current dot is match of 4
        {
            currentDot.isMatched = false;
            if (ColumbOrRowTry(false))
            {
                currentDot.normaldot.MakeRowBomb();
            }
            else
            {
                currentDot.SwipeDirection = 2;
                currentDot.normaldot.MakeColumnBomb();
            }
            if (findMatches.CurentMatches.Count - NumberOfSameCurrntDotTag == 4) //other dot is match of 4 as well
            {
                Dots otherDot = CheckWhichPieceMovedForBomb(findMatches.CurentMatches, currentDot.tag);
                otherDot.isMatched = false;
                if (currentDot.SwipeDirection == 2)
                {
                    otherDot.normaldot.MakeRowBomb();
                }
                else
                {
                    otherDot.normaldot.MakeColumnBomb();
                }
                return true;
            }
        }
        else
        {
            Dots otherDot = CheckWhichPieceMovedForBomb(findMatches.CurentMatches, currentDot.tag);
            otherDot.isMatched = false;
            if (currentDot.SwipeDirection == 2)
            {
                otherDot.normaldot.MakeColumnBomb();
            }
            else
            {
                otherDot.normaldot.MakeRowBomb();
            }
            return false;

        }
        return false;
    }
    private void MakeColorBombOrLtBomb() //5 //8 //9 //10
    {
        currentDot.isMatched = false;
        if(NumberOfSameCurrntDotTag == 5)
        {
            if(NumberOfSameCurrntDotTag == 5)
            {
                if (ColumbOrRowTry(true))
                {
                    currentDot.normaldot.MakeColorBomb();
                }
                else
                {
                    currentDot.normaldot.MakeLTBomb();
                }

            }
            if (findMatches.CurentMatches.Count - NumberOfSameCurrntDotTag == 5)
            {
                Dots otherDot = CheckWhichPieceMovedForBomb(findMatches.CurentMatches, currentDot.tag);
                otherDot.isMatched = false;
                if (CheckIfColorBomb(otherDot.gameObject))
                {
                    otherDot.normaldot.MakeColorBomb();
                }
                else
                {
                    otherDot.normaldot.MakeLTBomb();
                }
            }
        }
        else
        {
            Dots otherDot = CheckWhichPieceMovedForBomb(findMatches.CurentMatches, currentDot.tag);
            otherDot.isMatched = false;
            if (ColumbOrRowTry(true))
            {
                otherDot.normaldot.MakeColorBomb();
            }
            else
            {
                otherDot.normaldot.MakeLTBomb();
            }
        }
    }
   
    public bool CheckDoubleArrowType()
    {
        if (AllDots[currentDot.column - 1, currentDot.row] == null && AllDots[currentDot.column + 1, currentDot.row])
        {
            return true;
        }
        return false;
    }
    private void DestroyMatchAt(int column, int row)
    {

        if (AllDots[column, row].GetComponent<Dots>().isMatched)
        {
            /*            if (findMatches.CurentMatches.Count >= 4)
                        {
                            CheackToMakeBombs();
                        }*/

            Destroy(AllDots[column, row]);
            AllDots[column, row] = null;
        }
    }
    public void DestroyMatches()
    {
        scoregoal.CheckMatchesForGoal(findMatches.AllMatchedDots);
        SecretScore += findMatches.CurentMatches.Count;
        Debug.Log("Dots to matched and preparing to send " + findMatches.AllMatchedDots.Count.ToString());
        Debug.Log("CurentMatches.Count - " + findMatches.CurentMatches.Count.ToString());
        if(currentDot != null)
        {
            if (currentDot.isMatched || (currentDot.otherDot == null || currentDot.otherDot.GetComponent<Dots>().isMatched))
            {
                scoregoal.MoveMade();
            }
        }
        if (findMatches.CurentMatches.Count >= 4)
        {
            CheackToMakeBombs();
        }
        audioPlayer.PlayElementCombinedAudio();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (AllDots[i, j] != null)
                {
                    DestroyMatchAt(i, j);


                }
            }
        }

        //CheackToMakeBombs();
        scoregoal.UpdateScoreFromSecret(SecretScore);
        findMatches.CurentMatches.Clear();
        findMatches.AllMatchedDots.Clear();
        StartCoroutine(CollapseDots());
    }

   
    private IEnumerator CollapseDots()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (AllDots[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    AllDots[i, j].GetComponent<Dots>().previousRow = AllDots[i, j].GetComponent<Dots>().row;
                    AllDots[i, j].GetComponent<Dots>().row -= nullCount;
                    /* Debug.Log(AllDots[i, j].GetComponent<Dots>().previousRow);
                     Debug.Log(AllDots[i, j].GetComponent<Dots>().row);*/

                    AllDots[i, j] = null;
                }
                else
                {
                    AllDots[i, j].GetComponent<Dots>().previousRow = AllDots[i, j].GetComponent<Dots>().row;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(0.3f);
       
        StartCoroutine(FillBoard());
     



    }
    public bool CheckifAutoCollumnArrow()
    {
        if (AllDots[currentDot.column - 1, currentDot.row].tag == AllDots[currentDot.column + 1, currentDot.row].tag)
        {
            return true;
        }
        return false;
    }
    public bool CheckIfColorBomb(GameObject Dot)
    {
        Dots dot = Dot.GetComponent<Dots>();

        if (AllDots[dot.column - 1, dot.row].tag == AllDots[dot.column + 1, dot.row].tag && AllDots[dot.column, dot.row - 1].tag != dot.tag && AllDots[dot.column, dot.row + 1].tag != dot.tag)
        {
            return true;
        }
        if (AllDots[dot.column, dot.row - 1].tag == AllDots[dot.column, dot.row + 1].tag && AllDots[dot.column - 1, dot.row].tag != dot.tag && AllDots[dot.column + 1, dot.row].tag != dot.tag)
        {
            return true;

        }
        return false;
    }
    private void ReffilBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (AllDots[i, j] == null)
                {
                    Vector2 tempPos = new Vector2(i, j + OffsetPos);
                    int dotToUse = Random.Range(0, Dots.Length);
                    GameObject dot = Instantiate(Dots[dotToUse], tempPos, Quaternion.identity, transform.parent = this.transform);
                    AllDots[i, j] = dot;

                    dot.GetComponent<Dots>().row = j;
                    dot.GetComponent<Dots>().column = i;
                }
            }
        }
        

    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (AllDots[i, j] != null)
                {
                    if (AllDots[i, j].GetComponent<Dots>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private void MixUpElementsAfterDeadLock()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (AllDots[i, j] != null)
                {
                    Destroy(AllDots[i, j]);
                    AllDots[i, j] = null;
                }
            }
        }
      /*  StopCoroutine(FillBoard());
        StartCoroutine(FillBoard());*/

    }
    private IEnumerator FillBoard()
    {
        ReffilBoard();
        int SecondsToWait = 0;
        yield return new WaitForSeconds(0.5f);
        //StartCoroutine(DetectDeadLock());
        // DeadLock = true;
        Debug.Log("Current deadlock is " + DeadLock);
        while (MatchesOnBoard()||findMatches.CurentMatches.Count > 0)
        {
            //Debug.Log("Current deadlock is " + DeadLock);
            SecondsToWait++;
            DestroyMatches();
            yield return new WaitForSeconds(1f);
            Debug.Log("Deadlock start");

           // yield return StartCoroutine(IEDetectDeadLock());
            Debug.Log("Deadlock finished");
            /*yield return new WaitForSeconds(3f);*/
        }
        yield return StartCoroutine(IEDetectDeadLock());
        findMatches.CurentMatches.Clear();
        currentDot = null;
        yield return new WaitForSeconds(0.2f);
        CurentState = GameState.move;
        yield return null;
        FinishedSetUp = true;
        
    }
    public IEnumerator IEDetectDeadLock()
    {
        while (DeadLock)
        {

            Debug.Log("Deadlock start");
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (AllDots[i, j] == null)
                    {
                        Debug.Log("DeadLock null" + i + " " + j); ;
                        //DeadLockNullReference();
                        DeadLock = false;
                        break;           
                    }
                    if (AllDots[i, j].GetComponent<Dots>().isBomb)
                    {

                        Debug.Log("DeadLock Bomb" + i + " " + j);
                        DeadLock = false;
                        break;
                    }
                    if (IsThereHorizontalMatches_ForDeadLock(i, j))
                    {

                        Debug.Log("DeadLock Horizont" + i + " " + j);
                        DeadLock = false;
                        break;
                    }
                    if (IsThereVerticalMatches_ForDeadLock(i, j))
                    {

                        Debug.Log("DeadLock Vert" + i + " " + j);
                        DeadLock = false;
                        break;
                    }
                }
            }
            //DeadLock = true;
            if (DeadLock)
            {
                Debug.Log("DEADLOCKFOUND");
                yield return new WaitForSeconds(0.5f);
                MixUpElementsAfterDeadLock();
                StopCoroutine(FillBoard());
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(FillBoard());
                // StartCoroutine(FillBoard());
            }


        }
    }
    public void DetectDeadLock()
    {
        Debug.Log("Deadlock start");
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (AllDots[i, j] == null)
                {
                    Debug.Log("DeadLock null" + i + " " + j); ;
                    //DeadLockNullReference();
                    return;
                }
                if (AllDots[i, j].GetComponent<Dots>().isBomb)
                {

                    Debug.Log("DeadLock Bomb" + i + " " + j);
                    return;
                }
                if (IsThereHorizontalMatches_ForDeadLock(i, j))
                {

                    Debug.Log("DeadLock Horizont" + i + " " + j);
                    return;
                }
                if (IsThereVerticalMatches_ForDeadLock(i, j))
                {

                    Debug.Log("DeadLock Vert" + i + " " + j);
                    return;
                }
            }
        }
        Debug.Log("DEADLOCKFOUND");
        // MixUpElementsAfterDeadLock();
        //StartCoroutine(FillBoard());
        // StartCoroutine(FillBoard());

    }

    private bool IsThereHorizontalMatches_ForDeadLock(int i, int j)
    {
        /*        j -= 2;
                if (AllDots[i, j] == null)
                {
                    return false;
                }
                if (AllDots[i - 2, j] == null)
                {
                    return false;
                }*/
        if (i +1 < width)
        {
            // X X ?
            
            if (AllDots[i, j].tag == AllDots[i + 1, j].tag)
            {
                if (i + 2 < width)
                {

                    //X X O X
                    Debug.Log("i - " + i + " j - " + j);
                    if (i + 3 < width && AllDots[i + 3, j].tag == AllDots[i, j].tag)
                    {
                        return true;
                    }
                    //X X O
                    //O O X
                    if (i + 2 < width)
                    {
                        Debug.Log("i - " + i + " j - " + j);
                        if (j - 1 >= 0 && AllDots[i + 2, j - 1].tag == AllDots[i, j].tag)
                        {
                            return true;
                        }
                        //O O X
                        //X X O
                        Debug.Log("i - " + i + " j - " + j);
                        if (j + 1 < height && AllDots[i + 2, j + 1].tag == AllDots[i, j].tag)
                        {
                            return true;
                        }
                    }
                }

                //O X X
                if (i - 1 >= 0)
                {
                    if (j - 1 >= 0)
                    {
                        //O X X
                        //X O O 
                        Debug.Log("i - " + i + " j - " + j);
                        if (AllDots[i - 1, j - 1].tag == AllDots[i, j].tag)
                        {
                            return true;
                        }
                    }
                    if (j + 1 < height)
                    {
                        //O X X
                        //X O O 
                        Debug.Log("i - " + i + " j - " + j);
                        if (AllDots[i - 1, j + 1].tag == AllDots[i, j].tag)
                        {
                            return true;
                        }
                    }
                }
                if (i - 2 >= 0)
                {
                    //X O X X
                    Debug.Log("i - " + i + " j - " + j);
                    if (AllDots[i - 2, j].tag == AllDots[i, j].tag)
                    {
                        return true;
                    }
                }

            }
            //X O X
            //O ? O
            if(i + 2 < width)
            {
                if (AllDots[i, j].tag == AllDots[i + 2, j].tag)
                {
                    if (j - 1 >= 0)
                    {
                        //X O X
                        //O X O
                        Debug.Log("i - " + i + " j - " + j);
                        if (AllDots[i + 1, j - 1].tag == AllDots[i, j].tag)
                        {
                            return true;
                        }
                    }
                    if (j + 1 < height)
                    {
                        //O X O
                        //X O X
                        Debug.Log("i - " + i + " j - " + j);
                        if (AllDots[i + 1, j + 1].tag == AllDots[i, j].tag)
                        {
                            return true;
                        }
                    }
                }
            }
            
        }
        
        

            return false;
    }
    private bool IsThereVerticalMatches_ForDeadLock(int i, int j)
    {
        
            //?
            //X
            //X

            if (j+1 < height && AllDots[i, j + 1].tag == AllDots[i, j].tag)
            {

                if (j + 2 < height)
                {
                    //O X
                    //O
                    //X
                    //X
                    if (i + 1 < height)
                    {
                        Debug.Log("i - " + i + " j - " + j);
                        if (AllDots[i + 1, j + 2].tag == AllDots[i, j].tag)
                        {
                            return true;
                        }
                    }
                    //X O
                    //O
                    //X
                    //X
                    if (i - 1 >= 0)
                    {
                        Debug.Log("i - " + i + " j - " + j);
                        if (AllDots[i - 1, j + 2].tag == AllDots[i, j].tag)
                        {
                            return true;
                        }
                    }
                }

            //X
            //O
            //X
            //X
            Debug.Log("BUGED HERE ugh i - " + i + " j - " + j);
            if (j + 3 < height)
                {
                Debug.Log("BUGED HERE tooo i - " + i + " j - " + j);
                if (AllDots[i, j + 3].tag == AllDots[i, j].tag)
                    {
                        return true;
                    }
                }

                if (j - 1 >= 0)
                {
                //X
                //X
                //O
                //X
                Debug.Log("BUGED HERE as well i - " + i + " j - " + j);
                if (j - 2 >= 0 && AllDots[i, j - 2].tag == AllDots[i, j].tag)
                    {
                        return true;
                    }

                    if (i -1 >= 0)
                    {
                        //X
                        //X
                      //X O
                        Debug.Log("BUGED HERE WTF i - " + i + " j - " + j);
                        if (AllDots[i - 1, j - 1].tag == AllDots[i, j].tag)
                        {
                            return true;
                        }
                    }
                    if (i+1 < width)
                    {
                        //X
                        //X
                        //O X
                        Debug.Log("i - " + i + " j - " + j);
                        if (AllDots[i + 1, j - 1].tag == AllDots[i, j].tag)
                        {
                            return true;
                        }
                    }

                }

            //X
            //O
            //X
            if (j+2 < height && AllDots[i, j + 2].tag == AllDots[i, j].tag)
            {
                if (i + 1 < height)
                {
                    Debug.Log("i - " + i + " j - " + j);
                    if (AllDots[i + 1, j + 1].tag == AllDots[i, j].tag)
                    {
                        return true;
                    }
                }
                if (i - 1 >= 0)
                {
                    Debug.Log("i - " + i + " j - " + j);
                    if (AllDots[i - 1, j + 1].tag == AllDots[i, j].tag)
                    {
                        return true;
                    }
                }
            }

        }
        
        

            return false;
    }
    }
