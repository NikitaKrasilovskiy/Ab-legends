using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovableCard : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerClickHandler, IPointerDownHandler
{
    private RectTransform _rectTransform;
    public Canvas canvas;
    public CanvasGroup canvasGroup;
    public static bool cardPlaced = false;
    public static CardViewer lastMovedCard;
    public RectTransform moveContainer;
    private Vector3 startPosition;
    private Transform _transform;
    private CardViewer _cardViewer;
    private Transform _lastParent;
    public CanvasGroup deckGroup;
    public CardDataPanel cardDataViewer;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _transform = transform;
        _cardViewer = GetComponent<CardViewer>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        SoundEngine.PlayEffect("menu_card_get");
        cardPlaced = false;
        canvasGroup.blocksRaycasts = false;
        deckGroup.blocksRaycasts = false;
        startPosition = _transform.position;
        lastMovedCard = _cardViewer;
        _lastParent = _transform.parent;
        _transform.SetParent(moveContainer);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        _rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true;
        deckGroup.blocksRaycasts = true;
        if (!cardPlaced)
        {
            _transform.position = startPosition;
            _transform.SetParent(_lastParent);
            SoundEngine.PlayEffect("menu_card_release");
        }
        else
        {
            SoundEngine.PlayEffect("menu_card_placed");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        SoundEngine.PlayEffect("click");
        cardDataViewer.Open(_cardViewer.warriorCard);
    }

    
}
