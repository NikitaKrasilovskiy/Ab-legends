using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mask : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image image;

    private void Update()
    {
        image.fillAmount = slider.value;
       //string.Format("{0:D}")
    }
}
