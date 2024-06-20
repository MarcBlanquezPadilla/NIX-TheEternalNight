using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBasicAttack : Hability {

    public GameObject basicAttackTriggerObj;
    public float damage;
    public List<HealthBehaviour> inBasicRange = new List<HealthBehaviour>();

    private ElementController elementController;

    private void Awake()
    {
        elementController = GetComponentInParent<ElementController>();

        basicAttackTriggerObj.SetActive(false);
    }

    protected override void Cast() {

        if (!basicAttackTriggerObj.activeInHierarchy)
        {
            basicAttackTriggerObj.SetActive(true);
            StartCoroutine(WaitForDisableObj());
        }
        

        int nearest = -1;
        float nearestDist = Mathf.Infinity;

        for (int i = 0; i < inBasicRange.Count; i++)
        {
            float currentDist = Vector3.Distance(GameManager.Instance.character.transform.position, inBasicRange[i].transform.position);
            if (currentDist<nearestDist)
            {
                nearestDist = currentDist;
                nearest = i;
            }
        }

        if (nearest!=-1)
        {
            inBasicRange[nearest].Hurt(damage);
            StartCooldown();
        }
    }
    IEnumerator WaitForDisableObj()
    {
        yield return new WaitUntil(CheckDisable);
        basicAttackTriggerObj.SetActive(false);
        inBasicRange.Clear();
    }

    bool CheckDisable()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) || elementController.currentElement != "Shadow")
            return true;
        else
            return false;
    }
}
