using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class CardLvlDataViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lvl;
    [SerializeField] private TextMeshProUGUI atk;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private AbilityDescriptions abilityDescriptions;
    [SerializeField] private AbilityViewer abilityViewer;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private LvlPanel lvlPanel;
    [SerializeField] private EventTrigger abilityTrigger;
    [SerializeField] private Image bg;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite notSelectedSprite;
    [SerializeField] private DataContainer dataContainer;
    private WarriorCard _warriorCard;

    public void ShowData(WarriorCard warriorCard, bool selected)
    {
        _warriorCard = warriorCard;
        lvl.text = warriorCard.lvl.ToString();
        atk.text = warriorCard.atack.ToString();
        health.text = warriorCard.health.ToString();
        bg.sprite = selected ? selectedSprite : notSelectedSprite;
        lvlPanel.SetLvl(warriorCard.lvl);
        abilityTrigger.triggers.Clear();
        abilityIcon.sprite = dataContainer.abilityCollection.GetSpriteByName(_warriorCard.ability);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data)=>ShowAbility(_warriorCard.ability));
        abilityTrigger.triggers.Add(entry);
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((data)=>abilityViewer.Close());
        abilityTrigger.triggers.Add(entry);
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data)=>abilityViewer.Close());
        abilityTrigger.triggers.Add(entry);
        lvlPanel.SetLvl(warriorCard.lvl);
    }

    void ShowAbility(string id)
    {
        var ability = abilityDescriptions.abilityDescs.Find(x => x.id.Equals(id));
        var icon = dataContainer.abilityCollection.GetSpriteByName(ability.id);
        var description = ability.description;
        abilityViewer.ShowAbility(icon, LocalizationManager.Localize(description));
    }

}
