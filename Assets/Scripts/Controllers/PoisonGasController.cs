using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoisonGasController : MonoBehaviour
{
    private Transform parent;

    public struct EnemyInRange
    {
        public HealthBehaviour health;
        public float damageTimer;
        public bool inRange;
    }

    [Header("PROPERTIES")]
    [SerializeField] private float growTime;
    [SerializeField] private float growSize;

    [SerializeField] private float unGrowTime;
    [SerializeField] private float fallDistance;

    [Header("DAMAGE")]
    [SerializeField] private float timeToDamage;
    [SerializeField] private float damage;

    private List<EnemyInRange> enemiesInRange = new List<EnemyInRange>();

    public UnityAction onEnd;

    public void Activate()
    {
        parent = transform.parent;
        transform.localScale = Vector3.zero;
        transform.localPosition = Vector3.down;
        
        transform.SetParent(null);
        enemiesInRange.Clear();

        gameObject.SetActive(true);

        GetComponent<AudioSource>().Play();
        
        StartCoroutine(C_GrowGas());
    }

    public void Deactivate()
    {
        onEnd.Invoke();
        gameObject.SetActive(false);
        transform.SetParent(parent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<HealthBehaviour>(out HealthBehaviour health))
        {
            int founded = -1;

            for (int i = 0; i < enemiesInRange.Count && founded == -1; i++)
            {
                if (enemiesInRange[i].health == health)
                {
                    founded = i;
                }
            }

            if (founded != -1)
            {
                EnemyInRange e = enemiesInRange[founded];
                e.inRange = true;
                enemiesInRange[founded] = e;
            }
            else
            {
                EnemyInRange damage = new EnemyInRange();
                damage.health = health;
                damage.damageTimer = 0;
                damage.inRange = true;

                enemiesInRange.Add(damage);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<HealthBehaviour>(out HealthBehaviour health))
        {
            int founded = -1;

            for (int i = 0; i < enemiesInRange.Count && founded == -1; i++)
            {
                if (enemiesInRange[i].health == health)
                {
                    founded = i;
                }
            }

            if (founded != -1)
            {
                EnemyInRange e = enemiesInRange[founded];
                e.inRange = false;
                enemiesInRange[founded] = e;
            }
        }
    }

    private void Update()
    {
        for(int i = 0; i < enemiesInRange.Count; i++)
        {
            if (enemiesInRange[i].damageTimer > 0)
            {
                EnemyInRange e = enemiesInRange[i];
                e.damageTimer -= Time.deltaTime;
                enemiesInRange[i] = e;
            }
            else
            {
                if (enemiesInRange[i].inRange)
                {
                    EnemyInRange e = enemiesInRange[i];

                    if (e.health.gameObject.GetComponentInChildren<ElementController>() != null) {

                        e.health.Hurt(damage * ElementDamageManager.Instance.ReturnDamageMultiple("Poison", e.health.gameObject.GetComponentInChildren<ElementController>().currentElement));
                    }
                    else {

                        e.health.Hurt(damage);
                    }

                    e.damageTimer = timeToDamage;
                    enemiesInRange[i] = e;

                    ResidualDamageController residualDamageController = e.health.gameObject.GetComponentInChildren<ResidualDamageController>();

                    if (residualDamageController != null)
                    {
                        residualDamageController.ResetTimers();
                    }
                    else
                    {
                        GameObject residualDamageObj = PoolingManager.Instance.GetPooledObject("PoisonResidualDamage");
                        residualDamageObj.transform.SetParent(e.health.transform);
                        residualDamageObj.SetActive(true);
                    }
                }
                else
                {
                    enemiesInRange.Remove(enemiesInRange[i]);
                }
            }
        }
    }

    public IEnumerator C_GrowGas()
    {
        float factor = 0;

        while (factor != 1)
        {
            factor += Time.deltaTime / growTime;
            factor = Mathf.Clamp(factor, 0, 1);
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * growSize, factor);
            yield return new WaitForEndOfFrame();
        }

        factor = 0;
        Vector3 startPos = transform.position;

        while (factor != 1)
        {
            factor += Time.deltaTime / unGrowTime;
            factor = Mathf.Clamp(factor, 0, 1);
            transform.localScale = Vector3.Lerp(Vector3.one * growSize, Vector3.zero, factor);
            yield return new WaitForEndOfFrame();
        }

        Deactivate();
    }
}