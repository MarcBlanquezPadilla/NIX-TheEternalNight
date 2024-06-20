using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour {

	#region Instance

	private static MainMenuManager _instance;
	public static MainMenuManager Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<MainMenuManager>();
			}
			return _instance;
		}
	}

	#endregion

	[SerializeField] private bool turnFadeIn;
    [SerializeField] private Image fadeIn;

    [SerializeField] private GameObject Main;
    [SerializeField] private GameObject Option;
    [SerializeField] private GameObject menuButton;

    private void Awake()
    {
        fadeIn = GetComponent<Image>();
    }

    public void LeaveGame() {

        Application.Quit();
    }

    public void loadGame(string sceneName) {

        if (turnFadeIn) {

            StartCoroutine(C_FadeIn());
        }

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator C_FadeIn() {

        fadeIn.CrossFadeAlpha(1f, 4f, false);

        yield return new WaitForEndOfFrame();
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Alpha7)) {

            SceneManager.LoadScene("Level1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {

            SceneManager.LoadScene("Level2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {

            SceneManager.LoadScene("Level3");
        }
    }

    public void ReturnOptionButton() {

        if (SceneManager.GetActiveScene().name == "MainMenuScene") {

            Main.SetActive(true);
        }
        else {

			GameManager.Instance.characterController.characterCanvas.SetActive(true);

			GameManager.Instance.characterController.blockControls = false;
            GameManager.Instance.cameraController.blockCamera = false;
			Time.timeScale = 1;
        }

        Option.SetActive(false);
    }

    public void OpenOptions() {

		GameManager.Instance.characterController.characterCanvas.SetActive(false);

		GameManager.Instance.characterController.blockControls = true;
		GameManager.Instance.cameraController.blockCamera = true;
		Time.timeScale = 0;

        menuButton.SetActive(true);
		Option.SetActive(true);    
    }

    public void GoToMainMenu() {

		Time.timeScale = 1;
		Destroy(FindObjectOfType<DontDestroy>().gameObject);

		Option.SetActive(false);
		menuButton.SetActive(false);
        SceneManager.LoadScene("MainMenuScene");
	}

    public void OpenMuerte() {

        gameObject.transform.GetChild(2).gameObject.SetActive(true);
    } 

    public void Respawn() {

	    SceneManager.LoadScene(SceneManager.GetActiveScene().name);

		GameManager.Instance.characterController.healthBehaviour.Heal(GameManager.Instance.characterController.healthBehaviour.ReturnMaxHealth());
		GameManager.Instance.characterController.fusion = false;

		StartCoroutine(GameManager.Instance.character.GetComponent<DissolveNina>().C_UNDissolveNina());

		GameManager.Instance.characterController.blockControls = false;
		GameManager.Instance.cameraController.blockCamera = false;

		gameObject.transform.GetChild(2).gameObject.SetActive(false);
	}

    public void Win() {

		gameObject.transform.GetChild(3).gameObject.SetActive(true);

		GameManager.Instance.characterController.blockControls = true;
		GameManager.Instance.cameraController.blockCamera = true;
	}
}
