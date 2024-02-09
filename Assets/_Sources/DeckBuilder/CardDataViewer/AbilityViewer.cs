using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityViewer : MonoBehaviour
{
    [SerializeField] private Image iconViewer;
    [SerializeField] private TextMeshProUGUI descriptionView;

    public void ShowAbility(Sprite icon, string description)
    {
        iconViewer.sprite = icon;
        descriptionView.text = description;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
