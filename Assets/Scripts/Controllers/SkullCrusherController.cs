using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullCrusherController : MonoBehaviour {

    private Vector2 xWalkableArea = new Vector2(10, -21);
    private Vector2 zWalkableArea = new Vector2(-137, -175);

    public enum State {

        None,
        Walk,
        Run,
        Attack,
        Fear,
        KnockedDown,
        Die,
    }
    [SerializeField] protected State actualState;

    [Header("Walk System")]
    private bool newDirection = true;
    [SerializeField] private Vector3 walkPosition = Vector3.zero;

    [Header("Knocked System")]
    private float knocedCounter = 0;
    [SerializeField] private float knockedTimer = 4f;
    [SerializeField] private GameObject knockedAreaDamaege;
    private bool canKnockDown = true;

    [Header("Run System")]
    [SerializeField] private LayerMask groundLayer;
    private Vector3 characterPosition = Vector3.zero;

    [Header("Attack System")]
    [SerializeField] private LayerMask characterLayer;
    [SerializeField] private float attackCooldown = 4f;
    [SerializeField] private GameObject attackArea;
    private bool canAttack = true;
    private bool attackingCooldown = true;

    private MovementBehaviour movementBehaviour => GetComponent<MovementBehaviour>();
    private Vector3 direction = Vector3.zero;
    private HealthBehaviour healthBehaviour => GetComponent<HealthBehaviour>();
    private float targerPercentHealth = 90;
    private Animator animator => GetComponent<Animator>();

    private Rigidbody rigidbody3D => GetComponent<Rigidbody>();

    private void FixedUpdate() {

        movementBehaviour.MoveRb3D(direction);
    }

    void Update() {

        switch (actualState) {

            case State.None:

                break;
            case State.Walk:

                Vector3 characterPos = GameManager.Instance.character.transform.position + Vector3.up;
                Vector3 myPosition = transform.position + Vector3.up;

                Vector3 characterDirection = (characterPos - myPosition).normalized;

                if (Physics.Raycast(myPosition, characterDirection, 10f, characterLayer) && attackingCooldown) {

                    newDirection = true;
                    actualState = State.Attack;
                }
                Debug.DrawRay(myPosition, characterDirection * 10f, Color.red);

                if (newDirection) {

                    movementBehaviour.SetSpeed(2);

                    animator.SetInteger("State", 1);

                    newDirection = false;

                    walkPosition = new Vector3(Random.Range(xWalkableArea.y, xWalkableArea.x), transform.position.y, Random.Range(zWalkableArea.y, zWalkableArea.x));

                    direction = (walkPosition - transform.position).normalized;

                    StartCoroutine(C_RotateToDirection());
                }

                if (Vector3.Distance(transform.position, walkPosition) < 2f) {

                    newDirection = true;
                }

                break;
            case State.Run:

                canAttack = false;

                if (Physics.Raycast(transform.position + Vector3.up, transform.forward, 2.5f, groundLayer) && canKnockDown) {

                    KnockDown();
                }
                Debug.DrawRay(transform.position + Vector3.up, transform.forward * 2.5f, Color.blue);

                if (newDirection) {

                    characterPosition = FindObjectOfType<CharacterController>().transform.position;

                    animator.SetInteger("State", 4);

                    newDirection = false;
                    movementBehaviour.SetSpeed(15);

                    direction = (characterPosition - transform.position).normalized;

                    characterPosition += (direction * 7.5f);

                    StartCoroutine(C_RotateToDirection());
                }

                if (Vector3.Distance(transform.position, characterPosition) < 2f) {

                    canAttack = true;
                    newDirection = true; 
                    actualState = State.Walk;
                }

                break;
            case State.Attack:

                if (canAttack) {

                    movementBehaviour.SetSpeed(4);

                    animator.SetInteger("State", 1);

                    walkPosition = GameManager.Instance.character.transform.position;

                    direction = (walkPosition - transform.position).normalized;

                    StartCoroutine(C_RotateToDirection());
                }

                if (Vector3.Distance(transform.position, walkPosition) < 3f) {

                    movementBehaviour.Stop();
                    attackingCooldown = false;
                    canAttack = false;
                    animator.SetInteger("State", 5);
                }

                break;
            case State.Fear:

                canAttack = false;
                animator.SetInteger("State", 3);

                break;
            case State.KnockedDown:

                knocedCounter += Time.deltaTime;
                    
                if (knocedCounter >= knockedTimer) {

                    UnKnockedDown();
                    
                    actualState = State.Walk;       
                }

                break;
            case State.Die:

                break;
        }
    }

    IEnumerator C_RotateToDirection() {

        float counter = 0;
        Quaternion startRotation = transform.rotation;

        while (counter <= 1) {

            transform.rotation = Quaternion.Lerp(startRotation, Quaternion.LookRotation(direction), counter);

            counter += Time.deltaTime * 3f;

            yield return new WaitForEndOfFrame();
        }
    }

    public void RecibeDamage() {

        bool executed = false;

        if (canAttack) {

            actualState = State.Attack;
        }

        while (healthBehaviour.ReturnHealthPercent()*100 <= targerPercentHealth) {

            targerPercentHealth -= 10;

            if (!executed) {

                executed = true;

                KnockDown();
            }
        }
    }

    public void HitCritickPoint() {

        healthBehaviour.Hurt(100);

        UnKnockedDown();

        newDirection = true;
        actualState = State.Fear;
    }

    public void KnockDown() {

        knocedCounter = 0;
        knockedAreaDamaege.SetActive(true);
        animator.SetInteger("State", 2);
        direction = Vector3.zero;

        rigidbody3D.useGravity = false;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        actualState = State.KnockedDown;
    }

    public void UnKnockedDown() {

        canAttack = true;

        newDirection = true;
        knockedAreaDamaege.SetActive(false);

        rigidbody3D.useGravity = true;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }

    public void Run() {

        StartCoroutine(C_KnockDownCoolDown());
        actualState = State.Run;
    }

    public void Attack() {

        actualState = State.Walk; 
        newDirection = true;

        StartCoroutine(C_AttackCooldonw());
    }

    private IEnumerator C_AttackCooldonw() {

        attackArea.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        attackArea.SetActive(false);
        canAttack = true;

        yield return new WaitForSeconds(attackCooldown);
        attackingCooldown = true;
    }

    IEnumerator C_KnockDownCoolDown() {

        canKnockDown = false;
        yield return new WaitForSeconds(.5f);
        canKnockDown = true;
    }

    public void Die() {

        MainMenuManager.Instance.Win();
    }
}
