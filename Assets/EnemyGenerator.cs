using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {

    public void RespawnEnemy(GameObject respawnEnemyObj, float timeToRespawn) {

        StartCoroutine(C_RespawnEnemy(respawnEnemyObj, timeToRespawn));
    }

    public IEnumerator C_RespawnEnemy(GameObject respawnEnemyObj, float timeToRespawn) {

        yield return new WaitForSeconds(timeToRespawn);

        respawnEnemyObj.SetActive(true);
        respawnEnemyObj.GetComponent<Enemy>().ChangeStateToRespawn();
    }
}
