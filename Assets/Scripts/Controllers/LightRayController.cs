using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRayController : MonoBehaviour {

    public struct EnemyInRange {

        public HealthBehaviour health;
        public float damageTimer;
        public bool inRange;
    }

    [Header("DAMAGE")]
    [SerializeField] private float timeToDamage;
    [SerializeField] private float damage;
    [SerializeField] private Transform shotPoint;

    private List<EnemyInRange> enemiesInRange = new List<EnemyInRange>();

    private void Awake() {

        gameObject.SetActive(false);
    }

    private void Update() {

        transform.position = shotPoint.position;
        transform.rotation = Quaternion.LookRotation(GameManager.Instance.cameraController.ReturnAim().point - shotPoint.position);

        GameManager.Instance.character.transform.rotation = Quaternion.Euler(GameManager.Instance.character.transform.rotation.eulerAngles.x, GameManager.Instance.mainCamera.transform.rotation.eulerAngles.y, GameManager.Instance.character.transform.rotation.eulerAngles.z);

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
                    e.health.Hurt(damage);
                    e.damageTimer = timeToDamage;
                    enemiesInRange[i] = e;
                }
                else
                {
                    enemiesInRange.Remove(enemiesInRange[i]);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.TryGetComponent<HealthBehaviour>(out HealthBehaviour health)) {

            int founded = -1;

            for (int i = 0; i < enemiesInRange.Count && founded == -1; i++)
            {
                if (enemiesInRange[i].health == health)
                {
                    founded = i;
                }
            }

            if (founded != -1) {

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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<LightInteraction>(out LightInteraction lightInteraction))
        {
            lightInteraction.Interact();
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
}
