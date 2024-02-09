using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileEmoji : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Image[] emojiViews;
    private int curentEmoji = 0;
    void Start()
    {
        curentEmoji = PlayerPrefs.GetInt("Emoji", 0);
        UpdateEmoji(curentEmoji);
    }

    public void UpdateEmoji()
    {
        curentEmoji++;
        if (curentEmoji >= sprites.Length)
            curentEmoji = 0;
        UpdateEmoji(curentEmoji);
    }

    public void UpdateEmoji(int id)
    {
        foreach (var emojiView in emojiViews)
        {
            emojiView.sprite = sprites[id];
        }
        PlayerPrefs.SetInt("Emoji", id);
    }
}
