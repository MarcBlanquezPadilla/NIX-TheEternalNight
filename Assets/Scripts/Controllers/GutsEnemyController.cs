using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GutsEnemyController : MonoBehaviour {

    public enum State {

        Idle,
        Patrol,
        Alert,
        Persecution,
    }
    [SerializeField] private State actualState;

    [Header("Idle System")]
    [SerializeField] private float idleTime;
    private float idleCounter = 0;

    [Header("Patrol System")]
    [SerializeField] private Transform[] patrolRoute;
    [SerializeField] private float patrolAreaAlert;
    private int actualPoint = 0;
    private int nextPoint = 0;

    [Header("Attack")]
    [SerializeField] private float explosionRange;

    [Header("Alert System")]
    [SerializeField] private float timeToContinue = 4f;
    [SerializeField] private LayerMask alertLayers;
    private float timeToContinueCounter = 0;

    [Header("Awarness System")]
    [SerializeField] private float awarnessAreaAlert = 10;


    [Header("References")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private SphereCollider area;


    void Awake() {

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update() {

        switch (actualState) {

            case State.Idle:

                navMeshAgent.isStopped = true;

                idleCounter += Time.deltaTime;

                if (idleCounter >= idleTime) {

                    actualState = State.Patrol;
                    idleCounter = 0;
                }
                CheckExplosion();

                break;
            case State.Patrol:

                navMeshAgent.isStopped = false;

                if (Vector3.Distance(transform.position, patrolRoute[nextPoint].position) <= 3f) {

                    actualPoint = nextPoint;
                    nextPoint++;

                    if (nextPoint == patrolRoute.Length) nextPoint = 0;

                    navMeshAgent.SetDestination(patrolRoute[nextPoint].position);

                    actualState = State.Idle;
                }
                CheckExplosion();

                break;
            case State.Alert:

                navMeshAgent.isStopped = true;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, (GameManager.Instance.character.transform.position - transform.position).normalized, out hit, patrolAreaAlert, alertLayers)) {

                    if (hit.collider.gameObject.GetComponent<CharacterController>()) {

                        actualState = State.Persecution;
                        timeToContinueCounter = 0;
                    }
                }

                timeToContinueCounter += Time.deltaTime;

                if (timeToContinueCounter >= timeToContinue) {

                    actualState = State.Patrol;
                    timeToContinueCounter = 0;
                }

                CheckExplosion();

                Debug.DrawRay(transform.position, (GameManager.Instance.character.transform.position - transform.position).normalized * patrolAreaAlert, Color.red);

                break;
            case State.Persecution:

                navMeshAgent.isStopped = false;

                area.radius = awarnessAreaAlert;
                navMeshAgent.SetDestination(GameManager.Instance.character.transform.position);

                CheckExplosion();

                break;
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.GetComponent<CharacterController>() != null) {

            actualState = State.Alert;
        }
    }

    private void OnTriggerExit(Collider other) {

        if (other.gameObject.GetComponent<CharacterController>() != null) {

            if (actualState == State.Persecution) {

                area.radius = patrolAreaAlert;
                actualState = State.Alert;
                navMeshAgent.SetDestination(patrolRoute[nextPoint].position);
            }
        }
    }

    public void ChangeState(State newState) {

        actualState = newState;
    }

    private void CheckExplosion()
    {
        float range = Vector3.Distance(transform.position, GameManager.Instance.character.transform.position);

        if (range <= explosionRange)
        {
            GameObject explosion = PoolingManager.Instance.GetPooledObject("BloodExplosion");
            explosion.transform.position = transform.position;
            explosion.GetComponent<BloodExplosionController>().Activate();
            this.gameObject.SetActive(false);
        }
    }
}