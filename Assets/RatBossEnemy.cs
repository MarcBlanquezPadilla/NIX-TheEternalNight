using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBossEnemy : Enemy {

    public override void Persecution() {

    }

    public override void Attack() {

        attacking = true;
        if (Random.Range(0,2) == 1)
        {
            elementController.currentElement = "Blood";
            GetComponent<Animator>().SetInteger("State", 2);
        }
        else
        {
            elementController.currentElement = "Poison";
            GetComponent<Animator>().SetInteger("State", 3);
        }
        navMeshAgent.isStopped = true;
    }

    public void AttackAnim()
    {
        elementController.CastHability(0);
    }

    public void EnablePowers()
    {
        GameManager.Instance.characterController.TakeBlood();
        GameManager.Instance.characterController.TakePoison();
    }

    public void LeaveAttacking()
    {
        attacking = false;
        actualState = State.Persecution;
    }

    public override IEnumerator C_Die(DissolveSpider dissolveSpider)
    {
        yield return null;
    }

    public override IEnumerator C_Die(DissolveSlime dissolveSlime)
    {
        yield return null;
    }

    public override IEnumerator C_Die(DissolveRata dissolveRata)
    {
        yield return StartCoroutine(dissolveRata.SDissolveRata());

        gameObject.SetActive(false);
    }
}
