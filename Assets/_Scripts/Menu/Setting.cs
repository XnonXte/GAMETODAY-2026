using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Setting : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.25f;

    private void Start()
    {
        CloseSettings();
    }

    public void StartGame()
    {
        GameSceneManager.Instance.ChangeScene(GameScene.Stage1);
    }

    public void OpenSettings()
    {
        if (settingsPanel == null) return;

        canvasGroup.DOFade(1, fadeDuration);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel == null) return;

        canvasGroup.DOFade(0, fadeDuration);
        settingsPanel.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        GameSceneManager.Instance.ChangeScene(GameScene.MainMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}