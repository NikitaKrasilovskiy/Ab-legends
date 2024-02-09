using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.SimpleLocalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using static BattleDataContainer;

public class CardViewer : MonoBehaviour
{
    [SerializeField]
    Image avatarView;

    [SerializeField] private Image _border;
    [SerializeField] private Sprite[] _fractionBorders;
    [SerializeField] private Image _bg;
    [SerializeField] private Sprite[] _fractionBgs;
    [SerializeField] private TextMeshProUGUI _nameViewer;
    [SerializeField]
    TextMeshProUGUI lvlView;
    [SerializeField]
    TextMeshProUGUI atkViewer;
    [SerializeField]
    TextMeshProUGUI healthViewer;
    [SerializeField] private Image abilityView;
    [SerializeField] private Animation abilityAnim;
    [SerializeField] private GameObject deathObject;
    int _atack;
    int _health;
    int _maxHealth;
    int _maxAtack;
    const string RED_STRING = "<color=orange>{0}</color>";
    const string GREEN_TEXT = "<color=green>{0}</color>";
    const string WHITE_TEXT = "{0}";
    private Transform _transform;
    public WarriorCard warriorCard;
    public bool zero_atck;
    private GameObject prefView;
    
    void Start()
    {
        _transform = transform;
    }

    public async UniTask ShowAbility()
    {
        Debug.Log("Play ability anim "+warriorCard.ability);
        abilityAnim.gameObject.SetActive(true);
        abilityAnim.Play();
        while(!abilityAnim.isPlaying)
            //await Task.Yield();
        while (abilityAnim.isPlaying)
        {
            //await Task.Yield();
        }
        abilityAnim.gameObject.SetActive(false);
    }
    
    public void SetCard(WarriorCard warriorCard)
    {
//        Debug.Log(warriorCard.id);
        //avatarView.sprite = DataContainer.Instance.avatarCollection.GetSpriteByName(warriorCard.id);
        if(prefView)
            Destroy(prefView);
        prefView = Instantiate(DataContainer.Instance.prefsCollection.GetSpriteByName(warriorCard.id),
            avatarView.transform);
        _border.sprite = _fractionBorders[(int)warriorCard.fraction];
        _bg.sprite = _fractionBgs[(int) warriorCard.fraction];
//        Debug.Log(JsonUtility.ToJson(warriorCard));
        _nameViewer.text = LocalizationManager.Localize(warriorCard.name);
        UpdateLvlView(warriorCard.lvl);
        _atack = warriorCard.atack;
        _health = warriorCard.health;
        _maxHealth = warriorCard.health;
        atkViewer.text = _atack.ToString();
        healthViewer.text = _health.ToString();
        abilityView.sprite = DataContainer.Instance.abilityCollection.GetSpriteByName(warriorCard.ability);
        this.warriorCard = warriorCard;
    }

    public void UpdateLvlView(int i)
    {
        lvlView.text = i.ToString();
//        Debug.Log("LVL "+i);
    }

    public async UniTask<int> Atack()
    {
        if (zero_atck)
        {
            zero_atck = false;
            return 0;
        }
        var yPos = transform.localPosition.y;
        SoundEngine.PlayEffect(warriorCard.id.Split('_')[0]+"_atack");
        transform.DOLocalMoveY(yPos+(100*(transform.position.y>0?-1:1)), 0.25f/timeScale);
        transform.DOLocalMoveY(yPos, 0.25f/timeScale);
        
        return _atack;
    }
    
    public async UniTask<int> Atack(int power)
    {
        var yPos = transform.localPosition.y;
        SoundEngine.PlayEffect(warriorCard.id.Split('_')[0]+"_atack");
        transform.DOLocalMoveY(yPos+(100*(transform.position.y>0?-1:1)), 0.25f/timeScale);
        transform.DOLocalMoveY(yPos, 0.25f/timeScale);
        
        return power;
    }

    public async UniTask<bool> Defence(int atack)
    {
        if (atack <= 0)
            return false;
        ChangeHealth(_health - atack);
        
        //SoundEngine.PlayEffect(warriorCard.id.Split('_')[0]+"_damage");
        
        if (_health <= 0)
        {
            await Death();
        }
        else
        {
            SoundEngine.PlayEffect("card_take_damage");
            float power = 2;
            transform.DOLocalMoveX(power,0.2f/timeScale);
            transform.DOLocalMoveX(-power,0.2f/timeScale);
            transform.DOLocalMoveX(power,0.2f/timeScale);
            transform.DOLocalMoveX(-power,0.2f/timeScale);
            transform.DOLocalMoveX(0f,0.2f/timeScale);
        }
        return _health <= 0;
    }

    public void ChangeHealth(int i)
    {
        _health = i;
        healthViewer.text = string.Format(_health < _maxHealth ? RED_STRING 
            : _health == _maxHealth ? WHITE_TEXT : RED_STRING, _health);
    }

    public void RepairHealth(int i)
    {
        _health += i;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        ChangeHealth(_health);
    }
    
    public void ChangeAtack(int i)
    {
        _atack += i;
        _atack = Mathf.Clamp(_atack,0, _atack);
        
        if (_atack < 1)
        {
            _atack = 1;
        }
        atkViewer.text = string.Format(_atack < _maxAtack ? RED_STRING 
            : _atack == _maxAtack ? WHITE_TEXT : RED_STRING, _atack);
    }
    
    public async UniTask Death()
    {
        deathObject.SetActive(true);
        SoundEngine.PlayEffect(warriorCard.id.Split('_')[0]+"_death");
        transform.DOShakePosition(1f/timeScale, 1, 100);
        
    }
}

public enum Debuff
{
    ZERO_ATACK
}
