using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Ami.BroAudio;

public class Setting : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button backButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    [Header("Volume Settings")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider soundSlider;
    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.25f;

    private bool isOpen = false;
    private Coroutine fadeCoroutine;

    private void Start()
    {
        CloseSettings();

        if (volumeSlider != null)
        {
            volumeSlider.value = 1f;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
        if (soundSlider != null)
        {
            soundSlider.value = 1f;
            soundSlider.onValueChanged.AddListener(OnSoundChanged);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            CloseSettings();
        }
    }

    public void OpenSettings()
    {
        if (settingsPanel == null) return;

        settingsPanel.SetActive(true);
        canvasGroup.alpha = 0f;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeIn());
    }

    public void CloseSettings()
    {
        if (settingsPanel == null) return;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        Time.timeScale = 0f;
        isOpen = true;
    }

    private IEnumerator FadeOut()
    {
        Time.timeScale = 1f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        settingsPanel.SetActive(false);
        isOpen = false;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void OnVolumeChanged(float value)
    {
        BroAudio.SetVolume(BroAudioType.All, value);
    }

    public void OnSoundChanged(float value)
    {
        BroAudio.SetVolume(BroAudioType.SFX, value);
    }
}