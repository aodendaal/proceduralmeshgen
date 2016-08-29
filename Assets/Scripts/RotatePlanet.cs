using UnityEngine;
using System.Collections;

public class RotatePlanet : MonoBehaviour {

    public float turnRate = 40.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, turnRate * Time.deltaTime);
	}
}
