using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopSwitcher : MonoBehaviour
{
    public static Action<ShopPanelType> OnOpenShopPanel;

    [Header("Список панелей с параметрами")]
    [SerializeField] private List<ShopPanel> panels;

    [Header("Заголовок типа")]
    [SerializeField] private TMP_Text titleText;

    [Header("Описание товаров")]
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private ButtonGroup buttonGroup;
    [SerializeField] private PanelManager panelManager;
    [SerializeField] private Panel panel;

    private void Start()
    {
        //OpenShopPanel(ShopPanelType.Backpacks);
        OnOpenShopPanel += OpenShopPanel;
    }
    private void OnDestroy()
    {
        OnOpenShopPanel -= OpenShopPanel;
    }
    private void OpenShopPanel(ShopPanelType panelType)
    {
        if(panelManager.curentPanel!=panel)
            panelManager.OpenPanel(panel);
        buttonGroup.ActivateButton((int)panelType);
        foreach (var panel in panels)
        {
            panel.gameObject.SetActive(panel.panelType == panelType);
            if (panel.panelType == panelType)
            {
                titleText.text = (panel.title != "") ? panel.title : panel.panelType.ToString();
                descriptionText.transform.parent.gameObject.SetActive(panel.description != "");
                descriptionText.text = panel.description;
            }
        }
    }
}

[Serializable]
public class ShopPanel
{
    public ShopPanelType panelType;
    public GameObject gameObject;
    public string title;
    public string description;
}

public enum ShopPanelType
{
    Backpacks,
    Coins,
    Vip,

}