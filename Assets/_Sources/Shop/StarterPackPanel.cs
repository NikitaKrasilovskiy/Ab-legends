using System;
using System.Collections;
using System.Collections.Generic;
using DevToDev;
using UnityEngine;

public class StarterPackPanel : MonoBehaviour
{
    public static bool show;
    [SerializeField] private PanelManager panelManager;
    private string PANEL_NAME = "STARTER_PACK";
    void Start()
    {
        if (show && !PlayerPrefs.HasKey(PANEL_NAME))
        {
            panelManager.OpenPanel(PANEL_NAME);
            show = false;
        }
    }
}

[Serializable]
public struct StarterPack
{
    public int goldCount;
    public int expCount;
    public string[] cards;

}
