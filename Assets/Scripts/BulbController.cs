using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbController : MonoBehaviour {

	[SerializeField]
	private GameObject pickupEffect;

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Player")) {
			Explode ();
		}
	}

	void Explode () {
		Instantiate (pickupEffect, transform.position, transform.rotation);
		Destroy (gameObject);
	}
}
