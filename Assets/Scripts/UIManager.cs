using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [Header("Cursor")]
    [SerializeField]
    private Texture2D cursor;

    [Header("Planets")]
    [SerializeField]
    private GameObject smallPlanetPrefab;
    [SerializeField]
    private GameObject mediumPlanetPrefab;
    [SerializeField]
    private GameObject largePlanetPrefab;

    private GameObject currentPlanet;

	// Use this for initialization
	void Start () {
        Cursor.SetCursor(cursor, Vector3.zero, CursorMode.Auto);

        MediumPlanet_Click();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LargePlanet_Click()
    {
        if (currentPlanet != null)
        {
            Destroy(currentPlanet);
        }

        currentPlanet = Instantiate(largePlanetPrefab, Vector3.zero, Quaternion.identity);
    }

    public void MediumPlanet_Click()
    {
        if (currentPlanet != null)
        {
            Destroy(currentPlanet);
        }

        currentPlanet = Instantiate(mediumPlanetPrefab, Vector3.zero, Quaternion.identity);
    }

    public void SmallPlanet_Click()
    {
        if (currentPlanet != null)
        {
            Destroy(currentPlanet);
        }

        currentPlanet = Instantiate(smallPlanetPrefab, Vector3.zero, Quaternion.identity);
    }
}
