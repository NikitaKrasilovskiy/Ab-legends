using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Star : MonoBehaviour
{
    [SerializeField]
    Sprite spriteOn;
    [SerializeField]
    Sprite spriteOff;
    public bool curentState { get; private set; }
    Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void ChangeState(bool state)
    {
        curentState = state;
        UpdateState();
    }

    void UpdateState()
    {
        if (curentState)
            _image.sprite = spriteOn;
        else
            _image.sprite = spriteOff;
    }
}
