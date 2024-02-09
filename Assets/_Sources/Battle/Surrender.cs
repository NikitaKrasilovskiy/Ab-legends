using System.Collections;
using System.Collections.Generic;
using DevToDev;
using UnityEngine;

public class Surrender : MonoBehaviour
{
    public void Surrend()
    {
        var eventsParams = new CustomEventParams();
        eventsParams.AddParam("type", BattleDataContainer.IsArenaBattle?"pvp":"story");
        eventsParams.AddParam("level", DataContainer.Instance.playerData.playerGameProgress.GetCurentLvl(BattleDataContainer.CurentPlayerFraction)-1);
        eventsParams.AddParam("result", "leave");
        eventsParams.AddParam("health_left", FindObjectOfType<LeaderViewer>().GetCurentHealth());
        eventsParams.AddParam("ads_watch", 0);
        //DevToDev.Analytics.CustomEvent("level_finish", eventsParams); 
    }
}
