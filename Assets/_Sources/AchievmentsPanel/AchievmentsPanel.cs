using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievmentsPanel : MonoBehaviour
{
    [SerializeField] private AchievView achievViewPref;
    [SerializeField] private Transform conteiner;
    private List<AchievView> _achievViews = new List<AchievView>();

    void Start()
    {
        SpawnTestAchievments();
    }

    void SpawnTestAchievments()
    {
        foreach (var variable in _achievViews)
        {
            Destroy(variable.gameObject);
        }

        var achievmentsData = new List<AchievData>();
        achievmentsData.Add(new AchievData(){description = "Test achievment 1", iconId = "test0", progress = 0, stageCount = 3, reward = 10});
        achievmentsData.Add(new AchievData(){description = "Test achievment 2", iconId = "test1", progress = 0, stageCount = 3, reward = 15});
        achievmentsData.Add(new AchievData(){description = "Test achievment 3", iconId = "test2", progress = 3, stageCount = 3, reward = 20});
        foreach (var achievData in achievmentsData)
        {
            var achivView = Instantiate(achievViewPref, conteiner);
            achivView.UpdateView(achievData);
        }
    }
    
}

[Serializable]
public class AchievData
{
    public string iconId;
    public string description;
    public int stageCount;
    public int progress;
    public bool isComplited;
    public int reward;
}
