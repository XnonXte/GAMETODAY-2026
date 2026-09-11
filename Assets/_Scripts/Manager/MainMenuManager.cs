using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System; // Replaced System.Resources to allow Enum parsing for scenes

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Menu Buttons")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;

    [Header("Target Scene")]
    [SerializeField] private GameScene targetScene = GameScene.CutsceneIntro;

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

        if (continueButton != null)
        {
            continueButton.interactable = SaveManager.HasSave();
        }
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
    /// Button event to start a completely fresh game.
    /// </summary>
    public void ButtonNewGame()
    {
        PlayClickSFX();

        // 1. Wipe old disk saves
        SaveManager.ClearSave();

        // 2. Wipe active RAM session and Gold
        if (GameSessionManager.Instance != null) GameSessionManager.Instance.ResetSession();
        GameResource.ResetGold();

        // 3. Load the target scene (Intro Cutscene)
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.ChangeScene(targetScene);
        }
        else
        {
            Debug.LogWarning("[MainMenuManager] GameSceneManager Instance is null!");
        }
    }

    /// <summary>
    /// Button event to continue from the last saved level.
    /// </summary>
    public void ButtonContinue()
    {
        PlayClickSFX();

        if (!SaveManager.HasSave()) return;

        // 1. Load Gold directly into static script
        GameResource.SetGoldAmount(PlayerPrefs.GetInt("Gold", 0));

        // 2. Load Items from the Resources/Items folder
        BaseItemSO savedConsumable = SaveManager.LoadItem("Consumable");
        BaseItemSO savedWeapon = SaveManager.LoadItem("Weapon");

        // 3. Inject disk data into the RAM Session Manager
        if (GameSessionManager.Instance != null)
        {
            GameSessionManager.Instance.SaveLevelData(
                PlayerPrefs.GetFloat("PlayerHP", -1f),
                PlayerPrefs.GetFloat("PayloadHP", -1f),
                savedConsumable,
                savedWeapon
            );
        }

        // 4. Figure out which scene they were entering and load it
        string savedSceneStr = PlayerPrefs.GetString("SavedScene", targetScene.ToString());

        if (Enum.TryParse(savedSceneStr, out GameScene sceneToLoad))
        {
            if (GameSceneManager.Instance != null) GameSceneManager.Instance.ChangeScene(sceneToLoad);
        }
        else
        {
            if (GameSceneManager.Instance != null) GameSceneManager.Instance.ChangeScene(targetScene); // Failsafe
        }
    }

    /// <summary>
    /// Opens the Settings panel with pop up animation.
    /// </summary>
    public void OpenSettings()
    {
        PlayClickSFX();
        CloseOtherPanels(settingsPanel);
        HideMainMenu();
        AnimatePanelOpen(settingsPanel, settingsRectTransform, settingsCanvasGroup);
    }

    /// <summary>
    /// Closes the Settings panel with pop up animation.
    /// </summary>
    public void CloseSettings()
    {
        PlayClickSFX();
        AnimatePanelClose(settingsPanel, settingsRectTransform, settingsCanvasGroup, ShowMainMenu);
    }

    public void ToggleSettings()
    {
        if (settingsPanel != null && settingsPanel.activeSelf) CloseSettings();
        else OpenSettings();
    }

    /// <summary>
    /// Opens the Credits panel with pop up animation.
    /// </summary>
    public void OpenCredits()
    {
        PlayClickSFX();
        CloseOtherPanels(creditsPanel);
        HideMainMenu();
        AnimatePanelOpen(creditsPanel, creditsRectTransform, creditsCanvasGroup);
    }

    /// <summary>
    /// Closes the Credits panel with pop up animation.
    /// </summary>
    public void CloseCredits()
    {
        PlayClickSFX();
        AnimatePanelClose(creditsPanel, creditsRectTransform, creditsCanvasGroup, ShowMainMenu);
    }

    public void ToggleCredits()
    {
        if (creditsPanel != null && creditsPanel.activeSelf) CloseCredits();
        else OpenCredits();
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

    #region Panel Helper

    private void CloseOtherPanels(GameObject exceptPanel)
    {
        // Passed 'null' instead of 'ShowMainMenu' here so the Main Menu doesn't glitch 
        // into view if you open Credits while Settings is already open.
        if (settingsPanel != null && settingsPanel != exceptPanel)
        {
            AnimatePanelClose(settingsPanel, settingsRectTransform, settingsCanvasGroup, null);
        }

        if (creditsPanel != null && creditsPanel != exceptPanel)
        {
            AnimatePanelClose(creditsPanel, creditsRectTransform, creditsCanvasGroup, null);
        }
    }

    private void HideMainMenu()
    {
        if (mainMenuPanel != null)
        {
            var cg = mainMenuPanel.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.DOKill();
                cg.interactable = false;
                cg.blocksRaycasts = false;
                cg.DOFade(0f, animDuration).SetEase(Ease.InQuad).SetUpdate(true).OnComplete(() =>
                {
                    mainMenuPanel.SetActive(false);
                });
            }
            else
            {
                mainMenuPanel.SetActive(false);
            }
        }
    }

    private void ShowMainMenu()
    {
        if (mainMenuPanel != null && mainMenuPanel.activeSelf == false)
        {
            var cg = mainMenuPanel.GetComponent<CanvasGroup>();
            mainMenuPanel.SetActive(true);
            if (cg != null)
            {
                cg.DOKill();
                cg.alpha = 0f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
                cg.DOFade(1f, animDuration).SetEase(Ease.OutQuad).SetUpdate(true);
            }
        }
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

    private void AnimatePanelClose(GameObject panel, RectTransform rectTransform, CanvasGroup canvasGroup, Action onComplete = null)
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
                onComplete?.Invoke();
            });
        }
        else
        {
            DOVirtual.DelayedCall(animDuration, () =>
            {
                panel.SetActive(false);
                onComplete?.Invoke();
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