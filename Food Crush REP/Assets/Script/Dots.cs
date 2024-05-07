using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Dots : MonoBehaviour
{
    [Header("DotsPositioning")]
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    [SerializeField] private int targetX;
    [SerializeField] private int targetY;
    public FindMatches findMatches;
    private PassingLevel passinglevel;
    private IDots idots;
    public Board board;
    public GameObject otherDot;
    private Vector2 FirstTouchPos;
    private Vector2 LastTouchPos;
    private Vector2 TargetPostion;
    public bool isMatched = false;
    private Vector2 tempPosition;

    [Header("Swiping")]
    public int SwipeDirection;
    public float SwipeResit = 1f;

    [Header("Bombs")]
    public bool isColorBomb;
    public bool isCollumnBomb;
    public bool isRowBomb;
    public bool isLTBomb;
    public bool isBomb;
    public GameObject rowArrow;
    public GameObject collumnArrow;
    public GameObject colorBomb;
    public GameObject ltBomb;
    public NormalDot normaldot;
    //Debug
    public int DebugMouseAction = 0;
    void Start()
    {
        isCollumnBomb = false;
        isRowBomb = false;
        isColorBomb = false;
        isLTBomb = false;
        isBomb = false;
        findMatches = FindObjectOfType<FindMatches>();
        board = FindObjectOfType<Board>();
        normaldot = GetComponent<NormalDot>();
        passinglevel = FindObjectOfType<PassingLevel>();
        //targetX=(int)this.transform.position.x;
        //targetY=(int)this.transform.position.y;
        //row = targetY;
        //column = targetX;
        /*         previousColumn = column;*/
        previousRow = row + 1;
    }
    //Debuging
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            switch (DebugMouseAction)
            {
                case 0:
                    isMatched = true;
                    StartCoroutine(DelayTheTurnBlac());
                    board.DeadLock = true;
                    board.DestroyMatches();
                    break;
                case 1:
                    normaldot.MakeRowBomb();
                    break;
                case 2:
                    normaldot.MakeColumnBomb();
                    break;
                case 3:
                    normaldot.MakeLTBomb();
                    break;
                case 4:
                    normaldot.MakeColorBomb();
                    break;
            }
            //ROWBOMB SPAWN
            //MakeRowBomb();
            //LTBOMB SPAWN
            //ColorBOmbSPAWN

            //Dot Removel
            /*    isMatched = true;
                StartCoroutine(DelayTheTurnBlac());
                board.DestroyMatches();*/
        }
    }
    void Update()
    {
        targetX = column;
        targetY = row;

        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {

            //MoveTowards
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 10 * Time.deltaTime);
            if (board.AllDots[column, row] != this.gameObject)
            {
                board.AllDots[column, row] = this.gameObject;
            }
            findMatches.StartFindAllMatchesCoroutine();
        }
        else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.AllDots[column, row] = this.gameObject;

        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {

            //MoveTowards
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 10 * Time.deltaTime);
            if (board.AllDots[column, row] != this.gameObject)
            {
                board.AllDots[column, row] = this.gameObject;
            }
            findMatches.StartFindAllMatchesCoroutine();
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.AllDots[column, row] = this.gameObject;

        }

    }

    private void OnMouseDown()
    {
        if (board.CurentState == GameState.move)
        {

            FirstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

    }
    private void OnMouseUp()
    {
        if (board.CurentState == GameState.move)
        {
            LastTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateSwipeDirection();

        }

    }
    private void CalculateSwipeDirection()
    {
        Vector2 SwipeDer = (LastTouchPos - FirstTouchPos);
        float posx = Mathf.Abs(SwipeDer.x);
        float posy = Mathf.Abs(SwipeDer.y);
        if (Mathf.Abs(LastTouchPos.x - FirstTouchPos.x) > SwipeResit || Mathf.Abs(LastTouchPos.y - FirstTouchPos.y) > SwipeResit)
        {


            if (posx > posy)
            {
                SwipeDirection = (SwipeDer.x > 0) ? SwipeDirection = 0 : SwipeDirection = 1;

                MovePieciesHorizontal(SwipeDirection);

            }
            else
            {
                SwipeDirection = (SwipeDer.y > 0) ? SwipeDirection = 2 : SwipeDirection = 3;
                MovePieciesVertical(SwipeDirection);

            }
            if (otherDot != null)
            {
                board.CurentState = GameState.wait;
                board.currentDot = this;
            }

        }
        else
        {
            board.CurentState = GameState.move;

        }

    }
    private void MovePieciesHorizontal(int Direction)
    {

        if (Direction == 0 && column < board.width - 1)//Right
        {

            otherDot = board.AllDots[column + 1, row];
            if (!isBomb || !otherDot.GetComponent<Dots>().isBomb)
            {
                previousColumn = column;
                previousRow = row;
                otherDot.GetComponent<Dots>().column -= 1;
                column += 1;
            }
            else
            {
                previousColumn = column;
                previousRow = row;
                column += 1;
            }

        }
        else if (Direction == 1 && column > 0)//Left
        {

            otherDot = board.AllDots[column - 1, row];
            if (!isBomb || !otherDot.GetComponent<Dots>().isBomb)
            {
                previousColumn = column;
                previousRow = row;
                otherDot.GetComponent<Dots>().column += 1;
                column -= 1;
            }
            else
            {
                previousColumn = column;
                previousRow = row;
                column -= 1;
            }

        }

        StartCoroutine(CheckMoveCor());


    }
    private void MovePieciesVertical(int Direction)
    {
        if (Direction == 2 && row < board.height - 1) //Up
        {
            otherDot = board.AllDots[column, row + 1];
            if (!isBomb || !otherDot.GetComponent<Dots>().isBomb)
            {
                previousColumn = column;
                previousRow = row;
                otherDot.GetComponent<Dots>().row -= 1;
                row += 1;
            }
            else
            {
                previousColumn = column;
                previousRow = row;
                row += 1;
            }


        }
        else if (Direction == 3 && row > 0)//Down
        {
            otherDot = board.AllDots[column, row - 1];
            if (!isBomb || !otherDot.GetComponent<Dots>().isBomb)
            {
                previousColumn = column;
                previousRow = row;
                otherDot.GetComponent<Dots>().row += 1;
                row -= 1;

            }
            else
            {
                previousColumn = column;
                previousRow = row;
                row -= 1;
            }


        }
        StartCoroutine(CheckMoveCor());


    }
    public IEnumerator DelayTheTurnBlac()
    {
        yield return new WaitForSeconds(0.3f);
        /*    SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(0f, 0f, 0f, .2f);*/
    }
    public void SetMatchTrue()
    {
        if (!isMatched)
        {
            IDots dotsComponent = GetComponent<IDots>();
            if (dotsComponent != null)
            {
                StartCoroutine(dotsComponent.IsMatched(this.GetComponent<Dots>(), null));
            }
        }
    }
    public IEnumerator CheckMoveCor()
    {
        if (otherDot != null)
        {
            board.audioPlayer.PlayElementMovedAudio();
            yield return new WaitForSeconds(0.4f);
            Dots otherDotDots = otherDot.GetComponent<Dots>();
            IDots dotsComponent = GetComponent<IDots>();
            if (dotsComponent != null)
            {
                yield return StartCoroutine(dotsComponent.IsMatched(this.GetComponent<Dots>(), otherDotDots));
            }
            yield return new WaitForSeconds(0.5f);
           // StartCoroutine(CheckIfMatched(otherDotDots));
            if (isMatched || otherDotDots.isMatched)
            {
                board.DeadLock = true;
                board.DestroyMatches();
            }
            else {
                board.currentDot = null;
                board.CurentState = GameState.move; 
            }
            /*            board.currentDot = null;
                        board.CurentState = GameState.move;*/
        }
    }
    public IEnumerator CheckIfMatched(Dots otherDotDots)
    {
        if (isMatched || otherDotDots.isMatched)
        {
            Debug.Log("Movee");
            passinglevel.MoveMade();
        }
            yield return null;
    }
    public IEnumerator DelayBeforeDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        //board.DestroyMatches();
    }
}
