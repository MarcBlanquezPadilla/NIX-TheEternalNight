using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitTextManager : MonoBehaviour
{
    [SerializeField] private float distanceToShow;
    [SerializeField] private TextMeshProUGUI hitText;

    private void Update()
    {
        hitText.text = "";
        if (Physics.Raycast(GameManager.Instance.cameraController.transform.GetChild(0).position, GameManager.Instance.cameraController.ReturnAimDirection(), out RaycastHit hit))
        {
            if (Vector3.Distance(GameManager.Instance.cameraController.transform.GetChild(0).position, hit.point) < distanceToShow && hit.collider.gameObject.TryGetComponent<ShowHitText>(out ShowHitText objectText))
            {
                if (hit.collider.gameObject.TryGetComponent<Interactable>(out Interactable interactable)) hitText.text += "[" + InputManager.Instance.keys["Interact"].ToString() + "] ";
                hitText.text += objectText.TextToShow;
            }
        }
    }
}
