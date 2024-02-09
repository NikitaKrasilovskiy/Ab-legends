using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RealMoneyLot : MonoBehaviour
{
    public string purId;
    [SerializeField] private PurchaseController _purchaseController;
    [SerializeField] private UnityEvent onError; 
    
    public void Buy()
    {
        IAPManager.Instance.BuyProduct(IAPManager.Instance.ConvertNameToShopProduct(purId), ProductBought);
    }

    private void ProductBought(IAPOperationStatus arg0, string arg1, StoreProduct arg2)
    {
        if (arg0 == IAPOperationStatus.Success)
        {
            _purchaseController.ValidateIAP(arg2);
        }
        else
        {
            onError.Invoke();   
        }
    }
}
