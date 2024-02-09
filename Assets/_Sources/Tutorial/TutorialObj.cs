using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialObj : MonoBehaviour
{
    public int id = 0;
    [SerializeField]
    public UnityEvent onNext;
}
