using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GraphicCollection", menuName = "ArgyBargy/GraphicCollection", order = 1)]
public class GraphicCollection : ScriptableObjectInstaller<GraphicCollection>
{
    public AvatarCollection avatarCollection;
    public AbilityCollection abilityCollection;
    public TaskIconCollection taskIconCollection;
    public AchievmentIconCollection achievmentIconCollection;
    public PrefsCollection prefsCollection;

    public override void InstallBindings()
    {
        Container.BindInstance(avatarCollection).IfNotBound();
        Container.BindInstance(abilityCollection).IfNotBound();
        Container.BindInstance(taskIconCollection).IfNotBound();
        Container.BindInstance(achievmentIconCollection).IfNotBound();
        Container.BindInstance(prefsCollection).IfNotBound();
    }
}

[Serializable]
public class PrefsCollection
{
    public List<NamedPrefs> namedPrefs;
    
    public GameObject GetSpriteByName(string id)
    {
        GameObject pref = null;
        Debug.Log(id);
        id = id.Remove(id.Length - 2, 2);
            foreach (var item in namedPrefs)
            {
                if (item.id.Contains(id))
                    pref = item.pref;
            }

        return pref;
    }
}

[Serializable]
public struct NamedPrefs
{
    public string id;
    public GameObject pref;
}

[Serializable]
public class AvatarCollection : SpriteCollection
{
    public override Sprite GetSpriteByName(string id)
    {
        var sprite = base.GetSpriteByName(id);
        if (!sprite)
        {
            id = id.Remove(id.Length - 2, 2);
            foreach (var item in sprites)
            {
                if (item.id.Contains(id))
                    sprite = item.sprite;
            }
        }

        return sprite;
    }
}
[Serializable]
public class AbilityCollection: SpriteCollection{} 

[Serializable]
public class TaskIconCollection: SpriteCollection{}

[Serializable]
public class AchievmentIconCollection: SpriteCollection{}
