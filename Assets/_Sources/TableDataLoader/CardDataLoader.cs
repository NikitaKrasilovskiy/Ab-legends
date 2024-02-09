using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using Assets.SimpleLocalization;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace _Sources.TableDataLoader
{
    [ExecuteInEditMode]
    public class CardDataLoader : MonoBehaviour
    {
        public string tableId;
        public Sheet[] sheets;
        public UnityEngine.Object saveFolder;
        private const string UrlPattern = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}";
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
                var url = string.Format(UrlPattern, tableId, sheet.Id);

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
                    var sheet = sheets.Single(i => url == string.Format(UrlPattern, tableId, i.Id));
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
            var resPath = "CardData" + "\\Cards";
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
            //var cardIds = new List<string>();
            var cardDictionary = new Dictionary<string, Dictionary<string, string>>();
            var parameters = lines[2].Split(',').Select(i => 
                i.Trim()).ToList();
            for (int i = 3; i < lines.Length; i++)
            {
                var lineData = lines[i].Split(',');
                var id = lineData[0];
                Debug.Log(id);
                cardDictionary.Add(id, new Dictionary<string, string>());
                for (int j = 0; j < parameters.Count; j++)
                {
                    try
                    {
                        cardDictionary[id].Add(parameters[j],lineData[j]);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(parameters[j]);
                        throw;
                    }
                    
                }
            }
            Debug.Log("<color=green>Card data updated!</color>");
            Debug.Log("<color=green>Generate card list!</color>");
            var warriorCards = new WarriorCardData();
            foreach (var item in cardDictionary)
            {
                var card = new WarriorCard();
                card.Deserialize(item.Value);
                warriorCards.cards.Add(card);
            }

            var warriorsData = JsonUtility.ToJson(warriorCards);
            GUIUtility.systemCopyBuffer = warriorsData; 
            Debug.Log("<color=green>Generate card list complete ! </color> "+ warriorsData);
            UploadTitleData("CardsData", warriorsData);
        }

        private string ReplaceMarkers(string text)
        {
            return text.Replace("[Newline]", "\n");
        }
        async void UploadTitleData(string key, string value)
        {
            var url = "https://9bbf4.playfabapi.com/Admin/SetTitleData";
            var webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            webRequest.SetRequestHeader("X-SecretKey", "6ZHTNP3EWG6F5NQFT8YD45UHJZHFN63D7AD1X8O4PXRSTIK175");
            var data = "{\""+key+"\": \""+value+"\"}";
            var uH = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
            uH.contentType = "application/json";
            webRequest.uploadHandler = uH;
            await webRequest.SendWebRequest();
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log("TidleData "+key+" uploaded");
            }
        }
#endif
    }
}

// {
//     "X-SecretKey": "6ZHTNP3EWG6F5NQFT8YD45UHJZHFN63D7AD1X8O4PXRSTIK175"
// }

[Serializable]
public class WarriorCardData
{
    [SerializeField]
    public List<WarriorCard> cards = new List<WarriorCard>();
}
