using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelPoint : MonoBehaviour
{
    [SerializeField] public Fraction campFraction;
    [SerializeField] public int level;
    [SerializeField] private Image enemy;
    [SerializeField] private Image myView;
    [SerializeField] private Image complete;
    [SerializeField] private Star[] stars;
    [SerializeField] private bool isBossLevel;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(LoadBattleScene);
    }

    private async void LoadBattleScene()
    {
        await SoundEngine.PlayEffect("click");
        BattleDataContainer.CompanyBattleData(campFraction, level);
        SceneManager.LoadScene("Battle_Preloader");
    }

    public void UpdateLvlData(LevelPointData levelPointData)
    {
        if (!levelPointData.IsOpen)
        {
            _button.interactable = false;
            _button.targetGraphic = enemy;
            return;
        }
        _button.interactable = true;
        
            UpdateView(levelPointData.StarCount);
    }

    private void UpdateView(int starCount)
    {
        switch (starCount)
        {
            case 0:
                {
                    enemy.gameObject.SetActive(true);
                    myView.gameObject.SetActive(false);
                    complete.gameObject.SetActive(false);
                    _button.targetGraphic = enemy;
                    
                    break;
                }
            case 1:
                {
                    enemy.gameObject.SetActive(false);
                    myView.gameObject.SetActive(true);
                    complete.gameObject.SetActive(false);
                    _button.targetGraphic = myView;
                    break;
                }
            case 2:
                {
                    enemy.gameObject.SetActive(false);
                    myView.gameObject.SetActive(true);
                    complete.gameObject.SetActive(false);
                    _button.targetGraphic = myView;
                    break;
                }
            case 3:
            {
                enemy.gameObject.SetActive(false);
                myView.gameObject.SetActive(false);
                complete.gameObject.SetActive(true);
                _button.targetGraphic = complete;
                _button.interactable = false;
                break;
            }
        }
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].ChangeState(starCount>i);
        }
    }
}

public struct LevelPointData
{
    public int StarCount;
    public bool IsOpen;
}
