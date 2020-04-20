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
    [SerializeField] Image winPanel;
    [SerializeField] TMPro.TMP_Text winText;
    [SerializeField] float fadeTime = 2f;

    GameObject player;
    GameObject playerCamera;
    public GameObject dancingScene;

    void Start() {
        duckWinCount = 3;//spawner.childCount;
        player = GameObject.Find("DuckPlayer");
        playerCamera = GameObject.Find("DuckCamera");
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
            StartCoroutine(WinCorroutine());
            //SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator WinCorroutine() {
        winPanel.gameObject.SetActive(true);
        winText.gameObject.SetActive(false);
        for (float t = 0.0f; t < 1;) {
            t += Time.deltaTime;
            winPanel.color = new Color(0f, 0f, 0f, t / (1));
            yield return null;
        }
        dancingScene.SetActive(true);
        player.SetActive(false);
        playerCamera.SetActive(false);
        for (float t = 1.0f; t > 0.0f;) {
            t -= Time.deltaTime;
            winPanel.color = new Color(0f, 0f, 0f, t / (1));
            //winText.color = new Color(239f, 184f, 16f, t / (-fadeTime));
            yield return null;
        }
        winText.gameObject.SetActive(true);
        for (float t = 0.0f; t < 10f;) {
            t += Time.deltaTime;
            yield return null;
        }
        dancingScene.SetActive(false);
        winPanel.gameObject.SetActive(false);
        playerCamera.SetActive(true);
        player.SetActive(true);
    }

    public void GameOver() {
        StartCoroutine(GameOverCorroutine());
    }

    IEnumerator GameOverCorroutine() {
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
