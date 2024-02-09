using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningShopPanel : MonoBehaviour
{
    public ShopPanelType panelType;

    public void OnOpenPanel()
    {
        ShopSwitcher.OnOpenShopPanel?.Invoke(panelType);
    }
}
