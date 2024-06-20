using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayController : MonoBehaviour
{
    [Header("DAMAGE")]
    public float damage;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PoisonGasController>(out PoisonGasController p))
        {
            meshRenderer.enabled = true;
        }

        if (other.gameObject.TryGetComponent<HealthBehaviour>(out HealthBehaviour health))
        {
            health.Hurt(damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PoisonGasController>(out PoisonGasController p))
        {
            meshRenderer.enabled = false;
        }
    }

    public void EnableMesh(bool b)
    {
        meshRenderer.enabled = b;
    }
}
