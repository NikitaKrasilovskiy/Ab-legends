using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class CompanyManager : MonoBehaviour
{
    [SerializeField]
    Fraction fraction;
    [SerializeField]
    GameObject closedJurnal;
    [SerializeField]
    GameObject openJurnal;
    [SerializeField]
    Page[] pages;
    int curentPage = 0;
    int curentFractionResult;
    [SerializeField] private Button prevPageBtn;
    [SerializeField] private Button nextPageBtn;
    [SerializeField] private Button end;
    public bool isOpen;

    private void Awake()
    {
        
        //OpenJurnal(curentPage);
        prevPageBtn.onClick.AddListener(PrevPage);
        nextPageBtn.onClick.AddListener(NextPage);
        end.onClick.AddListener(End);
    }

    private void End()
    {
        Close();
    }

    public async UniTask<bool> Close()
    {
        await UniTask.Delay(300);
        pages[curentPage].gameObject.SetActive(false);
        openJurnal.SetActive(false);
        closedJurnal.SetActive(true);
        nextPageBtn.gameObject.SetActive(false);
        prevPageBtn.gameObject.SetActive(false);
        end.gameObject.SetActive(false);
        isOpen = false;
        return true;
    }

    private void NextPage()
    {
        pages[curentPage].gameObject.SetActive(false);
        curentPage++;
        pages[curentPage].gameObject.SetActive(true);
        SoundEngine.PlayEffect("album_page_turn");
        UpdateBtnGroup();
    }

    private void PrevPage()
    {
        pages[curentPage].gameObject.SetActive(false);
        curentPage--;
        pages[curentPage].gameObject.SetActive(true);
        SoundEngine.PlayEffect("album_page_turn");
        UpdateBtnGroup();
    }

    private void UpdateBtnGroup()
    {
        nextPageBtn.gameObject.SetActive(curentPage<pages.Length-1);
        prevPageBtn.gameObject.SetActive(curentPage>0);
        end.gameObject.SetActive(curentPage==pages.Length-1);
    }

    public async void OpenJurnal()
    {
        if(isOpen)
            return;
        curentFractionResult = fraction == Fraction.Acorn ? 
            DataContainer.Instance.playerData.playerGameProgress.accornCompanyLvl : DataContainer.Instance.playerData.playerGameProgress.bobberCompanyLvl;
        curentPage = curentFractionResult / 18;
        await OpenJurnal(curentPage);
    }
    
    public async UniTask OpenJurnal(int page)
    {
        SoundEngine.PlayEffect("album_open");
        await UniTask.Delay(300);
        
        closedJurnal.SetActive(false);
        openJurnal.SetActive(true);
        pages[page].gameObject.SetActive(true);
        UpdateBtnGroup();
        isOpen = true;
    }

    public void UpdateLvlPoint(LevelPoint levelPoint)
    {
        var levelPointData = new LevelPointData();
        var starCount = curentFractionResult - ((levelPoint.level) * 3);
        starCount = Mathf.Clamp(starCount, -1, 3);
        levelPointData.IsOpen = starCount >= 0;
        levelPointData.StarCount = starCount;
        levelPoint.UpdateLvlData(levelPointData);
    }
}
