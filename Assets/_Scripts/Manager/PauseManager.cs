using UnityEngine;
using DG.Tweening;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseButton;

    [Header("Input Settings")]
    [SerializeField] private bool allowKeyboardToggle = true;
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

    [Header("Animation Settings")]
    [SerializeField] private float animDuration = 0.3f;
    [SerializeField] private Ease openEase = Ease.OutBack;
    [SerializeField] private Ease closeEase = Ease.InBack;

    private CanvasGroup pauseCanvasGroup;
    private RectTransform pauseRectTransform;
    private bool isPaused = false;

    public bool IsPaused => isPaused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        InitPanel();
    }

    private void Start()
    {
        HidePanelInstantly();
    }

    private void Update()
    {
        if (allowKeyboardToggle && Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    private void InitPanel()
    {
        if (pausePanel == null) return;

        pauseRectTransform = pausePanel.GetComponent<RectTransform>();
        pauseCanvasGroup = pausePanel.GetComponent<CanvasGroup>();

        if (pauseCanvasGroup == null)
        {
            pauseCanvasGroup = pausePanel.AddComponent<CanvasGroup>();
        }
    }

    private void HidePanelInstantly()
    {
        if (pausePanel == null) return;

        pausePanel.transform.DOKill();
        if (pauseRectTransform != null) pauseRectTransform.DOKill();
        if (pauseCanvasGroup != null)
        {
            pauseCanvasGroup.DOKill();
            pauseCanvasGroup.alpha = 0f;
            pauseCanvasGroup.interactable = false;
            pauseCanvasGroup.blocksRaycasts = false;
        }

        if (pauseRectTransform != null)
        {
            pauseRectTransform.localScale = Vector3.zero;
        }

        pausePanel.SetActive(false);
    }

    #region Public Button Callbacks

    /// <summary>
    /// Call this when pressing the Pause button in game UI.
    /// </summary>
    public void PauseGame()
    {
        if (isPaused) return;

        PlayClickSFX();
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseButton != null)
        {
            pauseButton.SetActive(false);
        }

        AnimatePanelOpen();
    }

    /// <summary>
    /// Button handler for "Continue" / "Resume" in the Pause Panel.
    /// </summary>
    public void ResumeGame()
    {
        if (!isPaused) return;

        PlayClickSFX();
        isPaused = false;
        Time.timeScale = 1f;

        AnimatePanelClose(() =>
        {
            if (pauseButton != null)
            {
                pauseButton.SetActive(true);
            }
        });
    }

    /// <summary>
    /// Alternative button handler alias for Continue.
    /// </summary>
    public void ContinueGame()
    {
        ResumeGame();
    }

    /// <summary>
    /// Button handler for "Retry" / "Restart" in the Pause Panel.
    /// </summary>
    public void RetryGame()
    {
        PlayClickSFX();
        Time.timeScale = 1f;
        isPaused = false;

        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.RestartScene();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }
    }

    /// <summary>
    /// Button handler for "Go to Main Menu" in the Pause Panel.
    /// </summary>
    public void GoToMainMenu()
    {
        PlayClickSFX();
        Time.timeScale = 1f;
        isPaused = false;

        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.ChangeScene(GameScene.MainMenu);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }

    /// <summary>
    /// Toggles pause state. Useful for keyboard shortcut (ESC).
    /// </summary>
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    #endregion

    #region Popup Animations (DOTween)

    private void AnimatePanelOpen()
    {
        if (pausePanel == null) return;

        pausePanel.transform.DOKill();
        if (pauseRectTransform != null) pauseRectTransform.DOKill();
        if (pauseCanvasGroup != null) pauseCanvasGroup.DOKill();

        pausePanel.SetActive(true);

        if (pauseRectTransform != null)
        {
            pauseRectTransform.localScale = Vector3.zero;
            pauseRectTransform.DOScale(Vector3.one, animDuration).SetEase(openEase).SetUpdate(true);
        }

        if (pauseCanvasGroup != null)
        {
            pauseCanvasGroup.alpha = 0f;
            pauseCanvasGroup.interactable = false;
            pauseCanvasGroup.blocksRaycasts = false;

            pauseCanvasGroup.DOFade(1f, animDuration).SetEase(Ease.OutQuad).SetUpdate(true).OnComplete(() =>
            {
                pauseCanvasGroup.interactable = true;
                pauseCanvasGroup.blocksRaycasts = true;
            });
        }
    }

    private void AnimatePanelClose(System.Action onComplete = null)
    {
        if (pausePanel == null || !pausePanel.activeSelf)
        {
            onComplete?.Invoke();
            return;
        }

        pausePanel.transform.DOKill();
        if (pauseRectTransform != null) pauseRectTransform.DOKill();
        if (pauseCanvasGroup != null) pauseCanvasGroup.DOKill();

        if (pauseCanvasGroup != null)
        {
            pauseCanvasGroup.interactable = false;
            pauseCanvasGroup.blocksRaycasts = false;
            pauseCanvasGroup.DOFade(0f, animDuration).SetEase(Ease.InQuad).SetUpdate(true);
        }

        if (pauseRectTransform != null)
        {
            pauseRectTransform.DOScale(Vector3.zero, animDuration).SetEase(closeEase).SetUpdate(true).OnComplete(() =>
            {
                pausePanel.SetActive(false);
                onComplete?.Invoke();
            });
        }
        else
        {
            DOVirtual.DelayedCall(animDuration, () =>
            {
                pausePanel.SetActive(false);
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
