using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AbilityDescriptions : MonoBehaviour
{
    public List<AbilityDesc> abilityDescs;
    [SerializeField] private TextAsset _textAsset;

    private void OnValidate()
    {
        if(_textAsset==null)
            return;
        var text = ReplaceMarkers(_textAsset.text).Replace("\"\"", "[quotes]");
        var lines = text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
        foreach (var VARIABLE in lines)
        {

            var list = abilityDescs.FindAll((x) => VARIABLE.Contains(x.id.Replace("x","_ABILITY").ToUpper()));
            foreach (var item in list)
            {
                item.title = VARIABLE;
            }
        }
    }
    private string ReplaceMarkers(string text)
    {
        return text.Replace("[Newline]", "\n");
    }
}


[Serializable]
public class AbilityDesc
{
    public string id;
    public string title;
    public string description;
}