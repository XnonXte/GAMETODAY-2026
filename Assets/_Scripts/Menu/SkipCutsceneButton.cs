using UnityEngine;

public class SkipButton : MonoBehaviour
{
    [SerializeField] private GameScene gameScene;

    public void SkipButtonClick()
    {
        AudioManager.Instance.StopAllCutsceneSFX();
        GameSceneManager.Instance.ChangeScene(gameScene);
    }
}
