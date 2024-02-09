using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattlePreloader : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null;
        var loader = SceneManager.LoadSceneAsync("Battle");
        loader.allowSceneActivation = false;
        while (loader.progress < 0.8f)
            yield return null;
        loader.allowSceneActivation = true;
    }
}
