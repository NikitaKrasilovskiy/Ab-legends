using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DataContainer : MonoBehaviour
{
    private static DataContainer _instance;

    public static DataContainer Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType<DataContainer>();
            return _instance;
        }
    }
    [Inject] public PlayerData playerData;
    [Inject] public AvatarCollection avatarCollection;
    [Inject] public AbilityCollection abilityCollection;
    [Inject] public CardDataContainer cardDataContainer;
    [Inject] public TaskIconCollection taskIconCollection;
    [Inject] public AchievmentIconCollection achievmentIconCollection;
    [Inject] public PrefsCollection prefsCollection;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
