using System;
using System.Collections;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;

    private void Start()
    {
        // foreach (var btn in buttons)
        // {
        //     btn.onClick.AddListener(()=>SelectButton(btn));
        // }
        if(!buttons.IsEmpty())
            ActivateButton(2);
    }

    public void ActivateButton(int i)
    {
        SelectButton(buttons[i]);
    }

    void SelectButton(Button button)
    {
        foreach (var btn in buttons)
        {
            Debug.Log(btn.name+" "+(btn != button));
            btn.interactable = btn != button;
        }
    }
}
