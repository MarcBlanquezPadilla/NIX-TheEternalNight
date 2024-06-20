using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSkill1 : Hability {

    [SerializeField] private GameObject tunnelObj;

    protected override void Cast() {

        tunnelObj.SetActive(true);
        StartCoroutine(C_WaitTillEndHability());
    }

    public bool CheckHabilityState() {

        return tunnelObj.activeInHierarchy;
    }

    IEnumerator C_WaitTillEndHability() {


        yield return new WaitUntil(CheckHabilityState);
        StartCoroutine(C_Coldown());
    }
}
