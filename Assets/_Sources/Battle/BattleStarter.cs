using Cysharp.Threading.Tasks;
using DevToDev;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class BattleStarter : MonoBehaviour
{
    [SerializeField] private BattleEngine battleEngine;
    [SerializeField] private TempCompanyData tempCompanyData;
    [SerializeField] private LeadersStartAnim leadersStartAnim;
    [SerializeField] private TextMeshProUGUI enemyNicknameViewer;
    [SerializeField] private FakeBattleData fakeData;
    [Inject] private CardDataContainer _cardDataContainer;
    private Deck _playerDeck;
    void Start()
    {
        Debug.Log("BattleStarter battle");
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            PlayerData.GetData(BattleDataContainer.playerId, DataContainer.Instance.playerData, StartBattle);
        }
        else
        {
            PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest()
            {
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true
            },
                result =>
                {
                    PlayerData.GetData(BattleDataContainer.playerId, DataContainer.Instance.playerData, StartBattle);
                },
                error => Debug.Log(error.ErrorMessage));
        }

    }

    async void StartBattle()
    {
        var enemyDeck = new Deck();
        var eventsParams = new CustomEventParams();
        if (!BattleDataContainer.IsCompanyBattle && !BattleDataContainer.IsArenaBattle)
        {
            PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest()
            {
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true
            }, result => { }, error => { });
        }
        await UniTask.WaitUntil(() => _cardDataContainer.isInit);
        _playerDeck = DataContainer.Instance.playerData.GetDeckByFraction(Fraction.Acorn);
        Debug.Log("Start battle");
        if (BattleDataContainer.IsCompanyBattle)
        {
            eventsParams.AddParam("type", "story");
            eventsParams.AddParam("level", DataContainer.Instance.playerData.playerGameProgress.GetCurentLvl(BattleDataContainer.CurentPlayerFraction));
            Debug.Log("Company Battle");
            // var deckInfos = BattleDataContainer.CurentPlayerFraction == Fraction.Acorn
            //     ? _cardDataContainer.bobberCompany.levelsData
            //     : tempCompanyData.bobberLvls;
            //            Debug.Log(BattleDataContainer.CurentPlayerFraction);
            //            Debug.Log(_playerData.playerGameProgress.GetCurentLvl(BattleDataContainer.CurentPlayerFraction));
            var deckInfo = _cardDataContainer.bobberCompany.
                GetDeckInfo(DataContainer.Instance.playerData.playerGameProgress.
                    GetCurentLvl(BattleDataContainer.CurentPlayerFraction));
            enemyDeck.leaderCard = _cardDataContainer.GetLeader(deckInfo.fraction, deckInfo.leaderLvl);
            enemyDeck.warriorCards = new List<WarriorCard>();
            Debug.Log("Oponent deck: " + JsonUtility.ToJson(deckInfo));
            for (int i = 0; i < deckInfo.wariorIds.Length; i++)
            {
                enemyDeck.warriorCards.Add(_cardDataContainer.GetWarriorCard(deckInfo.wariorIds[i]));
            }

            await leadersStartAnim.ShowAnim(enemyDeck.leaderCard, _playerDeck.leaderCard);
            if (!TutorialController.Instance.isTutorialComplete)
                TutorialController.Instance.ShowTutorial();
            battleEngine.Init(enemyDeck, _playerDeck);
        }

        if (BattleDataContainer.IsArenaBattle)
        {
            eventsParams.AddParam("type", "pvp");
            Debug.Log("Arena Battle");
            _playerDeck = DataContainer.Instance.playerData.GetDeckByFraction((Fraction)PlayerPrefs.GetInt("CurentDeck", 0));
            enemyNicknameViewer.text = BattleDataContainer.EnemyName;
            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                PlayFabId = BattleDataContainer.ArenaEnemyId
            }, result => CharacterDataResult(result), error => ErrorResult(error));
            //FB.LogAppEvent("Round Started");
            //DevToDev.Analytics.CustomEvent("Round Started");
        }

        if (!BattleDataContainer.IsCompanyBattle && !BattleDataContainer.IsArenaBattle)
        {
            if (fakeData)
            {
                Debug.Log("Fake Battle");
                await UniTask.WaitUntil(() => DataContainer.Instance.cardDataContainer != null);
                _playerDeck = fakeData.GetOponentDeck();
                enemyDeck = fakeData.GetPlayerDeck();
                await leadersStartAnim.ShowAnim(_playerDeck.leaderCard, enemyDeck.leaderCard);
                battleEngine.Init(_playerDeck, enemyDeck);
            }
            else
            {
                Debug.Log("Test Battle");
                await leadersStartAnim.ShowAnim(_playerDeck.leaderCard, _playerDeck.leaderCard);
                battleEngine.Init(_playerDeck, _playerDeck);
                //FB.LogAppEvent("Level Started");
                //DevToDev.Analytics.CustomEvent("Level Started");
            }
        }
        else
        {
            //DevToDev.Analytics.CustomEvent("level_start", eventsParams); 
        }

    }

    private void ErrorResult(PlayFabError error)
    {
        Debug.LogError(error.Error);
    }

    private async void CharacterDataResult(GetUserDataResult result)
    {
        var enemyDeck = JsonUtility.FromJson<Deck>(result.Data["AcornDeck"].Value);
        await leadersStartAnim.ShowAnim(enemyDeck.leaderCard, _playerDeck.leaderCard);
        battleEngine.Init(enemyDeck, _playerDeck);
    }



}
