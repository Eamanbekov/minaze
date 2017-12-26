using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbController : MonoBehaviour {

	private Light light;
	[SerializeField]
	private const float maxRange = 10f;
	[SerializeField]
	private const float minRange = 5f;
	private float step = 10f;

	// Use this for initialization
	void Start () {
		light = GetComponent<Light> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (light.range > maxRange || light.range < minRange) {
			step *= -1;
		}
		light.range = light.range - step * Time.deltaTime ;
	}
}
