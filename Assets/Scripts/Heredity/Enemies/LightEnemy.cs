using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEnemy : Enemy {

    public override void Persecution() {

    }

    public override void Attack() {

        navMeshAgent.isStopped = true;
        StartCoroutine(C_WaitUntilAtackEnds(attackDuration));
    }

    public override IEnumerator C_Die(DissolveSpider dissolveSpider)
    {
        yield return null;
    }

    public override IEnumerator C_Die(DissolveSlime dissolveSlime)
    {

        yield return StartCoroutine(dissolveSlime.SDissolveSlime());

        gameObject.SetActive(false);
    }

    public override IEnumerator C_Die(DissolveRata dissolveSlime)
    {
        yield return null;
    }
}
