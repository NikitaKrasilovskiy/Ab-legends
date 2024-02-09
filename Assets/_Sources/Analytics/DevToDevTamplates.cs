using System.Collections;
using System.Collections.Generic;
using DevToDev;
using UnityEngine;

public static class DevToDevTamplates
{
    public static void SoftCurrency(string category, string item_id, int value)
    {
        var eventParams = new CustomEventParams();
        eventParams.AddParam("category", category);
        eventParams.AddParam("item_id", item_id);
        eventParams.AddParam("value", value);
        Analytics.CustomEvent("soft_currency", eventParams);
    }
}
