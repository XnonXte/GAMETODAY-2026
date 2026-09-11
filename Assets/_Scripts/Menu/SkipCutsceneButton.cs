using UnityEngine;

public class SkipButton : MonoBehaviour
{
    [SerializeField] private GameScene gameScene;

    public void SkipButtonClick()
    {
        GameSceneManager.Instance.ChangeScene(gameScene);
    }
}
