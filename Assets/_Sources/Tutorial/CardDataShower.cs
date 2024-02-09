using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataShower : MonoBehaviour
{
    [SerializeField] private CardDataPanel cardDataPanel;

    public void OpenDataPanelForUpgrade()
    {
        cardDataPanel.Open(DataContainer.Instance.playerData.acornDeck.warriorCards[0]);
    }

    public void OpenDataPanelForShell()
    {
        cardDataPanel.Open(DataContainer.Instance.playerData.cardCollection.warriorCards[0]);
    }
}
