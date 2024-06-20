using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class EnableLightPuzzleController : MonoBehaviour {

    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private bool activable = true;

    public void RunTimeLine() {

        if (activable) {

			TimeLineManager.Instance.RunTimeLine(playableDirector);
            activable = false;
		}
	} 
}
