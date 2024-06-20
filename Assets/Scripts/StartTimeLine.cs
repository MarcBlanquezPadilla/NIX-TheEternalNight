using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StartTimeLine : MonoBehaviour
{
    [SerializeField] public PlayableDirector Cinematica;

    private BoxCollider bcollider;

    private void Start()
    {
        bcollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<CharacterController>(out CharacterController c))
        {
            PlayCinematica();
            bcollider.enabled = false;
        }
    }

    public void PlayCinematica()
    {
        TimeLineManager.Instance.RunTimeLine(Cinematica);
    }
}
