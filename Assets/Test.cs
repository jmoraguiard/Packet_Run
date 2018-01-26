using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void RandRotCube()
	{
		float rotAmount = Random.Range (-40, 40);
		Debug.Log (rotAmount);

		transform.rotation = Quaternion.Euler (0, rotAmount, 0);
	}
}
