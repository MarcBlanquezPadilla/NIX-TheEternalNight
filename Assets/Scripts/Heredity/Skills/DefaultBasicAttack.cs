using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBasicAttack : Hability {

    [SerializeField] private GunController gunController;

    protected override void Cast() {

        gunController.Shoot();
        StartCooldown();
    }
}
