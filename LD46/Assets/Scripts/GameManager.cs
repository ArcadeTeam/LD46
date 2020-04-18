using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { MAIN_MENU, RUNNING, PAUSE }
    private GameState state = GameState.MAIN_MENU;
    //public AudioManager audioManager;

    public Transform spawner;
    private int duckWinCount;
    private bool win = false;

    void Start()
    {
        duckWinCount = spawner.childCount;
    }

    void Update()
    {
        
    }

    public void CheckWinCondition(int babyCount)
    {
        Debug.Log("BabyCount: " + babyCount.ToString());
        Debug.Log("duckWinCount: " + duckWinCount.ToString());

        if (!win && babyCount >= duckWinCount)
        {
            win = true;
            Debug.Log("WIN!!");
        }
    }
}
