using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBasicTrigger : MonoBehaviour
{
    public Transform shootingPoint;

    private ShadowBasicAttack shadow;

    private void Awake()
    {
        shadow = GetComponentInParent<ShadowBasicAttack>();
    }

    private void Update()
    {
        transform.position = shootingPoint.position + GameManager.Instance.cameraController.ReturnAimDirection() * transform.localScale.z / 2;
        transform.rotation = Quaternion.LookRotation(GameManager.Instance.cameraController.ReturnAimDirection());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HealthBehaviour>(out HealthBehaviour health))
        {
            if (!shadow.inBasicRange.Contains(health))
            {
                shadow.inBasicRange.Add(health);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<HealthBehaviour>(out HealthBehaviour health))
        {
            if (shadow.inBasicRange.Contains(health))
            {
                shadow.inBasicRange.Remove(health);
            }
        }
    }
}
