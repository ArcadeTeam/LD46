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
    public int duckWinCount;
    private bool win = false;

    [SerializeField] Image gameOverPanel;
    [SerializeField] TMPro.TMP_Text gameOverText;
    [SerializeField] Image winPanel;
    [SerializeField] TMPro.TMP_Text winText;
    [SerializeField] float fadeTime = 2f;

    GameObject player;
    GameObject playerCamera;
    public GameObject dancingScene;
    public GameObject ducksInfoBubble;
    public GameObject eggsBubble;

    void Start() {
        duckWinCount = 5;//spawner.childCount;
        player = GameObject.Find("DuckPlayer");
        playerCamera = GameObject.Find("DuckCamera");
        ducksInfoBubble.SetActive(false);
        eggsBubble.SetActive(false);
        ShowBubbles(ducksInfoBubble);
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
        } else {
            ShowBubbles(ducksInfoBubble);
        }
    }

    public void ShowBubbles(GameObject bubbles) {
        IEnumerator Show() {
            bubbles.SetActive(true);
            CanvasGroup cg = bubbles.GetComponent<CanvasGroup>();
            cg.alpha = 0;
        
            for (float t = 0.0f; t < 7;) {
                t += Time.deltaTime;
                cg.alpha = t;
                yield return null;
            }
            for (float t = 1f; t > 0f;) {
                t -= Time.deltaTime;
                cg.alpha = t;
                yield return null;
            }
            bubbles.SetActive(false);
        }

        StartCoroutine(Show());
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
        SceneManager.LoadScene("Game");
    }
}
