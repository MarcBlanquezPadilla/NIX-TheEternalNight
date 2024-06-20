using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEnemy : Enemy {

    public override void Persecution() {

    }

    public override void Attack() {

        navMeshAgent.isStopped = true;
        GetComponent<Animator>().SetInteger("State", 2); 
    }

    public void AttackAnim()
    {
        elementController.CastHability(0);
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

    public void LeaveAttacking()
    {
        attacking = false;
        actualState = State.Persecution;
    }
}
