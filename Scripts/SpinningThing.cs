using UnityEngine;
using System.Collections;

public class SpinningThing : MonoBehaviour {
	float theta = 400f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(0, 0, theta*Time.deltaTime);
	}
}
