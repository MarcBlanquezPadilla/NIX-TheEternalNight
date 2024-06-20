using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoisonSkill : Hability {

    public Transform shootingPoint;
    public float xVelocity;

    protected override void Cast() {

        GameObject poisonBall = PoolingManager.Instance.GetPooledObject("EnemyPoisonBall");

        if (poisonBall != null) {

            float distance = Vector3.Distance(shootingPoint.position, GameManager.Instance.character.transform.position);
            float tiempo = distance / xVelocity;

            Vector3 rbVelocity = transform.parent.forward;
            rbVelocity.y = 0;
            rbVelocity.Normalize();

            rbVelocity.x = xVelocity * rbVelocity.x;
            rbVelocity.z = xVelocity * rbVelocity.z;
            float velocidadFinalY = (GameManager.Instance.character.transform.position.y - shootingPoint.position.y + 0.5f * (-9.81f) * Mathf.Pow(tiempo, 2)) / tiempo;
            rbVelocity.y = velocidadFinalY + 9.81f * tiempo;

            poisonBall.transform.position = shootingPoint.position;
            poisonBall.SetActive(true);
            poisonBall.GetComponent<Rigidbody>().velocity = rbVelocity;
        }

        StartCoroutine(C_Coldown());
    }
}
