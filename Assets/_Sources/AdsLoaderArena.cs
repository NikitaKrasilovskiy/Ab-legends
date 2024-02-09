using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsLoaderArena : MonoBehaviour
{
    private ArenaController arenaController;

    private RewardedAd rewardAd;

    private string rewardAdUnitId;
    
    private int arenaTicketAdStopLoad;
    
    void Awake()
    {
        MobileAds.Initialize(status => {});
    }
    
    public void Start()
    {
        rewardAdUnitId = "ca-app-pub-5853277310445367/2832189118";
        RewardLoad();
    }

    public void RewardLoad()
    {
        rewardAdUnitId = "ca-app-pub-5853277310445367/2832189118";

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
        if (arenaTicketAdStopLoad < 3)
        {
            arenaTicketAdStopLoad++;
            RewardLoad();
        }
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        arenaController = FindObjectOfType<ArenaController>();
        arenaController.PlayArena();
        arenaTicketAdStopLoad = 0;
        RewardLoad();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs e)
    {
        
    }

    public void HandleUserEarnedReward(object sender, Reward e)
    { }
}