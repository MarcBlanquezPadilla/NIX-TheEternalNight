using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hability : MonoBehaviour {

    [SerializeField] protected float coolDown;
    [HideInInspector] public GameObject launchedBy = null;
    [SerializeField] protected GameObject whoAmI;

    protected bool casteable = true;

    public void TryCast() {

        if (casteable) {

            Cast();
        }
    }

    protected abstract void Cast();

    protected IEnumerator C_Coldown() {

        casteable = false;
        float timer = 0;

        while (timer <= coolDown) {

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        casteable = true;
    }

    public string GetName() {

        return this.name;
    }

    public void StartCooldown() {

        StartCoroutine(C_Coldown());
    }
}
