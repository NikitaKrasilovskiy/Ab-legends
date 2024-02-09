using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeBattleData : MonoBehaviour
{
    [SerializeField]
    private Fraction leader1;
    [SerializeField] private string[] deck1;
    [SerializeField]
    private Fraction leader2;
    [SerializeField] private string[] deck2;

    public Deck GetPlayerDeck()
    {
        var deck = new Deck();
        deck.leaderCard = DataContainer.Instance.cardDataContainer.GetLeader(leader1, 1);
        deck.warriorCards = new List<WarriorCard>();
        foreach (var cardId in deck1)
        {
            Debug.Log(cardId);
            deck.warriorCards.Add(DataContainer.Instance.cardDataContainer.GetWarriorCard(cardId));
        }
        return deck;
    }
    
    public Deck GetOponentDeck()
    {
        var deck = new Deck();
        deck.leaderCard = DataContainer.Instance.cardDataContainer.GetLeader(leader2, 1);
        deck.warriorCards = new List<WarriorCard>();
        foreach (var cardId in deck2)
        {
            deck.warriorCards.Add(DataContainer.Instance.cardDataContainer.GetWarriorCard(cardId));
        }
        return deck;
    }
}
