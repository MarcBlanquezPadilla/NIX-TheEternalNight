using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    [SerializeField] private float cooldown = 1.5f;
    [SerializeField] private float maxAmmoCapacity = 48;
    [SerializeField] private float totalAmmo;
    [SerializeField] private float maxLoadedAmmo = 12;
    [SerializeField] private float loadedAmmo;
    [SerializeField] private GameObject flashLight;
    [SerializeField] private Material[] material;
    [SerializeField] private GameObject whoIAm;

    private bool readyToShoot = true;
    private bool reloading = false;

    private void Awake() {

        totalAmmo = maxAmmoCapacity;
        loadedAmmo = maxLoadedAmmo;
        flashLight.SetActive(true);
    }

    private void Update() {

        if (Input.GetKeyDown(InputManager.Instance.keys["Flashlight"])) {

            flashLight.SetActive(!flashLight.activeInHierarchy);
        }
    }

    public void Shoot() {

        Vector3 rotationDirection = GameManager.Instance.mainCamera.GetComponent<CameraController>().ReturnAimDirection();
        rotationDirection.y = 0;

        GameManager.Instance.character.transform.rotation = Quaternion.LookRotation(rotationDirection, Vector3.up);
        GameManager.Instance.characterController.AnimatorSetBool("isAiming", true);

        if (!reloading && readyToShoot && loadedAmmo > 0 && !GameManager.Instance.characterController.normalAnimator.GetCurrentAnimatorStateInfo(1).IsName("Aim") && !GameManager.Instance.characterController.normalAnimator.GetCurrentAnimatorStateInfo(1).IsName("Reloading") && !GameManager.Instance.characterController.normalAnimator.GetCurrentAnimatorStateInfo(1).IsName("Empty") && !GameManager.Instance.characterController.normalAnimator.IsInTransition(1))
        {
            Vector3 direction = Vector3.zero;
            GameObject bullet = null;

            direction = GameManager.Instance.cameraController.ReturnAim().point - transform.position;
            bullet = PoolingManager.Instance.GetPooledObject("CharacterBullet");
            direction.Normalize();

            if (bullet != null)
            {

                bullet.transform.position = transform.GetChild(0).position;
                bullet.SetActive(true);

                bullet.GetComponent<MovementBehaviour>().MoveRb3DAllAxis(direction);
                bullet.GetComponent<Rigidbody>().useGravity = false;

                readyToShoot = false;
                loadedAmmo--;


                transform.Find("Shoot").GetComponent<AudioSource>().Play();


                StartCoroutine(Cadency());
            }
        }
    }

    public void Reload() {

        bool reloaded = false;

        if (totalAmmo > 0 && loadedAmmo < maxLoadedAmmo) {

            float meCaben = maxLoadedAmmo - loadedAmmo;

            if (totalAmmo >= meCaben) {

                totalAmmo -= meCaben;
                loadedAmmo += meCaben;
                reloaded = true;
            }
            else if (totalAmmo < meCaben && totalAmmo > 0)
            {
                totalAmmo -= totalAmmo;
                loadedAmmo -= totalAmmo;
                reloaded = true;
            }

            if (reloaded)
            {
                StartCoroutine(C_Reload(2.5f));
                GameManager.Instance.characterController.AnimatorSetTrigger("reload");
                if (!GameManager.Instance.character.transform.Find("Reload").GetComponent<AudioSource>().isPlaying)
                {
                    GameManager.Instance.character.transform.Find("Reload").GetComponent<AudioSource>().Play();
                }

                GameManager.Instance.character.transform.Find("Reload").GetComponent<AudioSource>().UnPause();
            }
        }
    }

    public void GetAmmoBox() {

        totalAmmo += 12;

        if (totalAmmo > maxAmmoCapacity) {

            totalAmmo = maxAmmoCapacity;
        }
    }

    private IEnumerator C_Reload(float reloadAnimationDuration) {

        reloading = true;

        yield return new WaitForSeconds(reloadAnimationDuration);

        reloading = false;
    }

    private IEnumerator Cadency() {

        yield return new WaitForSeconds(cooldown);

        readyToShoot = true;
    }
}
