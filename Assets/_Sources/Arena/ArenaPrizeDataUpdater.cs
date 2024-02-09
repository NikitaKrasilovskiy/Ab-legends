using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaPrizeDataUpdater : MonoBehaviour
{
    [SerializeField] private ArenaPriseGroupViewer[] arenaPriseGroupViewers;
    public List<ArenaPriseGroup> arenaPriceGroups;
    private void OnEnable()
    {
        LoadTestData();
    }

    void LoadTestData()
    {
        arenaPriceGroups = new List<ArenaPriseGroup>();
        arenaPriceGroups.Add(new ArenaPriseGroup(){startPlace = 1,endPlace = 1, expCount = 150,goldCount = 100});
        arenaPriceGroups.Add(new ArenaPriseGroup(){startPlace = 2,endPlace = 2, expCount = 125,goldCount = 70});
        arenaPriceGroups.Add(new ArenaPriseGroup(){startPlace = 3,endPlace = 3, expCount = 100,goldCount = 50});
        arenaPriceGroups.Add(new ArenaPriseGroup(){startPlace = 4,endPlace = 10, expCount = 50,goldCount = 25});
        arenaPriceGroups.Add(new ArenaPriseGroup(){startPlace = 11,endPlace = 50, expCount = 30,goldCount = 10});
        arenaPriceGroups.Add(new ArenaPriseGroup(){startPlace = 51,endPlace = 100, expCount = 20,goldCount = 5});
        for (int i = 0; i < arenaPriceGroups.Count; i++)
        {
            if(i<arenaPriseGroupViewers.Length)
                arenaPriseGroupViewers[i].UpdateView(arenaPriceGroups[i]);            
        }

    }

    public ArenaPriseGroup GetPrizeDataByPosition(int position)
    {
        if(arenaPriceGroups==null)
            LoadTestData();
        return arenaPriceGroups.Find(x => position >= x.startPlace && position <= x.endPlace);
    }
}
