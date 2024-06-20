using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    [Header("Properties")]
    [SerializeField] private float elevatorSpeed;
    [SerializeField] private int waitTime;

    [Header("References")]
    [SerializeField] private Transform platform;
    [SerializeField] private Transform[] routePoints;
    public bool moving = false;
    public int actualPoint = 0;


    [Header("Coliders")]
    [SerializeField] private bool hasColider;
    [SerializeField] private GameObject colider;

    private void Start() {

        platform.transform.position = routePoints[actualPoint].transform.position;
    }

    public void GoTo() {

        if (!moving) {

            int point = (actualPoint + 1) >= routePoints.Length ? 0 : actualPoint + 1;

            StopAllCoroutines();
            StartCoroutine(C_GoTo(point));

            platform.gameObject.GetComponent<PlatformMotionController>().actualMovementSpeed = elevatorSpeed;
            platform.gameObject.GetComponent<PlatformMotionController>().movinDir = (routePoints[point].position - platform.position).normalized;
        }
    }

    IEnumerator C_GoTo(int point) {

        yield return new WaitForSeconds(waitTime);

        moving = true;
        platform.gameObject.GetComponent<PlatformMotionController>().moving = moving;

        float timer = 0;

        float movingTime = Vector3.Distance(platform.position, routePoints[point].position) / elevatorSpeed;

        if (elevatorSpeed != 0) {

            while (Vector3.Distance(platform.position, routePoints[point].position) != 0) {

                platform.position = Vector3.Lerp(routePoints[actualPoint].position, routePoints[point].position, timer / movingTime);
                timer += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
        }

        actualPoint = point;

        moving = false;
        platform.GetComponent<PlatformMotionController>().moving = moving;


        if (hasColider) {

            colider.SetActive(false);
        }
    }
}
