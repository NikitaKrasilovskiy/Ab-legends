using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        var cardDataContainer = new CardDataContainer();
        Container.BindInstance(cardDataContainer).AsSingle();
    }
}
