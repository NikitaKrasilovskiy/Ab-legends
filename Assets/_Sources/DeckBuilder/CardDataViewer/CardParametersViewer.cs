using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardParametersViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI abilityTitle;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private TextMeshProUGUI abilityDescription;
    [SerializeField] private CardViewer cardViewer;
    [SerializeField] private AbilityDescriptions abilityDescriptions;

    public void UpdateCardData(WarriorCard warriorCard)
    {
        cardViewer.SetCard(warriorCard);
        var ability = abilityDescriptions.abilityDescs.Find(x => 
            x.id.Equals(warriorCard.ability));
        abilityTitle.text = LocalizationManager.Localize(ability.title);
        abilityDescription.text = string.Format(LocalizationManager.Localize(ability.description), warriorCard.AbilityPower);
        abilityIcon.sprite = DataContainer.Instance.abilityCollection.
            GetSpriteByName(ability.id);
    }
}
