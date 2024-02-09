using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using PlayFab.ClientModels;
using PlayFab;
using Cysharp.Threading.Tasks;
using Zenject;

public class DisplayNameChanger : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    TMP_InputField inputField;

    [SerializeField] private Animator _nicknameRulesAnimator;
    [SerializeField] SceneLoader sceneLoader;
    Regex regex = new Regex(@"^[A-Za-z0-9_-]{3,16}$");
    [Inject] private PlayerData _playerData;
    [Inject] private CardDataContainer _cardDataContainer;
    private static readonly int Attention = Animator.StringToHash(AttentionTrigger);
    private const string AttentionTrigger = "Attention";
    
    public void ChangeName()
    {
        canvasGroup.interactable = false;
        string nickname = inputField.text;
        if (regex.IsMatch(nickname))
        {
            var request = new UpdateUserTitleDisplayNameRequest();
            request.DisplayName = nickname;
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnSuccess,OnError );
        }
        else
        {
            FormatError();
        }
    }

    void FormatError()
    {
        _nicknameRulesAnimator.SetTrigger(Attention);
        canvasGroup.interactable = true;
    }

    void OnSuccess(UpdateUserTitleDisplayNameResult result)
    {
        _playerData.SetDefaultPlayerData(_cardDataContainer, () => sceneLoader.LoadScene(500));
        _playerData.playerProfileModel.DisplayName = inputField.text;
    }

    void OnError(PlayFabError playFabError)
    {
        Debug.LogError(playFabError.Error);
        canvasGroup.interactable = true;
    }

    
}
