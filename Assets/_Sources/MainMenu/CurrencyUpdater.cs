using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldView;
    [SerializeField] private TextMeshProUGUI giftView;
    [SerializeField] private TextMeshProUGUI rubyView;

    private void OnDisable()
    {
        PlayerData.RemoveUpdateListener(UpdateData);
    }

    private void OnEnable()
    {
        PlayerData.AddUpdateListener(UpdateData);
        UpdateData();
    }

    private void UpdateData()
    {
        if(goldView)
            goldView.text = DataContainer.Instance.playerData.playerStaff.goldCount.ToString();
        if(giftView)
            giftView.text = DataContainer.Instance.playerData.playerStaff.giftCount.ToString();
        if(rubyView)
            rubyView.text = DataContainer.Instance.playerData.playerStaff.ruby.ToString();
    }
}
