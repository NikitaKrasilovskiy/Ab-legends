using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DeckViewer : MonoBehaviour
{
    public LeaderViewer leaderViewer;
    public List<CardViewer> cardViewers;
    public int deckLength = 0;
    public void InitViewer(Deck deck)
    {
        deckLength = deck.warriorCards.Count;
        for(int i = 0; i<deck.warriorCards.Count; i++)
        {
            cardViewers[i].gameObject.SetActive(true);
            cardViewers[i].SetCard(deck.warriorCards[i]);
        }
        leaderViewer.ShowCard(deck.leaderCard);
    }

    public void DestroyCard(CardViewer cardViewer)
    {
        cardViewers.Remove(cardViewer);
        Destroy(cardViewer.transform.parent.gameObject);
        deckLength--;
    }
    
    public CardViewer GetCard(int cardId)
    {
        if (cardId >= cardViewers.Count)
        {
            return null;
        }
        return cardViewers[cardId];
    }
}
