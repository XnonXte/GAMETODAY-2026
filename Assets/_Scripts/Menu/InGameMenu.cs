using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private Slider playerHealth;
    [SerializeField] private Slider payloadHealth;
    [SerializeField] private Image weaponSlot;
    [SerializeField] private Image consumeableSlot;
    [SerializeField] private TMP_Text goldCounter;
    [SerializeField] private CanvasGroup stageName;

    private void OnEnable()
    {
        EventHandler.OnGoldAmountChanged += UpdateGoldCounter;
        EventHandler.OnInventoryUpdated += UpdateInventoryUI;
        EventHandler.OnPlayerHealthChanged += UpdatePlayerHealth;
        EventHandler.OnPayloadHealthChanged += UpdatePayloadHealth;
    }

    private void OnDisable()
    {
        EventHandler.OnGoldAmountChanged -= UpdateGoldCounter;
        EventHandler.OnInventoryUpdated -= UpdateInventoryUI;
        EventHandler.OnPlayerHealthChanged -= UpdatePlayerHealth;
        EventHandler.OnPayloadHealthChanged -= UpdatePayloadHealth;
    }

    private void Start()
    {
        stageName.alpha = 0;
        UpdateGoldCounter();
        ShowStageName();

        if (PlayerInventory.Instance != null)
        {
            Sprite consumable = PlayerInventory.Instance.CurrentConsumable != null ? PlayerInventory.Instance.CurrentConsumable.itemSprite : null;
            Sprite weapon = PlayerInventory.Instance.CurrentWeapon != null ? PlayerInventory.Instance.CurrentWeapon.itemSprite : null;
            UpdateInventoryUI(consumable, weapon);
        }
    }

    public void ButtonReturnToMainMenu()
    {
        GameResource.ResetGold();
        if (GameSessionManager.Instance != null) GameSessionManager.Instance.ResetSession();
        GameSceneManager.Instance.ChangeScene(GameScene.MainMenu);
    }

    public void ButtonRestartScene()
    {
        GameSceneManager.Instance.RestartScene();
    }

    private void ShowStageName()
    {
        stageName.DOFade(1f, 3f).SetDelay(1f).OnComplete(() => { stageName.DOFade(0f, 3f).SetDelay(2f); });
    }

    private void UpdatePlayerHealth(float currentHp, float maxHp)
    {
        if (playerHealth == null || maxHp <= 0) return;
        float healthPercent = playerHealth.value = currentHp / maxHp;

        Debug.Log($"Health UI: {currentHp} / {maxHp} = {healthPercent}");
    }

    private void UpdatePayloadHealth(float currentHp, float maxHp)
    {
        if (payloadHealth == null || maxHp <= 0) return;
        payloadHealth.value = currentHp / maxHp;
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