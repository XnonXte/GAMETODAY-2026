using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    public void ButtonReturnToMainMenu()
    {
        GameSceneManager.Instance.ChangeScene(GameScene.MainMenu);
    }

    public void ButtonRestartScene()
    {
        GameSceneManager.Instance.RestartScene();
    }
}
