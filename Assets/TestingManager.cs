using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingManager : MonoBehaviour
{
    public GameObject main;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            main.SetActive(false);
            GameManager.Instance.brownieController.pet = false;
            ScenesManager.Instance.ChangeSacene("Level1");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            main.SetActive(false);
            GameManager.Instance.brownieController.pet = true;
            GameManager.Instance.characterController.TakeLight();
            GameManager.Instance.characterController.TakeBlood();
            GameManager.Instance.characterController.TakePoison();
            ScenesManager.Instance.ChangeSacene("Level2");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            main.SetActive(false);
            GameManager.Instance.brownieController.pet = true;
            GameManager.Instance.characterController.TakeLight();
            GameManager.Instance.characterController.TakeBlood();
            GameManager.Instance.characterController.TakePoison();
            ScenesManager.Instance.ChangeSacene("Level3");
        }
    }
}
