using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopArrows : MonoBehaviour
{
    public RectTransform content;
    public void OnArrowPressed(bool rightArrow)
    {
        int direction = rightArrow ? -1 : 1;
        content.anchoredPosition += direction * new Vector2(100, 0);

    }
}
