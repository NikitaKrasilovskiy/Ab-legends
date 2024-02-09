using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPrize : MonoBehaviour
{
    [SerializeField] private OpenBoxPanel openBoxPanel;

    public void ShowPrize()
    {
        PrizeData prizeData = new PrizeData();
        prizeData.goldCount = 100;
        prizeData.expCount = 200;
        prizeData.cardCount = 1;
        prizeData.prizeType = PrizeType.Big;
        openBoxPanel.ShowPrize(prizeData);
    }
}
