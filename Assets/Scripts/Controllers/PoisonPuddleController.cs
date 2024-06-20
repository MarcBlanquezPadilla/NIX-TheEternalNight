using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPuddleController : MonoBehaviour
{
    public struct EnemyInRange
    {
        public HealthBehaviour health;
        public float damageTimer;
        public bool inRange;
    }

    [Header("PROPERTIES")]
    public float diameter;

    [Header("BEHAVIOUR")]
    public float timeAlive;

    [Header("DAMAGE")]
    public float timeToDamage;
    public float damage;

    private float disableTimer;
    private List<EnemyInRange> enemiesInRange = new List<EnemyInRange>();

    private void Awake()
    {
        transform.localScale = new Vector3(diameter, transform.localScale.y, diameter);
    }

    public void Activate()
    {
        enemiesInRange.Clear();

        disableTimer = timeAlive;
        StartCoroutine(C_CountdownDisable());
    }

    private void Update()
    {
        for (int i = 0; i < enemiesInRange.Count; i++)
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
    private IEnumerator C_CountdownDisable()
    {
        yield return new WaitForSeconds(disableTimer);
        Disable();
    }

    private void Disable()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
