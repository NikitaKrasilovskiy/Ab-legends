using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public Panel mainPanel;
    public List<Panel> panelList = new List<Panel>();
    public Panel curentPanel;
    Panel lastPanel;

    private void Start()
    {
        foreach(var item in panelList)
        {
            item.Close();
        }
        mainPanel.Open();
        curentPanel = mainPanel;
    }

    public void CloseCurentPanel()
    {
        curentPanel.Close();
        var c = curentPanel;
        mainPanel.Open();
        curentPanel = mainPanel;
        lastPanel = c;
    }

    public void OpenPanel(string id)
    {
        OpenPanel(panelList.FindLast((x) => x.panelId == id));
    }

    public void OpenPanel(Panel panel)
    {
        if (panel == null)
            return;
        panel.Open();
        curentPanel.Close();
        lastPanel = curentPanel;
        curentPanel = panel;
    }
}
