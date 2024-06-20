using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveSpider : MonoBehaviour
{
    public SkinnedMeshRenderer skinMeshCuerpo;
    public SkinnedMeshRenderer skinMeshOjos;
    public SkinnedMeshRenderer skinMeshColmillos;

    public VisualEffect VFXGraph;

    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    private float counter = 0;

    private Material[] skinMatCuerpo;
    private Material[] skinMatOjos;
    private Material[] skinMatColmillos;    
 
    void Awake() {

        if (skinMeshCuerpo != null) 
        {
            skinMatCuerpo = skinMeshCuerpo.materials;
            skinMatOjos = skinMeshOjos.materials;
            skinMatColmillos = skinMeshColmillos.materials;
        }
    }
    // Update is called once per frame
    void Update() {

        /*if (Input.GetKeyDown(KeyCode.Space)) {

            StartCoroutine(SDissolveAraña());
        }*/
       
    }

    public IEnumerator SDissolveSpider(){

        if(VFXGraph != null){
            
            VFXGraph.Play();
        }
        
        if (skinMatCuerpo.Length > 0){

			while (skinMatCuerpo[0].GetFloat("_DissolveAmount") < 1){

                counter += dissolveRate;

                skinMatCuerpo[0].SetFloat("_DissolveAmount", counter);
                skinMatOjos[0].SetFloat("_DissolveAmount", counter);
                skinMatColmillos[0].SetFloat("_DissolveAmount", counter);

                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
    public void Reinicio() 
    {
		if (skinMatCuerpo.Length > 0)
        {
            skinMatCuerpo[0].SetFloat("_DissolveAmount", 1f);
            skinMatOjos[0].SetFloat("_DissolveAmount", 1f);
            skinMatColmillos[0].SetFloat("_DissolveAmount", 1f);
        }        
	}
}
