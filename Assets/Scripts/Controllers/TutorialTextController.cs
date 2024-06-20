using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextController : MonoBehaviour
{
    int listtext = 0;
    [SerializeField] private GameObject fx;
    //[SerializeField] private Transform particuleSpawn;
    [SerializeField] public ParticleSystem textparticle;

    [HideInInspector] public Animator animator => GetComponent<Animator>();
    // Start is called before the first frame update
    void Start()
    {
        GameObject character = GameObject.Find("Character");
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        transform.GetChild(listtext).gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        transform.GetChild(0).gameObject.SetActive(false);
        listtext++;

        if(listtext >= 1)
        {
            transform.gameObject.SetActive(false);
        }
        else
        {

        }
        
    }

    public void DestroyObject()
    {
        if (fx != null)
        {
            Instantiate(fx, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    public void Disable()
    {
        if (fx != null)
        {
            Instantiate(fx, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }
}
