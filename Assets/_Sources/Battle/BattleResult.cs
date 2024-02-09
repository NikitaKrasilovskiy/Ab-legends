using Cysharp.Threading.Tasks;
using DG.Tweening;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class BattleResult : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private BattleResultWindow battleResultWindow;
    public TempCompanyData _tempCompanyData;
    [Inject] private CardDataContainer _cardDataContainer;
    public CardViewer cardViewer;
    public LeaderViewer leaderViewer;
    [SerializeField] private GameObject winBg;
    private bool isWin = false;
    public async void Win()
    {
        winPanel.SetActive(true);
        if (!TutorialController.Instance.isTutorialComplete)
        {
            TutorialController.Instance.ShowTutorial();
            await UniTask.WaitForEndOfFrame();
            await UniTask.WaitWhile(() =>
                TutorialController.Instance.isTutorialComplete || (!TutorialController.Instance.TutorialEngine.isShow));
        }
        ValidateBattleResult(true);
    }

    public void Lose()
    {
        losePanel.SetActive(true);
        ValidateBattleResult(false);
    }

    void ValidateBattleResult(bool isWin)
    {
        this.isWin = isWin;
        //var eventsParams = new CustomEventParams();
        //eventsParams.AddParam("type", "story");
        //eventsParams.AddParam("level", DataContainer.Instance.playerData.playerGameProgress.GetCurentLvl(BattleDataContainer.CurentPlayerFraction)-1);
        //eventsParams.AddParam("health_left", FindObjectOfType<LeaderViewer>().GetCurentHealth());
        //eventsParams.AddParam("ads_watch", 0);

        if (BattleDataContainer.IsArenaBattle)
        {
            DataContainer.Instance.playerData.playerStaff.goldCount -= 25;
            Debug.Log(DataContainer.Instance.playerData.playerStaff.goldCount);

            Debug.Log(BattleDataContainer.PlayerRating + " player start rating");
            Debug.Log(BattleDataContainer.PlayerRating + " enemy start rating");
            int i = Mathf.RoundToInt((BattleDataContainer.ArenaEnemyRating - BattleDataContainer.PlayerRating) / 400);
            var e = 1 / (1 + Mathf.Pow(10, i));
            var rating = BattleDataContainer.PlayerRating + 16 * ((isWin ? 1 : 0) - e);
            Debug.Log(rating + " rating result " + (BattleDataContainer.PlayerRating));
            if (isWin)
            {
                DataContainer.Instance.playerData.playerGameProgress.winCount++;
                var companyTasks = DataContainer.Instance.playerData.
                    playerGameProgress.dailyTasks.tasks.FindAll(x => x.taskType == TaskType.ARENA_WIN);
                foreach (var VARIABLE in companyTasks)
                {
                    VARIABLE.progress++;
                }
                //eventsParams.AddParam("result", "win");
            }
            else
            {
                DataContainer.Instance.playerData.playerGameProgress.loseCount++;
                //eventsParams.AddParam("result", "lose");
            }
            DataContainer.Instance.playerData.playerGameProgress.winRate =
                (DataContainer.Instance.playerData.playerGameProgress.winCount /
                 ((float)DataContainer.Instance.playerData.playerGameProgress.loseCount +
                  DataContainer.Instance.playerData.playerGameProgress.winCount)) * 100;
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate> {
                    new StatisticUpdate {
                        StatisticName = "Arena",
                        Value = Mathf.RoundToInt(rating)
                    }
                }
            }, result => OnStatisticsUpdated(result), FailureCallback);
            //DevToDev.Analytics.CustomEvent("level_finish", eventsParams); 
            //FB.LogAppEvent("Round Complete");
            //DevToDev.Analytics.CustomEvent("Round Complete");
            return;
        }

        bool extra = false;
        bool leader = false;
        if (BattleDataContainer.IsCompanyBattle && isWin)
        {
            var fraction = BattleDataContainer.CurentPlayerFraction;
            AdjustEvents.Progress(DataContainer.Instance.playerData.playerGameProgress
                .GetCurentLvl(fraction) + 1, isWin);
            DataContainer.Instance.playerData.playerGameProgress.SetCurrentLvl(fraction,
                DataContainer.Instance.playerData.playerGameProgress.GetCurentLvl(fraction) + 1);
            extra = DataContainer.Instance.playerData.playerGameProgress.GetCurentLvl(fraction) % 3 == 0;

            Debug.Log(DataContainer.Instance.playerData.playerGameProgress.GetCurentLvl(fraction) % 3);
            leader = DataContainer.Instance.playerData.playerGameProgress.GetCurentLvl(fraction) % 93 == 0;
            Debug.Log(DataContainer.Instance.playerData.playerGameProgress.GetCurentLvl(fraction) % 93);
            // var deckInfos = BattleDataContainer.CurentPlayerFraction == Fraction.Acorn
            //     ? _cardDataContainer.bobberCompany.levelsData
            //     : _tempCompanyData.bobberLvls;
            var deckInfo = _cardDataContainer.bobberCompany.
                GetDeckInfo(DataContainer.Instance.playerData.playerGameProgress.GetCurentLvl(fraction));
            //eventsParams.AddParam("reward_soft", deckInfo.rewardGold);
            //eventsParams.AddParam("reward_exp", deckInfo.rewardExp);

            if (extra)
            {
                var companyTasks = DataContainer.Instance.playerData.playerGameProgress
                    .dailyTasks.tasks.FindAll(x => x.taskType == TaskType.COMPANY);
                foreach (var VARIABLE in companyTasks)
                {
                    VARIABLE.progress++;
                }
            }
            Debug.Log(JsonUtility.ToJson(deckInfo));
            if (!string.IsNullOrEmpty(deckInfo.rewardCard) && extra)
            {
                if (leader)
                {
                    ShowLeader(_cardDataContainer.GetLeader(BattleDataContainer.CurentPlayerFraction == Fraction.Acorn ?
                        Fraction.Bobber : Fraction.Candle, 1));
                    //FB.LogAppEvent((BattleDataContainer.CurentPlayerFraction == Fraction.Acorn ?
                    //    Fraction.Bobber : Fraction.Candle).ToString() + " Unlocked");
                }
                else
                {
                    var rewardCard = _cardDataContainer.GetWarriorCard(deckInfo.rewardCard);
                    Debug.Log(JsonUtility.ToJson(rewardCard));
                    ShowCard(rewardCard, deckInfo);

                }
            }
            else
            {
                ShowResult(deckInfo);
            }
            //eventsParams.AddParam("result", "win");

            //FB.LogAppEvent("Level Complete");
            //DevToDev.Analytics.CustomEvent("Level Complete");
            BalanceAnalytics.LvlComplite(DataContainer.Instance.playerData.playerGameProgress
                .GetCurentLvl(BattleDataContainer.CurentPlayerFraction), isWin);
        }
        else
        {
            AdjustEvents.Progress(DataContainer.Instance.playerData.playerGameProgress
                .GetCurentLvl(BattleDataContainer.CurentPlayerFraction) + 1, isWin);
            BalanceAnalytics.LvlComplite(DataContainer.Instance.playerData.playerGameProgress
                .GetCurentLvl(BattleDataContainer.CurentPlayerFraction) + 1, isWin);
            PlayerData.SetData(DataContainer.Instance.playerData, () => LoadScene());
            //eventsParams.AddParam("result", "lose");
            //FB.LogAppEvent("Level Failed");
            //DevToDev.Analytics.CustomEvent("Level Failed");
        }

        //DevToDev.Analytics.CustomEvent("level_finish", eventsParams); 
    }

    async UniTask ShowCard(WarriorCard warriorCard, DeckInfo deckInfo)
    {
        Debug.Log("SHOW CARD");
        winBg.gameObject.SetActive(false);
        cardViewer.SetCard(warriorCard);
        cardViewer.gameObject.SetActive(true);
        await UniTask.Delay(5);
        DataContainer.Instance.playerData.cardCollection.warriorCards.Add(warriorCard);
        ShowResult(deckInfo);
    }

    async UniTask ShowResult(DeckInfo deckInfo)
    {
        Debug.Log("SHOW RESULT");
        await SoundEngine.PlayEffect(isWin ? "win" : "lose");
        if (isWin)
            battleResultWindow.ShowReward(deckInfo);
        if (!TutorialController.Instance.isTutorialComplete)
            TutorialController.Instance.ShowTutorial();
    }

    async UniTask LoadScene()
    {
        await SoundEngine.PlayEffect(isWin ? "win" : "lose");
        SceneManager.LoadScene("Main_menu");
    }

    async void ShowLeader(LeaderCard leaderCard)
    {
        Debug.Log("SHOW LEADER");
        winBg.gameObject.SetActive(false);
        leaderViewer.ShowCard(leaderCard);
        leaderViewer.gameObject.SetActive(true);
        var cardTransform = leaderViewer.transform;
        cardTransform.DORotate(new Vector3(0, 0, 360), 1);
        cardTransform.DOScale(new Vector3(2, 2, 2), 1);
        await UniTask.Delay(5);
        DataContainer.Instance.playerData.GetDeckByFraction(leaderCard.fraction).leaderCard = leaderCard;
        PlayerPrefs.SetInt(BattleDataContainer.CurentPlayerFraction == Fraction.Acorn ? "Bobber" : "Candle", 1);
        PlayerData.SetData(DataContainer.Instance.playerData, () => LoadScene());
    }

    private void OnStatisticsUpdated(UpdatePlayerStatisticsResult result)
    {

        PlayerData.SetData(DataContainer.Instance.playerData, () => LoadScene());
    }

    private void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
}
