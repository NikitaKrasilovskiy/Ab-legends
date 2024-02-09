using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DeckBuilder : MonoBehaviour
{
    [SerializeField]
    Fraction curentFraction;
    [SerializeField]
    Button bobbersBtn;
    [SerializeField]
    Button acornsBtn;
    [SerializeField]
    Button candleBtn;
    [SerializeField] private CollectionView collectionView;
    [SerializeField] private DBuilder.DeckViewer deckViewer;
    [SerializeField] private GameObject lvl3Blocker;
    [SerializeField] private GameObject lvl5Blocker;
    [SerializeField] private GameObject lvl8Blocker;
    [SerializeField] private GameObject blockedPanel;
    private bool _isBobberActive = false;
    private bool _isCandleActive = false;


    void OnEnable()
    {
        _isBobberActive = PlayerPrefs.GetInt("Bobber", 0) > 0;
        _isCandleActive = PlayerPrefs.GetInt("Candle", 0) > 0;
        curentFraction = (Fraction)PlayerPrefs.GetInt("CurentDeck", 0);
        ChangeFraction((int)curentFraction);
        lvl3Blocker.SetActive(DataContainer.Instance.playerData.playerGameProgress.lvl<3);
        lvl5Blocker.SetActive(DataContainer.Instance.playerData.playerGameProgress.lvl<5);
        lvl8Blocker.SetActive(DataContainer.Instance.playerData.playerGameProgress.lvl<8);
    }

    public void InitCurentDeck()
    {
        deckViewer.SetDeck(DataContainer.Instance.playerData.GetDeckByFraction(curentFraction));
        var cards = DataContainer.Instance.playerData.cardCollection.warriorCards.FindAll(x => x.fraction == curentFraction);
        collectionView.SetCardCollection(cards);
    }

    public void MoveCardToCollection(CardViewer cardViewer)
    {
        List<WarriorCard> warriorCards = null;
        switch (curentFraction)
        {
            case Fraction.Acorn:
            {
                warriorCards = DataContainer.Instance.playerData.acornDeck.warriorCards;
                break;
            }
            case Fraction.Bobber:
            {
                warriorCards = DataContainer.Instance.playerData.bobberDeck.warriorCards;
                break;
            }
            case Fraction.Candle:
            {
                warriorCards = DataContainer.Instance.playerData.candleDeck.warriorCards;
                break;
            }
        }

        if (warriorCards!=null && warriorCards.Contains(cardViewer.warriorCard))
            warriorCards.Remove(cardViewer.warriorCard);
        if(!DataContainer.Instance.playerData.cardCollection.warriorCards.Contains(cardViewer.warriorCard))
            DataContainer.Instance.playerData.cardCollection.warriorCards.Add(cardViewer.warriorCard);
        InitCurentDeck();
    }
    
    public void RaplaceDeckCard(CardViewer cardViewer, int idPos)
    {
        CardViewer raplacedCard = null;
        if (idPos < deckViewer._cardViewers.Count)
        {
            raplacedCard = deckViewer._cardViewers[idPos];
        }
        RaplaceDeckCard(cardViewer, raplacedCard, curentFraction);
    }

    public void RaplaceDeckCard(CardViewer cardViewer, CardViewer raplacedCard, Fraction fraction)
    {
        Deck deck = DataContainer.Instance.playerData.GetDeckByFraction(fraction);
        if (deck.warriorCards.Contains(cardViewer.warriorCard))
        {
            deck.warriorCards.Remove(cardViewer.warriorCard);
        }
        if (DataContainer.Instance.playerData.cardCollection.warriorCards.Contains(cardViewer.warriorCard))
            DataContainer.Instance.playerData.cardCollection.warriorCards.Remove(cardViewer.warriorCard);
        if (raplacedCard != null)
        {
            var cardPos = deck.warriorCards.IndexOf(raplacedCard.warriorCard);
            if(cardPos>=0)
                deck.warriorCards[cardPos] = cardViewer.warriorCard;
        }
        else
            deck.warriorCards.Add(cardViewer.warriorCard);
        if(raplacedCard!=null)
            DataContainer.Instance.playerData.cardCollection.warriorCards.Add(raplacedCard.warriorCard);
        InitCurentDeck();
    }
    

    public void ChangeFraction(int fraction)
    {
        var curentFraction = (Fraction)fraction;
        switch (curentFraction)
        {
            case Fraction.Acorn:
                {
                    acornsBtn.interactable = false;
                    bobbersBtn.interactable = true;
                    candleBtn.interactable = true;
                    break;
                }
            case Fraction.Bobber:
                {
                    if (!_isBobberActive)
                    {
                        blockedPanel.SetActive(true);
                        return;
                    }
                    acornsBtn.interactable = true;
                    bobbersBtn.interactable = false;
                    candleBtn.interactable = true;
                    break;
                }
            case Fraction.Candle:
                {
                    if (!_isCandleActive)
                    {
                        blockedPanel.SetActive(true);
                        return;
                    }
                    acornsBtn.interactable = true;
                    bobbersBtn.interactable = true;
                    candleBtn.interactable = false;
                    break;
                }
        }
        this.curentFraction = curentFraction;
        PlayerPrefs.SetInt("CurentDeck", fraction);
        InitCurentDeck();
    }
}
