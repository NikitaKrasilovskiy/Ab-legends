using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurrendTutorial : MonoBehaviour
{
    [SerializeField] private Button btn;

    void Start()
    {
        if (!TutorialController.Instance.isTutorialComplete)
            btn.interactable = false;
    }
}
