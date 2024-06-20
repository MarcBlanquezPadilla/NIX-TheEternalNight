using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBallController : MonoBehaviour {

    [Header("EXPLOSION")]
    public float explosionRadius;
    public float explosionDamage;

    [Header("POISONPUDDLE")]
    public string puddlePoolingName;
    public LayerMask groundLayer;

    private void OnTriggerEnter(Collider other) {

        Collider[] collidersInExplosionRange = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in collidersInExplosionRange) {

            if (collider.TryGetComponent<HealthBehaviour>(out HealthBehaviour healthBehaviour)) {

                if (collider.GetComponentInChildren<ElementController>() != null) {

                    healthBehaviour.Hurt(explosionDamage * ElementDamageManager.Instance.ReturnDamageMultiple("Poison", collider.gameObject.GetComponentInChildren<ElementController>().currentElement));
                }
                else {

                    healthBehaviour.Hurt(explosionDamage);
                }

                ResidualDamageController residualDamageController = healthBehaviour.gameObject.GetComponentInChildren<ResidualDamageController>();

                if (residualDamageController != null)
                {
                    residualDamageController.ResetTimers();
                }
                else
                {
                    GameObject residualDamageObj = PoolingManager.Instance.GetPooledObject("PoisonResidualDamage");
                    residualDamageObj.transform.SetParent(healthBehaviour.transform);
                    residualDamageObj.SetActive(true);
                }
            }
        }


        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3, groundLayer))
        {
            GameObject poisonPuddle = PoolingManager.Instance.GetPooledObject(puddlePoolingName);
            if (poisonPuddle != null)
            {
                poisonPuddle.transform.position = hit.point;
                poisonPuddle.SetActive(true);
                poisonPuddle.GetComponent<PoisonPuddleController>().Activate();
            }
        }

        Disable();
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
