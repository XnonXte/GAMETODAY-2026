using UnityEngine;
using Ami.BroAudio;
using System.Collections.Generic;

public enum SoundType
{
    BGM_Menu,
    BGM_Gameplay,
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField] private SoundID soundBGM1;
    [SerializeField] private SoundID soundBGM2;

    private SoundType? currentBGM;

    private Dictionary<SoundType, SoundID> soundMappingDictionary;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        soundMappingDictionary = new Dictionary<SoundType, SoundID>
        {
            { SoundType.BGM_Menu, soundBGM1 },
            { SoundType.BGM_Gameplay, soundBGM2 },
        };
    }

    private void Start()
    {
        if (GameSceneManager.instance != null)
        {
            UpdateBGM(GameSceneManager.instance.CurrentScene);
        }
    }

    public void UpdateBGM(string sceneName)
    {
        SoundType targetBGM;

        if (sceneName == GameScene.MainMenu.ToString())
        {
            targetBGM = SoundType.BGM_Menu;
        }
        else
        {
            targetBGM = SoundType.BGM_Gameplay;
        }

        if (currentBGM.HasValue && currentBGM.Value == targetBGM) return;

        StopAllBGM();
        PlayAudio(targetBGM);

        currentBGM = targetBGM;
    }

    private void StopAllBGM()
    {
        StopAudio(SoundType.BGM_Menu);
        StopAudio(SoundType.BGM_Gameplay);
    }

    public void PlayAudio(SoundType sound)
    {
        if (soundMappingDictionary.TryGetValue(sound, out SoundID id))
        {
            BroAudio.Play(id);
        }
        else
        {
            Debug.LogWarning($"SoundType '{sound}' gk ada.");
        }
    }

    public void StopAudio(SoundType sound)
    {
        if (soundMappingDictionary.TryGetValue(sound, out SoundID id))
        {
            BroAudio.Stop(id);
        }
    }
}