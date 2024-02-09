using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievView : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image iconView;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI progressView;
    [SerializeField] private TextMeshProUGUI rewardCountView;
    [SerializeField] private Button rewardButton;
    [SerializeField] private Sprite disableBg;
    [SerializeField] private Sprite enableBg;
    [SerializeField] private GameObject complited;
    private AchievData _achievData;

    private void Awake()
    {
        rewardButton.onClick.AddListener(GetReward);
    }

    private void GetReward()
    {
        DataContainer.Instance.playerData.playerStaff.goldCount += _achievData.reward;
        _achievData.isComplited = true;
        PlayerData.SetData(DataContainer.Instance.playerData);
        UpdateView(_achievData);
    }

    public void UpdateView(AchievData achievData)
    {
        _achievData = achievData;
        bool isComplite = achievData.progress >= achievData.stageCount;
        background.sprite = isComplite ? enableBg : disableBg;
        iconView.sprite = DataContainer.Instance.achievmentIconCollection.GetSpriteByName(achievData.iconId + isComplite);
        description.text = achievData.description;
        rewardCountView.text = achievData.reward.ToString();
        progressView.text = string.Format("{0}/{1}", achievData.progress, achievData.stageCount);
        rewardButton.interactable = isComplite&&!achievData.isComplited;
        complited.SetActive(achievData.isComplited);
    }
}
