using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBallController : MonoBehaviour {

    [Header("EXPLOSION")]
    public float explosionRadius;
    public float explosionDamage;


    private void OnTriggerEnter(Collider other) {

        GameObject BloodExplosion = PoolingManager.Instance.GetPooledObject("BloodExplosion");
        BloodExplosion.transform.position = transform.position;
        BloodExplosion.GetComponent<BloodExplosionController>().Activate();

        Disable();
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
