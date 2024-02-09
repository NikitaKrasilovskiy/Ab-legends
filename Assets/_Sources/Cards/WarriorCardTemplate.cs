using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WarriorCardTemplate 
{
    public string cardId;
    public string cardName;
    public Fraction fraction;
    public int[] healthPerLvl;
    public int[] atackPerLvl;
    public string[] abilityPerLvl;

    public WarriorCard GetCard(short lvl)
    {
        return new WarriorCard() { id = cardId, lvl = lvl, health = healthPerLvl[lvl], atack = atackPerLvl[lvl], ability = abilityPerLvl[lvl] };
    }

    public WarriorCardTemplate(string cardId, string cardName, int[] healthPerLvl, int[] atackPerLvl, string[] abilityPerLvl)
    {
        this.cardId = cardId;
        this.cardName = cardName;
        this.healthPerLvl = healthPerLvl;
        this.atackPerLvl = atackPerLvl;
        this.abilityPerLvl = abilityPerLvl;
    }
}

public enum Fraction
{
    Acorn,
    Bobber,
    Candle
}
