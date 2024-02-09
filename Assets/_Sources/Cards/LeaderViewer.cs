using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class LeaderViewer : MonoBehaviour
{
    public Image avatar;
    public TextMeshProUGUI healthViewer;
    public Image bg;
    public Sprite[] bgs;
    [Inject] AvatarCollection avatarCollection;
    private int health = 0;
    private GameObject prefView;
    [SerializeField] private GameObject deathObject;
    public bool IsDead => health <= 0;

    public void ShowCard(LeaderCard leaderCard)
    {
        //avatar.sprite = avatarCollection.GetSpriteByName(leaderCard.id);
        if(prefView)
            Destroy(prefView);
        prefView = Instantiate(DataContainer.Instance.prefsCollection.GetSpriteByName(leaderCard.id),
            avatar.transform);
        bg.sprite = bgs[(int) leaderCard.fraction];
        health = leaderCard.health;
        UpdateHealth();
        gameObject.SetActive(true);
    }

    public int GetCurentHealth()
    {
        return health;
    }

    void UpdateHealth()
    {
        healthViewer.text = health.ToString();
    }

    public async UniTask<bool> Defence(int atack)
    {
        health -= atack;
        UpdateHealth();
        await SoundEngine.PlayEffect("card_atack");
        deathObject.SetActive(health <= 0);
        return health <= 0;
    }
}
