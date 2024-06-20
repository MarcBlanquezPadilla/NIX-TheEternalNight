using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSkill2 : Hability {

    [SerializeField] private float enemyExecutioner;

    protected override void Cast() {


        if (casteable) {

            ExecuteEnemy();   
        }
    }

    private void ExecuteEnemy() {

        RaycastHit hit = GameManager.Instance.mainCamera.GetComponent<CameraController>().ReturnAim();

        if (hit.collider.gameObject.TryGetComponent<HealthBehaviour>(out HealthBehaviour health)) {

            if (hit.collider.gameObject.TryGetComponent<ElementController>(out ElementController elementController)) {

                if (health.ReturnHealthPercent() <= enemyExecutioner * ElementDamageManager.Instance.ReturnDamageMultiple("Blood", elementController.currentElement)) {

                    health.Hurt(health.ReturnMaxHealth());
                    Pool(hit);
                }
            }
            else {

                health.Hurt(health.ReturnMaxHealth());
                Pool(hit);
            }
            
        }
    }

    public void Casteable(bool target) {

        casteable = target;
    }

    private void Pool(RaycastHit hit) {

        GameObject explosion = PoolingManager.Instance.GetPooledObject("BloodExplosion");

        explosion.transform.position = hit.collider.transform.position;
        explosion.GetComponent<BloodExplosionController>().Activate();
    }


    private void OnTriggerEnter(Collider other) {

        if (other.GetComponent<HealthBehaviour>() != null) {
            
            Casteable(true);
        }
    }

    private void OnTriggerExit(Collider other) {

        if (other.GetComponent<HealthBehaviour>() != null) {
            
            Casteable(false);
        }
    }
}
