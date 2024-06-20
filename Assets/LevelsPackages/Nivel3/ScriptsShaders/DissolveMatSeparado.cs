using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveMatSeparado : MonoBehaviour
{
    //Descomentar tantas lineas como materialas hayan
    
    
    public SkinnedMeshRenderer skinnedMesh0;
    //public SkinnedMeshRenderer skinnedMesh1;
    //public SkinnedMeshRenderer skinnedMesh2;
    //public SkinnedMeshRenderer skinnedMesh3;
    //public SkinnedMeshRenderer skinnedMesh4;
    //public SkinnedMeshRenderer skinnedMesh5;
    //public SkinnedMeshRenderer skinnedMesh6;
    //public SkinnedMeshRenderer skinnedMesh7;
    //public SkinnedMeshRenderer skinnedMesh8;
    //public SkinnedMeshRenderer skinnedMesh9;


    public VisualEffect VFXGraph;

    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    private Material[] skinnedMaterials0;
    //private Material[] skinnedMaterials1;
    //private Material[] skinnedMaterials2;
    //private Material[] skinnedMaterials3;
    //private Material[] skinnedMaterials4;
    //private Material[] skinnedMaterials5;
    //private Material[] skinnedMaterials6;
    //private Material[] skinnedMaterials7;
    //private Material[] skinnedMaterials8;
    //private Material[] skinnedMaterials9;
    
 
    void Start() {

        if (skinnedMesh0 != null) {

            skinnedMaterials0 = skinnedMesh0.materials;
            //skinnedMaterials1 = skinnedMesh1.materials;
            //skinnedMaterials2 = skinnedMesh2.materials;
            //skinnedMaterials3 = skinnedMesh3.materials;
            //skinnedMaterials4 = skinnedMesh4.materials;
            //skinnedMaterials5 = skinnedMesh5.materials;
            //skinnedMaterials6 = skinnedMesh6.materials;
            //skinnedMaterials7 = skinnedMesh7.materials;
            //skinnedMaterials8 = skinnedMesh8.materials;
            //skinnedMaterials9 = skinnedMesh9.materials;
        }
    }

    // Update is called once per frame
    void Update() {

        /*if (Input.GetKeyDown(KeyCode.Space)) {

            StartCoroutine(Dissolve());
        }*/
    }

    IEnumerator Dissolve(){
        
        if(VFXGraph != null){
            
            VFXGraph.Play();
        }
        
        if (skinnedMaterials0.Length > 0){
            
            float counter = 0;
            
            while(skinnedMaterials0[0].GetFloat("_DissolveAmount") < 1){
                counter += dissolveRate;

                skinnedMaterials0[0].SetFloat("_DissolveAmount", counter);
                //skinnedMaterials1[0].SetFloat("_DissolveAmount", counter);
                //skinnedMaterials2[0].SetFloat("_DissolveAmount", counter);
                //skinnedMaterials3[0].SetFloat("_DissolveAmount", counter);
                //skinnedMaterials4[0].SetFloat("_DissolveAmount", counter);
                //skinnedMaterials5[0].SetFloat("_DissolveAmount", counter);
                //skinnedMaterials6[0].SetFloat("_DissolveAmount", counter);
                //skinnedMaterials7[0].SetFloat("_DissolveAmount", counter);
                //skinnedMaterials8[0].SetFloat("_DissolveAmount", counter);
                //skinnedMaterials9[0].SetFloat("_DissolveAmount", counter);

                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
