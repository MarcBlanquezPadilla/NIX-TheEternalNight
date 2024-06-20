using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selecion2 : MonoBehaviour
{
    LayerMask mask;
    public float distancia = 1;


    Animator animator;


    void Start()
    {
        mask = LayerMask.GetMask("Raycast Detected2"); 
    }

    // Update is called once per frame
    void Update()
    {
        //Raycast(origen,direccion,out hit, distancia, mascara)

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),out hit, distancia, mask))
        {
            if(hit.collider.tag == "objeto interactivo")
            {
                if(Input.GetKeyDown(KeyCode.F))
                {
                    hit.collider.transform.GetComponent<objetointeractivo2>().ActivarObjeto();
                    animator.SetTrigger("subir");
                }
            }
        }
        
    }
}
