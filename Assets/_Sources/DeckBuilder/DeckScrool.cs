using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckScrool : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;

    public void MoveLeft()
    {
        var count = scrollRect.transform.GetChild(0).GetChild(0).childCount;
        var offset = scrollRect.horizontalNormalizedPosition - 1f / count;
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp(offset, 0, 1);
    }
    
    public void MoveRight()
    {
        var count = scrollRect.transform.GetChild(0).GetChild(0).childCount;
        var offset = scrollRect.horizontalNormalizedPosition + 1f / count;
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp(offset, 0, 1);
    }
}
