using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

[Serializable]
public class PlayerData
{
    public string playerId;
    public PlayerProfileModel playerProfileModel;
    public Deck acornDeck;
    public Deck bobberDeck;
    public Deck candleDeck;
    public CardCollection cardCollection;
    public PlayerStaff playerStaff;
    public PlayerGameProgress playerGameProgress;
    public int arenaPoint;
    const string ACORN_DECK = "AcornDeck";
    const string BOBBER_DECK = "BobberDeck";
    const string CANDLE_DECK = "CandleDeck";
    const string PLAYER_STAFF = "PlayerStaff";
    const string CARD_COLLECTION = "CardCollection";
    const string PLAYER_GAME_PROGRESS = "PlayerGameProgress";
    [NonSerialized] private static List<Action> _updateListeners = new List<Action>();
    public static PlayerData GetData(string myPlayFabId, PlayerData playerData, Action onComplite = null)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        },
        result => { Deserialize(result.Data, playerData); onComplite?.Invoke();},
        error => { }
        );
        return playerData;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public static void SetData(PlayerData playerData, Action onSucces = null, Action onError = null) {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = PlayerData.Serialize(playerData),
            Permission = UserDataPermission.Public
        },
        result =>
        {
            foreach (var listener in _updateListeners)
            {
                listener.Invoke();
            }
            onSucces?.Invoke();
        },
        error => onError?.Invoke()
        );
    }

    public static Dictionary<string, string> Serialize(PlayerData playerData)
    {
        var dictionary = new Dictionary<string, string>();
        dictionary.Add(ACORN_DECK, JsonUtility.ToJson(playerData.acornDeck));
        dictionary.Add(BOBBER_DECK, JsonUtility.ToJson(playerData.bobberDeck));
        dictionary.Add(CANDLE_DECK, JsonUtility.ToJson(playerData.candleDeck));
        dictionary.Add(CARD_COLLECTION, JsonUtility.ToJson(playerData.cardCollection));
        dictionary.Add(PLAYER_STAFF, JsonUtility.ToJson(playerData.playerStaff));
        dictionary.Add(PLAYER_GAME_PROGRESS, JsonUtility.ToJson(playerData.playerGameProgress));
        return dictionary;
    }

    public static void Deserialize(Dictionary<string, UserDataRecord> dictionary, PlayerData playerData)
    {
        playerData.acornDeck = JsonUtility.FromJson<Deck>(dictionary[ACORN_DECK].Value);
        playerData.bobberDeck = JsonUtility.FromJson<Deck>(dictionary[BOBBER_DECK].Value);
        playerData.candleDeck = JsonUtility.FromJson<Deck>(dictionary[CANDLE_DECK].Value);
        playerData.cardCollection = JsonUtility.FromJson<CardCollection>(dictionary[CARD_COLLECTION].Value);
        playerData.playerStaff = JsonUtility.FromJson<PlayerStaff>(dictionary[PLAYER_STAFF].Value);
        playerData.playerGameProgress = JsonUtility.FromJson<PlayerGameProgress>(dictionary[PLAYER_GAME_PROGRESS].Value);
//        Debug.Log(dictionary[PLAYER_GAME_PROGRESS].Value);
        foreach (var task in playerData.playerGameProgress.dailyTasks.tasks)
        {
            TimeSpan timeSpan = DateTime.Now - task.dateTime;
            Debug.Log(DateTime.Now+"-"+task.dateTime+"="+timeSpan.Days);
            if(timeSpan.Days>0)
                task.ResetTask();
        }
    }

    public Deck GetDeckByFraction(Fraction fraction)
    {
        switch (fraction)
        {
            case Fraction.Acorn:
            {
                return acornDeck;
            }
            case Fraction.Bobber:
            {
                return bobberDeck;
            }
            case Fraction.Candle:
            {
                return candleDeck;
            }
        }

        return acornDeck;
    }

    public void SetDefaultPlayerData(CardDataContainer _cardDataContainer, Action onComplite)
    {
        cardCollection = new CardCollection();
        cardCollection.warriorCards = new List<WarriorCard>();
        cardCollection.warriorCards.Add(_cardDataContainer.GetWarriorCard("acorn2_1"));
        // cardCollection.warriorCards.Add(_cardDataContainer.GetWarriorCard("acorn3_1"));
        // cardCollection.warriorCards.Add(_cardDataContainer.GetWarriorCard("acorn4_1"));
        acornDeck = new Deck();
        TutorialController.Instance.isTutorialComplete = false;
        acornDeck.warriorCards = new List<WarriorCard>();
        acornDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard("acorn0_1"));
        acornDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard("acorn1_1"));
        acornDeck.leaderCard = _cardDataContainer.GetAcornLeader(1);
        arenaPoint = 1500;
        bobberDeck = new Deck();
        candleDeck = new Deck();
        bobberDeck.warriorCards = new List<WarriorCard>();
        candleDeck.warriorCards = new List<WarriorCard>();
        PlayerPrefs.DeleteAll();
        playerGameProgress = new PlayerGameProgress();
        playerGameProgress.accornCompanyLvl = 0;
        playerGameProgress.bobberCompanyLvl = 0;
        playerGameProgress.dailyTasks = new DailyTasks();
        playerGameProgress.dailyTasks.tasks = new List<DailyTask>();
        Debug.Log(DateTime.Now);
        playerGameProgress.dailyTasks.tasks.Add(new DailyTask()
            {
                dateTime = DateTime.Now, 
                description = "DAILY_0",
                iconId = "daily0",
                isComplited = false,
                progress = 0,
                rewardCount = 10,
                stageCount = 1,
                taskType = TaskType.COMPANY
            });
        playerGameProgress.dailyTasks.tasks.Add(new DailyTask()
        {
            dateTime = DateTime.Now, 
            description = "DAILY_1",
            iconId = "daily1",
            isComplited = false,
            progress = 0,
            rewardCount = 25,
            stageCount = 3,
            taskType = TaskType.ARENA_WIN
        });
        playerGameProgress.dailyTasks.tasks.Add(new DailyTask()
        {
            dateTime = DateTime.Now, 
            description = "DAILY_2",
            iconId = "daily2",
            isComplited = false,
            progress = 0,
            rewardCount = 10,
            stageCount = 1,
            taskType = TaskType.PRIZE
        });
        playerStaff.giftsList.gifts = new List<PrizeType>();
        playerStaff.goldCount = 150;
        playerStaff.arenaCount = 3;
        
        
#if TEST
        playerGameProgress.lvl = 8;
        acornDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard("acorn2_1"));
        acornDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard("acorn3_1"));
        acornDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard("acorn4_1"));
        cardCollection.warriorCards.Add(_cardDataContainer.GetWarriorCard("bobber2_1"));
        cardCollection.warriorCards.Add(_cardDataContainer.GetWarriorCard("bobber3_1"));
        cardCollection.warriorCards.Add(_cardDataContainer.GetWarriorCard("bobber4_1"));
        bobberDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard("bobber0_1"));
        bobberDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard("bobber1_1"));
        bobberDeck.leaderCard = _cardDataContainer.GetBobberLeader(1);
        cardCollection.warriorCards.Add(_cardDataContainer.GetWarriorCard("candle2_1"));
        cardCollection.warriorCards.Add(_cardDataContainer.GetWarriorCard("candle3_1"));
        cardCollection.warriorCards.Add(_cardDataContainer.GetWarriorCard("candle4_1"));
        candleDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard("candle0_1"));
        candleDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard("candle1_1"));
        candleDeck.leaderCard = _cardDataContainer.GetCandleLeader(1);
        PlayerPrefs.SetInt("Bobber", 1);
        PlayerPrefs.SetInt("Candle", 1);
        //TutorialController.Instance.CompleteTutorial();
        playerStaff.giftsList.gifts.Add(PrizeType.Small);
        playerStaff.giftsList.gifts.Add(PrizeType.Normal);
        playerStaff.giftsList.gifts.Add(PrizeType.Big);
        playerStaff.giftsList.gifts.Add(PrizeType.Large);
        playerStaff.goldCount = 150000;
#endif
        
        
        
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "resetGifts",
            GeneratePlayStreamEvent = true,
        }, result => { }, error => { });
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = "Arena",
                    Value = arenaPoint
                }
            }
        }, result=> OnStatisticsUpdated(result, onComplite), FailureCallback);
    }
    
    public void SetDefaultPlayerData(CardDataContainer _cardDataContainer, string[] deck, int rating, Action onComplite)
    {
        cardCollection = new CardCollection();
        cardCollection.warriorCards = new List<WarriorCard>();
        acornDeck = new Deck();
        TutorialController.Instance.isTutorialComplete = false;
        acornDeck.warriorCards = new List<WarriorCard>();
        foreach (var card in deck)
        {
            acornDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard(card));
        }
        acornDeck.leaderCard = _cardDataContainer.GetAcornLeader(1);
        arenaPoint = rating;
        bobberDeck = new Deck();
        candleDeck = new Deck();
        bobberDeck.warriorCards = new List<WarriorCard>();
        candleDeck.warriorCards = new List<WarriorCard>();
        PlayerPrefs.DeleteAll();
        playerGameProgress = new PlayerGameProgress();
        playerGameProgress.accornCompanyLvl = 0;
        playerGameProgress.bobberCompanyLvl = 0;
        playerGameProgress.dailyTasks = new DailyTasks();
        playerGameProgress.dailyTasks.tasks = new List<DailyTask>();
        Debug.Log(DateTime.Now);
        playerGameProgress.dailyTasks.tasks.Add(new DailyTask()
            {
                dateTime = DateTime.Now, 
                description = "DAILY_0",
                iconId = "daily0",
                isComplited = false,
                progress = 0,
                rewardCount = 10,
                stageCount = 1,
                taskType = TaskType.COMPANY
            });
        playerGameProgress.dailyTasks.tasks.Add(new DailyTask()
        {
            dateTime = DateTime.Now, 
            description = "DAILY_1",
            iconId = "daily1",
            isComplited = false,
            progress = 0,
            rewardCount = 25,
            stageCount = 3,
            taskType = TaskType.ARENA_WIN
        });
        playerGameProgress.dailyTasks.tasks.Add(new DailyTask()
        {
            dateTime = DateTime.Now, 
            description = "DAILY_2",
            iconId = "daily2",
            isComplited = false,
            progress = 0,
            rewardCount = 10,
            stageCount = 1,
            taskType = TaskType.PRIZE
        });
#if TEST
        playerGameProgress.lvl = 8;
        acornDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard("acorn2_1"));
        acornDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard("acorn3_1"));
        acornDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard("acorn4_1"));
        cardCollection.warriorCards.Add(_cardDataContainer.GetWarriorCard("bobber2_1"));
        cardCollection.warriorCards.Add(_cardDataContainer.GetWarriorCard("bobber3_1"));
        cardCollection.warriorCards.Add(_cardDataContainer.GetWarriorCard("bobber4_1"));
        bobberDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard("bobber0_1"));
        bobberDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard("bobber1_1"));
        bobberDeck.leaderCard = _cardDataContainer.GetBobberLeader(1);
        cardCollection.warriorCards.Add(_cardDataContainer.GetWarriorCard("candle2_1"));
        cardCollection.warriorCards.Add(_cardDataContainer.GetWarriorCard("candle3_1"));
        cardCollection.warriorCards.Add(_cardDataContainer.GetWarriorCard("candle4_1"));
        candleDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard("candle0_1"));
        candleDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard("candle1_1"));
        candleDeck.leaderCard = _cardDataContainer.GetCandleLeader(1);
        PlayerPrefs.SetInt("Bobber", 1);
        PlayerPrefs.SetInt("Candle", 1);
        TutorialController.Instance.CompleteTutorial();
#endif
        playerStaff = new PlayerStaff();
        playerStaff.giftsList.gifts = new List<PrizeType>();
        playerStaff.goldCount = 150;
        playerStaff.arenaCount = 3;
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = "Arena",
                    Value = arenaPoint
                }
            }
        }, result=> OnStatisticsUpdated(result, onComplite), FailureCallback);
    }

    private void OnStatisticsUpdated(UpdatePlayerStatisticsResult updateResult, Action onComplite) {
        Debug.Log("Successfully submitted high score");
        PlayerData.SetData(this, onComplite);
    }

    
    
    private void FailureCallback(PlayFabError error){
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public static void AddUpdateListener(Action action)
    {
        _updateListeners.Add(action);
    }

    public static void RemoveUpdateListener(Action action)
    {
        _updateListeners.Remove(action);
    }
}

[Serializable]
public class PlayerStaff
{
    public int arenaCount;
    public int arenaDay;
    public int goldCount;
    public int giftCount
    {
        get { return giftsList.gifts.Count; }
    }
    public Gifts giftsList = new Gifts();
    public int ruby;
}

[Serializable]
public class Gifts
{
    public List<PrizeType> gifts = new List<PrizeType>();
}

[Serializable]
public class PlayerGameProgress
{
    public int accornCompanyLvl;
    public int bobberCompanyLvl = 0;
    public float winRate = 0;
    public float exp = 0;
    public int lvl = 1;
    public int winCount = 0;
    public int loseCount = 0;
    public DailyTasks dailyTasks;

    public int GetCurentLvl(Fraction fraction)
    {
//        Debug.Log(fraction);
        if (fraction == Fraction.Acorn)
            return accornCompanyLvl;

        if (fraction == Fraction.Bobber)
            return bobberCompanyLvl;
        return accornCompanyLvl;
    }

    public void SetCurrentLvl(Fraction fraction, int lvl)
    {
        if (fraction == Fraction.Acorn)
            accornCompanyLvl = lvl;

        if (fraction == Fraction.Bobber)
            bobberCompanyLvl = lvl;
    }
}

[Serializable]
public class CardCollection
{
    public List<WarriorCard> warriorCards;
}
