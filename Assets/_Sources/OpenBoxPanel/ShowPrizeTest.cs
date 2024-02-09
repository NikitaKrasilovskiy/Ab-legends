using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPrizeTest : MonoBehaviour
{
    [SerializeField] private OpenBoxPanel openBoxPanel;
    [SerializeField] private PrizeData[] prizeDatas;
    [SerializeField] private Button btn;

    public void Show()
    {
        if(DataContainer.Instance.playerData.playerStaff.giftCount<=0)
            return;
        openBoxPanel.ShowPrize(prizeDatas[Mathf.Clamp((int)DataContainer.Instance.playerData.playerStaff.
            giftsList.gifts[0],0,prizeDatas.Length)]);
    }
}
