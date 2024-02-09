using System;
using System.Collections.Generic;
using _Sources.Interfaces;
using UnityEngine;

[Serializable]
public class WarriorCard: IDictSerializable
{
    public string id;
    public string name;
    public Fraction fraction;
    public short lvl = 1;
    public int health = 1;
    public int atack = 1;
    public string ability;

    public float AbilityPower
    {
        get => abilityPower;
        set
        {
            var i = Mathf.RoundToInt(value * 100);
            abilityPower = i / 100f;
        }
    }
    public float abilityPower = float.MinValue;
    #region Parameter names

    private const string ID = "id";
    private const string LVL = "lvl";
    private const string NAME = "name";
    private const string FRACT = "fraction";
    private const string HEALTH = "health";
    private const string ATACK = "atack";
    private const string ABILITY = "abilityId";
    private const string ABILITY_POWER = "abilityPower";

    #endregion
    public void Deserialize(Dictionary<string, string> dictionary)
    {
        id = dictionary[ID];
        name = dictionary[NAME];
        fraction = (Fraction)Enum.Parse(typeof(Fraction), dictionary[FRACT]);
        short.TryParse(dictionary[LVL], out lvl);
        int.TryParse(dictionary[HEALTH], out health);
        int.TryParse(dictionary[ATACK], out atack);
        ability = dictionary[ABILITY];
        var aPower = dictionary[ABILITY_POWER].Replace("[comma]", ",");
        float pow = 0;
        float.TryParse(aPower, out pow);
        Debug.Log("Pow "+pow);
        AbilityPower = pow;
        Debug.Log(AbilityPower);
    }

    public void UpdateCardData(WarriorCard warriorCard)
    {
        id = warriorCard.id;
        name = warriorCard.name;
        fraction = warriorCard.fraction;
        lvl = warriorCard.lvl;
        health = warriorCard.health;
        atack = warriorCard.atack;
        ability = warriorCard.ability;
        AbilityPower = warriorCard.AbilityPower;
    }
}
