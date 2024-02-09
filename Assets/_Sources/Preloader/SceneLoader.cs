using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] Slider loadBar;
    [NonSerialized][Inject] public WarriorCardData warriorCardData;
    public async void LoadScene(int delay =0)
    {
        if (string.IsNullOrEmpty(sceneName))
            throw new Exception("empty scene name");
        else
            LoadScene(sceneName, delay);
    }

    public async void LoadScene(string sceneName, int delay =0)
    {   
        await UniTask.Delay(delay);
        await ASLoadScene(sceneName);
    }

    async UniTask ASLoadScene(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < 0.8f)
        {
            await UniTask.WaitForEndOfFrame();
            if (loadBar != null)
                loadBar.value = asyncOperation.progress;
        }
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), ResultCallback, ErrorCallback);
        asyncOperation.allowSceneActivation = true;
        loadBar.value = 1;
    }

    private void ErrorCallback(PlayFabError obj)
    {
        Debug.LogError(obj.Error);
    }

    private void ResultCallback(GetTitleDataResult obj)
    {
        string cardData = obj.Data["CardsData"];
//        Debug.Log(cardData);
        warriorCardData.cards = JsonUtility.FromJson<WarriorCardData>(cardData).cards;
    }
}
