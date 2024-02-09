using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenBoxPanel : MonoBehaviour
{
    [SerializeField] private Animator prizeAnimator;
    [SerializeField] private TextMeshProUGUI coinView;
    [SerializeField] private TextMeshProUGUI cardView;
    [SerializeField] private Button prizeButton;
    [SerializeField] private Image openedBox;
    [SerializeField] private Image closedBox;
    [SerializeField] private Sprite[] openedBoxesSprites;
    [SerializeField] private Sprite[] closedBoxesSprites;
    [SerializeField] private CardViewer cardViewer;
    [SerializeField] private PlayerDataViewer playerDataViewer;
    private static readonly int Open = Animator.StringToHash("Open");
    private bool _continue = false;
    private PrizeData _prizeData;
    public GameObject vibrationAudio;
    public GameObject openAudio;
 
    void OnEnable()
    {
        DellayActivation(OpenPrize);
        vibrationAudio.SetActive(true);
    }

    public void ShowPrize(PrizeData prizeData)
    {
        _prizeData = prizeData;
        var boxData =
            DataContainer.Instance.cardDataContainer.metaGameData.boxes.Find(x => 
                x.prizeType == prizeData.prizeType);
        _prizeData.cardCount = 1;
        _prizeData.goldCount = boxData.gold;
        _prizeData.rubyCount = boxData.ruby;
        var prizeId = (int) prizeData.prizeType;
        openedBox.sprite = openedBoxesSprites[prizeId];
        closedBox.sprite = closedBoxesSprites[prizeId];
        gameObject.SetActive(true);
    }
    
    async void DellayActivation(Action action)
    {
        await UniTask.Delay(7000);
        if (_continue)
        {
            return;
        }
        else
        {
            action.Invoke();
        }
    }

    public async void OpenPrize()
    {
        _continue = true;
        prizeAnimator.SetTrigger(Open);
        vibrationAudio.SetActive(false);
        openAudio.SetActive(true);
        if(DataContainer.Instance.playerData.playerStaff.giftsList.gifts.Count>0)
            DataContainer.Instance.playerData.playerStaff.giftsList.gifts.RemoveAt(0);
        coinView.text = _prizeData.goldCount.ToString();
        cardView.text = _prizeData.rubyCount.ToString();
        DataContainer.Instance.playerData.playerStaff.goldCount += _prizeData.goldCount;
        DataContainer.Instance.playerData.playerStaff.ruby += _prizeData.rubyCount;
        BalanceAnalytics.GettingGold(CurrencySource.Chest, _prizeData.goldCount);
        Debug.Log("Wait for anim complite");
        await UniTask.WaitUntil(() => prizeAnimator==null || prizeAnimator.GetCurrentAnimatorStateInfo(0).IsName("LvlUp_openBox"));
        await UniTask.Delay(2000);
        var warriorCard = DataContainer.Instance.cardDataContainer.GetRandomWarriorCard(Fraction.Acorn, 1,5);
        cardViewer.SetCard(warriorCard);
        cardViewer.gameObject.SetActive(true);
        DataContainer.Instance.playerData.cardCollection.warriorCards.Add(warriorCard);
        PlayerData.SetData(DataContainer.Instance.playerData, () =>
        {
            var companyTasks = DataContainer.Instance.playerData.playerGameProgress.dailyTasks.tasks.FindAll(x =>
                x.taskType == TaskType.PRIZE);
            foreach (var VARIABLE in companyTasks)
            {
                VARIABLE.progress++;
            }

            DellayActivation(Close);
        });
    }

    public async void Close()
    {
        
        PlayerData.SetData(DataContainer.Instance.playerData, () =>
        {
            OnClose();
        });
    }

    async void OnClose()
    {
        prizeButton.interactable = true;
        _continue = false;
        cardViewer.gameObject.SetActive(false);
        await playerDataViewer.SyncAndUpdate();
        gameObject.SetActive(false);
        if (!TutorialController.Instance.isTutorialComplete)
            TutorialController.Instance.ShowTutorial();
    }
}

[Serializable]
public class PrizeData
{
    public PrizeType prizeType;
    public int goldCount;
    public int cardCount;
    public int rubyCount;
    public int expCount;
}

public enum PrizeType
{
    Small,
    Normal,
    Big,
    Large
}