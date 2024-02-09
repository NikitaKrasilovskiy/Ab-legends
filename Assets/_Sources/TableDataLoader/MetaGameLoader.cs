using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Assets.SimpleLocalization;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

[ExecuteInEditMode]
public class MetaGameLoader : MonoBehaviour
{
        public string tableId;
        public Sheet[] sheets;
        public UnityEngine.Object saveFolder;
        private const string URL_PATTERN = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}";
        private readonly Dictionary<string, Dictionary<string, string>> _dictionary = new Dictionary<string, Dictionary<string, string>>();
#if UNITY_EDITOR

        /// <summary>
        /// Sync spreadsheets.
        /// </summary>
        public void Sync()
        {
            SyncCoroutine();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private async void SyncCoroutine()
        {
            var folder = AssetDatabase.GetAssetPath(saveFolder);

            Debug.Log("<color=yellow>Sync started, please wait for confirmation message...</color>");

            var dict = new Dictionary<string, UnityWebRequest>();

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
            Debug.Log("<color=yellow>Parsing started, please wait for confirmation message...</color>");
            _dictionary.Clear();
            var resPath = "CardData" + "\\Boxes";
            var textAsset = Resources.Load<TextAsset>(resPath);
            var text = ReplaceMarkers(textAsset.text).Replace("\"\"", "[quotes]");
            var matches = Regex.Matches(text, "\"[\\s\\S]+?\"");
            foreach (Match match in matches)
            {
                text = text.Replace(match.Value,
                    match.Value.Replace("\"", null).Replace(",", "[comma]").Replace("\n", "[newline]"));
            }

            var lines = text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            var metaGameData = new MetaGameData();
            for (int i = 1; i < lines.Length; i++)
            {
                var lineData = lines[i].Split(',');
                var prizeType = PrizeType.Small;
                try
                {
                    prizeType = (PrizeType) Enum.Parse(typeof(PrizeType), lineData[0]);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    continue;
                }
                float.TryParse(lineData[1], out var probability);
                int.TryParse(lineData[2], out var coolDown);
                int.TryParse(lineData[3], out var gold);
                int.TryParse(lineData[4], out var ruby);
                var boxData = new BoxData(probability, coolDown, gold,prizeType, ruby);
                metaGameData.boxes.Add(boxData);
            }
            resPath = "CardData" + "\\MetaData";
            textAsset = Resources.Load<TextAsset>(resPath);
            text = ReplaceMarkers(textAsset.text).Replace("\"\"", "[quotes]");
            matches = Regex.Matches(text, "\"[\\s\\S]+?\"");
            foreach (Match match in matches)
            {
                text = text.Replace(match.Value,
                    match.Value.Replace("\"", null).Replace(",", "[comma]").Replace("\n", "[newline]"));
            }
            lines = text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 2; i < lines.Length; i++)
            {
                var lineData = lines[i].Split(',');
                int.TryParse(lineData[0], out var playerLvl);
                int.TryParse(lineData[1], out var exp);
                int.TryParse(lineData[2], out var cardLvl);
                int.TryParse(lineData[3], out var upgradePrice);
                int.TryParse(lineData[4], out var sellPrice);
                metaGameData.lvlDatas.Add(new LvlData(){exp=exp,lvl=playerLvl});
                metaGameData.cardPrices.Add(new CardPrice(){lvl=cardLvl,sellPrice = sellPrice,upgradePrice = upgradePrice});
            }

            var metaDataString = JsonUtility.ToJson(metaGameData);
            Debug.Log(metaDataString);
            GUIUtility.systemCopyBuffer = metaDataString;
        }

        private string ReplaceMarkers(string text)
        {
            return text.Replace("[Newline]", "\n");
        }

#endif
}

[Serializable]
public class MetaGameData
{
    public List<LvlData> lvlDatas = new List<LvlData>();
    public List<CardPrice> cardPrices = new List<CardPrice>();
    public List<BoxData> boxes = new List<BoxData>();
}

[Serializable]
public struct LvlData
{
    public int lvl;
    public int exp;
}

[Serializable]
public struct CardPrice
{
    public int lvl;
    public int upgradePrice;
    public int sellPrice;
}

[Serializable]
public struct BoxData
{
    public PrizeType prizeType;
    public float probability;
    public int coolDown;
    public int ruby;
    public int gold;
    public float cardChance;

    public BoxData(float probability, int coolDown, int gold, PrizeType prizeType, int ruby)
    {
        this.probability = probability;
        this.coolDown = coolDown;
        this.gold = gold;
        this.prizeType = prizeType;
        this.ruby = ruby;
        cardChance = 1;
    }
}