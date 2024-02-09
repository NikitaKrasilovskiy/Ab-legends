using UnityEngine;

namespace _Sources.Cards
{
    public class TurnProcessor : MonoBehaviour
    {
        [SerializeField] private CardViewer battleCard;
        private CardViewer _lastSelectedCard;
        
        public void HandleTurn(CardViewer selectedCard)
        {
            _lastSelectedCard = selectedCard;
            battleCard.SetCard(_lastSelectedCard.warriorCard);
        }
        
        public void TurnComplete()
        {
            _lastSelectedCard.SetCard(battleCard.warriorCard);
        }
    }
}