using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSkill2 : Hability {
    
    [SerializeField] private GameObject poisonGasObj;

    private void Awake()
    {
        poisonGasObj.GetComponent<PoisonGasController>().onEnd += StartCooldown;
    }

    protected override void Cast()
    {
        casteable = false;
        poisonGasObj.GetComponent<PoisonGasController>().Activate();
    }
}
