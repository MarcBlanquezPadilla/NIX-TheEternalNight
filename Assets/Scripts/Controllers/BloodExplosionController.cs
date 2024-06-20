using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodExplosionController : MonoBehaviour {

    [Header("EXPLOSION")]
    [SerializeField] private float explosionRange;
    [SerializeField] private float explosionTime;
    [SerializeField] private float damage;

    public void Activate() {

        transform.localScale = Vector3.zero;

        gameObject.SetActive(true);
        gameObject.transform.parent = null;
        StartCoroutine(C_Explosion());
        GetComponent<AudioSource>().Play();
    }

    public void Deactivate() {

        GetComponent<AudioSource>().Stop();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.TryGetComponent<HealthBehaviour>(out HealthBehaviour health)) {

            if (other.gameObject.GetComponentInChildren<ElementController>() != null) {

                health.Hurt(damage * ElementDamageManager.Instance.ReturnDamageMultiple("Blood", other.gameObject.GetComponentInChildren<ElementController>().currentElement));
            }
            else {

                health.Hurt(damage);
            }
        }

        if (other.gameObject.TryGetComponent<BloodInteraction>(out BloodInteraction bloodInteraction)) { 

            bloodInteraction.Interact();
        }
    }

    private IEnumerator C_Explosion() {

        float factor = 0;

        while (factor != 1) {

            factor += Time.deltaTime / explosionTime;
            factor = Mathf.Clamp(factor, 0, 1);
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * explosionRange, factor);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.3f);

        while (factor != 0)
        {

            factor -= Time.deltaTime / explosionTime;
            factor = Mathf.Clamp(factor, 0, 1);
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * explosionRange, factor);
            yield return new WaitForEndOfFrame();
        }

        Deactivate();
    }
}
