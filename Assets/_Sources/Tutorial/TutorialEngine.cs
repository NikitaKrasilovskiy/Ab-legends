using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialEngine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionView;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private RectTransform messageBody;
    [SerializeField] private RectTransform mask;
    [SerializeField] private Button nextBtn;
    [SerializeField] private RectTransform handTransform;
    [SerializeField] private Image handImage;
    [SerializeField] private Sprite[] hands;
    [SerializeField] private RectTransform avatarTransform;
    [SerializeField] private Image avatarImg;
    [SerializeField] private Sprite[] avatars;
    [SerializeField] private List<TutorialStep> tutorialSteps;
    [SerializeField] private int currentStep;
    private Action _currentAction;
    public bool isShow = false;
    private void Awake()
    {
        nextBtn.onClick.AddListener(InvokeCurrentAction);
    }

    public void SetCurrentStep(int step, Action currentAction = null)
    {
        this.currentStep = step;
        UpdateStepInfo();
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        mask.gameObject.SetActive(true);
        blackScreen.SetActive(true);
        isShow = true;
        _currentAction = currentAction;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        isShow = false;
        mask.gameObject.SetActive(false);
        blackScreen.SetActive(false);
    }

    void InvokeCurrentAction()
    {
        _currentAction?.Invoke();
    }

    void UpdateStepInfo()
    {
        var stepInfo = tutorialSteps[currentStep];
        descriptionView.text = LocalizationManager.Localize(stepInfo.description);
        messageBody.anchoredPosition = stepInfo.messagePosition;
        RectTransformExtensions.SetAnchor(mask,stepInfo.maskAnchorPreset);
        mask.anchoredPosition = stepInfo.maskAnchoredPos;
        mask.localScale = stepInfo.maskScale;
        mask.GetComponent<SpriteMask>().enabled = stepInfo.showMask;
        handImage.gameObject.SetActive(stepInfo.handState!=HandState.None);
        if(stepInfo.handState!=HandState.None)
            handImage.sprite = hands[(int) stepInfo.handState];
        avatarImg.sprite = avatars[(int) stepInfo.avatarEmotion];
        switch (stepInfo.tutHandPosition)
        {
            case TutHandPosition.UpLeft:
            {
                handTransform.anchoredPosition = new Vector2(-190, 137);
                handTransform.localEulerAngles = new Vector3(0, 0, 0);
                handTransform.localScale = new Vector3(1, 1, 1);
                break;
            }
            case TutHandPosition.UpRight:
            {
                handTransform.anchoredPosition = new Vector2(190, 137);
                handTransform.localEulerAngles = new Vector3(0, 0, 0);
                handTransform.localScale = new Vector3(-1, 1, 1);
                break;
            }
            case TutHandPosition.DownLeft:
            {
                handTransform.anchoredPosition = new Vector2(-190, -170);
                handTransform.localEulerAngles = new Vector3(0, 0, 100);
                handTransform.localScale = new Vector3(1, 1, 1);
                break;
            }
            case TutHandPosition.DownRight:
            {
                handTransform.anchoredPosition = new Vector2(190, -170);
                handTransform.localEulerAngles = new Vector3(0, 0, -100);
                handTransform.localScale = new Vector3(-1, 1, 1);
                break;
            }
            case TutHandPosition.Custom:
            {
                handTransform.anchoredPosition = stepInfo.handCustomPosition;
                handTransform.localEulerAngles = stepInfo.handCustomRotation;
                handTransform.localScale = stepInfo.handCustomScale;
                break;
            }
        }

        switch (stepInfo.tutAvatarOrigin)
        {
            case TutAvatarOrigin.Left:
            {
                descriptionView.rectTransform.anchoredPosition = new Vector2(50, -5);
                avatarTransform.anchoredPosition = new Vector2(-290, 37);
                break;
            }
            case TutAvatarOrigin.Right:
            {
                descriptionView.rectTransform.anchoredPosition = new Vector2(-50, -5);
                avatarTransform.anchoredPosition = new Vector2(290, 37);
                break;
            }
        }
    }

    private void OnValidate()
    {
        UpdateStepInfo();
    }
}

[Serializable]
public class TutorialStep
{
    public string description = "";
    public Vector2 messagePosition;
    public AnchorPresets maskAnchorPreset;
    public Vector3 maskAnchoredPos;
    public Vector3 maskScale;
    public TutHandPosition tutHandPosition;
    public TutAvatarOrigin tutAvatarOrigin;
    public AvatarEmotion avatarEmotion;
    public HandState handState;
    public Vector2 handCustomPosition;
    public Vector3 handCustomRotation;
    public Vector3 handCustomScale;
    public bool showMask = true;
}

public enum TutHandPosition
{
    UpLeft,
    UpRight,
    DownLeft,
    DownRight,
    Custom
}

public enum HandState
{
    Normal,
    Greeting,
    Ok,
    Admonition,
    Joy,
    None
}

public enum TutAvatarOrigin
{
    Left,
    Right
}

public enum AvatarEmotion
{
    Normal,
    Admonition,
    Joy
}