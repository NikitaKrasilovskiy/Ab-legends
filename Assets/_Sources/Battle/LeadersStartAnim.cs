using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

public class LeadersStartAnim : MonoBehaviour
{
    [SerializeField] private Transform leftLeaderContainer;
    [SerializeField] private Transform rightLeaderContainer;
    [SerializeField] private GameObject[] leaderPrefs;
    [SerializeField] private PlayableDirector playableDirector;

    public async UniTask ShowAnim(LeaderCard enemy, LeaderCard player)
    {
        Debug.Log((int) enemy.fraction);
        var leftObject = Instantiate(leaderPrefs[(int) enemy.fraction], leftLeaderContainer);
        var rightObject = Instantiate(leaderPrefs[(int) player.fraction], rightLeaderContainer);
        rightObject.transform.localScale = new Vector3(-1, 1, 1);
        gameObject.SetActive(true);
        playableDirector.Play();
        await UniTask.WaitUntil(() => playableDirector.state != PlayState.Playing);
        gameObject.SetActive(false);
    }
}
