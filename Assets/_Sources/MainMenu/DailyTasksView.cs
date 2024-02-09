using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Assets.SimpleLocalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyTasksView : MonoBehaviour
{
    [SerializeField] private Button prevBtn;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Image iconView;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI descriptionTxt;
    [SerializeField] private Button rewardBtn;
    [SerializeField] private TextMeshProUGUI rewardCountView;
    [SerializeField] private GameObject compliteObj;
    private DailyTasks _dailyTasks;
    private int _currentTask = 0;
    private const string ProgressText = "{0}/{1}";

    private void Awake()
    {
        prevBtn.onClick.AddListener(PrevTask);
        nextBtn.onClick.AddListener(NextTask);
        rewardBtn.onClick.AddListener(TakeReward);
    }

    private void TakeReward()
    {
        var currentTask = _dailyTasks.tasks[_currentTask];
        currentTask.isComplited = true;
        DataContainer.Instance.playerData.playerStaff.goldCount += currentTask.rewardCount;
        UpdateCurrentTask();
        PlayerData.SetData(DataContainer.Instance.playerData);
    }

    private void NextTask()
    {
        _currentTask++;
        if (_currentTask >= _dailyTasks.tasks.Count)
            _currentTask = 0;
        UpdateCurrentTask();
    }

    private void PrevTask()
    {
        _currentTask--;
        if (_currentTask < 0)
            _currentTask = _dailyTasks.tasks.Count - 1;
        UpdateCurrentTask();
    }

    public void UpdateTasks(DailyTasks dailyTasks)
    {
        _dailyTasks = dailyTasks;
        UpdateCurrentTask();
    }

    void UpdateCurrentTask()
    {
        var currentTask = _dailyTasks.tasks[_currentTask];
        iconView.sprite = DataContainer.Instance.taskIconCollection.GetSpriteByName(currentTask.iconId);
        progressText.text = string.Format(ProgressText, currentTask.progress, currentTask.stageCount);
        descriptionTxt.text = LocalizationManager.Localize(currentTask.description);
        rewardCountView.text = currentTask.rewardCount.ToString();
        if (currentTask.progress >= currentTask.stageCount && !currentTask.isComplited)
            rewardBtn.interactable = true;
        else
            rewardBtn.interactable = false;
        compliteObj.SetActive(currentTask.isComplited);
    }
}

[Serializable]
public class DailyTasks
{
    public List<DailyTask> tasks;
}

[Serializable]
public class DailyTask
{
    public string description;
    public int stageCount;
    public int progress;
    public string iconId;
    public int rewardCount;
    public TaskType taskType;
    public bool isComplited = false;
    public JsonDateTime dateTime;

    public void ResetTask()
    {
        progress = 0;
        isComplited = false;
        dateTime = DateTime.Now;
    }
}

public enum TaskType
{
    COMPANY,
    ARENA_WIN,
    PRIZE
}


