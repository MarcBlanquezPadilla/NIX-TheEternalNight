using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public abstract class Enemy : MonoBehaviour {


    public enum State {

        Patrol,
        Alert,
        Persecution,
        Attack,
        Die,
        Respawn,
    }
    [SerializeField] protected State actualState;
    [SerializeField] private bool respawn = true;

    [Header("Patrol System")]
    [SerializeField] protected Transform[] patrolRoute;
    protected int actualPoint;
    protected int nextPoint;
    [SerializeField] protected float patrolTargetDistance;
    [SerializeField] private float patrolSpeed = .5f;
    [SerializeField] private bool patrol = true;

    [Header("Alert System")]
    [SerializeField] protected float timeToContinue = 4f;
    [SerializeField] protected LayerMask alertLayers;
    protected float timeToContinueCounter = 0f;
    [SerializeField] protected float alertTargetDistance;

    [Header("Persecution System")]
    [SerializeField] protected float persecutionTargetDistance;
    [SerializeField] protected float maxtimeWithoutVision = 5f;
    [SerializeField] protected float persecutionSpeed = 1f;
    protected float timeWithoutVisionCounter = 0f;
    protected bool reciveDamage = false;

    [Header("Attack System")]
    [SerializeField] protected float attackDuration;
    [SerializeField] protected float attackDistance;
    public bool attacking = false;

    [Header("VFXSystem")]
    [SerializeField] private VisualEffect visualEffect;
    [SerializeField] private Vector3 visualEffectOffset;

    protected NavMeshAgent navMeshAgent;
    protected Animator animator => GetComponent<Animator>();
    //
    //protected MeshRenderer meshRenderer;
    protected Transform character;
    protected ElementController elementController;

    //private Material defaultMaterial;

    private bool dead = false;

    void Awake() {

        navMeshAgent = GetComponent<NavMeshAgent>();

        actualPoint = 0;
        nextPoint = 0;

        //meshRenderer = GetComponent<MeshRenderer>();
        //defaultMaterial = GetComponent<MeshRenderer>().material;

        elementController = GetComponentInChildren<ElementController>();

        dead = false;

		ResetMaterial();
    }

    private void Start() {

        character = GameManager.Instance.character.transform;

        //if (TryGetComponent<DissolveSpider>(out DissolveSpider dissolveSpider)) {

        //    dissolveSpider.Reinicio();
        //}
    }

	void Update() {

        switch (actualState) {

            case State.Patrol:

                if ( navMeshAgent.velocity != Vector3.zero) {

                    animator.SetInteger("State", 1);
                }

                navMeshAgent.isStopped = false;
                navMeshAgent.speed = patrolSpeed;

                if (Vector3.Distance(transform.position, patrolRoute[nextPoint].position) <= 1.2f) {

                    actualPoint = nextPoint;
                    nextPoint++;

                    if (nextPoint == patrolRoute.Length) nextPoint = 0;

                    navMeshAgent.SetDestination(patrolRoute[nextPoint].position);

                    actualState = State.Alert;
                }

                if (ReturnCanSeeCharacter(patrolTargetDistance)) {

                    actualState = State.Persecution;
                    timeToContinueCounter = 0;
                }

                if (!transform.Find("Move").GetComponent<AudioSource>().isPlaying)
                {

                    transform.Find("Move").GetComponent<AudioSource>().Play();
                }

                transform.Find("Move").GetComponent<AudioSource>().UnPause();

                Debug.DrawRay(transform.position, (character.position - transform.position).normalized * patrolTargetDistance, Color.green);

                break;
            case State.Alert:

                animator.SetInteger("State", 0);

                navMeshAgent.isStopped = true;

                if (ReturnCanSeeCharacter(alertTargetDistance)) {

                    actualState = State.Persecution;
                    timeToContinueCounter = 0;
                }

                Debug.DrawRay(transform.position, (character.position - transform.position).normalized * alertTargetDistance, Color.yellow);

                timeToContinueCounter += Time.deltaTime;

                if (timeToContinueCounter >= timeToContinue) {

                    actualState = State.Patrol;

                    timeToContinueCounter = 0;
                }

                transform.Find("Move").GetComponent<AudioSource>().Pause();


                break;
            case State.Persecution:

                animator.SetInteger("State", 1);

                if (!ReturnCanSeeCharacter(Mathf.Infinity)) {

                    timeWithoutVisionCounter += Time.deltaTime;

                    if (timeWithoutVisionCounter >= maxtimeWithoutVision) {

                        actualState = State.Alert;
                    }
                }
                else {

                    timeWithoutVisionCounter = 0;
                }

                if (!transform.Find("Move").GetComponent<AudioSource>().isPlaying)
                {

                    transform.Find("Move").GetComponent<AudioSource>().Play();
                }

                transform.Find("Move").GetComponent<AudioSource>().UnPause();

                navMeshAgent.speed = persecutionSpeed;
                navMeshAgent.isStopped = false;

                navMeshAgent.SetDestination(character.position);

                float characterDistance = Vector3.Distance(transform.position, character.position);

                float targetDistance = persecutionTargetDistance;

                if (reciveDamage) {

                    targetDistance = Mathf.Infinity;
                }

                if (characterDistance > targetDistance) {


                    actualState = State.Alert;
                    navMeshAgent.SetDestination(patrolRoute[nextPoint].position);
                }
                else {

                    if (characterDistance < attackDistance) {

                        actualState = State.Attack;
                        timeToContinueCounter = 0;
                    }
                    else {

                        Debug.DrawRay(transform.position, (character.position - transform.position).normalized * persecutionTargetDistance, Color.red);
                    }
                }

                Persecution();

                break;
            case State.Attack:

                Vector3 targetRotation = Quaternion.LookRotation(character.position - transform.position).eulerAngles;
                targetRotation.x = 0;
                targetRotation.z = 0;
                transform.rotation = Quaternion.Euler(targetRotation);

                navMeshAgent.isStopped = true;
                navMeshAgent.velocity = Vector3.zero;

                characterDistance = Vector3.Distance(transform.position, character.position);

                if (characterDistance < attackDistance && !attacking && !dead) {

                    attacking = true;
                    Attack();
                }

                timeToContinueCounter += Time.deltaTime;

                if (timeToContinueCounter >= attackDuration) {

                    if (characterDistance > attackDistance) {

  
                        actualState = State.Persecution;
                        timeToContinueCounter = 0;
                    }

                }

                transform.Find("Move").GetComponent<AudioSource>().Pause();

                break;
            case State.Die:

                navMeshAgent.destination = transform.position;
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<CapsuleCollider>().enabled = false;

                break;
            case State.Respawn:

                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<CapsuleCollider>().enabled = true;

                actualState = State.Patrol;

                break;
        }
    }

    public void ChangeStateToPersecution() {

        if (!attacking)
        {
            actualState = State.Persecution;
        }
        StartCoroutine(C_AfterReciveDamage());
    }

    public void ChangeStateToRespawn() {

        actualState = State.Respawn;
    }

    IEnumerator C_AfterReciveDamage() {

        Blood();

        reciveDamage = true;

        if (!transform.Find("Hit").GetComponent<AudioSource>().isPlaying)
        {

            transform.Find("Hit").GetComponent<AudioSource>().Play();
        }

        transform.Find("Hit").GetComponent<AudioSource>().UnPause();

        yield return new WaitForSeconds(10f);

        reciveDamage = false;
    }

    public void ResetMaterial() {

        StopAllCoroutines();
        StartCoroutine(C_RestetMaterial());
    }

    public IEnumerator C_WaitUntilAtackEnds(float time)
    {
        yield return new WaitForSeconds(time);
        attacking = false;
    }

    IEnumerator C_RestetMaterial() {

        yield return new WaitForSeconds(0.2f);

        //meshRenderer.material = defaultMaterial;
    }

    public bool ReturnCanSeeCharacter(float targetDistance) {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, (character.position - transform.position).normalized, out hit, targetDistance, alertLayers)) {

            if (hit.collider.gameObject.GetComponent<CharacterController>() && !hit.collider.gameObject.GetComponent<CharacterController>().shading) {

                return true;
            }
        }

        return false;
    }
    abstract public void Persecution();
    abstract public void Attack();

    abstract public IEnumerator C_Die(DissolveSpider dissolveSpider);
    abstract public IEnumerator C_Die(DissolveSlime dissolveSlime);
    abstract public IEnumerator C_Die(DissolveRata dissolveRata);

    public void Die() {

        if (!dead) {

            transform.Find("Move").GetComponent<AudioSource>().Pause();

            dead = true;

            Blood();

            actualState = State.Die;
            animator.SetInteger("State", 0);

            if (respawn) FindObjectOfType<EnemyGenerator>().RespawnEnemy(gameObject, 20f);

            visualEffect.enabled = false;

            if (TryGetComponent<DissolveSpider>(out DissolveSpider dissolveSpider))  {

                StartCoroutine(C_Die(dissolveSpider));
            }
            else if (TryGetComponent<DissolveSlime>(out DissolveSlime dissolveSlime))  {

                StartCoroutine(C_Die(dissolveSlime));
            }
            else if (TryGetComponent<DissolveRata>(out DissolveRata dissolveRata))
            {
                StartCoroutine(C_Die(dissolveRata));
            }
            else {

                gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable() {

        transform.position = patrolRoute[0].transform.position;
        GetComponent<HealthBehaviour>().RestoreHealh();
        
        if (visualEffect!=null) visualEffect.enabled = true;
    }


    private void Blood() {

        visualEffect.transform.position = new Vector3(transform.position.x, 0, transform.position.z) + visualEffectOffset;
        visualEffect.gameObject.transform.eulerAngles = new Vector3(visualEffect.transform.rotation.eulerAngles.x, Random.Range(0f, 360f), visualEffect.transform.rotation.eulerAngles.z);
        visualEffect.Play();
    }
}