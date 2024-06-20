using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureObject : MonoBehaviour
{
    public GameObject originalObject;
    public GameObject fractureobject;
    public GameObject explosionVFX;
    public float explosionMinForce = 5;
    public float explosionMaxForce = 100;
    public float explosionForceRad = 10;
    public float fragilscalefactor = 0.05f;

    public void Explode()
    {
        if(originalObject != null)
        {
            originalObject.SetActive(false);

            if(fractureobject != null)
            {
                fractureobject.transform.position = originalObject.transform.position;
                fractureobject.SetActive(true);
               
                foreach(Transform t in fractureobject.transform)
                {
                    var rb = t.GetComponent<Rigidbody>();

                    if(rb != null)
                        rb.AddExplosionForce(Random.Range(explosionMinForce, explosionMaxForce), originalObject.transform.position, explosionForceRad);
                    
                    StartCoroutine(Shrink(t, 2));
                }

                Destroy(fractureobject, 5);
            }
        }
    }
    
    public void Reset()
    {
        Destroy(fractureobject);
        originalObject.SetActive(true);
    }

    IEnumerator Shrink (Transform t, float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 newScale = t.localScale;

        while(newScale.x >= 0)
        {
            newScale -= new Vector3(fragilscalefactor, fragilscalefactor, fragilscalefactor);
            t.localScale = newScale;
            yield return new WaitForSeconds (0.05f);
        }
    }
}
