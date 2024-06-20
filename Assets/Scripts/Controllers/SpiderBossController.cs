using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SpiderBossController : MonoBehaviour {

    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject spiderBossObj;

    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private bool activable = true;

    public void PrepareBossArea() {

        Debug.Log("Entra");

        GameManager.Instance.character.transform.position = playerTransform.position;
        spiderBossObj.SetActive(true);
    }

    public void RunTimeLine() {

        if (activable) {

            TimeLineManager.Instance.RunTimeLine(playableDirector);
            activable = false;
        }
    }
}
