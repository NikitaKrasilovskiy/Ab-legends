using PlayFab;
using PlayFab.ClientModels;
using PlayFab.ProfilesModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayfabAuth : MonoBehaviour
{
    [SerializeField] DisplayNameChanger nameChanger;
    [SerializeField] SceneLoader sceneLoader;
    [Inject] PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID || UNITY_EDITOR || UNITY_STANDALONE
        var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnAndroidID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnAndroidLoginSucces, OnAndroidLoginFailure);
#endif
    }

    private string ReturnAndroidID()
    {
        string deviceId = SystemInfo.deviceUniqueIdentifier;
        // #if UNITY_EDITOR
        //     deviceId += "TT";
        // #endif
        return deviceId;
    }

    void OnAndroidLoginSucces(LoginResult loginResult)
    {
//        Debug.Log("Login succes - "+loginResult.ToJson());
        playerData.playerId = loginResult.PlayFabId;
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest() { PlayFabId = loginResult.PlayFabId}, OnGetUserData, 
            (x) => { Debug.LogError(x.ErrorMessage); });
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void OnAndroidLoginFailure(PlayFabError playFabError)
    {
        Debug.LogError("Login failure: " + playFabError.GenerateErrorReport());
    }

    void OnGetUserData(GetPlayerProfileResult responce)
    {
        
        playerData.playerProfileModel = responce.PlayerProfile;
        string displayName = playerData.playerProfileModel.DisplayName;
        Debug.Log(displayName);
        if (string.IsNullOrEmpty(displayName))
        {
            nameChanger.gameObject.SetActive(true);
        }
        else
        {
            BattleDataContainer.playerId = playerData.playerId;
            TutorialController.Instance.CompleteTutorial();
            PlayerData.GetData(playerData.playerId, playerData,() => sceneLoader.LoadScene());
        }
    }
}
