using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSlimeController : MonoBehaviour
{
    private float time;
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other) {


        if (other.gameObject.TryGetComponent<HealthBehaviour>(out HealthBehaviour health)) {

            health.Hurt(damage);
        }
    }


}
