using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSkill1 : Hability {

    [SerializeField] private GameObject bloodExplosion;

    protected override void Cast() {

        bloodExplosion.transform.position = transform.position;
        bloodExplosion.GetComponent<BloodExplosionController>().Activate();
        StartCoroutine(C_Coldown());
    }
}
