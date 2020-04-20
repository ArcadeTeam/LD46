using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] float fadeTime = 2f;
    [SerializeField] float cinematicTime = 20f;
    [SerializeField] GameObject customizationPanel;
    public Image fadePanel;
    public GameObject m_panelMenu;

    public Scrollbar masterSlider;
    public Scrollbar musicSlider;
    public Scrollbar effectsSlider;
    public Scrollbar sensibilitySlider;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public TMPro.TMP_Dropdown qualityDropdown;
    public Toggle fullscreen;

    public SplineController splineController;

    float counter = 72f;

    GameManager gameManager;
    Settings settings;

    AudioSource[] bgMusic;

    void Start()
    {
        fadePanel.gameObject.SetActive(true);
        StartCoroutine(UnFading());

        gameManager = FindObjectOfType<GameManager>();
        settings = FindObjectOfType<Settings>();
        bgMusic = FindObjectsOfType<AudioSource>();

        masterSlider.value = settings.masterVolume;
        effectsSlider.value = settings.effectsVolume;
        musicSlider.value = settings.musicVolume;
        sensibilitySlider.value = settings.sensibility;
        fullscreen.isOn = settings.fullscreen;

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(settings.resolutionsNames);
        resolutionDropdown.value = settings.initialResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        qualityDropdown.value = settings.quality;
    }

    public void SetMasterVolume(float value) { settings.SetMasterVolume(value); }
    public void SetEffectsVolume(float value) { settings.SetEffectsVolume(value); }
    public void SetMusicVolume(float value) { settings.SetMusicVolume(value); }
    public void SetSensibility(float value) { settings.SetSensibility(value); }
    public void SetQuality(int value) { settings.SetQuality(value); }
    public void SetFullscreen(bool value) { settings.SetFullscreen(value); }
    public void SetResolution(int value) { settings.SetResolution(value); }

    public void Ready() { SceneManager.LoadScene("Game"); }

    public void NextSuit() { GameObject.Find("DuckPlayer").GetComponent<Customization>().NextItem(); }

    void Update() {

    }

    public void Play() {
        GameObject.Find("Logo").SetActive(false);
        m_panelMenu.SetActive(false);
        splineController.FollowSpline();
        StartCoroutine(ShowCustomization());
    }

    public void Quit() { Application.Quit();}

    IEnumerator ShowCustomization() {
        for (float t = 0.0f; t < cinematicTime;) {
            t += Time.deltaTime;
            yield return null;
        }
        customizationPanel.SetActive(true);
    }

    IEnumerator Fading()
    {
        for (float t = 0.0f; t < fadeTime;)
        {
            t += Time.deltaTime;
            fadePanel.color = new Color(0f, 0f, 0f, t / (fadeTime));
            yield return null;
        }
    }
    IEnumerator UnFading()
    {
        for (float t = fadeTime; t > 0.0f;)
        {
            t -= Time.deltaTime;
            fadePanel.color = new Color(0f, 0f, 0f, t / (fadeTime));
            yield return null;
        }
        fadePanel.gameObject.SetActive(false);
    }
}
