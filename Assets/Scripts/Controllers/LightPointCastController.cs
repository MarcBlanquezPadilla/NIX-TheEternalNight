using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPointCastController : MonoBehaviour {

    [SerializeField] private float energyAmount;
    [SerializeField] private float maxEnergy;
    private bool casteable = false;

    [SerializeField] private float rechargeTime;
    [SerializeField] private float rechargeAmount;
    private float rechargeTimer;

    private void Awake()
    {
        energyAmount = maxEnergy;
    }

    public void Casteable(bool target) {

        casteable = target;
    }

    public void LightningRays(float damage) {
       
        RaycastHit aim =  GameManager.Instance.mainCamera.GetComponent<CameraController>().ReturnAim();
        if (energyAmount > 0) {

            if (Physics.Raycast(transform.position, aim.point - transform.position, out RaycastHit hit)) {

                if (hit.collider.TryGetComponent<HealthBehaviour>(out HealthBehaviour healthBehaviour)) {

                    if (hit.collider.GetComponentInChildren<ElementController>() != null) {

                        healthBehaviour.Hurt(damage * ElementDamageManager.Instance.ReturnDamageMultiple("Light", hit.collider.gameObject.GetComponentInChildren<ElementController>().currentElement));
                    }
                    else {

                        healthBehaviour.Hurt(damage);
                    }
                }

                if(hit.collider.TryGetComponent<LightInteraction>(out LightInteraction lightInteraction)) {

                    lightInteraction.Interact();
                }
            }

            Debug.DrawRay(transform.position, aim.point - transform.position, Color.yellow, 5f);
        }
    }

    public void EnergyUsage(float lightCost) {

        energyAmount = energyAmount - lightCost;

        StopAllCoroutines();
        StartCoroutine(RechargeEnergy());
    }

    public float LightAmount() {

        return energyAmount;
    }


    IEnumerator RechargeEnergy()
    {
        rechargeTimer = 0;

        while (energyAmount < maxEnergy) {

            rechargeTimer += Time.deltaTime;

            if (rechargeTimer > rechargeTime) {

                energyAmount += rechargeAmount;
                rechargeTimer = 0;

                energyAmount = Mathf.Clamp(energyAmount, 0, maxEnergy);
            }
            
            yield return new WaitForEndOfFrame();
        }
        Debug.Log(energyAmount);
    }
}
