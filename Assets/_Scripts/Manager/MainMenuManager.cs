using UnityEngine;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Target Scene")]
    [SerializeField] private GameScene targetScene = GameScene.Stage1;

    [Header("Animation Settings")]
    [SerializeField] private float animDuration = 0.3f;
    [SerializeField] private Ease openEase = Ease.OutBack;
    [SerializeField] private Ease closeEase = Ease.InBack;

    private CanvasGroup settingsCanvasGroup;
    private RectTransform settingsRectTransform;

    private CanvasGroup creditsCanvasGroup;
    private RectTransform creditsRectTransform;

    private void Awake()
    {
        InitPanel(settingsPanel, ref settingsRectTransform, ref settingsCanvasGroup);
        InitPanel(creditsPanel, ref creditsRectTransform, ref creditsCanvasGroup);
    }

    private void Start()
    {
        HidePanelInstantly(settingsPanel, settingsRectTransform, settingsCanvasGroup);
        HidePanelInstantly(creditsPanel, creditsRectTransform, creditsCanvasGroup);
    }

    private void InitPanel(GameObject panel, ref RectTransform rectTransform, ref CanvasGroup canvasGroup)
    {
        if (panel == null) return;

        rectTransform = panel.GetComponent<RectTransform>();
        canvasGroup = panel.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
        }
    }

    private void HidePanelInstantly(GameObject panel, RectTransform rectTransform, CanvasGroup canvasGroup)
    {
        if (panel == null) return;

        panel.transform.DOKill();
        if (rectTransform != null) rectTransform.DOKill();
        if (canvasGroup != null)
        {
            canvasGroup.DOKill();
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        if (rectTransform != null)
        {
            rectTransform.localScale = Vector3.zero;
        }

        panel.SetActive(false);
    }

    #region Button Handler Methods

    /// <summary>
    /// Button event to start playing the game (loads target scene).
    /// </summary>
    public void PlayGame()
    {
        PlayClickSFX();

        if (GameSceneManager.Instance != null)
        {
              GameSceneManager.Instance.ChangeScene(GameScene.Stage1);
        }
        else
        {
            Debug.LogWarning("[MainMenuManager] GameSceneManager Instance is null!");
        }
    }

    /// <summary>
    /// Opens the Settings panel with pop up animation.
    /// </summary>
    public void OpenSettings()
    {
        PlayClickSFX();
        AnimatePanelOpen(settingsPanel, settingsRectTransform, settingsCanvasGroup);
    }

    /// <summary>
    /// Closes the Settings panel with pop up animation.
    /// </summary>
    public void CloseSettings()
    {
        PlayClickSFX();
        AnimatePanelClose(settingsPanel, settingsRectTransform, settingsCanvasGroup);
    }

    /// <summary>
    /// Toggles the Settings panel open/close state.
    /// </summary>
    public void ToggleSettings()
    {
        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            CloseSettings();
        }
        else
        {
            OpenSettings();
        }
    }

    /// <summary>
    /// Opens the Credits panel with pop up animation.
    /// </summary>
    public void OpenCredits()
    {
        PlayClickSFX();
        AnimatePanelOpen(creditsPanel, creditsRectTransform, creditsCanvasGroup);
    }

    /// <summary>
    /// Closes the Credits panel with pop up animation.
    /// </summary>
    public void CloseCredits()
    {
        PlayClickSFX();
        AnimatePanelClose(creditsPanel, creditsRectTransform, creditsCanvasGroup);
    }

    /// <summary>
    /// Toggles the Credits panel open/close state.
    /// </summary>
    public void ToggleCredits()
    {
        if (creditsPanel != null && creditsPanel.activeSelf)
        {
            CloseCredits();
        }
        else
        {
            OpenCredits();
        }
    }

    /// <summary>
    /// Button event to exit/quit the game.
    /// </summary>
    public void ExitGame()
    {
        PlayClickSFX();
        Debug.Log("[MainMenuManager] Quitting Game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    #endregion

    #region Popup Animation Helpers

    private void AnimatePanelOpen(GameObject panel, RectTransform rectTransform, CanvasGroup canvasGroup)
    {
        if (panel == null) return;

        panel.transform.DOKill();
        if (rectTransform != null) rectTransform.DOKill();
        if (canvasGroup != null) canvasGroup.DOKill();

        panel.SetActive(true);

        if (rectTransform != null)
        {
            rectTransform.localScale = Vector3.zero;
            rectTransform.DOScale(Vector3.one, animDuration).SetEase(openEase).SetUpdate(true);
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            canvasGroup.DOFade(1f, animDuration).SetEase(Ease.OutQuad).SetUpdate(true).OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            });
        }
    }

    private void AnimatePanelClose(GameObject panel, RectTransform rectTransform, CanvasGroup canvasGroup)
    {
        if (panel == null || !panel.activeSelf) return;

        panel.transform.DOKill();
        if (rectTransform != null) rectTransform.DOKill();
        if (canvasGroup != null) canvasGroup.DOKill();

        if (canvasGroup != null)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOFade(0f, animDuration).SetEase(Ease.InQuad).SetUpdate(true);
        }

        if (rectTransform != null)
        {
            rectTransform.DOScale(Vector3.zero, animDuration).SetEase(closeEase).SetUpdate(true).OnComplete(() =>
            {
                panel.SetActive(false);
            });
        }
        else
        {
            DOVirtual.DelayedCall(animDuration, () =>
            {
                panel.SetActive(false);
            }).SetUpdate(true);
        }
    }

    private void PlayClickSFX()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAudio(AudioManager.Instance.UI_Click);
        }
    }

    #endregion
}
