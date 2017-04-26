using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBody : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionEnter(Collision collision)
    {
        transform.parent.GetComponent<HeroController>().OnCollisionEnter(collision);
    }

    public void OnCollisionExit(Collision collision)
    {
        transform.parent.GetComponent<HeroController>().OnCollisionExit(collision);
    }
}
