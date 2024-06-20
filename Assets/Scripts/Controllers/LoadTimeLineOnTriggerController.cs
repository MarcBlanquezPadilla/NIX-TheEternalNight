using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LoadTimeLineOnTriggerController : MonoBehaviour {

    [SerializeField] private PlayableDirector playableDirector;

    private void OnTriggerEnter(Collider other) {

        if (other.GetComponent<CharacterController>() != null) {

            TimeLineManager.Instance.RunTimeLine(playableDirector);
        }
    } 
}
