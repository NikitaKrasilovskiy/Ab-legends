using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSaver : MonoBehaviour
{
    [SerializeField] private PanelManager panelManager;

    public void SaveAndClose()
    {
        PlayerData.SetData(DataContainer.Instance.playerData, Close, Close);    
    }

    private void Close()
    {
        panelManager.CloseCurentPanel();
    }
}
