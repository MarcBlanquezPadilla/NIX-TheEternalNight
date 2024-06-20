using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


public class HealthBehaviour : MonoBehaviour {

    [Header("Properties")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private bool inmortal = false;

    [Header("Events")]
    [SerializeField] private UnityEvent<float> onHurt;
    [SerializeField] private UnityEvent<float> onHeal;
    [SerializeField] private UnityEvent onDie;

    public void Start() {

        health = maxHealth;
        //onHurt.Invoke(ReturnHealthPercent());
    }

    public void Hurt(float damage) {

        if (!inmortal) {

            health -= damage;

            if (health <= 0) {

                health = 0;
                onDie.Invoke();
            }
            else if (damage > 0) {

                onHurt.Invoke(ReturnHealthPercent());
            }
        }
    }

    public void Heal(float addedHealth) {

        health += addedHealth;

        if (health > maxHealth) {

            health = maxHealth;
        }

        if (addedHealth > 0) {

            onHeal.Invoke(ReturnHealthPercent());
        }
    }

    public float ReturnHealth() {

        return health;
    }

    public float ReturnMaxHealth() {

        return maxHealth;
    }

    public float ReturnHealthPercent() {

        return health / maxHealth;
    }

    public bool SetInmortal() { 

        return inmortal = true;
    }

    public bool SetMortal() {

        return inmortal = false;
    }

    public void SetMaxHealth(float newHealth) {

        maxHealth = newHealth;
    }

    public void RestoreHealh() {

        health = maxHealth;
    }
}
