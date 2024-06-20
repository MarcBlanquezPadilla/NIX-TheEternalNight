using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemiesController : MonoBehaviour {

    private Material defaultMaterial;

    [Header("References")]
    private MeshRenderer meshRenderer;

    private void Awake() {

        meshRenderer = GetComponent<MeshRenderer>();
        defaultMaterial = GetComponent<MeshRenderer>().material;

        ResetMaterial();
    }

    public void ResetMe() {

        meshRenderer.enabled = false;

        for (int i = 0; i < transform.childCount; i++) {

            transform.GetChild(i).gameObject.SetActive(false);
        }

        StartCoroutine(C_RestMe());
    }

    public void ResetMaterial() {

        StopAllCoroutines();
        StartCoroutine(C_RestetMaterial());
    }

    IEnumerator C_RestMe() {

        yield return new WaitForSeconds(2f);

        meshRenderer.material = defaultMaterial;
        GetComponent<HealthBehaviour>().Heal(GetComponent<HealthBehaviour>().ReturnMaxHealth());

        meshRenderer.enabled = true;

        for (int i = 0; i < transform.childCount; i++) {

            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    IEnumerator C_RestetMaterial() {

        yield return new WaitForSeconds(0.2f);

        meshRenderer.material = defaultMaterial;
    }

    public void OnDie()
    {
        DeactivateResidual();
        ResetMe();
    }

    private void DeactivateResidual()
    {
        ResidualDamageController residual = GetComponentInChildren<ResidualDamageController>();

        if (residual != null)
        {
            residual.SetDefaultParent();
            residual.gameObject.SetActive(false);
        }
    }
}
