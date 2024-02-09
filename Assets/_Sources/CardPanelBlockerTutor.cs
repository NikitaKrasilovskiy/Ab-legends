using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardPanelBlockerTutor : MonoBehaviour
{
    [SerializeField] private UnityEvent onClick;

    public void OnClick()
    {
        if (TutorialController.Instance.TutorialStep == 11)
        {
            return;
        }
        onClick.Invoke();
    }
}
