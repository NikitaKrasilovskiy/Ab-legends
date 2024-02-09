using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlowController : MonoBehaviour
{
    [Range(0,6)]
    [SerializeField] private float glow;
    private Material _material;

    private void Awake()
    {
        _material = GetComponent<Image>().material;
    }

    void Update()
    {
        _material.SetFloat("_AlphaIntensity_Fade_1", glow);
    }
}
