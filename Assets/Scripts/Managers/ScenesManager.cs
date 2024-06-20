using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour {

    #region instance

    private static ScenesManager _instance;
    public static ScenesManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScenesManager>();
                //DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    #endregion

    [HideInInspector] public bool changingScene = false;

    [SerializeField] private CanvasGroup feid;
    [SerializeField] private float feidDuration;
    private float timer;


    public void ChangeSacene(string sceneName) {

        StartCoroutine(C_ChangeScene(sceneName));
    }

    IEnumerator C_ChangeScene(string sceneName) {
        
        timer = 0;

        while (timer < 1) {

            timer += Time.deltaTime / feidDuration;

            timer = Mathf.Clamp(timer, 0, 1);
            
            feid.alpha = timer;

            yield return new WaitForEndOfFrame();
        }
        changingScene = true;

        string currentScene = SceneManager.GetActiveScene().name;

		var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		asyncLoad.allowSceneActivation = true;

		while (!(asyncLoad.isDone)) {

			yield return new WaitForEndOfFrame();
		}

		SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));


        yield return new WaitForSeconds(1f);

		SceneManager.UnloadSceneAsync(currentScene);

        changingScene = false;


        while (timer > 0)
        {

            timer -= Time.deltaTime / feidDuration;

            timer = Mathf.Clamp(timer, 0, 1);

            feid.alpha = timer;

            yield return new WaitForEndOfFrame();
        }

    }

}
