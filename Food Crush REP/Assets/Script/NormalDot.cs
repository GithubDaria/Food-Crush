using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NormalDot : MonoBehaviour, IDots
{
    private Dots ThisDot;
    public FindMatches findMatches;
    public Board board;
    void Start()
    {
        findMatches = this.GetComponent<Dots>().findMatches;
        board = this.GetComponent<Dots>().board;
        ThisDot = this.GetComponent<Dots>();
    }
    public IEnumerator IsMatched(Dots currentdot, Dots otherdot)
    {
        if(otherdot != null)
        {
            if (otherdot.isBomb)
            {
                board.SecretScore += 15;
                IDots dotsComponent = otherdot.GetComponent<IDots>();
                if (dotsComponent != null)
                {
                    dotsComponent.IsMatched(otherdot, currentdot);
                }
            }
            else if(otherdot.isMatched || currentdot.isMatched)
            {
                board.SecretScore += 3;
                currentdot.isMatched = true;
                board.DeadLock = true;
                board.DestroyMatches();
            }
            else
            {
                FailledSwap(currentdot, otherdot);
            }
        }
        else
        {
            currentdot.isMatched = true;
        }
        yield return null;
    }
    private void FailledSwap(Dots currentdot, Dots otherdot)
    {
        otherdot.row = currentdot.row;
        otherdot.column = currentdot.column;
        currentdot.row = currentdot.previousRow;
        currentdot.column = currentdot.previousColumn;
    }
    public void MakeRowBomb()
    {
        ThisDot.isRowBomb = true;
        ThisDot.isBomb = true;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        GameObject arrow = Instantiate(ThisDot.rowArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;
        this.gameObject.tag = "ArrowBomb";
        this.AddComponent<RowArrowBomb>();
        Destroy(this.GetComponent<NormalDot>());
    }
    public void MakeColumnBomb()
    {
        ThisDot.isCollumnBomb = true;
        ThisDot.isBomb = true;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        GameObject arrow = Instantiate(ThisDot.collumnArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;
        this.gameObject.tag = "ArrowBomb";
        this.AddComponent<CollumnArrowBomb>();
        Destroy(this.GetComponent<NormalDot>());

    }
    public void MakeColorBomb()
    {
        ThisDot.isColorBomb = true;
        ThisDot.isBomb = true;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        GameObject color = Instantiate(ThisDot.colorBomb, transform.position, Quaternion.identity);
        color.transform.parent = this.transform;
        this.gameObject.tag = "ColorBomb";
        this.AddComponent<ColorBomb>();
        Destroy(this.GetComponent<NormalDot>());
    }
    public void MakeLTBomb()
    {
        ThisDot.isLTBomb = true;
        ThisDot.isBomb = true;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        GameObject ltbomb = Instantiate(ThisDot.ltBomb, transform.position, Quaternion.identity);
        ltbomb.transform.parent = this.transform;
        this.gameObject.tag = "LtBomb";
        this.AddComponent<LtBombClass>();
        Destroy(this.GetComponent<NormalDot>());

    }
}
