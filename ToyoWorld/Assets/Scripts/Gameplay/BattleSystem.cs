using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

	public GameObject playerPrefab;
	public GameObject enemyPrefab;

	public Animator enemyAnimator;
	public Animator playerAnimator;

	public Transform playerBattleStation;
	public Transform enemyBattleStation;

	Unit playerUnit;
	Unit enemyUnit;
	EffectHandler effectHandler;

	public Text dialogueText;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public GameObject combatButtons;

	public GameObject endPanel;

	public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
		state = BattleState.START;
		StartCoroutine(SetupBattle());
    }

	IEnumerator SetupBattle()
	{
		effectHandler = GetComponent<EffectHandler>();
		GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
		playerUnit = playerGO.GetComponent<Unit>();
		playerAnimator = playerGO.GetComponent<Animator>();

		GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
		enemyUnit = enemyGO.GetComponent<Unit>();
		enemyAnimator = enemyGO.GetComponent<Animator>();

		dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);

		yield return new WaitForSeconds(2f);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

	IEnumerator PlayerAttack()
	{
        combatButtons.SetActive(false);
        bool isCrit = Random.Range(0, 10) > 5;
		int damage = playerUnit.damage;

        if (isCrit)
		{
			damage = damage * 2;
		}
		else
		{
            damage = playerUnit.damage;
        }

		bool isDead = enemyUnit.TakeDamage(damage);

		playerAnimator.SetTrigger("Attacked");

		enemyHUD.SetHP(enemyUnit.currentHP);
		if(isCrit)
		{
            dialogueText.text = "The attack is successful! It was a critical hit!!";
        }
		else { 
			dialogueText.text = "The attack is successful!";

        }

        yield return new WaitForSeconds(0.7f);

        Instantiate(effectHandler.PlayerAttack, effectHandler.enemyPoint);
        enemyAnimator.SetTrigger("isHurt");

        yield return new WaitForSeconds(2f);

        if (isDead)
		{
			state = BattleState.WON;
			EndBattle();
            yield return new WaitForSeconds(2f);
            endPanel.SetActive(true);
        } else
		{
			state = BattleState.ENEMYTURN;
			StartCoroutine(EnemyTurn());
			combatButtons.SetActive(false);
		}
	}

    IEnumerator UltimatePlayerAttack()
    {
        combatButtons.SetActive(false);
        bool isCrit = Random.Range(0, 10) > 5;
        bool doesLand = Random.Range(0, 100) > 75;
        int damage = playerUnit.damage;

        if (isCrit)
        {
            damage = damage * 2;
        }
        else
        {
            damage = playerUnit.damage;
        }

		if(!doesLand)
		{
			bool isDead = enemyUnit.TakeDamage(0);

            dialogueText.text = "Your Toyo couldn't land the attack...";

            yield return new WaitForSeconds(2f);

            if (isDead)
            {
                state = BattleState.WON;
                EndBattle();
                yield return new WaitForSeconds(2f);
                endPanel.SetActive(true);
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }
		else {

            bool isDead = enemyUnit.TakeDamage(damage*5);
            playerAnimator.SetTrigger("Attacked");
			enemyHUD.SetHP(enemyUnit.currentHP);
			if (isCrit)
			{
				dialogueText.text = "The Ultimate attack, it worked! It was a critical hit!!";
                Instantiate(effectHandler.UltimateEffect, effectHandler.enemyPoint);
                enemyAnimator.SetTrigger("isHurt");
			}
			else
			{
				dialogueText.text = "The Ultimate attack, it worked!";
                Instantiate(effectHandler.UltimateEffect, effectHandler.enemyPoint);
                enemyAnimator.SetTrigger("isHurt");
			}

			yield return new WaitForSeconds(2f);

			if (isDead)
			{
				state = BattleState.WON;
				EndBattle();
                yield return new WaitForSeconds(2f);
                endPanel.SetActive(true);
            }
			else
			{
				state = BattleState.ENEMYTURN;
				StartCoroutine(EnemyTurn());
			}
		}

    }

    IEnumerator EnemyTurn()
	{
		dialogueText.text = enemyUnit.unitName + " attacks!";
        enemyAnimator.SetTrigger("Attacked");
        playerAnimator.SetTrigger("isHurt");
        Instantiate(effectHandler.EnemyAttack, effectHandler.playerPoint);
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

		playerHUD.SetHP(playerUnit.currentHP);

		yield return new WaitForSeconds(2f);

        if (isDead)
		{
			state = BattleState.LOST;
			EndBattle();
            yield return new WaitForSeconds(1f);
			endPanel.SetActive(true);
        } else
		{
			state = BattleState.PLAYERTURN;
			PlayerTurn();
            combatButtons.SetActive(true);
        }

    }

	void EndBattle()
	{
		if(state == BattleState.WON)
		{
			dialogueText.text = "You won the battle!";
			enemyAnimator.SetBool("isDead", true);

		} else if (state == BattleState.LOST)
		{
            playerAnimator.SetBool("isDead", true);
            dialogueText.text = "You were defeated.";
		}
	}

	void PlayerTurn()
	{
		dialogueText.text = "Choose an action:";
	}

	IEnumerator PlayerHeal()
	{
        combatButtons.SetActive(false);
        playerAnimator.SetTrigger("Casted");
        Instantiate(effectHandler.HealEffect, effectHandler.playerPoint);
		playerUnit.Heal(5);

        playerHUD.SetHP(playerUnit.currentHP);
		dialogueText.text = "You feel renewed strength!";

		yield return new WaitForSeconds(2f);

		state = BattleState.ENEMYTURN;
		StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerBigHeal()
    {
        combatButtons.SetActive(false);
        playerUnit.Heal(15);

        bool canHeal = Random.Range(0, 10) > 5;

		if(canHeal)
		{
			playerAnimator.SetTrigger("Casted");
			playerHUD.SetHP(playerUnit.currentHP);
			dialogueText.text = "Power courses through you!";
            Instantiate(effectHandler.HealEffect, effectHandler.playerPoint);
        } else
		{
            dialogueText.text = "The ritual failed!";
        }


        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerAttack());
	}

    public void OnUltButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(UltimatePlayerAttack());
    }

    public void OnHealButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerHeal());
	}

    public void OnBigHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerBigHeal());
    }

}
