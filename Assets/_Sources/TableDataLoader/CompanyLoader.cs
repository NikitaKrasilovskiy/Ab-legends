
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.SimpleLocalization;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class CompanyLoader : MonoBehaviour
{
#if UNITY_EDITOR
    public string tableId;
    public Sheet[] sheets;
    [SerializeField] private UnityEngine.Object saveFolder;
    private const string URL_PATTERN = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}";
    private readonly Dictionary<string, Dictionary<string, string>> _dictionary = new Dictionary<string, Dictionary<string, string>>();
    private List<DeckInfo> bobberCompany = new List<DeckInfo>();
    
    public async void Sync()
    {
        var folder = AssetDatabase.GetAssetPath(saveFolder);
        var dict = new Dictionary<string, UnityWebRequest>();
        bobberCompany.Clear();
        foreach (var sheet in sheets)
        {
            var url = string.Format(URL_PATTERN, tableId, sheet.Id);

            Debug.LogFormat("Downloading: {0}...", url);

            dict.Add(url, UnityWebRequest.Get(url));
        }
        
        foreach (var entry in dict)
        {
            var url = entry.Key;
            var request = entry.Value;

            if (!request.isDone)
            {
                await request.SendWebRequest();
            }

            if (request.error == null)
            {
                var sheet = sheets.Single(i => url == string.Format(URL_PATTERN, tableId, i.Id));
                var path = System.IO.Path.Combine(folder, sheet.Name + ".csv");

                System.IO.File.WriteAllBytes(path, request.downloadHandler.data);
                Debug.LogFormat("Sheet {0} downloaded to {1}", sheet.Id, path);
            }
            else
            {
                throw new Exception(request.error);
            }
        }
        var resPath = "CardData" + "\\CAMPAIGN";
        Debug.Log(resPath);
        var textAsset = Resources.Load<TextAsset>(resPath);
        Debug.Log(textAsset);
        var text = ReplaceMarkers(textAsset.text).Replace("\"\"", "[quotes]");
        var matches = Regex.Matches(text, "\"[\\s\\S]+?\"");
        foreach (Match match in matches)
        {
            text = text.Replace(match.Value,
                match.Value.Replace("\"", null).Replace(",", "[comma]").Replace("\n", "[newline]"));
        }

        var lines = text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 2; i < lines.Length; i++)
        {
            var lineData = lines[i].Split(',');
            var deckInfo = new DeckInfo();
            deckInfo.fraction = Fraction.Bobber;
            int.TryParse(lineData[5], out deckInfo.leaderLvl);
            List<string> warriorIds = new List<string>();
            for (int x = 6; x < 15; x+=2)
            {
                if (!lineData[x].Equals("-"))
                {
                    warriorIds.Add(lineData[x]);
                }
            }
            deckInfo.wariorIds = warriorIds.ToArray();
            int.TryParse(lineData[17], out deckInfo.rewardExp);
            int.TryParse(lineData[18], out deckInfo.rewardGold);
            if (lineData.Length >= 20)
            {
                deckInfo.rewardCard = lineData[19];
                if (!string.IsNullOrEmpty(lineData[19]))
                {
                    Debug.Log("CARD "+lineData[19]);
                }
            }

            bobberCompany.Add(deckInfo);
        }

        var companyData = new CompanyData();
        companyData.levelsData = new List<DeckInfo>(bobberCompany);
        GUIUtility.systemCopyBuffer = JsonUtility.ToJson(companyData); 
        Debug.Log(JsonUtility.ToJson(companyData));
    }

    private string ReplaceMarkers(string text)
    {
        return text.Replace("[Newline]", "\n");
    }
#endif
}


[Serializable]
public class CompanyData
{
    public List<DeckInfo> levelsData;

    public DeckInfo GetDeckInfo(int i)
    {
        return levelsData[Mathf.Clamp(i-1, 0, int.MaxValue)];
    }
}