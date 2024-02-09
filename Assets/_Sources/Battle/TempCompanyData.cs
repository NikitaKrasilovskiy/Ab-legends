using System;
using System.Collections.Generic;
using UnityEngine;

public class TempCompanyData : MonoBehaviour
{
    public List<DeckInfo> acornLvls;
    public List<DeckInfo> bobberLvls;
    [SerializeField] private TextAsset textAsset;
    public int i = 0;
    public void OnValidate()
    {
        if(textAsset==null)
            return;
        var json = new JSONObject(textAsset.text);
        acornLvls = new List<DeckInfo>();
        foreach (var page in json.GetField("campaigns")[0].list)
        {
            foreach (var areas in page.GetField("areas").list)
            {
                foreach (var lvl in areas.list)
                {
                    var cards = lvl.GetField("cards");
                    var deckInfo = new DeckInfo();
                    deckInfo.fraction = Fraction.Bobber;
//                    Debug.Log(cards);
                    deckInfo.leaderLvl = (int)cards[0].GetField("level").i;
                    var listIds = new List<string>();
                    for (int i = 1; i < cards.Count; i++)
                    {
                        int id = (int) cards[i].GetField("id").i;
                        if (id != -1)
                        {
                            listIds.Add(ConvertId(id, (int)cards[i].GetField("level").i));
                        }
                    }
                    deckInfo.wariorIds = listIds.ToArray();
                    deckInfo.rewardGold = 10;
                    deckInfo.rewardExp = 100;
                    acornLvls.Add(deckInfo);
                }
            }
        }

        CompanyContainer cc = new CompanyContainer();
        cc.deckInfos = acornLvls;
        Debug.Log(JsonUtility.ToJson(cc));
        bobberLvls = new List<DeckInfo>();
        foreach (var page in json.GetField("campaigns")[1].list)
        {
            foreach (var areas in page.GetField("areas").list)
            {
                foreach (var lvl in areas.list)
                {
                    var cards = lvl.GetField("cards");
                    var deckInfo = new DeckInfo();
                    deckInfo.fraction = Fraction.Candle;
                    deckInfo.leaderLvl = (int)cards[0].GetField("level").i;
                    var listIds = new List<string>();
                    for (int i = 1; i < cards.Count; i++)
                    {
                        int id = (int) cards[i].GetField("id").i;
                        if (id != -1)
                        {
                            listIds.Add(ConvertId(id, (int)cards[i].GetField("level").i));
                        }
                    }
                    deckInfo.wariorIds = listIds.ToArray();
                    deckInfo.rewardGold = 10;
                    deckInfo.rewardExp = 100;
                    bobberLvls.Add(deckInfo);
                    if(bobberLvls.Count>=90)
                        break;
                }
                if(bobberLvls.Count>=90)
                    break;
            }
        }
        cc.deckInfos = bobberLvls;
        Debug.Log(JsonUtility.ToJson(cc));
    }

    string ConvertId(int id, int lvl)
    {
        string result = "acorn0_1";
        switch (id)
        {
            case 1:
            {
                result = "accorn1";
                break;
            }
            case 2:
            {
                result = "accorn2";
                break;
            }
            case 3:
            {
                result = "accorn4";
                break;
            }
            case 4:
            {
                result = "accorn3";
                break;
            }
            case 5:
            {
                result = "accorn0";
                break;
            }
            case 7:
            {
                result = "bobber0";
                break;
            }
            case 8:
            {
                result = "bobber1";
                break;
            }
            case 9:
            {
                result = "bobber2";
                break;
            }
            case 10:
            {
                result = "bobber3";
                break;
            }
            case 11:
            {
                result = "bobber4";
                break;
            }
            case 13:
            {
                result = "candle0";
                break;
            }
            case 14:
            {
                result = "candle1";
                break;
            }
            case 15:
            {
                result = "candle2";
                break;
            }
            case 16:
            {
                result = "candle3";
                break;
            }
            case 17:
            {
                result = "candle4";
                break;
            }
        }

        result += "_" + lvl;
        return result;
    }
}

[Serializable]
public class CompanyContainer
{
    public List<DeckInfo> deckInfos;
}

[Serializable]
public struct DeckInfo
{
    public Fraction fraction;
    public int leaderLvl;
    public string[] wariorIds;
    public int rewardExp;
    public int rewardGold;
    public string rewardCard;
}