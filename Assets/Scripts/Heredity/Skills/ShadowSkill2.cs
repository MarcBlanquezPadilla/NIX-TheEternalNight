using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShadowSkill2 : Hability {

    [Header("Properties")]
    [SerializeField] private float maxTimeActive = 2f;
    [SerializeField] private float speedMultiplierShading = 3;
    [SerializeField] private int abilityLayer;
    private int defaultLayer;

    [Header("References")]
    [SerializeField] private GameObject defaultSkin;
    [SerializeField] private GameObject shadeSkin;

    private Material defeaultMaterial;

    private void Start() {

        defaultLayer = whoAmI.layer;
    }

    protected override void Cast() {

        StartCoroutine(C_WhileShade());

        if (whoAmI.GetComponent<CharacterController>() != null)  {

            whoAmI.GetComponent<CharacterController>().shading = true;
        }

        if (whoAmI.GetComponent<Enemy>() != null) {

            whoAmI.GetComponent<CapsuleCollider>().enabled = false;
        }

        whoAmI.layer = abilityLayer;

        for (int i = 0; i < whoAmI.transform.childCount; i++) {

            whoAmI.transform.GetChild(i).gameObject.layer = abilityLayer;
        }

        if (whoAmI.GetComponent<CharacterController>() != null) {

			ShadowWallController[] shadowWallController = FindObjectsOfType<ShadowWallController>();

			for (int i = 0; i < shadowWallController.Length; i++) {

				shadowWallController[i].ShadeWall(maxTimeActive);
			}
		}

        defaultSkin.SetActive(false);
        shadeSkin.SetActive(true);

        StartCoroutine(C_DisableShade());
    }

    IEnumerator C_WhileShade() {

        float counter = 0f;

        while (counter < maxTimeActive) {

            if (whoAmI.GetComponent<MovementBehaviour>() != null) {

                whoAmI.GetComponent<MovementBehaviour>().SetSpeed(whoAmI.GetComponent<MovementBehaviour>().ReturnDefaultSpeed() * speedMultiplierShading);
            }

            //if (whoAmI.GetComponent<NavMeshAgent>() != null) {

            //    whoAmI.GetComponent<NavMeshAgent>().velocity = whoAmI.GetComponent<NavMeshAgent>().velocity * 1.5f;
            //}

            counter += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator C_DisableShade() {

        yield return new WaitForSeconds(maxTimeActive);

        whoAmI.layer = defaultLayer;
        for (int i = 0; i < whoAmI.transform.childCount; i++)
        {
            whoAmI.transform.GetChild(i).gameObject.layer = defaultLayer;
        }

        shadeSkin.SetActive(false);
        defaultSkin.SetActive(true);

        if (whoAmI.GetComponent<MovementBehaviour>() != null) {

            whoAmI.GetComponent<MovementBehaviour>().SetSpeed(whoAmI.GetComponent<MovementBehaviour>().ReturnDefaultSpeed());
        }

        if (whoAmI.GetComponent<CharacterController>() != null) {

            whoAmI.GetComponent<CharacterController>().shading = false;
        }

        if (whoAmI.GetComponent<Enemy>() != null) {

            whoAmI.GetComponent<CapsuleCollider>().enabled = true;
        }

        StartCoroutine(C_Coldown());
    }
}
