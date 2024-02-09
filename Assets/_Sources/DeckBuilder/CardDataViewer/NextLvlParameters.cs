using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Assets.SimpleLocalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextLvlParameters : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lvlView;
    [SerializeField] private TextMeshProUGUI atackView;
    [SerializeField] private TextMeshProUGUI healthView;
    [SerializeField] private Transform nextAbilityView;
    [SerializeField] private TextMeshProUGUI abilityTitle;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private TextMeshProUGUI abilityPower;
    [SerializeField] private AbilityDescriptions abilityDescriptions;

    public void ShowNextLevel(WarriorCard currentCard, WarriorCard nextGradeCard)
    {
        lvlView.text = nextGradeCard.lvl.ToString();
        atackView.text = (nextGradeCard.atack - currentCard.atack).ToString();
        healthView.text = (nextGradeCard.health - currentCard.health).ToString();
        var nextAbility = abilityDescriptions.abilityDescs.Find(x => 
            x.id.Equals(nextGradeCard.ability));
        abilityTitle.text = LocalizationManager.Localize(nextAbility.title);
        abilityIcon.sprite = DataContainer.Instance.abilityCollection.
            GetSpriteByName(nextAbility.id);
        var powerDifference = nextGradeCard.AbilityPower - currentCard.AbilityPower;
        nextAbilityView.gameObject.SetActive(!(currentCard.ability.Equals(nextAbility.id) && powerDifference==0));
        if (Mathf.Abs(powerDifference)>=1)
        {
            abilityPower.text = string.Format("+{0}", powerDifference);
        }
        else
        {
            if(powerDifference>=0)
                abilityPower.text = string.Format("+{0:P1}", powerDifference);
            else
            {
                abilityPower.text = string.Format("{0:P1}", powerDifference);
            }
        }
    }
}
