using System;
using System.Collections;
using System.Collections.Generic;
using _Sources.Common;
using DevToDev;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDataPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _shellPriceView;
    [SerializeField] private TextMeshProUGUI _upgradePriceView;
    [SerializeField] private int _basePrice;
    [SerializeField] private int _upgradeBasePrice;
    [SerializeField] private CardParametersViewer cardParametersViewer;
    [SerializeField] private NextLvlParameters nextLvlParameters;
    private WarriorCard _warriorCard;
    [SerializeField] private Button _shellBtn;
    [SerializeField] private Button _upgradeBtn;
    [SerializeField] private DeckBuilder _deckBuilder;
    [SerializeField] private GameObject _noGoldPopup;
    [SerializeField] private GameObject noShellPopup;
    [SerializeField] private GameObject nextGradeArrow;
    [SerializeField] private TextMeshProUGUI nextGradeTxt;
    [SerializeField] private GameObject prevGradeArrow;
    [SerializeField] private TextMeshProUGUI prevGradeTxt;
    [SerializeField] private MaterialChanger _materialChanger;
    private WarriorCard _currentViewCard;
    
    private void Awake()
    {
        _shellBtn.onClick.AddListener(Shell); 
        _upgradeBtn.onClick.AddListener(Upgrade);
    }

    public void Open(CardViewer cardViewer)
    {
        Open(cardViewer.warriorCard);
    }
    
    public void Open(WarriorCard warriorCard)
    {
        _warriorCard = warriorCard;
        ShowParameters(_warriorCard);
        var cardPrice =
            DataContainer.Instance.cardDataContainer.metaGameData.cardPrices.Find(x => 
                x.lvl == _warriorCard.lvl);
        _shellPriceView.text = cardPrice.sellPrice.ToString();
        var upgradePrice = cardPrice.upgradePrice;
        _upgradePriceView.text = upgradePrice.ToString();
        var nextGradeCard = DataContainer.Instance.cardDataContainer.GetNextEvolution(warriorCard);
        nextLvlParameters.gameObject.SetActive(nextGradeCard!=null);
        _upgradeBtn.interactable = nextGradeCard != null;
        if(nextGradeCard!=null)
            nextLvlParameters.ShowNextLevel(warriorCard, nextGradeCard);
        else
        {
            _upgradePriceView.text = "";
        }
        if (DataContainer.Instance.playerData.playerStaff.goldCount < upgradePrice)
            _upgradeBtn.interactable = false;
        gameObject.SetActive(true);
    }

    void ShowParameters(WarriorCard warriorCard)
    {
        cardParametersViewer.UpdateCardData(warriorCard);
        _currentViewCard = warriorCard;
        if(_currentViewCard.lvl!=_warriorCard.lvl)
            _materialChanger.SwitchMaterials();
        else
        {
            _materialChanger.ChangeBackMaterial();
        }
        UpdateGradeButtons();
    }

    private void UpdateGradeButtons()
    {
        var grades = DataContainer.Instance.cardDataContainer.GetAllEvolution(_warriorCard.id);
        int maxGrade = 0;
        foreach (var VARIABLE in grades)
        {
            if (maxGrade < VARIABLE.lvl)
                maxGrade = VARIABLE.lvl;
        }
        prevGradeArrow.SetActive(_currentViewCard.lvl>_warriorCard.lvl);
        prevGradeTxt.text = (_currentViewCard.lvl - 1).ToString();
        nextGradeArrow.SetActive(_currentViewCard.lvl<maxGrade);
        nextGradeTxt.text = (_currentViewCard.lvl + 1).ToString();
    }

    public async void Shell()
    {
        ScreenBlocker.BlockScreen(true);
        var cardPrice =
            DataContainer.Instance.cardDataContainer.metaGameData.cardPrices.Find(x => 
                x.lvl == _warriorCard.lvl);
        RemoveCard();
        var goldCount = _basePrice * _warriorCard.lvl;
        DataContainer.Instance.playerData.playerStaff.goldCount += cardPrice.sellPrice;
        //DevToDevTamplates.SoftCurrency("sell", _warriorCard.name, goldCount);
        await SoundEngine.PlayEffect("card_remove");

        PlayerData.SetData(DataContainer.Instance.playerData, () =>
        {
            _deckBuilder.InitCurentDeck();
            
            gameObject.SetActive(false);
            ScreenBlocker.BlockScreen(false);
            if(!TutorialController.Instance.isTutorialComplete)
                TutorialController.Instance.ShowTutorial();
        });
        
    }

    public void Upgrade()
    {
        var cardPrice =
            DataContainer.Instance.cardDataContainer.metaGameData.cardPrices.Find(x => 
                x.lvl == _warriorCard.lvl);
        if (DataContainer.Instance.playerData.playerStaff.goldCount < cardPrice.upgradePrice)
        {
            ShopSwitcher.OnOpenShopPanel?.Invoke(ShopPanelType.Coins);
            return;
        }

        var eventParams = new CustomEventParams();
        eventParams.AddParam("type", _warriorCard.name);
        eventParams.AddParam("currency", "soft");
        
        var goldCount = cardPrice.upgradePrice;
        eventParams.AddParam("value", -goldCount);
        DataContainer.Instance.playerData.playerStaff.goldCount -= goldCount;
        _warriorCard.UpdateCardData(DataContainer.Instance.cardDataContainer.GetNextEvolution(_warriorCard));
        eventParams.AddParam("lvl", _warriorCard.lvl);
        SoundEngine.PlayEffect("card_upgrade");
        //DevToDev.Analytics.CustomEvent("upgrade", eventParams);
        DevToDevTamplates.SoftCurrency("upgrade", _warriorCard.name, -goldCount);
        PlayerData.SetData(DataContainer.Instance.playerData, () => Open(_warriorCard));
    }

    bool RemoveCard()
    {
        var deck = DataContainer.Instance.playerData.GetDeckByFraction(_warriorCard.fraction);
        var cardsCollection =
            DataContainer.Instance.playerData.cardCollection.warriorCards.FindAll((x) =>
                x.fraction == _warriorCard.fraction);
        if (deck.warriorCards.Count + cardsCollection.Count <= 2)
        {
            noShellPopup.SetActive(true);
            return false;
        }
        if (DataContainer.Instance.playerData.cardCollection.warriorCards.Contains(_warriorCard))
            DataContainer.Instance.playerData.cardCollection.warriorCards.Remove(_warriorCard);
        if (DataContainer.Instance.playerData.acornDeck.warriorCards.Contains(_warriorCard))
            DataContainer.Instance.playerData.acornDeck.warriorCards.Remove(_warriorCard);
        if (DataContainer.Instance.playerData.bobberDeck.warriorCards.Contains(_warriorCard))
            DataContainer.Instance.playerData.bobberDeck.warriorCards.Remove(_warriorCard);
        if (DataContainer.Instance.playerData.candleDeck.warriorCards.Contains(_warriorCard))
            DataContainer.Instance.playerData.candleDeck.warriorCards.Remove(_warriorCard);
        return true;
    }

    public void ShowNextGrade()
    {
        var nextGradeCard = DataContainer.Instance.cardDataContainer.GetNextEvolution(_currentViewCard);
        if (nextGradeCard != null)
        {
            ShowParameters(nextGradeCard);
        }
    }
    
    public void ShowPrevGrade()
    {
        var prevGradeCard = DataContainer.Instance.cardDataContainer.GetPrevEvolution(_currentViewCard);
        if (prevGradeCard != null)
        {
            ShowParameters(prevGradeCard);
        }
    }
}
