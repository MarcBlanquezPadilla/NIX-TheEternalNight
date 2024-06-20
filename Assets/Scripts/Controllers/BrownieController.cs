using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class BrownieController : MonoBehaviour
{
    #region instance

    private static BrownieController _instance;
    public static BrownieController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BrownieController>();
            }
            return _instance;
        }
    }

    #endregion

    public enum State
    {
        Idle,
        Follow,
    }

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    [Header("PROPERTIES")]
    [SerializeField] private float distanceToStop;
    [SerializeField] private float distanceToBuffSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float aimSpeed;
    [SerializeField] private float maxWalkSpeed;
    [SerializeField] private float maxRunSpeed;
    [SerializeField] private float maxAimSpeed;
    [SerializeField] private float maxDistanceWithPlayer;
    [SerializeField] private float sleepTime;
    [SerializeField] private float tpTime;



    [Header("INFORMATION")]
    [SerializeField] private Vector3 targetPosition = Vector3.zero;
    [SerializeField] private bool waitingForAnim = false;
    [SerializeField] private State actualState = State.Idle;
    [SerializeField] private float sleepTimer = 0;
    [SerializeField] private float tpTimer = 0;

    private NavMeshPath workingPath;
    private NavMeshHit workingHit;

    public bool pet = false;

    void Awake()
    {
        workingPath = new NavMeshPath();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!waitingForAnim)
        {
            actualState = State.Idle;

            if (Vector3.Distance(transform.position, GameManager.Instance.character.transform.position) > distanceToStop)
                actualState = State.Follow;

            if (GameManager.Instance.characterController.moving)
                sleepTimer = 0;
            else
                sleepTimer += Time.deltaTime;

            switch (actualState)
            {
                case State.Idle:
                    targetPosition = transform.position;
                    navMeshAgent.SetDestination(targetPosition);

                    if (sleepTimer<sleepTime)
                        animator.SetInteger("State", 0);
                    else
                    {
                        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Acostarse") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Sleep"))
                        {
                            animator.SetInteger("State", 4);
                            waitingForAnim = true;
                        }
                    }
                  
                    break;

                case State.Follow:
                    
                    targetPosition = GameManager.Instance.character.transform.position;                
                    navMeshAgent.SetDestination(targetPosition);
                    if (Vector3.Distance(targetPosition, transform.position) < maxDistanceWithPlayer)
                    {
                        tpTimer = 0;
                        if (!GameManager.Instance.characterController.sprinting)
                        {
                            animator.SetInteger("State", 2);
                            if (!GameManager.Instance.characterController.aiming)
                            {
                                if (Vector3.Distance(transform.position, targetPosition) < distanceToBuffSpeed)
                                    navMeshAgent.speed = walkSpeed;
                                else
                                    navMeshAgent.speed = maxWalkSpeed;
                            }
                            else
                            {
                                if (Vector3.Distance(transform.position, targetPosition) < distanceToBuffSpeed)
                                    navMeshAgent.speed = aimSpeed;
                                else
                                    navMeshAgent.speed = maxAimSpeed;
                            }
                        }
                        else
                        {
                            animator.SetInteger("State", 1);
                            if (Vector3.Distance(transform.position, targetPosition) < distanceToBuffSpeed)
                                navMeshAgent.speed = runSpeed;
                            else
                                navMeshAgent.speed = maxRunSpeed;
                        }
                    }
                    else
                    {
                        targetPosition = transform.position;
                        animator.SetInteger("State", 0);

                        tpTimer += Time.deltaTime;
                        if (tpTimer > tpTime)
                        {
                            Vector3 tpDir = new Vector3(transform.position.x - GameManager.Instance.character.transform.position.x, 0, transform.position.z - GameManager.Instance.character.transform.position.z).normalized;
                            TryToTp(navMeshAgent, GameManager.Instance.character.transform.position + tpDir * distanceToStop, GameManager.Instance.character.transform.position);
                            tpTimer = 0;
                        }
                    }

                    break;
            }
        }
    }

    public void ResetWaitingForAnim()
    {
        waitingForAnim = false;
    }

    public void TryToTp(NavMeshAgent agent, Vector3 pointA, Vector3 pointB)
    {
        if (NavMesh.SamplePosition(pointA, out workingHit, 0.5f, agent.areaMask))
        {
            if (NavMesh.CalculatePath(workingHit.position, pointB, agent.areaMask, workingPath))
            {
                navMeshAgent.enabled = false;
                transform.position = workingHit.position;
                transform.rotation = Quaternion.LookRotation(pointB-pointA);
                navMeshAgent.enabled = true;
            }
        }
    }
}