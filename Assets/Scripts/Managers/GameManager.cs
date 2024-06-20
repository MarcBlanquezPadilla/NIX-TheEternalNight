using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region Instance

    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    #endregion

    public static bool pause;

    public GameObject character;
    public GameObject mainCamera;
    public GameObject characterCanvas;
    public CharacterController characterController;
    public CameraController cameraController;
    public BrownieController brownieController;

    public void Pause() {

        pause = true;
        Time.timeScale = 0;
    }
    
    public void UnPause() {

        pause = false;
        Time.timeScale = 1;
    }
}
