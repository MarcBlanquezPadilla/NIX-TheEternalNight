using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageBehaviour : MonoBehaviour {

	[Header("Properties")]
	[SerializeField] private int damage;
	[SerializeField] private enum WhenDamage { 
	
		On_Collision,
		On_Trigger,
		Never,
	}
	[SerializeField] private WhenDamage whenDamage;

	[SerializeField] private UnityEvent onDoDamage;

	private void OnTriggerEnter(Collider collision) {

		if (whenDamage == WhenDamage.On_Trigger && collision.gameObject.TryGetComponent(out HealthBehaviour _hb)) {

			_hb.Hurt(damage);
			onDoDamage.Invoke();
		}
	}

	private void OnCollisionEnter(Collision collision) {

		if (whenDamage == WhenDamage.On_Collision && collision.gameObject.TryGetComponent(out HealthBehaviour _hb)) {

			_hb.Hurt(damage);
			onDoDamage.Invoke();
		}
	}
}
