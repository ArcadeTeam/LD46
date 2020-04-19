using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { MAIN_MENU, RUNNING, PAUSE }
    private GameState state = GameState.MAIN_MENU;
    //public AudioManager audioManager;

    public Transform spawner;
    private int duckWinCount;
    private bool win = false;

    [SerializeField] Image gameOverPanel;
    [SerializeField] TMPro.TMP_Text gameOverText;
    [SerializeField] float fadeTime = 2f;

    GameObject player;

    void Start() {
        duckWinCount = 3;//spawner.childCount;
        player = GameObject.Find("DuckPlayer");
    }

    void Update() {
        
    }

    public void CheckWinCondition(int babyCount)
    {
        Debug.Log("BabyCount: " + babyCount.ToString());
        Debug.Log("duckWinCount: " + duckWinCount.ToString());

        if (!win && babyCount >= duckWinCount)
        {
            win = true;
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void GameOver() {
        StartCoroutine(Fading());
    }

    IEnumerator Fading() {
        gameOverPanel.gameObject.SetActive(true);
        for (float t = 0.0f; t < fadeTime;) {
            t += Time.deltaTime;
            gameOverPanel.color = new Color(0f, 0f, 0f, t / (fadeTime));
            gameOverText.color = new Color(255f, 255f, 255f, t / (fadeTime));
            yield return null;
        }
        SceneManager.LoadScene("MainMenu");
    }
}
