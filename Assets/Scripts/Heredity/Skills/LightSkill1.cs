using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSkill1 : Hability {

    [Header("Properties")]
    [SerializeField] private float damage;
    [SerializeField] private float lightCost;

    public List<LightPointCastController> lightPointCastControllers;

    protected override void Cast() {

        float distance = 0f;
        float nearestDistance = 999f;
        int nearestLight = 0;

        if (lightPointCastControllers.Count > 0) {

            

            for (int i = 0; i < lightPointCastControllers.Count; i++) {

                distance = Vector3.Distance(lightPointCastControllers[i].transform.position, transform.position);
                if (distance < nearestDistance) {

                    nearestDistance = distance;
                    nearestLight = i;
                }
            }

            if (lightPointCastControllers[nearestLight].LightAmount() >= lightCost) {

                lightPointCastControllers[nearestLight].EnergyUsage(lightCost);
                lightPointCastControllers[nearestLight].LightningRays(damage);
            }
        }
    }


    private void OnTriggerEnter(Collider other) {

        if (other.GetComponent<LightPointCastController>() != null) {

            lightPointCastControllers.Add(other.GetComponent<LightPointCastController>());
            other.GetComponent<LightPointCastController>().Casteable(true);
        }
    }

    private void OnTriggerExit(Collider other) {

        if (other.GetComponent<LightPointCastController>() != null) {

            lightPointCastControllers.Remove(other.GetComponent<LightPointCastController>());
            other.GetComponent<LightPointCastController>().Casteable(false);
        }
    }
}
