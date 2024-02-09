using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftsTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _view;
    [SerializeField] private Button btn;
    private const string TIMER_STRING = "{0}:{1}:{2}"; 
    private DateTime _dateTime;
    [SerializeField]
    private string _giftId = "Gift";

    [SerializeField] private PlayerDataViewer playerDataViewer;
    private bool isBusy = false;

    private void Awake()
    {
        btn.onClick.AddListener(OnClick);
    }

    public void UpdateGiftData(long mil, string giftId)
    {
        _dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddMilliseconds(mil);
        _giftId = giftId;
    }

    void OnClick()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = _giftId.Equals("Gift")?"getGifts":"getArenaGifts",
            GeneratePlayStreamEvent = true,
            FunctionParameter = new { inputValue = _giftId }
        }, (result)=>
        {
            AddGift(result);
        }, error => { Debug.LogError(error.Error);});
        isBusy = true;
        btn.interactable = false;
    }

    private async UniTask AddGift(ExecuteCloudScriptResult result)
    {
        JsonObject o = (JsonObject) result.FunctionResult;
        JSONObject jo = new JSONObject(o.ToString());
        int gt = (int)jo.GetField("GiftSize").i;
        DataContainer.Instance.playerData.playerStaff.giftsList.gifts.Add((PrizeType)gt);
        PlayerData.SetData(DataContainer.Instance.playerData, UpdateData);
        
        
    }

    public async  void UpdateData()
    {
        await playerDataViewer.SyncAndUpdate();
        isBusy = false;
    }

    void Update()
    {
        TimeSpan timeSpan = _dateTime-DateTime.Now.ToUniversalTime();
        if (timeSpan.TotalSeconds > 0)
            _view.text = string.Format(TIMER_STRING, ToDoubleFormat(timeSpan.Hours), ToDoubleFormat(timeSpan.Minutes), ToDoubleFormat(timeSpan.Seconds));
        if(!isBusy)
            btn.interactable = timeSpan.TotalSeconds <= 0;

    }

    string ToDoubleFormat(int i)
    {
        string s = "";
        if (i < 10)
        {
            s += "0" + i;
        }
        else
        {
            s += i;
        }

        return s;
    }
}
