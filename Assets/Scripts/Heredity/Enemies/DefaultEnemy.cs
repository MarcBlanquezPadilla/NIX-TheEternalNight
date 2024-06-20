using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemy : Enemy {

    public override void Persecution() {

    }

    public override void Attack() {


    }

    public override IEnumerator C_Die(DissolveSpider dissolveSpider)
    {

        yield return null;
    }

    public override IEnumerator C_Die(DissolveSlime dissolveSlime)
    {
        yield return null;
    }

    public override IEnumerator C_Die(DissolveRata dissolveSlime)
    {
        yield return null;
    }
}
