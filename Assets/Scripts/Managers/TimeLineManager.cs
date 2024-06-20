using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class TimeLineManager : MonoBehaviour
{

    #region Instance

    private static TimeLineManager _instance;
    public static TimeLineManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TimeLineManager>();
            }
            return _instance;
        }
    }

    #endregion

    [SerializeField] private GameObject timeLineCamera;
    [SerializeField] private float fadeTime;
    private PlayableDirector actualPlayable;

    private float timer;

    public void RunTimeLine(PlayableDirector playableDirector)
    {
        StartCoroutine(C_RunTimeLine(playableDirector));
    }

    public void EndTimeLine()
    {
        StartCoroutine(C_EndTimeLine(actualPlayable));
    }

    private IEnumerator C_RunTimeLine(PlayableDirector playableDirector)
    {
        timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime / fadeTime;
            timer = Mathf.Clamp(timer, 0, 1);
            GetComponentInChildren<CanvasGroup>().alpha = timer;
            yield return new WaitForEndOfFrame();
        }

        GameManager.Instance.cameraController.gameObject.SetActive(false);
        GameManager.Instance.characterController.gameObject.SetActive(false);
        GameManager.Instance.characterCanvas.SetActive(false);
        GameManager.Instance.brownieController.gameObject.SetActive(false);
        GameManager.Instance.characterController.characterCanvas.SetActive(false);

        timeLineCamera.SetActive(true);
        playableDirector.Play();

        actualPlayable = playableDirector;

        while (timer > 0)
        {
            timer -= Time.deltaTime / fadeTime;
            timer = Mathf.Clamp(timer, 0, 1);
            GetComponentInChildren<CanvasGroup>().alpha = timer;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator C_EndTimeLine(PlayableDirector playableDirector)
    {
        GetComponentInChildren<CanvasGroup>().alpha = 1;
        GameManager.Instance.characterController.gameObject.SetActive(true);
        GameManager.Instance.cameraController.gameObject.SetActive(true);
        GameManager.Instance.characterCanvas.SetActive(true);
        GameManager.Instance.characterController.characterCanvas.SetActive(true);

        if (GameManager.Instance.brownieController.pet && !GameManager.Instance.characterController.fusion) GameManager.Instance.brownieController.gameObject.SetActive(true);
        else GameManager.Instance.brownieController.gameObject.SetActive(false);


        actualPlayable.Stop();

        

        timer = 1;

        while (timer > 0)
        {
            timer -= Time.deltaTime / fadeTime;
            timer = Mathf.Clamp(timer, 0, 1);
            GetComponentInChildren<CanvasGroup>().alpha = timer;
            yield return new WaitForEndOfFrame();
        }
    }
}
