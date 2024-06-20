using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSkill3 : Hability {

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
        while (Input.GetKey(InputManager.Instance.keys["Cast Hability 2"]) && factor < 1)
        {
            factor += Time.deltaTime / timeToCharge;
            yield return new WaitForEndOfFrame();
        }
        factor = Mathf.Clamp(factor, 0, 1);
        float throwForce = Mathf.Lerp(minThrowBallForce, maxThrowBallForce, factor);
        
        GameObject bloodBall = PoolingManager.Instance.GetPooledObject("BloodBall");
        if (bloodBall != null)
        {
            bloodBall.transform.position = shootingPoint.position;
            bloodBall.SetActive(true);
            bloodBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bloodBall.GetComponent<Rigidbody>().AddForce(GameObject.Find("CharacterCamera").transform.forward * throwForce);
        }

        StartCoroutine(C_Coldown());
    }
}
