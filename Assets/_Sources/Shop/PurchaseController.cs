using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseController : MonoBehaviour
{
    [SerializeField] private GameObject blockScreen;

    public void BlockScreen(bool enable = true)
    {
        blockScreen.SetActive(enable);
    }

    public void ValidateIAP(Product product)
    {
        AdjustEvents.AfPurchase(product);
        switch (product.definition.id)
        {
            case "hard_1_new":
                {
                    DataContainer.Instance.playerData.playerStaff.ruby += 200;
                    BalanceAnalytics.GettingRuby(CurrencySource.Shop, 200);
                    break;
                }
            case "hard_2_new":
                {
                    DataContainer.Instance.playerData.playerStaff.ruby += 500;
                    BalanceAnalytics.GettingRuby(CurrencySource.Shop, 500);
                    break;
                }
            case "hard_3_new":
                {
                    DataContainer.Instance.playerData.playerStaff.ruby += 1200;
                    BalanceAnalytics.GettingRuby(CurrencySource.Shop, 1200);
                    break;
                }
            case "hard_4_new":
                {
                    DataContainer.Instance.playerData.playerStaff.ruby += 2600;
                    BalanceAnalytics.GettingRuby(CurrencySource.Shop, 2600);
                    break;
                }
            case "hard_5_new":
                {
                    DataContainer.Instance.playerData.playerStaff.ruby += 7000;
                    BalanceAnalytics.GettingRuby(CurrencySource.Shop, 7000);
                    break;
                }
            case "hard_6_new":
                {
                    DataContainer.Instance.playerData.playerStaff.ruby += 15000;
                    BalanceAnalytics.GettingRuby(CurrencySource.Shop, 15000);
                    break;
                }
            case "starterpack_sale":
                {
                    DataContainer.Instance.playerData.playerStaff.ruby += 150;
                    DataContainer.Instance.playerData.playerStaff.goldCount += 900;
                    DataContainer.Instance.playerData.playerStaff.giftsList.gifts.Add(PrizeType.Big);
                    DataContainer.Instance.playerData.playerStaff.giftsList.gifts.Add(PrizeType.Big);
                    DataContainer.Instance.playerData.playerStaff.giftsList.gifts.Add(PrizeType.Big);
                    DataContainer.Instance.playerData.playerStaff.giftsList.gifts.Add(PrizeType.Big);
                    break;
                }
        }
        //FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventPurchase);
        PlayerData.SetData(DataContainer.Instance.playerData,
            () => blockScreen.SetActive(false));
    }

    public void ValidateIAP(StoreProduct product)
    {
        AdjustEvents.AfPurchase(product);
        switch (product.productName)
        {
            case "hard_1":
                {
                    DataContainer.Instance.playerData.playerStaff.ruby += 200;
                    break;
                }
            case "hard_2":
                {
                    DataContainer.Instance.playerData.playerStaff.ruby += 500;
                    break;
                }
            case "hard_3":
                {
                    DataContainer.Instance.playerData.playerStaff.ruby += 1200;
                    break;
                }
            case "hard_4":
                {
                    DataContainer.Instance.playerData.playerStaff.ruby += 2600;
                    break;
                }
            case "hard_5":
                {
                    DataContainer.Instance.playerData.playerStaff.ruby += 7000;
                    break;
                }
            case "hard_6":
                {
                    DataContainer.Instance.playerData.playerStaff.ruby += 15000;
                    break;
                }
            case "starterpack_sale":
                {
                    DataContainer.Instance.playerData.playerStaff.ruby += 150;
                    DataContainer.Instance.playerData.playerStaff.goldCount += 900;
                    DataContainer.Instance.playerData.playerStaff.giftsList.gifts.Add(PrizeType.Big);
                    DataContainer.Instance.playerData.playerStaff.giftsList.gifts.Add(PrizeType.Big);
                    DataContainer.Instance.playerData.playerStaff.giftsList.gifts.Add(PrizeType.Big);
                    DataContainer.Instance.playerData.playerStaff.giftsList.gifts.Add(PrizeType.Big);
                    break;
                }
        }
        PlayerData.SetData(DataContainer.Instance.playerData,
            () => blockScreen.SetActive(false));
    }

    public void BuyGold(HcLotData hcLotData)
    {
        DataContainer.Instance.playerData.playerStaff.ruby -= hcLotData.price;
        DataContainer.Instance.playerData.playerStaff.goldCount += hcLotData.productCount;
        BalanceAnalytics.GettingGold(CurrencySource.Shop, hcLotData.productCount);
        PlayerData.SetData(DataContainer.Instance.playerData,
            () => blockScreen.SetActive(false));
    }

    public void BuyRareBackpack()
    {
        for (var i = 0; i < 4; i++)
            DataContainer.Instance.playerData.playerStaff.giftsList.gifts.Add(PrizeType.Big);
        DataContainer.Instance.playerData.playerStaff.ruby -= 600;
        PlayerData.SetData(DataContainer.Instance.playerData,
            () => blockScreen.SetActive(false));
    }

    public void BuyLegendaryBackpack()
    {
        for (var i = 0; i < 10; i++)
            DataContainer.Instance.playerData.playerStaff.giftsList.gifts.Add(PrizeType.Large);
        DataContainer.Instance.playerData.playerStaff.ruby -= 3000;
        PlayerData.SetData(DataContainer.Instance.playerData,
            () => blockScreen.SetActive(false));
    }
}
