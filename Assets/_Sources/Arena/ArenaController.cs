using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;
using GoogleMobileAds.Api;

public class ArenaController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _leaderboardNames;
    [SerializeField] private Button startBattleButton, toPlay;
    [SerializeField] private TextMeshProUGUI playerPositionView;
    [SerializeField] private ArenaPrizeDataUpdater _arenaPrizeDataUpdater;
    private List<PlayerLeaderboardEntry> _leaderboardEntries;
    private PlayerLeaderboardEntry _playerLeaderboardEntry;
    [SerializeField] private TextMeshProUGUI arenaGoldCount;
    [SerializeField] private TextMeshProUGUI arenaExpCount;
    [SerializeField] private GameObject menuAbsMoney;

    private void Awake()
    {
        startBattleButton.onClick.AddListener(StartArenaBattle);
        startBattleButton.interactable = false;
    }

    void OnEnable()
    { RequestLeaderboard(); }
    
    public void RequestLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = "Arena", 
            StartPosition = 0,
            MaxResultsCount = 10
        }, result => DisplayLeaderboard(result), FailureCallback);
    }

    private void DisplayLeaderboard(GetLeaderboardResult result)
    {
        _leaderboardEntries = result.Leaderboard.FindAll(x=>x.PlayFabId!=DataContainer.Instance.playerData.playerProfileModel.PlayerId);
        bool playerOnBoard = false;

        for (int i = 0; i < _leaderboardNames.Length; i++)
        {
            _leaderboardNames[i].text = i < result.Leaderboard.Count ? result.Leaderboard[i].DisplayName : string.Empty;

            if (i >= result.Leaderboard.Count) continue;

            if (result.Leaderboard[i].PlayFabId == DataContainer.Instance.playerData.playerProfileModel.PlayerId)
            {
                _leaderboardNames[i].text = OrangeText(_leaderboardNames[i].text);
                _playerLeaderboardEntry = result.Leaderboard[i];
                UpdatePrizeViewer(_arenaPrizeDataUpdater.GetPrizeDataByPosition(_playerLeaderboardEntry.Position+1),
                    _playerLeaderboardEntry.Position+1);
                playerOnBoard = true;
            }
        }

        if (playerOnBoard)
        { startBattleButton.interactable = true; }
        else
        {
            PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest()
            {
                StatisticName = "Arena",
                MaxResultsCount = 10
            }, complite=>GetLeaderboardAround(complite), FailureCallback);
        }
    }

    private void GetLeaderboardAround(GetLeaderboardAroundPlayerResult complite)
    {
        _playerLeaderboardEntry = complite.Leaderboard.Find(x => x.PlayFabId == DataContainer.Instance.playerData.playerProfileModel.PlayerId);
        _leaderboardEntries = complite.Leaderboard.FindAll(x=>x.PlayFabId!=DataContainer.Instance.playerData.playerProfileModel.PlayerId);
        //TODO: пофиксить баг, после боя в компании вылазит баг в арене на этой строке

        if (_playerLeaderboardEntry != null)
        {
            var pos = _playerLeaderboardEntry.Position + 1;
            UpdatePrizeViewer(_arenaPrizeDataUpdater.GetPrizeDataByPosition(pos),
                pos);
        }

        startBattleButton.interactable = true;
    }

    void UpdatePrizeViewer(ArenaPriseGroup arenaPriseGroup, int playerPosition)
    {
        playerPositionView.text = string.Format(LocalizationManager.Localize("ARENA_RANK"), playerPosition);
        arenaGoldCount.text = arenaPriseGroup.goldCount.ToString();
        arenaExpCount.text = arenaPriseGroup.expCount.ToString();
    }

    private void FailureCallback(PlayFabError error){
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public void StartArenaBattle()
    {
        if (DataContainer.Instance.playerData.playerStaff.arenaCount <= 0)
        {
            if (DataContainer.Instance.playerData.playerStaff.arenaDay == DateTime.Today.Day)
            {
                menuAbsMoney.SetActive(true);
            }
            else if (DataContainer.Instance.playerData.playerStaff.arenaDay != DateTime.Today.Day)
            {
                DataContainer.Instance.playerData.playerStaff.arenaCount = 3;
                DataContainer.Instance.playerData.playerStaff.arenaCount--;
                PlayerData.SetData(DataContainer.Instance.playerData);
                PlayArena();
            }
        }
        else
        {
            DataContainer.Instance.playerData.playerStaff.arenaDay = DateTime.Today.Day;
            DataContainer.Instance.playerData.playerStaff.arenaCount--;
            PlayerData.SetData(DataContainer.Instance.playerData);
            PlayArena();
        }
    }

    public void ToPlay()
    {
        if (DataContainer.Instance.playerData.playerStaff.goldCount >= 25)
        {
            var enemy = _leaderboardEntries[UnityEngine.Random.Range(0, _leaderboardEntries.Count)];

            BattleDataContainer.ArenaBattle(enemy.PlayFabId, enemy.StatValue, _playerLeaderboardEntry.StatValue, enemy.DisplayName);
            SceneManager.LoadScene("Battle");
        }
        else toPlay.interactable = false;

    }

    public void PlayArena()
    {
        var enemy = _leaderboardEntries[UnityEngine.Random.Range(0, _leaderboardEntries.Count)];

        BattleDataContainer.ArenaBattle(enemy.PlayFabId, enemy.StatValue, _playerLeaderboardEntry.StatValue, enemy.DisplayName);
        SceneManager.LoadScene("Battle");
    }

    private string OrangeText(string text)
    { return string.Format("<color=yellow>{0}</color>", text); }
}