using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidualDamageController : MonoBehaviour
{
    [Header("PROPERTIES")]
    [SerializeField] private float timeToDamage = 0f;
    [SerializeField] private float timeToStartDamage = 0f;
    [SerializeField] private float timeToEnd = 0f;
    [SerializeField] private float damage = 0f;

    private float timerToDamage = 0f;
    private float timerToEnd = 0f;
    private float timerStartDamage = 0f;

    private Transform defaultParent;

    private void Awake()
    {
        defaultParent = transform.parent;
    }

    private void OnEnable()
    {
        timerToEnd = 0f;
        timerToDamage = 0f;
    }

    void Update()
    {
        timerToEnd += Time.deltaTime;

        if (timerToEnd >= timeToEnd)
        {
            transform.SetParent(defaultParent);
            gameObject.SetActive(false);
        }

       
        if (timerStartDamage < timeToStartDamage)
        {
            timerStartDamage += Time.deltaTime;
        }
        else
        {
            timerToDamage += Time.deltaTime;

            if (timerToDamage >= timeToDamage) {

                timerToDamage = 0f;

                if (transform.parent.GetComponentInChildren<ElementController>() != null) {

                    GetComponentInParent<HealthBehaviour>().Hurt(damage * ElementDamageManager.Instance.ReturnDamageMultiple("Poison", transform.parent.GetComponentInChildren<ElementController>().currentElement));
                }
                else {

                    GetComponentInParent<HealthBehaviour>().Hurt(damage);
                }
            }
        }
    }

    public void ResetTimers()
    {
        timerToDamage = 0f;
        timerToEnd = 0f;
        timerStartDamage = 0f;
    }

    public void SetDefaultParent()
    {
        transform.SetParent(defaultParent);
    }
    
}
