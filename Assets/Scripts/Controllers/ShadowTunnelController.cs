using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTunnelController : MonoBehaviour {

    [Header("Attributes System")]
    [Header("Size & Speed")]
    [SerializeField] private float range;
    [SerializeField] private Vector2 area;
    [SerializeField] private float maxTimeActive = 2f;
    [SerializeField] private float growthTime = 0.5f;
    [SerializeField] private float srinkTime = 0.5f;

    [Header("Damage")]
    [SerializeField] private float damage;
    [SerializeField] private float delayDamage = 0.2f;

    [SerializeField] private Transform castPoint;

    public struct EnemyInRange {

        public HealthBehaviour health;
        public float damageTimer;
        public bool inRange;
    }

    private List<EnemyInRange> enemiesInRange = new List<EnemyInRange>();
    private BoxCollider boxCollider;

    private void Awake() {

        transform.SetParent(null);
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update() {

        for (int i = 0; i < enemiesInRange.Count; i++) {

            if (enemiesInRange[i].damageTimer > 0) {

                EnemyInRange e = enemiesInRange[i];
                e.damageTimer -= Time.deltaTime;
                enemiesInRange[i] = e;
            }
            else {

                if (enemiesInRange[i].inRange) {

                    EnemyInRange e = enemiesInRange[i];

                    if (e.health.gameObject.GetComponentInChildren<ElementController>() != null) {

                        e.health.Hurt(damage * ElementDamageManager.Instance.ReturnDamageMultiple("Shadow", e.health.gameObject.GetComponentInChildren<ElementController>().currentElement));
                    }
                    else {

                        e.health.Hurt(damage);
                    }

                    e.damageTimer = delayDamage;
                    enemiesInRange[i] = e;
                }
                else {

                    enemiesInRange.Remove(enemiesInRange[i]);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.TryGetComponent<HealthBehaviour>(out HealthBehaviour health)) {

            int founded = -1;

            for (int i = 0; i < enemiesInRange.Count && founded == -1; i++) {

                if (enemiesInRange[i].health == health) {
                    founded = i;
                }
            }

            if (founded != -1) {

                EnemyInRange e = enemiesInRange[founded];
                e.inRange = true;
                enemiesInRange[founded] = e;
            }
            else {

                EnemyInRange damage = new EnemyInRange();
                damage.health = health;
                damage.damageTimer = 0;
                damage.inRange = true;

                enemiesInRange.Add(damage);
            }
        }

        if (other.gameObject.TryGetComponent<ShadowWallController>(out ShadowWallController shadowWallController)) {

            shadowWallController.ShadeWall(maxTimeActive + growthTime*2 + .5f);
        }
    }

    private void OnTriggerExit(Collider other) {

        if (other.gameObject.TryGetComponent<HealthBehaviour>(out HealthBehaviour health)) {

            int founded = -1;

            for (int i = 0; i < enemiesInRange.Count && founded == -1; i++) {

                if (enemiesInRange[i].health == health) {

                    founded = i;
                }
            }

            if (founded != -1) {

                EnemyInRange e = enemiesInRange[founded];
                e.inRange = false;
                enemiesInRange[founded] = e;
            }
        }
    }

    private void OnEnable() {

        StartCoroutine(C_TunnerlActive());
    }

    IEnumerator C_TunnerlActive() {

        boxCollider.size = Vector3.one;

        transform.position = castPoint.position;
        transform.rotation = Quaternion.LookRotation(GameManager.Instance.cameraController.ReturnAimDirection());
        transform.localScale = new Vector3(0, 0, 0);

        float counter = 0;
        Vector3 startSize = transform.localScale;

        while (counter < growthTime) {

            transform.localScale = Vector3.Lerp(startSize, new Vector3(area.x, area.y, range), counter / growthTime);

            counter += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(maxTimeActive);
        counter = 0;
        startSize = transform.localScale;

        while (counter < srinkTime) {

            transform.localScale = Vector3.Lerp(startSize, new Vector3(0, 0, range), counter / growthTime);

            counter += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        boxCollider.size = Vector3.zero;

        yield return new WaitForSeconds(.25f);
        gameObject.SetActive(false);
    }
}
