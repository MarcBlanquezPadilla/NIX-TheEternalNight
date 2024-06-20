using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LightBasicAttack : Hability {

    [SerializeField] private GameObject lightRay;
    [SerializeField] private float maxTimeActive = 1f;

    private bool checking = false;

    protected override void Cast() {

        lightRay.SetActive(true);

        if (!checking) {

            StartCoroutine(C_CheckInput());
        }

        if (!checking) {

            StartCoroutine(C_Coldown());
        }
    }

    private IEnumerator C_CheckInput() {

        checking = true;

        lightRay.GetComponent<BoxCollider>().enabled = true;
        lightRay.GetComponent<AudioSource>().Play();

        yield return new WaitUntil(() => Input.GetKeyUp(InputManager.Instance.keys["Basic Attack"]));

        lightRay.GetComponent<AudioSource>().Stop();
        lightRay.GetComponent<BoxCollider>().enabled = false;
        lightRay.SetActive(false);
        checking = false;
    }

	private void OnDisable() {
		
        StopAllCoroutines();
		lightRay.GetComponent<BoxCollider>().enabled = false;
		lightRay.SetActive(false);
		checking = false;
	}
}
