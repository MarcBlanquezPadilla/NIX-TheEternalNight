using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour {

    [Header("Movement")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField][Range(0,1)] private float aimReduction = 0;
    private float defaultSpeed;

    

    [Header("References")]
    [SerializeField] private Rigidbody _rb;
    
    private void Awake() {

        if (GetComponent<Rigidbody>() != null)
            _rb = GetComponent<Rigidbody>();

        SetDefaultSpeed();
    }

    #region MOVEMENT

    #region 3D

    public void MoveRb3D(Vector3 moveDirection) {

        _rb.velocity = new Vector3 (moveDirection.x * movementSpeed, _rb.velocity.y, moveDirection.z * movementSpeed);
    }

    public void MoveRb3DAllAxis(Vector3 moveDirection) {

        _rb.velocity = moveDirection * movementSpeed;
    }

    #endregion

    public void SetSpeed(float newSpeed) {

        movementSpeed = newSpeed;
    }

    public float ReturnSpeed() {

        return movementSpeed;
    }

    public void SetDefaultSpeed() {

        defaultSpeed = movementSpeed;
    }

    public void Stop() {

        movementSpeed = 0;
    }

    public float ReturnDefaultSpeed() {

        return defaultSpeed;
    }

    public float ReturnSprintMultiplier() {

        return sprintMultiplier;
    }

    public float ReturnAimReduction() {

        return aimReduction;
    }

    #endregion

    #region DASH

    public IEnumerator C_Dash(Vector3 moveDirection, float dashForce, float dashDuration) {

        _rb.useGravity = false;

        _rb.velocity = new Vector3(moveDirection.x * dashForce, 0f, moveDirection.z * dashForce);

        yield return new WaitForSeconds(dashDuration);

        _rb.useGravity = true;

        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
    }

    #endregion
}
