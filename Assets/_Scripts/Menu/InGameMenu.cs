using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private Slider playerHealth;
    [SerializeField] private Slider payloadHealth;
    [SerializeField] private Image weaponSlot;
    [SerializeField] private Image consumeableSlot;
    [SerializeField] private TMP_Text goldCounter;

    private void OnEnable()
    {
        EventHandler.OnGoldAmountChanged += UpdateGoldCounter;
        EventHandler.OnInventoryUpdated += UpdateInventoryUI;
        EventHandler.OnPlayerHealthChanged += UpdatePlayerHealth;
    }

    private void OnDisable()
    {
        EventHandler.OnGoldAmountChanged -= UpdateGoldCounter;
        EventHandler.OnInventoryUpdated -= UpdateInventoryUI;
        EventHandler.OnPlayerHealthChanged -= UpdatePlayerHealth;
    }

    private void Start()
    {
        goldCounter.text = "0";
    }

    public void ButtonReturnToMainMenu()
    {
        GameSceneManager.Instance.ChangeScene(GameScene.MainMenu);
    }

    public void ButtonRestartScene()
    {
        GameSceneManager.Instance.RestartScene();
    }

    private void UpdatePlayerHealth(float currentHp, float maxHp)
    {
        if (playerHealth == null || maxHp <= 0) return;

        float healthPercent = playerHealth.value = currentHp / maxHp;

        Debug.Log($"Health UI: {currentHp} / {maxHp} = {healthPercent}");
    }

    private void UpdateGoldCounter()
    {
        Debug.Log("Coin Go UP");
        goldCounter.text = GameResource.GetGoldAmount().ToString();
    }

    private void UpdateInventoryUI(Sprite consumableSprite, Sprite weaponSprite)
    {
        UpdateSlotVisual(consumeableSlot, consumableSprite);
        UpdateSlotVisual(weaponSlot, weaponSprite);
    }

    private void UpdateSlotVisual(Image slotImage, Sprite itemSprite)
    {
        if (itemSprite != null)
        {
            slotImage.sprite = itemSprite;
            slotImage.color = Color.white;
        }
        else
        {
            slotImage.sprite = null;
            slotImage.color = new Color(1, 1, 1, 0);
        }
    }
}