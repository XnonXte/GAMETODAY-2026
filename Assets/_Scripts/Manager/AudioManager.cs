using UnityEngine;
using Ami.BroAudio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    #region [Music/BGM]
    [Header("Music/BGM")]
    public SoundID BGM_Menu;
    public SoundID BGM_Gameplay;
    public SoundID BGM_Gameplay4;
    public SoundID BGM_Gameplay5_7;
    #endregion

    #region [Consumeable/Weapon SFX]
    [Header("Consumeable SFX")]
    public SoundID SFX_Coin;
    public SoundID SFX_Purchase;
    public SoundID SFX_Hammer;
    public SoundID SFX_Heal;
    public SoundID SFX_PowerUp;
    public SoundID SFX_Speed;

    [Header("Weapon SFX")]
    public SoundID SFX_ActivateAbility;
    #endregion

    #region [Entity SFX]
    [Header("Entity SFX")]
    public SoundID SFX_Melee;
    public SoundID SFX_Hit;
    #endregion

    #region [UI SFX]
    [Header("UI")]
    public SoundID UI_Click;
    public SoundID UI_Hover;
    public SoundID UI_Start;
    #endregion

    private SoundID currentBGM;
    private bool hasCurrentBGM;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (GameSceneManager.Instance != null)
        {
            UpdateBGM(GameSceneManager.Instance.CurrentScene);
        }
    }

    public void UpdateBGM(string sceneName)
    {
        SoundID targetBGM;

        if (sceneName == GameScene.MainMenu.ToString() || sceneName == GameScene.CutsceneIntro.ToString())
        {
            targetBGM = BGM_Menu;
        }
        else if (sceneName == GameScene.Stage1.ToString() || sceneName == GameScene.Stage2.ToString() ||  sceneName == GameScene.Stage3.ToString())
        {
            targetBGM = BGM_Gameplay;
        }
        else if (sceneName == GameScene.Stage4.ToString())
        {
            targetBGM = BGM_Gameplay4;
        }
        else
        {
            targetBGM = BGM_Gameplay5_7;
        }

        if (hasCurrentBGM && currentBGM.Equals(targetBGM)) return;

        StopAllBGM();
        BroAudio.Play(targetBGM);
        Debug.Log(targetBGM);
        currentBGM = targetBGM;
    }

    private void StopAllBGM()
    {
        StopAudio(BGM_Menu);
        StopAudio(BGM_Gameplay);
    }

    public void PlayAudio(SoundID sound) =>  BroAudio.Play(sound);
    public void StopAudio(SoundID sound) => BroAudio.Stop(sound);
}