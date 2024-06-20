using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSkill1 : Hability {

    public Transform shootingPoint;
    public float minThrowBallForce;
    public float maxThrowBallForce;
    public float timeToCharge;

    protected override void Cast() {

        StartCoroutine(C_ThrowBall());
    }

    private IEnumerator C_ThrowBall()
    {
        float factor = 0;
        float throwForce = minThrowBallForce;
        while (Input.GetKey(InputManager.Instance.keys["Cast Hability 1"]) && factor < 1)
        {
            factor += Time.deltaTime / timeToCharge;
            yield return new WaitForEndOfFrame();
        }
        factor = Mathf.Clamp(factor, 0, 1);
        throwForce = Mathf.Lerp(minThrowBallForce, maxThrowBallForce, factor);
        
        GameObject poisonBall = PoolingManager.Instance.GetPooledObject("PoisonBall");
        if (poisonBall != null)
        {
            poisonBall.transform.position = shootingPoint.position;
            poisonBall.SetActive(true);
            poisonBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            poisonBall.GetComponent<Rigidbody>().AddForce(GameObject.Find("CharacterCamera").transform.forward * throwForce);
        }

        StartCoroutine(C_Coldown());
    }
}
