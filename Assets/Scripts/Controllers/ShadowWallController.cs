using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowWallController : MonoBehaviour {

	[Header("Properties")]
	[SerializeField] private float timeToRepair = 3f;
	[SerializeField] private float damageEffectTime = 0.1f;
	[SerializeField] private Material shadowMaterial;
	[SerializeField] private int shadeWallLayer;
	private bool shade = false;

	[Header("References")]
	[SerializeField] private MeshRenderer meshRenderer;
	[SerializeField] private BoxCollider boxCollider;

	private Material defaultMaterial;
	private int defaultLayer;


	private void Awake() {

		meshRenderer = GetComponent<MeshRenderer>();
		boxCollider = GetComponent<BoxCollider>();

		defaultMaterial = meshRenderer.material;
		defaultLayer = gameObject.layer;
	}

	public void ShadeWall(float duration) {

        if (!shade) {

			shade = true;

			meshRenderer.material = shadowMaterial;
			gameObject.layer = shadeWallLayer;

			Invoke(nameof(UnshadeWall), duration);
		}
	}

	public void UnshadeWall() {

		meshRenderer.material = defaultMaterial;
		gameObject.layer = defaultLayer;

		shade = false;
	}
}
