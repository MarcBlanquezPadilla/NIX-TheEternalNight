using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class OnLoadedScene : MonoBehaviour
{
    [SerializeField] bool enabled = true;
    [SerializeField] PlayableDirector playableDirectorAwake;

    [SerializeField] Transform startPlayerPosition;
    [SerializeField] Transform startBrowniePosition;

    void Start() {

        GameManager.Instance.character.transform.position = startPlayerPosition.transform.position;
        GameManager.Instance.character.transform.rotation = startPlayerPosition.transform.rotation;
        GameManager.Instance.character.SetActive(enabled);
        GameManager.Instance.characterCanvas.SetActive(enabled);
        GameManager.Instance.cameraController.gameObject.SetActive(enabled);

        if (GameManager.Instance.brownieController.pet) 
        {
            GameManager.Instance.brownieController.transform.position = startBrowniePosition.position;
            GameManager.Instance.brownieController.transform.rotation = startBrowniePosition.rotation;
            GameManager.Instance.brownieController.gameObject.SetActive(true);
        }
        else GameManager.Instance.brownieController.gameObject.SetActive(false);


        if (playableDirectorAwake != null) TimeLineManager.Instance.RunTimeLine(playableDirectorAwake);
    }
}
