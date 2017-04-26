using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformType
{
    NormalPlatform = 1,
    PricklyPlatform = 2,
    CylinderPlatform = 3,
};

public class Platform : MonoBehaviour {   

    public PlatformType type;
    public int idx = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
