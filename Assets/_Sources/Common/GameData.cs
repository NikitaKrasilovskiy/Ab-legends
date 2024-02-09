using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameData", menuName = "ArgyBargy/GameData", order = 1)]
public class GameData : ScriptableObjectInstaller<GameData>
{
    public PlayerData playerData;
    public Settings settings;
    public WarriorCardData warriorCardData;

    [Serializable]
    public class Settings
    {
        public float masterVolume;
        public int language;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void InstallBindings()
    {
        Container.BindInstance(playerData).AsSingle();
        Container.BindInstance(settings).AsSingle();
        Container.BindInstance(warriorCardData).AsSingle();
    }
}
