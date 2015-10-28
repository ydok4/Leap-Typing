/**
*Filename ResevoirScript.cs
*Description Holds the variables used in main menus
*/
using UnityEngine;
using System.Collections;

public class ReservoirScript : MonoBehaviour {

	public bool BCMode, timeOFF, capOFF, spawnOFF;
	public int row, mode;
	public static float inputtedTime, inputtedSpawn;
    public static bool nameEntered;

	// Use this for initialization
	void Start () {
        //Note: this line will cause errors, its fine ignore it.
        //nameEntered = GameObject.Find("Reservoir").GetComponent<ReservoirScript>().nameEntered;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
