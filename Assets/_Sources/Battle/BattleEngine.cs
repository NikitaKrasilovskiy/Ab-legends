using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Zenject;

public class BattleEngine : MonoBehaviour
{
    [SerializeField]
    DeckViewer enemyViewer;
    [SerializeField]
    DeckViewer playerViewer;

    [SerializeField] private BattleResult _battleResult;
    Deck enemyDeck;
    Deck playerDeck;

    float startDellay = 0;
    bool playerTurn = true;
    bool battleComplite = false;
    [Inject] private CardDataContainer _cardDataContainer;

    public void Init(Deck enemyDeck, Deck playerDeck)
    {
        this.enemyDeck = enemyDeck;
        this.playerDeck = playerDeck;
        enemyViewer.InitViewer(enemyDeck);
        playerViewer.InitViewer(playerDeck);
        //Temp timescale
        //Time.timeScale = 4;
        //
        if(TutorialController.Instance.isTutorialComplete)
            Battle();
    }

    async UniTask BattleCircle()
    {
        await UniTask.WaitForEndOfFrame();
        while (!battleComplite)
        {
            if (playerTurn)
            {
                if (await Turn(playerViewer, enemyViewer))
                    Win();
            }
            else
            {
                if (await Turn(enemyViewer, playerViewer))
                    Lose();
            }
            playerTurn = !playerTurn;
        }
    }

    private void Lose()
    {
        
        battleComplite = true;
        _battleResult.Lose();
    }

    private void Win()
    {
        battleComplite = true;
        _battleResult.Win();
    }

    async UniTask<bool> Turn(DeckViewer attackingDeck, DeckViewer atackedDeck)
    {
        for(int i = 0; i<attackingDeck.deckLength; i++)
        {
            if (atackedDeck.deckLength > 0)
            {
                int atackedId = Mathf.Clamp(i, 0, atackedDeck.deckLength - 1);
                var atackedCard = atackedDeck.cardViewers[atackedId];
                bool isCardDie = await ProcessTurn(attackingDeck.cardViewers[i],
                    atackedCard);
                if (isCardDie)
                {
                    atackedDeck.DestroyCard(atackedCard);
                }

                if (atackedDeck.leaderViewer.IsDead)
                    return true;
            }
            else
            {
                bool isLeaderDead = await ProcessTurn(attackingDeck.cardViewers[i],
                    atackedDeck.leaderViewer);
                if(isLeaderDead)
                    return true;
            }

        }
        await UniTask.Delay(1000/BattleDataContainer.timeScale);
        return false;
    }

    private async Task<bool> ProcessTurn(CardViewer attackingDeckCardViewer, LeaderViewer atackedDeckLeaderViewer)
    {
        int extraDamage = 0;
        switch (attackingDeckCardViewer.warriorCard.ability)
        {
            case "acorn4x0":
            {
                await attackingDeckCardViewer.ShowAbility();
                extraDamage = (int)attackingDeckCardViewer.warriorCard.abilityPower;
                break;
            }
            case "acorn4x1":
            {
                await attackingDeckCardViewer.ShowAbility();
                extraDamage = (int)attackingDeckCardViewer.warriorCard.abilityPower;
                break;
            }
        }
        return await atackedDeckLeaderViewer.Defence(await attackingDeckCardViewer.Atack()+extraDamage);
    }

    async UniTask<bool> ProcessTurn(CardViewer attackingCard, CardViewer atackedCard)
    {
       // Debug.Log("Process turn "+JsonUtility.ToJson(attackingCard.warriorCard));
        bool dontAtack = false;
        if (attackingCard.warriorCard.ability.Equals("bobber3x0"))
        {
            dontAtack = Random.Range(1, 10000) <= (10000*attackingCard.warriorCard.abilityPower);
        }
        if (attackingCard.warriorCard.ability.Equals("bobber3x1"))
        {
            dontAtack = Random.Range(1, 10000) <= (10000*attackingCard.warriorCard.abilityPower);
        }

        if (dontAtack)
        {
            await attackingCard.ShowAbility();
            return false;
        }

        bool isEnemyDead = await atackedCard.Defence(await attackingCard.Atack());
      //  Debug.Log("Ability processing...");
        switch (attackingCard.warriorCard.ability)
        {
            case "NaN":
            {
                //Debug.Log("Nan Turn complite");
                return isEnemyDead;
            }
            case "acorn0x0":
            {
                int range = Random.Range(1, 10000);
                if (range <= (10000*attackingCard.warriorCard.abilityPower))
                {
                    await attackingCard.ShowAbility();
                    isEnemyDead = await atackedCard.Defence(await attackingCard.Atack(atackedCard.warriorCard.health));
                }

                break;
            }
            case "acorn0x1":
            {
                int range = Random.Range(1, 10000);
                if (range <= (10000*attackingCard.warriorCard.abilityPower))
                {
                    await attackingCard.ShowAbility();
                    isEnemyDead = await atackedCard.Defence(await attackingCard.Atack(atackedCard.warriorCard.health));
                }

                break;
            }
            case "acorn1x0":
            {
                int range = Random.Range(0, 3);
                if (range == 1 || range == 2 || range == 3)
                {
                    await attackingCard.ShowAbility();
                    atackedCard.ChangeAtack(-(int)attackingCard.warriorCard.abilityPower);
                }
                break;
            }
            case "acorn1x1":
            {
                int range = Random.Range(0, 3);
                if (range == 1 || range == 2 || range == 3)
                {
                    await attackingCard.ShowAbility();
                    atackedCard.ChangeAtack(-(int)attackingCard.warriorCard.abilityPower);
                }
                break;
            }
            case "acorn2x0":
            {
                int range = Random.Range(0, 3);
                if (range == 1 || range == 2 || range == 3)
                {
                    var repairDeck = playerTurn ? playerViewer : enemyViewer;
                    await attackingCard.ShowAbility();
                    foreach (var VARIABLE in repairDeck.cardViewers)
                    {
                        VARIABLE.RepairHealth((int)attackingCard.warriorCard.abilityPower);
                    }
                }
                break;
            }
            case "acorn2x1":
            {
                int range = Random.Range(0, 3);
                if (range == 1 || range == 2 || range == 3)
                {
                    var repairDeck = playerTurn ? playerViewer : enemyViewer;
                    await attackingCard.ShowAbility();
                    foreach (var VARIABLE in repairDeck.cardViewers)
                    {
                        VARIABLE.RepairHealth((int)attackingCard.warriorCard.abilityPower);
                    }
                }
                break;
            }
            case "acorn3x0":
            {
                int range = Random.Range(1, 10000);
                var oponentDeck = !playerTurn ? playerViewer : enemyViewer;
                if (range <= (10000*attackingCard.warriorCard.abilityPower))
                {
                    await attackingCard.ShowAbility();
                    await oponentDeck.leaderViewer.Defence(await attackingCard.Atack());
                }

                break;
            }
            case "acorn3x1":
            {
                int range = Random.Range(1, 10000);
                var oponentDeck = !playerTurn ? playerViewer : enemyViewer;
                if (range <= (10000*attackingCard.warriorCard.abilityPower))
                {
                    await attackingCard.ShowAbility();
                    await oponentDeck.leaderViewer.Defence(await attackingCard.Atack());
                }

                break;
            }
            case "bobber0x0":
            {
                int range = Random.Range(1, 10000);
                if (range <= (10000*attackingCard.warriorCard.abilityPower))
                {
                    await attackingCard.ShowAbility();
                    atackedCard.zero_atck = true;
                }

                break;
            }
            case "bobber0x1":
            {
                int range = Random.Range(1, 10000);
                if (range <= (10000*attackingCard.warriorCard.abilityPower))
                {
                    await attackingCard.ShowAbility();
                    atackedCard.zero_atck = true;
                }

                break;
            }
            case "bobber1x0":
            {
                int range = Random.Range(1, 10000);
                if (range <= (10000*attackingCard.warriorCard.abilityPower))
                {
                    await attackingCard.ShowAbility();
                    isEnemyDead = await atackedCard.Defence(await attackingCard.Atack());
                }

                break;
            }
            case "bobber4x0":
            {
                var repairDeck = playerTurn ? playerViewer : enemyViewer;
                await attackingCard.ShowAbility();
                repairDeck.cardViewers[Random.Range(0,repairDeck.cardViewers.Count)].RepairHealth((int)attackingCard.warriorCard.abilityPower);
                break;
            }
            case "bobber4x1":
            {
                var repairDeck = playerTurn ? playerViewer : enemyViewer;
                await attackingCard.ShowAbility();
                repairDeck.cardViewers[Random.Range(0,repairDeck.cardViewers.Count)].RepairHealth((int)attackingCard.warriorCard.abilityPower);
                break;
            }
        }
        //Debug.Log("Turn complite");
        return isEnemyDead;
    }

    public async void Battle()
    {
        await UniTask.Yield();
        await UniTask.Delay((int)(startDellay * 1000));
        await BattleCircle();
    }


}
