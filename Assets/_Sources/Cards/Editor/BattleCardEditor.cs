using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class BattleCardEditor : EditorWindow
{
    string cardId;
    string cardName;
    Fraction fraction; 
    public int[] healthPerLvl;
    public int[] atackPerLvl;
    public string[] abilityPerLvl;
    CardsData cardData = new CardsData();

    [MenuItem("Window/BattleCardEditor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        BattleCardEditor window = (BattleCardEditor)EditorWindow.GetWindow(typeof(BattleCardEditor));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        cardId = EditorGUILayout.TextField("CardId", cardId);
        cardName = EditorGUILayout.TextField("CardName", cardName);
        ScriptableObject scriptableObj = this;
        SerializedObject serialObj = new SerializedObject(scriptableObj);
        SerializedProperty healthPerLvl = serialObj.FindProperty("healthPerLvl");
        SerializedProperty atackPerLvl = serialObj.FindProperty("atackPerLvl");
        SerializedProperty abilityPerLvl = serialObj.FindProperty("abilityPerLvl");
        fraction = (Fraction)EditorGUILayout.EnumPopup("Fraction", fraction);
        EditorGUILayout.PropertyField(healthPerLvl, true);
        EditorGUILayout.PropertyField(atackPerLvl, true);
        EditorGUILayout.PropertyField(abilityPerLvl, true);
        if (GUILayout.Button("Add"))
        {
            var cardTemplate = new WarriorCardTemplate(cardId, cardName, this.healthPerLvl, this.atackPerLvl, this.abilityPerLvl);
            cardData.data.Add(JsonUtility.ToJson(cardTemplate));
        }
        if (GUILayout.Button("Clear"))
        {
            cardData.data.Clear();
        }
        EditorGUILayout.TextField(JsonUtility.ToJson(cardData),GUILayout.MaxHeight(100));
        
        serialObj.ApplyModifiedProperties();
    }
}

[Serializable]
public class CardsData
{
    public List<string> data = new List<string>();
}
