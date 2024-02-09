using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LvlUpPanel : MonoBehaviour
{
    [SerializeField] private DataContainer _dataContainer;
    [SerializeField] private TextMeshProUGUI lvlView;
    [SerializeField] private Animator prizeAnimator;
    [SerializeField] private TextMeshProUGUI coinView;
    [SerializeField] private TextMeshProUGUI premiumView;
    [SerializeField] private Button prizeButton;
    private static readonly int Open = Animator.StringToHash("Open");
    private bool _continue = false;

    void OnEnable()
    {
        lvlView.text = DataContainer.Instance.playerData.playerGameProgress.lvl.ToString();
        prizeButton.interactable = true;
        DellayActivation();
    }

    async void DellayActivation()
    {
        await UniTask.Delay(7000);
        if (_continue)
        {
            return;
        }
        else
        {
            OpenPrize();
        }
    }

    public void OpenPrize()
    {
        _continue = true;
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "getLvlGift",
            GeneratePlayStreamEvent = true,
        }, ShowPrize, error => { Debug.LogError(error.Error);});
    }

    void ShowPrize(ExecuteCloudScriptResult result)
    {
        int goldCount = 50 * DataContainer.Instance.playerData.playerGameProgress.lvl;
        coinView.text = goldCount.ToString();
        premiumView.text = 0.ToString();
        DataContainer.Instance.playerData.playerStaff.goldCount += goldCount;
        DevToDev.Analytics.LevelUp(_dataContainer.playerData.playerGameProgress.lvl);
        //prizeAnimator.SetTrigger(Open);
        PlayerData.SetData(DataContainer.Instance.playerData, ()=>prizeAnimator.SetTrigger(Open));
    }

    public void Close()
    {
        prizeButton.interactable = true;
        _continue = false;
    }
}

[Serializable]
public class LvlUpPrize
{
    public int gold;
    public int premiumDays;
    public string cart;
}
