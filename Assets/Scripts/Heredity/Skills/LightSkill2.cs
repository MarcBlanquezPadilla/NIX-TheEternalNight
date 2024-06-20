using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSkill2 : Hability {

    [Header("Properties")]
    public float dashForce;
    public float dashDuration;

    [Header("References")]
    public CharacterController characterController;

    private void Start() {

        characterController = GameManager.Instance.characterController;
    }

    private void DoDash() {

        characterController.dashing = true;

        if (characterController.direction.x != 0 || characterController.direction.y != 0) {

            StartCoroutine(GameManager.Instance.character.GetComponent<MovementBehaviour>().C_Dash(characterController.direction, dashForce, dashDuration));
        }
        else {

            StartCoroutine(GameManager.Instance.character.GetComponent<MovementBehaviour>().C_Dash(GameManager.Instance.mainCamera.GetComponent<CameraController>().ReturnAimDirection(), dashForce, dashDuration));
        }

        //GameManager.Instance.cameraController.ChangeState(CameraController.State.Dash);

        //Invoke(nameof(UnDash), dashDuration / 2);

        StartCoroutine(C_DisableDash());
    }

    //private void UnDash() {

    //    GameManager.Instance.cameraController.ChangeState(CameraController.State.UnDash);
    //}

    IEnumerator C_DisableDash() {

        yield return new WaitForSeconds(dashDuration);

        characterController.dashing = false;
        StartCoroutine(C_Coldown());

        //GameManager.Instance.cameraController.ChangeState(CameraController.State.Default);
    }

    protected override void Cast() {

        DoDash();
    }
}
