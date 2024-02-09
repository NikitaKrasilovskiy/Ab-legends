using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DBuilder
{
    public class DeckViewer : MonoBehaviour
    {
        [SerializeField] private RectTransform[] placeholders;
        [SerializeField] private CardViewer _cardViewerPref;
        public List<CardViewer> _cardViewers = new List<CardViewer>();
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _deckContainer;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _moveContainer;
        [SerializeField] private CanvasGroup _deckGroup;
        [SerializeField] private CardDataPanel _cardDataPanel;
        public void SetDeck(Deck deck)
        {
            ClearDeck();
            int counter = 0;
            foreach (var VARIABLE in deck.warriorCards)
            {
                var cardViewer = Instantiate(_cardViewerPref, _deckContainer);
                cardViewer.transform.position = placeholders[counter].position;
                cardViewer.SetCard(VARIABLE);
                var movableCard = cardViewer.gameObject.AddComponent<MovableCard>();
                movableCard.canvasGroup = _canvasGroup;
                movableCard.canvas = _canvas;
                movableCard.moveContainer = _moveContainer;
                movableCard.deckGroup = _deckGroup;
                movableCard.cardDataViewer = _cardDataPanel;
                _cardViewers.Add(cardViewer);
                counter++;
            }
        }

        private void ClearDeck()
        {
            foreach (var item in _cardViewers)
            {
                Destroy(item.gameObject);
            }
            _cardViewers.Clear();
        }
    }
}
