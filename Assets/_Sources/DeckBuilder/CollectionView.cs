using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CollectionView : MonoBehaviour
{
    [SerializeField] private Transform rectZone;
    private List<CardViewer> _cardViewers = new List<CardViewer>();
    [SerializeField]private CardViewer _cardViewerPref;
    [SerializeField]private CanvasGroup _canvasGroup;
    [SerializeField]private Canvas _canvas;
    [SerializeField] private RectTransform _moveContainer;
    [SerializeField]private CanvasGroup _deckGroup;
    [SerializeField] private CardDataPanel _cardDataPanel;
    [SerializeField] private Mask mask;

    private void Start()
    {
        mask.enabled = TutorialController.Instance.isTutorialComplete;
    }

    public void SetCardCollection(List<WarriorCard> cardCollection)
    {
        Clear();
        foreach (var item in cardCollection)
        {
            var cardViewer = Instantiate(_cardViewerPref, rectZone);
            cardViewer.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            cardViewer.SetCard(item);
            var movableCard = cardViewer.gameObject.AddComponent<MovableCard>();
            movableCard.canvasGroup = _canvasGroup;
            movableCard.canvas = _canvas;
            movableCard.moveContainer = _moveContainer;
            movableCard.deckGroup = _deckGroup;
            movableCard.cardDataViewer = _cardDataPanel;
            _cardViewers.Add(cardViewer);
        }
    }

    public void Clear()
    {
        foreach (var item in _cardViewers)
        {
            Destroy(item.gameObject);
        }
        _cardViewers.Clear();
    }
}
