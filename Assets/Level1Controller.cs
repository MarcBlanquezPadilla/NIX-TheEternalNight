using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Controller : MonoBehaviour
{
    public void ChangeScene(string scene) {

        ScenesManager.Instance.ChangeSacene(scene);

        GameManager.Instance.cameraController.gameObject.SetActive(false);
        GameManager.Instance.characterController.gameObject.SetActive(false);
        GameManager.Instance.brownieController.gameObject.SetActive(false);
    }
}
