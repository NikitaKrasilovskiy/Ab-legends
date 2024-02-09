using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour
{
    public string pageId;
    [SerializeField]
    CompanyManager companyManager;
    [SerializeField]
    List<LevelPoint> levelPoints;

    private void Start()
    {
        foreach (var item in levelPoints)
            companyManager.UpdateLvlPoint(item);
    }
}
