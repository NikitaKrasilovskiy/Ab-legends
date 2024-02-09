using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TestDataSave : MonoBehaviour
{
    [Inject] PlayerData playerData;

    private void Start()
    {
        var deck = new Deck();
        deck.leaderCard = new LeaderCard();
        deck.warriorCards = new List<WarriorCard>();
        deck.warriorCards.Add(new WarriorCard());
        deck.warriorCards.Add(new WarriorCard());
        deck.warriorCards.Add(new WarriorCard());
        playerData.acornDeck = deck;
        PlayerData.SetData(playerData);
    }
}
