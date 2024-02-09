using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerDataViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldCount;
    [SerializeField] private TextMeshProUGUI giftCount;
    [SerializeField] private TextMeshProUGUI rubyCount;
    [SerializeField] private Button giftButton;
    [SerializeField] private TextMeshProUGUI winRate;
    [SerializeField] private TextMeshProUGUI lvl;
    [SerializeField] private Image lvlProgress;
    [SerializeField] private GiftsTimer daylyGift;
    [SerializeField] private GiftsTimer arenaGift;
    [SerializeField] private DailyTasksView dailyTasksView;
    [SerializeField] private TextMeshProUGUI tasksProgress;
    [SerializeField] private GameObject complitedTasksView;
    private const string WIN_RATE = "PROFILE_STATWINRATE";
    private bool _isSyncComplite = false;
    private void Awake()
    {
        PlayerData.AddUpdateListener(UpdateData);
    }

    private void OnDisable()
    {
        PlayerData.RemoveUpdateListener(UpdateData);
    }

    void OnEnable()
    {
        SyncAndUpdate();
    }

    public async UniTask SyncAndUpdate()
    {
        _isSyncComplite = false;
        PlayerData.GetData(DataContainer.Instance.playerData.playerId, DataContainer.Instance.playerData, UpdateData);
        await UniTask.WaitUntil(()=>_isSyncComplite);
    }

    public void UpdateData()
    {
        goldCount.text = DataContainer.Instance.playerData.playerStaff.goldCount.ToString();
        giftCount.text = DataContainer.Instance.playerData.playerStaff.giftCount.ToString();
        rubyCount.text = DataContainer.Instance.playerData.playerStaff.ruby.ToString();
        giftButton.interactable = DataContainer.Instance.playerData.playerStaff.giftCount > 0;
        winRate.text = string.Format(LocalizationManager.Localize(WIN_RATE), DataContainer.Instance.playerData.playerGameProgress.winRate/100);
        lvl.text = DataContainer.Instance.playerData.playerGameProgress.lvl.ToString();
//        Debug.Log(JsonUtility.ToJson(_playerData.playerGameProgress.dailyTasks));
        dailyTasksView.UpdateTasks(DataContainer.Instance.playerData.playerGameProgress.dailyTasks);
        var complitedTasks = DataContainer.Instance.playerData.playerGameProgress.dailyTasks.tasks.FindAll(x => x.isComplited);
        tasksProgress.text = string.Format("{0}/{1}", complitedTasks.Count,
            DataContainer.Instance.playerData.playerGameProgress.dailyTasks.tasks.Count);
        var radyTasks = DataContainer.Instance.playerData.playerGameProgress.dailyTasks.tasks.FindAll(x => x.progress>=x.stageCount);
        complitedTasksView.SetActive(radyTasks.Count>complitedTasks.Count);
        var expForNextLvl = 1000 + 100 * DataContainer.Instance.playerData.playerGameProgress.lvl;
        lvlProgress.fillAmount = DataContainer.Instance.playerData.playerGameProgress.exp / (float)expForNextLvl;
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "getGiftsInfo",
            GeneratePlayStreamEvent = true,
        }, UpdateGiftsTimers, error => { });
    }

    void UpdateGiftsTimers(ExecuteCloudScriptResult result)
    {
        JsonObject o = (JsonObject) result.FunctionResult;
        object gift;
        o.TryGetValue("Gift", out gift);
        object giftTime = null;
        ((JsonObject) gift)?.TryGetValue("Value", out giftTime);
        long gt = 0;
        long.TryParse((string)giftTime, out gt);
        daylyGift.UpdateGiftData(gt, "Gift");
        gift = null;
        giftTime = null;
        gt = 0;
        o.TryGetValue("ArenaGift", out gift);
        ((JsonObject) gift)?.TryGetValue("Value", out giftTime);
        long.TryParse((string)giftTime, out gt);
        arenaGift.UpdateGiftData(gt, "ArenaGift");
        _isSyncComplite = true;
    }
}
