using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CampsController : MonoBehaviour
{
    [SerializeField] private CompanyManager acornCompany;
    [SerializeField] private CompanyManager bobberCompany;
    [SerializeField] private Toggle acornToggle;
    [SerializeField] private Toggle bobberToggle;
    [SerializeField] private RectTransform container;
    [SerializeField] private CanvasGroup _canvasGroup;

    private void Start()
    {
        acornToggle.onValueChanged.AddListener(ShowAcorn);
        bobberToggle.onValueChanged.AddListener(ShowBobber);
    }

    void ShowAcorn(bool b)
    {
        if(b)
            ShowAcornCompany();
    }

    void ShowBobber(bool b)
    {
        if(b)
            ShowBobberCompany();
    }
    
    public async void ShowAcornCompany(bool b = false)
    {
        _canvasGroup.interactable = false;
        if (bobberCompany.isOpen)
        {
            await bobberCompany.Close();
        }
        if(Mathf.RoundToInt(container.localPosition.x)!=-960)
            container.DOLocalMoveX(-960, 0.7f);
        _canvasGroup.interactable = true;
        if (b)
            acornCompany.OpenJurnal();
        acornToggle.isOn = true;
        

    }
    
    public async void ShowBobberCompany(bool b = false)
    {
        _canvasGroup.interactable = false;
        if (acornCompany.isOpen)
        {
            await acornCompany.Close();
            await UniTask.Delay(300);
        }
        if(Mathf.RoundToInt(container.localPosition.x)!=-2880)
            container.DOLocalMoveX(-2880, 0.7f);
        _canvasGroup.interactable = true;
        if (b)
            bobberCompany.OpenJurnal();
        bobberToggle.isOn = true;
    }
}
