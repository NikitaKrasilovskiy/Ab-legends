using System.Collections;
using System.Collections.Generic;
using PlayFab;
using UnityEngine;

public class BattleResultHandler : MonoBehaviour
{
    [SerializeField] private PanelManager panelManager; 
    [SerializeField] private CampsController campsController;
    [SerializeField] private LvlUpPanel _lvlUpPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        Show();
    }

    async void Show()
    {
        if(!TutorialController.Instance.isTutorialComplete)
            return;
        if (BattleDataContainer.IsCompanyBattle)
        {
            panelManager.OpenPanel("AcornCamp");
            switch (BattleDataContainer.CurentPlayerFraction)
            {
                case Fraction.Acorn:
                {
                    campsController.ShowAcornCompany(true);
                    break;
                }
                case Fraction.Bobber:
                {
                    campsController.ShowBobberCompany(true);
                    break;
                }
            }
        }

        if (BattleDataContainer.IsArenaBattle)
        {
            panelManager.OpenPanel("Arena");
        }

        if (BattleDataContainer.IsLvlUp)
        {
            Debug.Log("LVL UUUUUUUP");
            BattleDataContainer.IsLvlUp = false;
            _lvlUpPanel.gameObject.SetActive(true);
        }
    }
}
