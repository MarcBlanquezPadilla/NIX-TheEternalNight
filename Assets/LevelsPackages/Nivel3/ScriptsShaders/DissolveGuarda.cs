using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveGuarda : MonoBehaviour
{
    public SkinnedMeshRenderer skinMeshCuerpo;
    public SkinnedMeshRenderer skinMeshCabeza;
    public SkinnedMeshRenderer skinMeshCuernos;
    public SkinnedMeshRenderer skinMeshChaleco;
    public SkinnedMeshRenderer skinMeshHombreras;
    public SkinnedMeshRenderer skinMeshCylin03;
    public SkinnedMeshRenderer skinMeshCylin01;
    public SkinnedMeshRenderer skinMeshBambas;
    public SkinnedMeshRenderer skinMeshBox103;
    public SkinnedMeshRenderer skinMeshBox108;
    public SkinnedMeshRenderer skinMeshBox109;
    public SkinnedMeshRenderer skinMeshBox110;
    
    public VisualEffect VFXGraph;

    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    private Material[] skinMatCuerpo;
    private Material[] skinMatCabeza;
    private Material[] skinMatCuernos;
    private Material[] skinMatChaleco;
    private Material[] skinMatHombreras;
    private Material[] skinMatCylin03;
    private Material[] skinMatCylin01;
    private Material[] skinMatBambas;
    private Material[] skinMatBox103;
    private Material[] skinMatBox108;
    private Material[] skinMatBox109;
    private Material[] skinMatBox110;
 
    void Start() {

        if (skinMeshCuerpo != null) 
        {
            skinMatCuerpo = skinMeshCuerpo.materials;
            skinMatCabeza = skinMeshCabeza.materials;
            skinMatCuernos = skinMeshCuernos.materials;
            skinMatChaleco = skinMeshChaleco.materials;
            skinMatHombreras = skinMeshHombreras.materials;
            skinMatCylin03 = skinMeshCylin03.materials;
            skinMatCylin01 = skinMeshCylin01.materials;
            skinMatBambas = skinMeshBambas.materials;
            skinMatBox103 = skinMeshBox103.materials;
            skinMatBox108 = skinMeshBox108.materials;
            skinMatBox109 = skinMeshBox109.materials;
            skinMatBox110 = skinMeshBox110.materials;
        }
    }
    // Update is called once per frame
    void Update() {

        /*if (Input.GetKeyDown(KeyCode.Space)) {

            StartCoroutine(SDissolveAraña());
        }*/
    }

    IEnumerator SDissolveGuarda(){
        
        if(VFXGraph != null){
            
            VFXGraph.Play();
        }
        
        if (skinMatCuerpo.Length > 0){
            
            float counter = 0;
            
            while(skinMatCuerpo[0].GetFloat("_DissolveAmount") < 1){
                counter += dissolveRate;

                skinMatCuerpo[0].SetFloat("_DissolveAmount", counter);
                skinMatCabeza[0].SetFloat("_DissolveAmount", counter);
                skinMatCuernos[0].SetFloat("_DissolveAmount", counter);
                skinMatChaleco[0].SetFloat("_DissolveAmount", counter);
                skinMatHombreras[0].SetFloat("_DissolveAmount", counter);
                skinMatCylin03[0].SetFloat("_DissolveAmount", counter);
                skinMatCylin01[0].SetFloat("_DissolveAmount", counter);
                skinMatBambas[0].SetFloat("_DissolveAmount", counter);
                skinMatBox103[0].SetFloat("_DissolveAmount", counter);
                skinMatBox108[0].SetFloat("_DissolveAmount", counter);
                skinMatBox109[0].SetFloat("_DissolveAmount", counter);
                skinMatBox110[0].SetFloat("_DissolveAmount", counter);
                
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }

    public void Reinicio()
    {
        skinMatCuerpo[0].SetFloat("_DissolveAmount", 1);
        skinMatCabeza[0].SetFloat("_DissolveAmount", 1);
        skinMatCuernos[0].SetFloat("_DissolveAmount", 1);
        skinMatChaleco[0].SetFloat("_DissolveAmount", 1);
        skinMatHombreras[0].SetFloat("_DissolveAmount", 1);
        skinMatCylin03[0].SetFloat("_DissolveAmount", 1);
        skinMatCylin01[0].SetFloat("_DissolveAmount", 1);
        skinMatBambas[0].SetFloat("_DissolveAmount", 1);
        skinMatBox103[0].SetFloat("_DissolveAmount", 1);
        skinMatBox108[0].SetFloat("_DissolveAmount", 1);
        skinMatBox109[0].SetFloat("_DissolveAmount", 1);
        skinMatBox110[0].SetFloat("_DissolveAmount", 1);
    }
}
