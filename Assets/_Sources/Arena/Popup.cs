using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private Button _cancel;
    private Action _action;
    private static Popup _instance;

    public static Popup Instance
    {
        get
        {
            if (_instance)
                return _instance;
            else
            {
                _instance = Instantiate(Resources.Load<Popup>("Popup"));
                return _instance;
            }
        }
    }
    
    private void Awake()
    {
        _cancel.onClick.AddListener(OnCancel);
    }

    public void AddAction(Action action = null)
    {
        _action = action;
        
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void DoSomething()
    {
        AddAction(Log);
    }

    void Log()
    {
        Debug.Log("Im");
    }
    
    void OnCancel()
    {
        _action?.Invoke();
        gameObject.SetActive(false);
    }
}
