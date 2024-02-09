using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckPlace : MonoBehaviour, IDropHandler
{
    public int placeNumber = 0;
    [SerializeField] private DeckBuilder _deckBuilder;
    [SerializeField] private CanvasGroup _deckGroup;
    [SerializeField] private CanvasGroup _moveGroup;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drop "+gameObject.name+" "+eventData.hovered[0].name);
        MovableCard.cardPlaced = true;
        _deckBuilder.RaplaceDeckCard(MovableCard.lastMovedCard,placeNumber);
        _deckGroup.blocksRaycasts = true;
        _moveGroup.blocksRaycasts = true;
    }
}
