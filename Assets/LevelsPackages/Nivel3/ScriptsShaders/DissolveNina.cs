using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveNina : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh1;
	public SkinnedMeshRenderer skinnedMesh2;

	public VisualEffect VFXGraph;

    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    private Material[] skinnedMaterials1;
	private Material[] skinnedMaterials2;

	public GameObject[] objects;

	void Start()
    {

        if (skinnedMesh1 != null)
        {
            skinnedMaterials1 = skinnedMesh1.materials;
        }

		if (skinnedMesh2 != null) {
			skinnedMaterials2 = skinnedMesh2.materials;
		}
	}

    // Update is called once per frame
    void Update()
    {

        /*if (Input.GetKeyDown(KeyCode.Space)) 
        {
            StartCoroutine(Dissolve());
        }*/
    }

    public void TryDie() {

        StartCoroutine(C_DissolveNina());
    }

    public IEnumerator C_DissolveNina()
    {

		objects[0].SetActive(false);
		objects[1].SetActive(false);

		if (VFXGraph != null)
        {

            VFXGraph.Play();
        }

        if (skinnedMaterials1.Length > 0)
        {

            float counter = 0;

            while (skinnedMaterials1[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;

                for (int i = 0; i < skinnedMaterials1.Length; i++)
                {
                    skinnedMaterials1[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }

	public IEnumerator C_UNDissolveNina() {

		gameObject.transform.GetChild(0).gameObject.SetActive(true);

		PrepareNina();

		if (VFXGraph != null) {

			VFXGraph.Play();
		}

		if (skinnedMaterials1.Length > 0) {

			float counter = 1;

			while (skinnedMaterials1[0].GetFloat("_DissolveAmount") > 0) {

				counter -= dissolveRate;

				for (int i = 0; i < skinnedMaterials1.Length; i++) {

					skinnedMaterials1[i].SetFloat("_DissolveAmount", counter);
				}
				yield return new WaitForSeconds(refreshRate);
			}
		}


		objects[0].SetActive(true);
		objects[1].SetActive(true);
	}

	public IEnumerator C_DissolveNinaFusion() {

		objects[2].SetActive(false);
		objects[3].SetActive(false);
		objects[4].SetActive(false);
		objects[5].SetActive(false);

		if (VFXGraph != null) {

			VFXGraph.Play();
		}

		if (skinnedMaterials2.Length > 0) {

			float counter = 0;

			while (skinnedMaterials2[0].GetFloat("_DissolveAmount") < 1) {
				counter += dissolveRate;

				for (int i = 0; i < skinnedMaterials2.Length; i++) {
					skinnedMaterials2[i].SetFloat("_DissolveAmount", counter);
				}
				yield return new WaitForSeconds(refreshRate);
			}
		}
	}

	public IEnumerator C_UNDissolveNinaFusion() {

		gameObject.transform.GetChild(1).gameObject.SetActive(true);

		PrepareNinaFusion();

		if (VFXGraph != null) {

			VFXGraph.Play();
		}

		if (skinnedMaterials2.Length > 0) {

			float counter = 1;

			while (skinnedMaterials2[0].GetFloat("_DissolveAmount") > 0) {

				counter -= dissolveRate;

				for (int i = 0; i < skinnedMaterials2.Length; i++) {

					skinnedMaterials2[i].SetFloat("_DissolveAmount", counter);
				}
				yield return new WaitForSeconds(refreshRate);
			}
		}

		objects[2].SetActive(true);
		objects[3].SetActive(true);
		objects[4].SetActive(true);
		objects[5].SetActive(true);

	}

	public void Reinicio()
    {
        for (int i = 0; i < skinnedMaterials1.Length; i++)
        {
            skinnedMaterials1[i].SetFloat("_DissolveAmount", 1);
        }
    }

    public void PrepareNina() {

		objects[0].SetActive(false);
		objects[1].SetActive(false);

		for (int i = 0; i < skinnedMaterials1.Length; i++) {
			skinnedMaterials1[i].SetFloat("_DissolveAmount", 1);
		}
	}

	public void PrepareNinaFusion() {

		objects[2].SetActive(false);
		objects[3].SetActive(false);
		objects[4].SetActive(false);
		objects[5].SetActive(false);

		for (int i = 0; i < skinnedMaterials2.Length; i++) {
			skinnedMaterials2[i].SetFloat("_DissolveAmount", 1);
		}
	}
}