using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public static class BalanceAnalytics 
{
    public static void GettingGold(CurrencySource currencySource, int count)
    {
        var data = new Dictionary<string, object> {{"Source", currencySource.ToString()}, {"Count", count}};
        Analytics.CustomEvent("GettingGold", data);
    }
    
    public static void GettingRuby(CurrencySource currencySource, int count)
    {
        var data = new Dictionary<string, object> {{"Source", currencySource.ToString()}, {"Count", count}};
        Analytics.CustomEvent("GettingRuby", data);
    }

    public static void TutorialStep(int step)
    {
        var data = new Dictionary<string, object> {{"Step", step}};
        Analytics.CustomEvent("Tutorial", data);
    }

    public static void LvlComplite(int lvl, bool isWin)
    {
        var data = new Dictionary<string, object> {{"Lvl", lvl}, {"Result", isWin?"win":"lose"}};
        Analytics.CustomEvent("CompanyLvl", data);
    }
}

public enum CurrencySource
{
    Shop,
    Company,
    Chest
}
