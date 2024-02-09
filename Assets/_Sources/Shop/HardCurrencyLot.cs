using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HardCurrencyLot : MonoBehaviour
{
    public HCLot hcType;
    [SerializeField] private TextMeshProUGUI priceView;
    [SerializeField] private TextMeshProUGUI countView;
    public PurchaseController purchaseController;
    private HcLotData _hcLotData;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        switch (hcType)
        {
            case HCLot.SOFT_CURRENCY_LOT_1:
                _hcLotData = new HcLotData() {price = 60, productCount = 250};
                break;
            case HCLot.SOFT_CURRENCY_LOT_2:
                _hcLotData = new HcLotData() {price = 500, productCount = 2500};
                break;
            case HCLot.SOFT_CURRENCY_LOT_3:
                _hcLotData = new HcLotData() {price = 4500, productCount = 25000};
                break;
            case HCLot.RARE_CHEST:
                _hcLotData = new HcLotData() {price = 600, productCount = 4};
                break;
            case HCLot.LEGENDARY_CHEST:
                _hcLotData = new HcLotData() {price = 3000, productCount = 10};
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        priceView.text = _hcLotData.price.ToString();
        countView.text = _hcLotData.productCount.ToString();
        _button.onClick.AddListener(ValidateLot);
    }

    private void ValidateLot()
    {
        switch (hcType)
        {
            case HCLot.SOFT_CURRENCY_LOT_1:
            case HCLot.SOFT_CURRENCY_LOT_2:
            case HCLot.SOFT_CURRENCY_LOT_3:
                if(DataContainer.Instance.playerData.playerStaff.ruby>=_hcLotData.price)
                    purchaseController.BuyGold(_hcLotData);
                else
                {
                    ShopSwitcher.OnOpenShopPanel?.Invoke(ShopPanelType.Vip);
                }
                break;
            case HCLot.RARE_CHEST:
                if(DataContainer.Instance.playerData.playerStaff.ruby>=_hcLotData.price)
                    purchaseController.BuyRareBackpack();
                else
                {
                    ShopSwitcher.OnOpenShopPanel?.Invoke(ShopPanelType.Vip);
                }
                break;
            case HCLot.LEGENDARY_CHEST:
                if(DataContainer.Instance.playerData.playerStaff.ruby>=_hcLotData.price)
                    purchaseController.BuyLegendaryBackpack();
                else
                {
                    ShopSwitcher.OnOpenShopPanel?.Invoke(ShopPanelType.Vip);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum HCLot
{
    SOFT_CURRENCY_LOT_1,
    SOFT_CURRENCY_LOT_2,
    SOFT_CURRENCY_LOT_3,
    RARE_CHEST,
    LEGENDARY_CHEST
}

public struct HcLotData
{
    public int price;
    public int productCount;
}