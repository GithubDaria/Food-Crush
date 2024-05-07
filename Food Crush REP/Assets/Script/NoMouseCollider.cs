using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class NoMouseCollider : MonoBehaviour
{
    public Board board;

    public void PauseGame()
    {
        board.CurentState = GameState.wait;
    }
    public void UnPauseGame()
    {
        board.CurentState = GameState.move;
    }
    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
}
