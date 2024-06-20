using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowEnemy : Enemy {

    [Header("Invisibility")]
    [SerializeField] private float timeToInvisibility = 10f;

    [SerializeField] private GameObject meleArea;

    public override void Persecution() {

        SetInvisible();
    }

    public override void Attack() {

        navMeshAgent.isStopped = true;
        GetComponent<Animator>().SetInteger("State", 2);
        StartCoroutine(C_WaitUntilAtackEnds(attackDuration));
    }


    public void SetInvisible() {

        elementController.elements[elementController.currentElement].CastHability(1);
        elementController.elements[elementController.currentElement].CastHability(2);
        elementController.elements[elementController.currentElement].CastHability(3);
    }


    public void GiveShadow() {

        FindObjectOfType<CharacterController>().TakeShadow();
    }

    public void EnableAttackArea() {

        meleArea.SetActive(true);
    }

    public void DisableAttackArea() {

        meleArea.SetActive(false);
    }

    public override IEnumerator C_Die(DissolveSlime dissolveSlime) {

        yield return null;
    }

    public override IEnumerator C_Die(DissolveSpider dissolveSpider) {

        yield return StartCoroutine(dissolveSpider.SDissolveSpider());

        gameObject.SetActive(false);
    }

    public override IEnumerator C_Die(DissolveRata dissolveSlime)
    {
        yield return null;
    }
}
