using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class CameraController : MonoBehaviour {

    public enum State {

        Default,
        Aim,
    }
    private State actualState = State.Default;

    [Header("Camera Settings")]

    [Header("Default")]
    [SerializeField] private Vector3 defaultPosition;
    [SerializeField] private Vector3 actualPosition;
    [SerializeField] private Vector3 defaultRotation;
    [SerializeField] private Vector3 actualRotation;
    [SerializeField] private Vector2 sensitivity;
    [SerializeField] private float fov = 65f;
    [SerializeField] public LayerMask collisionMask;
    [SerializeField] public float hitDistance;


    [Header("Aim")]
    [SerializeField] private Vector3 aimPosition;
    [SerializeField] private Vector2 aimSensitivity;

    [Header("Interactables")]
    [SerializeField] private float maxInteractionDistance;
    [SerializeField] private LayerMask interactableLayer;

    [Header("References")]
    [SerializeField] private Transform followTo;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject characterSpine;

    [HideInInspector] public bool invertHorizontalAxis;
    [HideInInspector] public bool inverVerticalAxis;

    private Vector2 rotation = Vector2.zero;

    [Header("Raycast")]
    [SerializeField] private bool showAimRaycast;
    [SerializeField] private bool showInteractableRaycast;
    private RaycastHit aimHit;
    private RaycastHit interactableHit;
    [SerializeField] private LayerMask aimHitLayers;

    [HideInInspector] public bool blockCamera = false;

    public Renderer fullScreenShadersRenderer;

    private void Awake() {

        cam = transform.GetChild(0).GetComponent<Camera>();
    }

	void Start() {

        actualPosition = defaultPosition;
        actualRotation = defaultRotation;
    }

    void LateUpdate() {

        if (!blockCamera) {

			Cursor.lockState = CursorLockMode.Locked;

			//if (!ScenesManager.Instance.changingScene) {

			transform.position = followTo.position;

            RaycastHit interactHit;

            if (Physics.Raycast(transform.GetChild(0).position, transform.GetChild(0).forward, out interactHit, maxInteractionDistance, interactableLayer)) interactableHit = interactHit;
            else interactableHit = new RaycastHit();

            if (showInteractableRaycast) Debug.DrawRay(transform.GetChild(0).position, transform.GetChild(0).forward * maxInteractionDistance, Color.green);

            RaycastHit hit;
            if (Physics.Raycast(transform.GetChild(0).position, transform.GetChild(0).forward, out hit, Mathf.Infinity, aimHitLayers)) aimHit = hit;

            if (showAimRaycast) Debug.DrawRay(transform.GetChild(0).position, transform.GetChild(0).forward * 100, Color.black);

            RaycastHit hitBetweenPlayer;

            if (Physics.Raycast(GameManager.Instance.character.transform.position + Vector3.up, transform.GetChild(0).position - (GameManager.Instance.character.transform.position + Vector3.up), out hitBetweenPlayer, 2, collisionMask)) {
                actualPosition = transform.InverseTransformPoint(hitBetweenPlayer.point);
            }
            else {
                actualPosition = defaultPosition;
            }

            switch (actualState) {

                case State.Default:

                    transform.GetChild(0).transform.localPosition = actualPosition;
                    transform.GetChild(0).transform.localRotation = Quaternion.Euler(defaultRotation);

                    fov = 30f;

                    rotation.y += Input.GetAxis("Mouse X") * sensitivity.x;
                    rotation.x += -Input.GetAxis("Mouse Y") * sensitivity.y;

                    rotation.x = Mathf.Clamp(rotation.x, -40, 25);

                    //characterSpine.transform.localRotation = Quaternion.Euler(0, 90, 0);

                    break;
                case State.Aim:

                    transform.GetChild(0).transform.localPosition = actualPosition;
                    transform.GetChild(0).transform.localRotation = Quaternion.Euler(defaultRotation);

                    fov = 15f;

                    rotation.y += Input.GetAxis("Mouse X") * aimSensitivity.x;
                    rotation.x += -Input.GetAxis("Mouse Y") * aimSensitivity.y;

                    rotation.x = Mathf.Clamp(rotation.x, -40, 25);

                    characterSpine.transform.localRotation = Quaternion.Euler(0, 90, rotation.x * .75f);

                    break;
            }

            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, Time.deltaTime * 10f);
            //cam.fieldOfView = fov;

            if (!invertHorizontalAxis && !inverVerticalAxis) transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
            else if (invertHorizontalAxis && inverVerticalAxis) transform.rotation = Quaternion.Euler(-rotation.x, -rotation.y, 0);
            else if (invertHorizontalAxis) transform.rotation = Quaternion.Euler(rotation.x, -rotation.y, 0);
            else if (inverVerticalAxis) transform.rotation = Quaternion.Euler(-rotation.x, rotation.y, 0);
            Debug.DrawRay(FindObjectOfType<CharacterController>().transform.position + Vector3.up, transform.GetChild(0).position - (FindObjectOfType<CharacterController>().transform.position + Vector3.up), Color.red);
            //}
        }
        else {

			Cursor.lockState = CursorLockMode.None;
		}
	}

    private void Update()
    {
        if (Input.GetMouseButtonDown(2) && !blockCamera)
        {

            actualPosition = defaultPosition;
        }
    }

    public void ChangeState(State stateToChange) {

        actualState = stateToChange;
    }

    public RaycastHit ReturnAim() {

        return aimHit;
    }

    public RaycastHit ReturnInteractable() {

        return interactableHit;
    }

    public Vector3 ReturnAimDirection() {

        Vector3 aimDirection = transform.GetChild(0).forward;

        return aimDirection;
    }
}
