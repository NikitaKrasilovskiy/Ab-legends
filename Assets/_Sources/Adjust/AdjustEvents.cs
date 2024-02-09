using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using AppsFlyerSDK;
using UnityEngine;
using UnityEngine.Purchasing;

public static class AdjustEvents
{
    private const string FIRST_IAP = "firstIap";
    private const string LEVEL = "Level";
    private const string STATUS = "Status";
    private const string WIN = "win";
    private const string LOSE = "lose";
    private const string PROGRESS = "Progress";
    private const string PLACEMENT = "Placement";
    private const string RV_FINISH = "RV_finish";
    private const string SKU = "SKU";
    private const string TRANSACTION_ID = "TRANSACTION_ID";
    private const string AF_PURCHASE = "af_purchase";
    private const string UNIQUE_PU = "Unique_pu";
    public static void Progress(int lvl, bool isWin)
    {
        var progressEvent = new Dictionary<string, string>
        {
            {LEVEL, lvl.ToString()}, {STATUS, isWin ? WIN : LOSE}
        };
        AppsFlyer.sendEvent(PROGRESS, progressEvent);
        LogEvent(PROGRESS, lvl.ToString()+";"+isWin);
    }

    public static void RvFinish(string placement)
    {
        var rvFinishEvent = new Dictionary<string, string> {{PLACEMENT, placement}};
        AppsFlyer.sendEvent(RV_FINISH, rvFinishEvent);
        LogEvent(RV_FINISH, placement);
    }

    public static void AfPurchase(Product product)
    {
        var purchaseEvent = new Dictionary<string, string>
        {
            {AFInAppEvents.CURRENCY, product.metadata.isoCurrencyCode},
            {AFInAppEvents.REVENUE, product.metadata.localizedPrice.ToString(CultureInfo.InvariantCulture)},
            {AFInAppEvents.QUANTITY, 1.ToString()},
            {SKU, product.definition.id},
            {TRANSACTION_ID, product.transactionID}
        };
        AppsFlyer.sendEvent(AF_PURCHASE, purchaseEvent);
        LogEvent(AF_PURCHASE, product.definition.id);
        if(PlayerPrefs.HasKey(FIRST_IAP))
            return;
        PlayerPrefs.SetInt(FIRST_IAP,0);
        var uniquePuEvent = new Dictionary<string, string>();
        AppsFlyer.sendEvent(UNIQUE_PU, uniquePuEvent);
        LogEvent(UNIQUE_PU, null);
    }
    
    public static void AfPurchase(StoreProduct product)
    {
        var purchaseEvent = new Dictionary<string, string>
        {
            {AFInAppEvents.CURRENCY, product.isoCurrencyCode},
            {AFInAppEvents.REVENUE, product.localizedPriceString.ToString(CultureInfo.InvariantCulture)},
            {AFInAppEvents.QUANTITY, 1.ToString()},
            {SKU, product.idGooglePlay},
            {TRANSACTION_ID, "0"}
        };
        AppsFlyer.sendEvent(AF_PURCHASE, purchaseEvent);
        LogEvent(AF_PURCHASE, product.idGooglePlay);
        if(PlayerPrefs.HasKey(FIRST_IAP))
            return;
        PlayerPrefs.SetInt(FIRST_IAP,0);
        var uniquePuEvent = new Dictionary<string, string>();
        AppsFlyer.sendEvent(UNIQUE_PU, uniquePuEvent);
        LogEvent(UNIQUE_PU, null);
    }

    private static void LogEvent(string eventID, string data)
    {
        Debug.Log(string.Format("AdjEvent: {0}, Value: {1}",eventID,data));
    }
}
