using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlPanel : MonoBehaviour
{
    public GameObject[] lvlViewers;

    public void SetLvl(int lvl)
    {
        for (int i = 0; i < lvlViewers.Length; i++)
        {
            lvlViewers[i].SetActive(lvl>=(i+1));
        }
    }
}
