using DG.Tweening;
using DG.Tweening.Core.Easing;
using TMPro;
using UnityEngine;

public class CutsceneScript : MonoBehaviour
{

    [System.Serializable]
    public class CutscenePanel
    {
        [Tooltip("The image displayed for this panel.")]
        public CanvasGroup image;

        [Tooltip("All text objects that belong to this image.")]
        public TMP_Text[] texts;
    }


    [System.Serializable]
    public class CutsceneStripe
    {
        [Tooltip("The CanvasGroup containing the entire stripe.")]
        public CanvasGroup panel;

        [Tooltip("Images and their corresponding text.")]
        public CutscenePanel[] panels;
    }

    [Header("Cutscene Stripes")]
    [SerializeField] private CutsceneStripe[] stripes;

    [Header("Settings")]

    [SerializeField]
    private float panelFadeDuration = 0.3f;

    [SerializeField]
    private float imageFadeDuration = 0.5f;

    [SerializeField]
    private float textFadeDuration = 0.4f;

    [Tooltip("How long to wait after the image has fully appeared before the text starts.")]
    [SerializeField]
    private float imageToTextDelay = 0.5f;

    [Tooltip("How long to wait between text objects.")]
    [SerializeField]
    private float delayBetween = 0.3f;

    [Tooltip("How long to wait before moving to the next stripe.")]
    [SerializeField]
    private float stripeDelay = 0.5f;
    [SerializeField] private GameScene endScene;


    // ============================================================
    // DOTWEEN
    // ============================================================

    private Sequence cutsceneSequence;


    // ============================================================
    // START
    // ============================================================

    private void Start()
    {
        SetupCutscene();
        PlayCutscene();
    }


    // ============================================================
    // SETUP
    // ============================================================

    private void SetupCutscene()
    {
        if (stripes == null)
            return;

        foreach (CutsceneStripe stripe in stripes)
        {
            if (stripe == null)
                continue;

            // Hide stripe
            if (stripe.panel != null)
            {
                stripe.panel.alpha = 0f;
                stripe.panel.gameObject.SetActive(false);
            }

            // Hide all images and text
            if (stripe.panels == null)
                continue;

            foreach (CutscenePanel cutscenePanel in stripe.panels)
            {
                if (cutscenePanel == null)
                    continue;

                if (cutscenePanel.image != null)
                {
                    cutscenePanel.image.alpha = 0f;
                }

                if (cutscenePanel.texts != null)
                {
                    foreach (TMP_Text text in cutscenePanel.texts)
                    {
                        if (text != null)
                        {
                            text.alpha = 0f;
                        }
                    }
                }
            }
        }
    }


    // ============================================================
    // PLAY CUTSCENE
    // ============================================================

    private void PlayCutscene()
    {
        cutsceneSequence?.Kill();

        cutsceneSequence = DOTween.Sequence();

        if (stripes == null || stripes.Length == 0)
        {
            Debug.LogWarning("No cutscene stripes configured.");
            return;
        }

        foreach (CutsceneStripe stripe in stripes)
        {
            if (stripe == null || stripe.panel == null)
                continue;


            // ====================================================
            // SHOW STRIPE
            // ====================================================

            stripe.panel.gameObject.SetActive(true);

            cutsceneSequence.Append(
                stripe.panel
                    .DOFade(1f, panelFadeDuration)
                    .SetEase(Ease.OutQuad)
            );


            // ====================================================
            // PLAY EACH IMAGE + TEXT
            // ====================================================

            if (stripe.panels != null)
            {
                foreach (CutscenePanel cutscenePanel in stripe.panels)
                {
                    AddImageAndText(
                        cutsceneSequence,
                        cutscenePanel
                    );
                }
            }


            // ====================================================
            // HIDE STRIPE
            // ====================================================

            cutsceneSequence.AppendInterval(stripeDelay);

            cutsceneSequence.Append(
                stripe.panel
                    .DOFade(0f, panelFadeDuration)
                    .SetEase(Ease.OutQuad)
            );

            // Need to capture the current stripe
            // for the callback.
            CutsceneStripe currentStripe = stripe;

            cutsceneSequence.AppendCallback(() =>
            {
                if (currentStripe.panel != null)
                {
                    currentStripe.panel.gameObject.SetActive(false);
                }
            });
        }


        // ========================================================
        // FINISHED
        // ========================================================

        cutsceneSequence.OnComplete(() =>
        {
            FinishCutscene();
            Debug.Log("Cutscene finished!");
        });
    }


    // ============================================================
    // IMAGE + TEXT
    // ============================================================

    private void AddImageAndText(
        Sequence sequence,
        CutscenePanel cutscenePanel)
    {
        if (cutscenePanel == null)
            return;


        // ========================================================
        // IMAGE
        // ========================================================

        if (cutscenePanel.image != null)
        {
            sequence.Append(
                cutscenePanel.image
                    .DOFade(1f, imageFadeDuration)
                    .SetEase(Ease.OutQuad)
            );

            // Wait after the image is completely visible.
            sequence.AppendInterval(imageToTextDelay);
        }


        // ========================================================
        // TEXT
        // ========================================================

        if (cutscenePanel.texts != null)
        {
            foreach (TMP_Text text in cutscenePanel.texts)
            {
                if (text == null)
                    continue;

                sequence.Append(
                    text
                        .DOFade(1f, textFadeDuration)
                        .SetEase(Ease.OutQuad)
                );

                // Wait before the next text object.
                sequence.AppendInterval(delayBetween);
            }
        }
    }

    private void FinishCutscene()
    {
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.ChangeScene(endScene);
        }
    }

    public void ButtonSkipCutscene()
    {
        GameSceneManager.Instance.ChangeScene(endScene);
    }

    private void OnDestroy()
    {
        cutsceneSequence?.Kill();
    }
}