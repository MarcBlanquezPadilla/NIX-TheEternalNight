using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrowniePickerController : MonoBehaviour
{
    public void OnPick()
    {
        GameManager.Instance.brownieController.transform.position = transform.position;
        GameManager.Instance.brownieController.transform.rotation = transform.rotation;
        GameManager.Instance.brownieController.pet = true;
        GameManager.Instance.brownieController.gameObject.SetActive(true);
    }

    public void DestroyObject() {

        Destroy(this.gameObject);
    }
}
