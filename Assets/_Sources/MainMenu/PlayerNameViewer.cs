using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;

public class PlayerNameViewer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textView;
    [SerializeField] private TMP_InputField _inputField;

    void Start()
    {
        if(_textView!=null)
            _textView.text = DataContainer.Instance.playerData.playerProfileModel.DisplayName;
        if (_inputField != null)
            _inputField.text = DataContainer.Instance.playerData.playerProfileModel.DisplayName;
    }
}
