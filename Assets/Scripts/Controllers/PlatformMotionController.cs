using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMotionController : MonoBehaviour {

    [HideInInspector] public float actualMovementSpeed;
    [HideInInspector] public bool moving = false;
    [HideInInspector] public Vector3 movinDir = Vector3.zero;

    private void OnTriggerStay(Collider other) {

        if (moving && other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) {

            rigidbody.velocity = new Vector3(rigidbody.velocity.x, actualMovementSpeed * movinDir.y, rigidbody.velocity.z);
        }
    }
}
