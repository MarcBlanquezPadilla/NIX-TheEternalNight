using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelePoisonController : MonoBehaviour {

	[SerializeField] private float damage;

	private void OnTriggerEnter(Collider other) {

		if (other.gameObject.TryGetComponent<HealthBehaviour>(out HealthBehaviour healthBehaviour)) {

            if (other.gameObject.GetComponentInChildren<ElementController>() != null)  {

                healthBehaviour.Hurt(damage * ElementDamageManager.Instance.ReturnDamageMultiple("Poison", other.gameObject.GetComponentInChildren<ElementController>().currentElement));
            }
            else {

                healthBehaviour.Hurt(damage);
            }

            ResidualDamageController residualDamageController = healthBehaviour.gameObject.GetComponentInChildren<ResidualDamageController>();

            if (residualDamageController != null) {

                residualDamageController.ResetTimers();
            }
            else {

                GameObject residualDamageObj = PoolingManager.Instance.GetPooledObject("PoisonResidualDamage");
                residualDamageObj.transform.SetParent(healthBehaviour.transform);
                residualDamageObj.SetActive(true);
            }
        }
	}
}
