using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBasicAttack : Hability {

    [SerializeField] private GameObject meleArea;
    [SerializeField] private float timeCasting;

    private float timer;


    protected override void Cast()
    {
        timer = 0;
        meleArea.SetActive(true);
        StartCooldown();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeCasting)
        {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        meleArea.SetActive(false);
    }
}
