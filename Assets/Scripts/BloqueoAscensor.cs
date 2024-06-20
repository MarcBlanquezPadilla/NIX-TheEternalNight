using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueoAscensor : MonoBehaviour
{
    private PlatformController platformController;
    private BoxCollider boxCollider;

    [Header("PROPERTIES")]
    [SerializeField] private int plantToBloc;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        platformController = GetComponentInParent<PlatformController>();
    }

    private void Update()
    {
        if (platformController.actualPoint != plantToBloc || platformController.moving)
            boxCollider.enabled = true;
        else
            boxCollider.enabled = false;
    }
}
