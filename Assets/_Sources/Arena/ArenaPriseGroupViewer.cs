using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using TMPro;
using UnityEngine;

public class ArenaPriseGroupViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldPrize;
    [SerializeField] private TextMeshProUGUI expPrize;
    [SerializeField] private TextMeshProUGUI positions;
    private const string POSITIONS = "ARENAHELPER_RANK";

    public void UpdateView(ArenaPriseGroup arenaPriseGroup)
    {
        goldPrize.text = arenaPriseGroup.goldCount.ToString();
        expPrize.text = arenaPriseGroup.expCount.ToString();
        var positionsString = arenaPriseGroup.startPlace == arenaPriseGroup.endPlace
            ? arenaPriseGroup.startPlace.ToString()
            : arenaPriseGroup.startPlace + "-" + arenaPriseGroup.endPlace;
        positions.text = string.Format(LocalizationManager.Localize(POSITIONS), positionsString);
    }
}
