using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyButton : MonoBehaviour
{
    public ShopPanelType shopPanelType;
    
    public void OpenShopPanel()
    {
        ShopSwitcher.OnOpenShopPanel?.Invoke(shopPanelType);
    }
}
