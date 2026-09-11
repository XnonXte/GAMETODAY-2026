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
    [Header("Ability Cut-In Settings")]
    [SerializeField] private RectTransform abilityAvatarIndicator;
    [SerializeField] private float offScreenX = -1000f; // Far left off-screen
    [SerializeField] private float onScreenX = 0f;     // Where it rests on-screen
    [SerializeField] private float slideInDuration = 0.25f;
    [SerializeField] private float holdDuration = 1.25f;
    [SerializeField] private float slideOutDuration = 0.2f;

    private Sequence cutinSequence;

    private void OnEnable()
    {
        EventHandler.OnGoldAmountChanged += UpdateGoldCounter;
        EventHandler.OnInventoryUpdated += UpdateInventoryUI;
        EventHandler.OnPlayerHealthChanged += UpdatePlayerHealth;
        EventHandler.OnPayloadHealthChanged += UpdatePayloadHealth;
        EventHandler.OnAnyAbilityUsed += ShowAbilityIndicator;
    }

    private void OnDisable()
    {
        EventHandler.OnGoldAmountChanged -= UpdateGoldCounter;
        EventHandler.OnInventoryUpdated -= UpdateInventoryUI;
        EventHandler.OnPlayerHealthChanged -= UpdatePlayerHealth;
        EventHandler.OnPayloadHealthChanged -= UpdatePayloadHealth;
        EventHandler.OnAnyAbilityUsed -= ShowAbilityIndicator;
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

        if (abilityAvatarIndicator != null)
        {
            abilityAvatarIndicator.anchoredPosition = new Vector2(offScreenX, abilityAvatarIndicator.anchoredPosition.y);
            abilityAvatarIndicator.gameObject.SetActive(false);
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

    private void ShowAbilityIndicator()
    {
        if (abilityAvatarIndicator == null) return;

        // 1. If an animation is already playing, kill it so it doesn't glitch if spammed
        cutinSequence?.Kill();

        // 2. Snap it to the starting off-screen position and turn it on
        abilityAvatarIndicator.gameObject.SetActive(true);
        abilityAvatarIndicator.anchoredPosition = new Vector2(offScreenX, abilityAvatarIndicator.anchoredPosition.y);

        // 3. Create a new DOTween sequence
        cutinSequence = DOTween.Sequence();

        // Step 1: Slide in aggressively (Ease.OutExpo looks very punchy/anime style)
        cutinSequence.Append(abilityAvatarIndicator.DOAnchorPosX(onScreenX, slideInDuration).SetEase(Ease.OutExpo));

        // Step 2: Hold it there so the player can see it
        cutinSequence.AppendInterval(holdDuration);

        // Step 3: Slide it back out to the left
        cutinSequence.Append(abilityAvatarIndicator.DOAnchorPosX(offScreenX, slideOutDuration).SetEase(Ease.InExpo));

        // Step 4: Turn the GameObject off when the sequence completely finishes
        cutinSequence.OnComplete(() => abilityAvatarIndicator.gameObject.SetActive(false));
    }
}