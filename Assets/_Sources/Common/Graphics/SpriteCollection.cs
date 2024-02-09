using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpriteCollection
{
    public NamedSprite[] sprites;

    public virtual Sprite GetSpriteByName(string id)
    {
        foreach (var item in sprites)
        {
            if (item.id.Equals(id))
                return item.sprite;
        }
        return null;
    }
}
