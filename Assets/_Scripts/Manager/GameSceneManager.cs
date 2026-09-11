using EasyTransition;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameScene
{
    MainMenu,
    CutsceneIntro,
    Stage1, //cutscene demon lord mati
    Stage2, //lorong
    Stage3, //zona lava
    Stage4, //goa
    Stage5, //outdor goa
    Stage6, //desa
    Stage7, //perbatasan
    CutsceneOutro
}

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private TransitionSettings transitionMode;
    [SerializeField] private float loadDelay;

    public static GameSceneManager Instance { get; private set; }

    public string CurrentScene => SceneManager.GetActiveScene().name;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
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
        
    }

    public void ChangeScene(GameScene sceneEnum)
    {
        //if (isChangingScene) return;
        TransitionManager.Instance().Transition(sceneEnum.ToString(), transitionMode, loadDelay);
        AudioManager.Instance.UpdateBGM(sceneEnum.ToString());
        //StartCoroutine(ChangeSceneRoutine(sceneEnum.ToString()));
    }

    public void RestartScene()
    {
        //if (isChangingScene) return;
        TransitionManager.Instance().Transition(SceneManager.GetActiveScene().name, transitionMode, loadDelay);
        //StartCoroutine(ChangeSceneRoutine(SceneManager.GetActiveScene().name));
    }

    //private IEnumerator ChangeSceneRoutine(string sceneName)
    //{
    //    isChangingScene = true;
    //    Time.timeScale = 1f;


    //    yield return SceneManager.LoadSceneAsync(sceneName); // Start the panel in the center, covering the new scene.
    
    //    transitionPanel.anchoredPosition = Vector2.zero; // Slide LEFT to reveal the new scene.
    //    yield return transitionPanel .DOAnchorPosX(-slideDistance, transitionDuration).SetEase(Ease.OutCubic).SetUpdate(true).WaitForCompletion();

    //    isChangingScene = false;
    //}
}