using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleResultWindow : MonoBehaviour
{
    [SerializeField] private Button getRewardButton;
    //[SerializeField] private Button adRewardButton;
    [SerializeField] private TextMeshProUGUI baseExp;
    [SerializeField] private TextMeshProUGUI baseGold;
    [SerializeField] private TextMeshProUGUI bigExp;
    [SerializeField] private TextMeshProUGUI bigGold;
    private DeckInfo _deckInfo;
    
    private void Awake()
    {
        getRewardButton.onClick.AddListener(GetReward);
        //adRewardButton.onClick.AddListener(ShowAdReward);
    }

    public void ShowAdReward()
    {
        DataContainer.Instance.playerData.playerStaff.goldCount += _deckInfo.rewardGold*2;
        DataContainer.Instance.playerData.playerGameProgress.exp += _deckInfo.rewardExp*2;
        var playerLvl = DataContainer.Instance.playerData.playerGameProgress.lvl;
        var lvlData = DataContainer.Instance.cardDataContainer.metaGameData.lvlDatas.Find(x => x.lvl == playerLvl);
        float expForNextLvl;

        if (lvlData.lvl!=0)
        { expForNextLvl = lvlData.exp; }
        else
        { expForNextLvl = playerLvl * 150; }

        if (DataContainer.Instance.playerData.playerGameProgress.exp > expForNextLvl)
        {
            DataContainer.Instance.playerData.playerGameProgress.exp =
            DataContainer.Instance.playerData.playerGameProgress.exp - expForNextLvl;
            DataContainer.Instance.playerData.playerGameProgress.lvl++;
            BattleDataContainer.IsLvlUp = true;
        }

        //var eventsParams = new CustomEventParams();
        //eventsParams.AddParam("type", "story");
        //eventsParams.AddParam("level", DataContainer.Instance.playerData.playerGameProgress.GetCurentLvl(BattleDataContainer.CurentPlayerFraction)-1);
        //eventsParams.AddParam("result", "win");
        //eventsParams.AddParam("health_left", FindObjectOfType<LeaderViewer>().GetCurentHealth());
        //eventsParams.AddParam("reward_soft", _deckInfo.rewardGold);
        //eventsParams.AddParam("reward_exp", _deckInfo.rewardExp);
        //eventsParams.AddParam("ads_watch", 0);
        //DevToDev.Analytics.CustomEvent("level_finish", eventsParams);
        DevToDevTamplates.SoftCurrency("reward", "story_win_ad", _deckInfo.rewardGold);
        PlayerData.SetData(DataContainer.Instance.playerData, LoadMainMenu);
    }

    private void GetReward()
    {
        DataContainer.Instance.playerData.playerStaff.goldCount += _deckInfo.rewardGold;
        BalanceAnalytics.GettingGold(CurrencySource.Company, _deckInfo.rewardGold);
        DataContainer.Instance.playerData.playerGameProgress.exp += _deckInfo.rewardExp;
        var playerLvl = DataContainer.Instance.playerData.playerGameProgress.lvl;
        var lvlData = DataContainer.Instance.cardDataContainer.metaGameData.lvlDatas.Find(x => x.lvl == playerLvl);
        float expForNextLvl;

        if (lvlData.lvl!=0)
        { expForNextLvl = lvlData.exp; }
        else
        { expForNextLvl = playerLvl * 150; }

        if (DataContainer.Instance.playerData.playerGameProgress.exp > expForNextLvl)
        {
            DataContainer.Instance.playerData.playerGameProgress.exp =
            DataContainer.Instance.playerData.playerGameProgress.exp - expForNextLvl;
            DataContainer.Instance.playerData.playerGameProgress.lvl++;
            BattleDataContainer.IsLvlUp = true;
        }

        //var eventsParams = new CustomEventParams();
        //eventsParams.AddParam("type", "story");
        //eventsParams.AddParam("level", DataContainer.Instance.playerData.playerGameProgress.GetCurentLvl(BattleDataContainer.CurentPlayerFraction)-1);
        //eventsParams.AddParam("result", "win");
        //eventsParams.AddParam("health_left", FindObjectOfType<LeaderViewer>().GetCurentHealth());
        //eventsParams.AddParam("reward_soft", _deckInfo.rewardGold);
        //eventsParams.AddParam("reward_exp", _deckInfo.rewardExp);
        //eventsParams.AddParam("ads_watch", 0);
        //DevToDev.Analytics.CustomEvent("level_finish", eventsParams); 
        DevToDevTamplates.SoftCurrency("reward", "story_win", _deckInfo.rewardGold);
        PlayerData.SetData(DataContainer.Instance.playerData, LoadMainMenu);
    }

    void LoadMainMenu()
    { SceneManager.LoadScene("Main_menu"); }

    public void ShowReward(DeckInfo deckInfo)
    {
        _deckInfo = deckInfo;
        baseExp.text = deckInfo.rewardExp.ToString();
        baseGold.text = deckInfo.rewardGold.ToString();
        bigExp.text = (deckInfo.rewardExp*2).ToString();
        bigGold.text = (deckInfo.rewardGold*2).ToString();
        gameObject.SetActive(true);
    }
}