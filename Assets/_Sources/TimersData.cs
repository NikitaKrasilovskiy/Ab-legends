using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimersData : MonoBehaviour
{
    public TextMeshProUGUI textTimer;
    private ArenaController arenaController;
    public Button buttonFreeArena;
    public void OnEnable()
    {
        DateTime toDay  = DateTime.Today;
        textTimer.text = /* toDay.ToShortDateString().ToString() + "\n" + */ DataContainer.Instance.playerData.playerStaff.arenaCount.ToString();
        arenaController = FindObjectOfType<ArenaController>();
    }
    public void TodayArena()
    {
        if (DataContainer.Instance.playerData.playerStaff.arenaCount <= 0)
        {
            buttonFreeArena.interactable = false;
        }
        else
        {
            DataContainer.Instance.playerData.playerStaff.arenaCount--;
            PlayerData.SetData(DataContainer.Instance.playerData);
            arenaController.PlayArena();
        }
    }
}
