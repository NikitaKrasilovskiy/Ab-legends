using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Random = UnityEngine.Random;

public class UserGenerator : MonoBehaviour
{
    [SerializeField] private TextAsset textNames;
    [SerializeField] private TextAsset cardDataText;
    [SerializeField] private int count = 10;
    private CardDataContainer _cardDataContainer;
    private List<string> names = new List<string>();
    private FakeUserData _fakeUserData;
    private bool isCreate = false;
#if UNITY_EDITOR  
    void Start()
    {
        //_cardDataContainer = JsonUtility.FromJson<CardDataContainer>(cardDataText.text);
        var lines = textNames.text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
        foreach (var VARIABLE in lines)
        {
            names.Add(VARIABLE);
        }
        Debug.Log("Lets GO");
        CreateFakeUsers(count);
    }

    async UniTask CreateFakeUsers(int count)
    {
        Debug.Log(count);
        for (int i = 0; i < count; i++)
        {
            Debug.Log(i);
            isCreate = true;
            _fakeUserData = new FakeUserData();
            string name = names[Random.Range(0, names.Count)];
            _fakeUserData.nickname = name;
            _fakeUserData.id = ReturnAndroidID(_fakeUserData.nickname);
            _fakeUserData.rating = 1400 + (i * Random.Range(50, 100));
            
            Debug.Log(JsonUtility.ToJson(_fakeUserData));
            Login();
            
            names.Remove(name);
            await UniTask.WaitUntil(() => !isCreate);
            Debug.Log("Creating user "+_fakeUserData.nickname+" complite");
        }
    }

    string[] GetDeckByRating(int rating)
    {
        Debug.Log(rating);
        string[] deck;
        if (rating > 2000)
        {
            deck = new string[5];
            for (int i = 0; i < deck.Length; i++)
            {
                deck[i] = _cardDataContainer.GetRandomWarriorCard(Fraction.Acorn, 6, 10).id;
            }

            return deck;
        }

        if (rating > 1700)
        {
            deck = new string[4];
            for (int i = 0; i < deck.Length; i++)
            {
                deck[i] = _cardDataContainer.GetRandomWarriorCard(Fraction.Acorn, 4, 8).id;
            }

            return deck;
        }

        if (rating > 1500)
        {
            deck = new string[3];
            for (int i = 0; i < deck.Length; i++)
            {
                deck[i] = _cardDataContainer.GetRandomWarriorCard(Fraction.Acorn, 1, 4).id;
            }

            return deck;
        }
        
        deck = new string[2];
        for (int i = 0; i < deck.Length; i++)
        {
            deck[i] = _cardDataContainer.GetRandomWarriorCard(Fraction.Acorn, 1, 3).id;
        }

        return deck;
    }
    
    private string ReturnAndroidID(string name)
    {
        var id = name.GetHashCode() + "TESTER" + Random.Range(0, 99999);
        return id;
    }
    
    #region Playfab

    void Login()
    {
        Debug.Log("Start creating user "+_fakeUserData.nickname);
        var requestAndroid = new LoginWithAndroidDeviceIDRequest
            {AndroidDeviceId = _fakeUserData.id, CreateAccount = true};
  
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnAndroidLoginSucces, OnAndroidLoginFailure);
    }

    private void OnAndroidLoginFailure(PlayFabError obj)
    {
        Debug.Log(obj.ErrorMessage);
    }

    private void OnAndroidLoginSucces(LoginResult obj)
    {
        LoadCards();
    }

    async UniTask LoadCards()
    {
        var request = new UpdateUserTitleDisplayNameRequest();
        request.DisplayName = _fakeUserData.nickname;
        _cardDataContainer = new CardDataContainer();
        await UniTask.WaitUntil(() => _cardDataContainer.isInit);
        _fakeUserData.deck = GetDeckByRating(_fakeUserData.rating);
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnSuccess,OnError );
    }

    private void OnError(PlayFabError obj)
    {
        throw new NotImplementedException();
    }

    private void OnSuccess(UpdateUserTitleDisplayNameResult obj)
    {
        var playerData = new PlayerData();
        playerData.SetDefaultPlayerData(_cardDataContainer, _fakeUserData.deck, _fakeUserData.rating,
            ()=>PlayerData.SetData(playerData,()=>isCreate=false));
    }

    #endregion
    
    
    #endif
}

public struct FakeUserData
{
    public string id;
    public string nickname;
    public int rating;
    public string[] deck;
}