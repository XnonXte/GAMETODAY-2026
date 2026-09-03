using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.VisualScripting;

public enum GameScene
{
    MainMenu,
    Stage1, //cutscene demon lord mati
    Stage2, //lorong
    Stage3, //zona lava
    Stage4, //goa
    Stage5 
}

public class GameSceneManager : MonoBehaviour
{
    [Header("Transition")]
    [SerializeField] private RectTransform transitionPanel; 
    [SerializeField] private float transitionDuration = 0.5f; 
    [SerializeField] private float slideDistance = 1920f;

    private bool isChangingScene;

    public static GameSceneManager instance { get; private set; }

    public string CurrentScene => SceneManager.GetActiveScene().name;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {

    }

    private void Start()
    {
        transitionPanel.anchoredPosition = Vector2.zero; 
        transitionPanel.DOAnchorPosX(-slideDistance, transitionDuration).SetEase(Ease.OutCubic).SetUpdate(true);
    }

    public void ChangeScene(GameScene sceneEnum)
    {
        if (isChangingScene) return;

        StartCoroutine(ChangeSceneRoutine(sceneEnum.ToString()));
    }

    public void RestartScene()
    {
        if (isChangingScene) return;

        StartCoroutine(ChangeSceneRoutine(SceneManager.GetActiveScene().name));
    }

    private IEnumerator ChangeSceneRoutine(string sceneName)
    {
        isChangingScene = true;
        Time.timeScale = 1f;

        transitionPanel.anchoredPosition = new Vector2(slideDistance, 0f); 
        yield return transitionPanel.DOAnchorPosX(0f, transitionDuration).SetEase(Ease.OutCubic).SetUpdate(true).WaitForCompletion();
        yield return SceneManager.LoadSceneAsync(sceneName); // Start the panel in the center, covering the new scene.
        transitionPanel.anchoredPosition = Vector2.zero; // Slide LEFT to reveal the new scene.
        yield return transitionPanel .DOAnchorPosX(-slideDistance, transitionDuration).SetEase(Ease.OutCubic).SetUpdate(true).WaitForCompletion();

        isChangingScene = false;
    }
}