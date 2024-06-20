using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    public bool dashing = false;
    public bool shading = false;
    public bool moving = false;
    public bool sprinting = false;
    public bool aiming = false;
    public bool fusion = false;
    public bool casting = false;
    public Vector3 direction;
    private Vector2 input;
    
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer1;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer2;

    private GameObject normalObj => transform.GetChild(0).gameObject;
    private GameObject fusionObj => transform.GetChild(1).gameObject;

    [HideInInspector] public MovementBehaviour movementBehaviour => GetComponent<MovementBehaviour>();
    [HideInInspector] public HealthBehaviour healthBehaviour => GetComponent<HealthBehaviour>();
    [HideInInspector] public GunController gunController => GetComponentInChildren<GunController>();
    [HideInInspector] public ElementController elementController => GetComponentInChildren<ElementController>();
    [HideInInspector] public Animator normalAnimator => normalObj.GetComponent<Animator>();
    [HideInInspector] public Animator fusionAnimator => fusionObj.GetComponent<Animator>();


    private bool light = true;
    private bool poison = false;
    private bool blood = false;
    private bool shadow = false;

    [HideInInspector] public bool blockControls;

    [SerializeField] private float fusionDuration;

    public GameObject characterCanvas; 

    private void OnEnable() {

        UpdateHealthIndicator(healthBehaviour.ReturnHealthPercent());
    }

    private void Update() {

        if (!dashing && !blockControls)
            MyInput();
    }

    private void FixedUpdate() {

        if(!dashing)
            movementBehaviour.MoveRb3D(direction);
    }

    public void MyInput() {

        input.x = 0;
        input.y = 0;

        if (Input.GetKey(InputManager.Instance.keys["Options"])) {

            MainMenuManager.Instance.OpenOptions();
            GameManager.Instance.cameraController.transform.GetChild(0).GetComponent<AudioListener>().enabled = false;
        }

        if (Input.GetKey(InputManager.Instance.keys["Forward Movement"]) || Input.GetKey(InputManager.Instance.keys["Backward Movement"]) || Input.GetKey(InputManager.Instance.keys["Rightward Movement"]) || Input.GetKey(InputManager.Instance.keys["Leftward Movement"])) {

            if (!transform.Find("Move").GetComponent<AudioSource>().isPlaying) {

                transform.Find("Move").GetComponent<AudioSource>().Play();
            }

            transform.Find("Move").GetComponent<AudioSource>().UnPause();

            if (Input.GetKey(InputManager.Instance.keys["Forward Movement"])) {

                input.x = 1;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, GameManager.Instance.mainCamera.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }

            if (Input.GetKey(InputManager.Instance.keys["Backward Movement"])) {

                input.x = -1;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, GameManager.Instance.mainCamera.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }

            if (Input.GetKey(InputManager.Instance.keys["Leftward Movement"])) {

                input.y = -1;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, GameManager.Instance.mainCamera.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }

            if (Input.GetKey(InputManager.Instance.keys["Rightward Movement"])) {

                input.y = 1;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, GameManager.Instance.mainCamera.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }


            if (Input.GetKey(InputManager.Instance.keys["Sprinting"]) && !Input.GetKey(InputManager.Instance.keys["Aim"]) && !shading)
            {

                movementBehaviour.SetSpeed(movementBehaviour.ReturnDefaultSpeed() * movementBehaviour.ReturnSprintMultiplier());

                if (!transform.Find("Run").GetComponent<AudioSource>().isPlaying)
                {
                    transform.Find("Run").GetComponent<AudioSource>().Play();
                }

                transform.Find("Move").GetComponent<AudioSource>().Pause();

                transform.Find("Run").GetComponent<AudioSource>().UnPause();
            }
            else
            {
                transform.Find("Run").GetComponent<AudioSource>().Pause();
            }


            if (Input.GetKeyUp(InputManager.Instance.keys["Sprinting"]) && !shading)
            {
                movementBehaviour.SetSpeed(movementBehaviour.ReturnDefaultSpeed());
            }

            AnimatorSetInt("State", 1);

            AnimatorSetFloat("movementSpeed", movementBehaviour.ReturnSpeed());
        }
        else {

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, GameManager.Instance.mainCamera.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            AnimatorSetInt("State", 0);
            transform.Find("Move").GetComponent<AudioSource>().Pause();
            transform.Find("Run").GetComponent<AudioSource>().Pause();
            AnimatorSetFloat("movementSpeed", 0);
        }

        AnimatorSetFloat("xDir", input.x);
        AnimatorSetFloat("yDir", input.y);

        AnimatorSetFloat("healthPercent", healthBehaviour.ReturnHealthPercent());

        //... Move to facing direction ...//
        Vector3 dirX = GameManager.Instance.mainCamera.transform.right * input.y;
        Vector3 dirZ = GameManager.Instance.mainCamera.transform.forward * input.x;
        Vector3 facingDirection = dirX + dirZ;
        facingDirection.Normalize();

        direction = facingDirection;

        // ************** CAMBIAR! ****************** //
        if (fusion && !shading)
        {
            if (light && Input.GetKey(InputManager.Instance.keys["Basic Attack"])) elementController.CastHability(0, "Light");
            if (poison && Input.GetKeyDown(InputManager.Instance.keys["Cast Hability 1"])) elementController.CastHability(0, "Poison");
            if (blood && Input.GetKeyDown(InputManager.Instance.keys["Cast Hability 2"])) elementController.CastHability(0, "Blood");
            if (shadow && Input.GetKey(InputManager.Instance.keys["Cast Hability 3"])) elementController.CastHability(0, "Shadow");

            if (Input.GetKey(InputManager.Instance.keys["Unfusion"])) StartFusion();
        }
        else
        {
            if (Input.GetKey(InputManager.Instance.keys["Basic Attack"])) elementController.CastHability(0, "Default");
            if (Input.GetKeyDown(InputManager.Instance.keys["Reload"]))
            {
                GetComponentInChildren<GunController>().Reload();
            }
        }

        if (Input.GetKey(InputManager.Instance.keys["Aim"]) || Input.GetKey(InputManager.Instance.keys["Basic Attack"]))
        {
            AnimatorSetBool("isAiming", true);
        }
        else
        {
            AnimatorSetBool("isAiming", false);
        }

        if (Input.GetKeyDown(InputManager.Instance.keys["Aim"])) {

            GameManager.Instance.cameraController.ChangeState(CameraController.State.Aim);
        }
        if (Input.GetKey(InputManager.Instance.keys["Aim"])) {

            movementBehaviour.SetSpeed(movementBehaviour.ReturnDefaultSpeed() - movementBehaviour.ReturnDefaultSpeed() * movementBehaviour.ReturnAimReduction());
        }
        if (Input.GetKeyUp(InputManager.Instance.keys["Aim"])) {

            movementBehaviour.SetSpeed(movementBehaviour.ReturnDefaultSpeed());
            GameManager.Instance.cameraController.ChangeState(CameraController.State.Default);
        }

        if (Input.GetKeyDown(InputManager.Instance.keys["Interact"]) && GameManager.Instance.cameraController.ReturnInteractable().collider != null) {

            if (GameManager.Instance.cameraController.ReturnInteractable().collider.gameObject.TryGetComponent<Interactable>(out Interactable interactable))
                interactable.Interact();
        }

        if (Input.GetKeyDown(InputManager.Instance.keys["Inventory"]))
        {
            InventoryManager.Instance.OpenInventory(!InventoryManager.Instance.inventory.activeInHierarchy);
        }

        if (Input.GetKeyDown(InputManager.Instance.keys["Up Arrow"]))
        {
            InventoryManager.Instance.SelectNext(false);
        }
        if (Input.GetKeyDown(InputManager.Instance.keys["Down Arrow"]))
        {
            InventoryManager.Instance.SelectNext(true);
        }

        if(Input.GetKeyDown(InputManager.Instance.keys["Enter"]))
        {
            InventoryManager.Instance.UseItems();
            
        }

        if (Input.GetKey(InputManager.Instance.keys["Basic Attack"]) || Input.GetKey(InputManager.Instance.keys["Cast Hability 1"]) || Input.GetKey(InputManager.Instance.keys["Cast Hability 2"]) || Input.GetKey(InputManager.Instance.keys["Cast Hability 3"]))
            casting = true;
        else
            casting = false;

        if (Input.GetKey(InputManager.Instance.keys["Forward Movement"]) || Input.GetKey(InputManager.Instance.keys["Backward Movement"]) || Input.GetKey(InputManager.Instance.keys["Rightward Movement"]) || Input.GetKey(InputManager.Instance.keys["Leftward Movement"]))
            moving = true;
        else 
            moving = false;

        if (Input.GetKey(InputManager.Instance.keys["Sprinting"]) && moving && !Input.GetKey(InputManager.Instance.keys["Aim"]))
            sprinting = true;
        else
            sprinting = false;

        if (Input.GetKey(InputManager.Instance.keys["Aim"]))
            aiming = true;
        else
            aiming = false;
    }

    public void UpdateHealthIndicator(float healthPercent) {

        StartCoroutine(C_DamageIndicator(healthPercent));
        Debug.Log(GameManager.Instance.cameraController.fullScreenShadersRenderer.material);
        GameManager.Instance.cameraController.fullScreenShadersRenderer.material.SetFloat("_LifePercent", healthPercent);
    }

    public void AnimatorSetBool(string name, bool value)
    {
        if (normalAnimator.gameObject.activeInHierarchy)
        {
            normalAnimator.SetBool(name, value);
        }
        if (fusionAnimator.gameObject.activeInHierarchy)
        {
            fusionAnimator.SetBool(name, value);
        }
    }

    public void AnimatorSetInt(string name, int value)
    {
        if (normalAnimator.gameObject.activeInHierarchy)
        {
            normalAnimator.SetInteger(name, value);
        }
        if (fusionAnimator.gameObject.activeInHierarchy)
        {
            fusionAnimator.SetInteger(name, value);
        }
    }

    public void AnimatorSetFloat(string name, float value)
    {
        if (normalAnimator.gameObject.activeInHierarchy)
        {
            normalAnimator.SetFloat(name, value);
        }
        if (fusionAnimator.gameObject.activeInHierarchy)
        {
            fusionAnimator.SetFloat(name, value);
        }
    }

    public void AnimatorSetTrigger(string name)
    {
        if (normalAnimator.gameObject.activeInHierarchy)
        {
            normalAnimator.SetTrigger(name);
        }
        if (fusionAnimator.gameObject.activeInHierarchy)
        {
            fusionAnimator.SetTrigger(name);
        }
    }

    IEnumerator C_DamageIndicator(float healthPercent) {

        float counter = 0;

        while (counter < 2) {

            skinnedMeshRenderer1.materials[0].SetInt("_ColorEmissivo", 0);
            skinnedMeshRenderer2.materials[0].SetInt("_ColorEmissivo", 0);

			yield return new WaitForSeconds(.2f);

			skinnedMeshRenderer1.materials[0].SetInt("_ColorEmissivo", 20);
            skinnedMeshRenderer2.materials[0].SetInt("_ColorEmissivo", 20);

			counter += 1;
        }

		skinnedMeshRenderer1.materials[0].SetInt("_ColorEmissivo", 20);
		skinnedMeshRenderer2.materials[0].SetInt("_ColorEmissivo", 20);

		if (healthPercent > .75f) {

			skinnedMeshRenderer1.materials[0].SetColor("_ColorEmissive", Color.green);
			skinnedMeshRenderer2.materials[0].SetColor("_ColorEmissive", Color.green);
		}
		else if (healthPercent <= .75f && healthPercent > .50f) {

			skinnedMeshRenderer1.materials[0].SetColor("_ColorEmissive", Color.yellow);
			skinnedMeshRenderer2.materials[0].SetColor("_ColorEmissive", Color.yellow);
		}
		else if (healthPercent <= .50f && healthPercent > .25f) {

			skinnedMeshRenderer1.materials[0].SetColor("_ColorEmissive", new Color(1f, .5f, 0));
			skinnedMeshRenderer2.materials[0].SetColor("_ColorEmissive", new Color(1f, .5f, 0));
		}
		else if (healthPercent <= .25f && healthPercent > 0f) {

			skinnedMeshRenderer1.materials[0].SetColor("_ColorEmissive", Color.red);
			skinnedMeshRenderer2.materials[0].SetColor("_ColorEmissive", Color.red);
		}
		else if (healthPercent == 0) {

			skinnedMeshRenderer1.materials[0].SetColor("_ColorEmissive", Color.black);
			skinnedMeshRenderer2.materials[0].SetColor("_ColorEmissive", Color.black);
		}
	}

    public void TakeLight() {

        light = true;
    }

    public void TakePoison() {

        poison = true;
    }

    public void TakeBlood() {

        blood = true;
    }

    public void TakeShadow() {

        shadow = true;
    }

    public void StartFusion()
    {
        if (!dashing && !shading && !moving && !sprinting && !aiming && !casting && BrownieController.Instance.pet)
        {
            StartCoroutine(C_StartFusion());
        }
    }

    public void Die() {

        StartCoroutine(C_Die());
    }

    private IEnumerator C_Die() {

        blockControls = true;
        GameManager.Instance.cameraController.blockCamera = true;

        if (fusion) {

            yield return StartCoroutine(GetComponent<DissolveNina>().C_DissolveNinaFusion());
        }
        else {

            yield return StartCoroutine(GetComponent<DissolveNina>().C_DissolveNina());
        }

        MainMenuManager.Instance.OpenMuerte();
    }

    private IEnumerator C_StartFusion()
    {
        blockControls = true;
        GameManager.Instance.cameraController.blockCamera = true;

        if (fusion) {

			yield return StartCoroutine(GetComponent<DissolveNina>().C_DissolveNinaFusion());

			normalObj.SetActive(true);
			fusionObj.SetActive(false);
			GameManager.Instance.brownieController.gameObject.SetActive(true);
			GameManager.Instance.brownieController.transform.position = transform.position;

            StartCoroutine(GameManager.Instance.brownieController.GetComponent<DissolveBrownie>().SUndissolveBrownie());
            yield return StartCoroutine(GetComponent<DissolveNina>().C_UNDissolveNina());
		}
        else {

            StartCoroutine(GameManager.Instance.brownieController.GetComponent<DissolveBrownie>().SDissolveBrownie());
            yield return StartCoroutine(GetComponent<DissolveNina>().C_DissolveNina());

            normalObj.SetActive(false);
			fusionObj.SetActive(true);
			GameManager.Instance.brownieController.gameObject.SetActive(false);

            yield return StartCoroutine(GetComponent<DissolveNina>().C_UNDissolveNinaFusion());
		}
        
        fusion = !fusion;
        UpdateHealthIndicator(healthBehaviour.ReturnHealthPercent());

        blockControls = false;
        GameManager.Instance.cameraController.blockCamera = false;
    }

    public void GetElement(string element)
    {
        if (element == "Poison")
        {
            poison = true;
        }
        else if (element == "Blood")
        {
            blood = true;
        }
        else if (element == "Light")
        {
            light = true;
        }
        else if (element == "Shadow")
        {
            shadow = true;
        }
    }
}
