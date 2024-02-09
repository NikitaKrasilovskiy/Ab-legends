using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AdsLoaderCompany : MonoBehaviour
{
    private BattleResultWindow battleResultWindow;

    private RewardedAd rewardAd;

    private string rewardAdUnitId;

    private int companyTicketAdStopLoad;

    void Awake()
    {
        MobileAds.Initialize(status => { });
    }

    public void Start()
    {
        rewardAdUnitId = "ca-app-pub-5853277310445367/4063361378";
        RewardLoad();
    }

    public void RewardLoad()
    {
        rewardAdUnitId = "ca-app-pub-5853277310445367/4063361378";

        rewardAd = new RewardedAd(rewardAdUnitId);

        rewardAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardAd.OnAdClosed += HandleRewardedAdClosed;
        rewardAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        rewardAd.OnAdFailedToLoad += HandleFailedToLoad;

        AdRequest request = new AdRequest.Builder().Build();
        rewardAd.LoadAd(request);
    }

    private void HandleFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        if (companyTicketAdStopLoad < 3)
        {
            companyTicketAdStopLoad++;
            RewardLoad();
        }
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs e)
    { }

    public void HandleRewardedAdClosed(object sender, EventArgs e)
    {

    }

    public void HandleUserEarnedReward(object sender, Reward e)
    {
        battleResultWindow = FindObjectOfType<BattleResultWindow>();
        battleResultWindow.ShowAdReward();
        companyTicketAdStopLoad = 0;
        RewardLoad();
    }
}