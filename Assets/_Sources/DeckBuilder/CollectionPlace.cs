using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollectionPlace : MonoBehaviour, IDropHandler
{
    [SerializeField] private DeckBuilder _deckBuilder;
    [SerializeField] private CanvasGroup _deckGroup;
    [SerializeField] private CanvasGroup _moveGroup;
    
    
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drop "+gameObject.name+" "+eventData.hovered[0].name);
        MovableCard.cardPlaced = true;
        _deckBuilder.MoveCardToCollection(MovableCard.lastMovedCard);
        _deckGroup.blocksRaycasts = true;
        _moveGroup.blocksRaycasts = true;
    }
}
