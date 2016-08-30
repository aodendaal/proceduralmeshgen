using UnityEngine;
using System.Collections;

/// <summary>
/// This MonoBehaviour can be added to any GameObject which will make it rotate around the Y-axis
/// </summary>
public class RotateGameObject : MonoBehaviour {

    [SerializeField]
    private float turnRate = 40.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // rotate the the object around the Y-axis
        transform.Rotate(Vector3.up, turnRate * Time.deltaTime);
	}
}
