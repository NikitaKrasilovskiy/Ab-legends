using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class CardDataContainer
{
    private WarriorCardData _warriorCardData;
    public CompanyData bobberCompany;
    public MetaGameData metaGameData;
    public bool isInit = false;
    
    public CardDataContainer()
    {
        LoadCardCollection();
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public async UniTask LoadCardCollection()
    {
        await UniTask.WaitUntil(()=>PlayFabClientAPI.IsClientLoggedIn());
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), 
            LoadComplit,
            error => Debug.LogError(error.Error));
    }

    void LoadComplit(GetTitleDataResult result)
    {
        _warriorCardData = JsonUtility.FromJson<WarriorCardData>(result.Data["CardsData"]);
        bobberCompany = JsonUtility.FromJson<CompanyData>(result.Data["BobberCompany"]);
        metaGameData = JsonUtility.FromJson<MetaGameData>(result.Data["MetaGameData"]);
        isInit = true;
    }

    public WarriorCard GetWarriorCard(string id)
    {
 //       Debug.Log(id);
        var warriorCard = _warriorCardData.cards.Find(x => x.id.Equals(id)) ?? new WarriorCard();
        return warriorCard;
    }

    public WarriorCard GetRandomWarriorCard(Fraction fraction = Fraction.Acorn, int minLvl = 0, int maxLvl = 10)
    {
        
        var warriorCards = _warriorCardData.cards.FindAll(x => (x.fraction==fraction)&&(x.lvl>=minLvl)&&(x.lvl<=maxLvl));
        var warriorCard = warriorCards[Random.Range(0, warriorCards.Count)];
        return warriorCard;
    }

    public LeaderCard GetAcornLeader(int lvl)
    {
        var leaderCard = new LeaderCard();
        leaderCard.id = "acorn_leader";
        leaderCard.health = 10*lvl;
        leaderCard.fraction = Fraction.Acorn;
        return leaderCard;
    }

    public LeaderCard GetLeader(Fraction fraction, int lvl)
    {
        switch (fraction)
        {
            case Fraction.Acorn:
            {
                return GetAcornLeader(lvl);
            }
            case Fraction.Bobber:
            {
                return GetBobberLeader(lvl);
            }
            case Fraction.Candle:
            {
                return GetCandleLeader(lvl);
            }
        }
        return GetAcornLeader(lvl);
    }

    public LeaderCard GetCandleLeader(int lvl)
    {
        var leaderCard = new LeaderCard();
        leaderCard.id = "candle_leader";
        leaderCard.health = 10*lvl;
        leaderCard.fraction = Fraction.Candle;
        return leaderCard;
    }

    public LeaderCard GetBobberLeader(int lvl)
    {
        var leaderCard = new LeaderCard();
        leaderCard.id = "bobber_leader";
        leaderCard.health = 10*lvl;
        leaderCard.fraction = Fraction.Bobber;
        return leaderCard;
    }

    public List<WarriorCard> GetAllEvolution(string cardId)
    {
        var id = cardId.Remove(cardId.Length - 2, 2);
        Debug.Log(id);
        var cardList = _warriorCardData.cards.FindAll(x => x.id.Contains(id));
        return cardList;
    }

    public WarriorCard GetNextEvolution(WarriorCard warriorCard)
    {
        var id = warriorCard.id.Remove(warriorCard.id.Length - 2, 2);
        var card = _warriorCardData.cards.Find(x => x.id.Contains(id)&&x.lvl==warriorCard.lvl+1);
        return card;
    }
    
    public WarriorCard GetPrevEvolution(WarriorCard warriorCard)
    {
        var id = warriorCard.id.Remove(warriorCard.id.Length - 2, 2);
        var card = _warriorCardData.cards.Find(x => x.id.Contains(id)&&x.lvl==warriorCard.lvl-1);
        return card;
    }

}
