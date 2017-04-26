using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MotionType {
    None,
    RotationR,
    RotationL,
    //Reciprocate,
    CombinationR,
    CombinationL,
}

public class CylinderPlatform : MonoBehaviour {

    public MotionType motionType = MotionType.None;

    public float rotationSpeed = 1;
    public float reciprocateSpeed = 1;

    private Rigidbody rigidbody;

    private float dir_y = 1;
	// Use this for initialization
	void Awake () {
        rigidbody = gameObject.GetComponent<Rigidbody>();
       
    }

    private void Start()
    {
    }

    public void Reset()
    {
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
        rigidbody.velocity = new Vector3(0, 0, 0);
        rigidbody.angularVelocity = new Vector3(0, 0, 0);
    }


	// Update is called once per frame
	void Update () {
        switch (this.motionType) {
            case MotionType.None:
                break;
            case MotionType.RotationR:
                rigidbody.angularVelocity = new Vector3(0,-rotationSpeed,0);
                break;
            case MotionType.RotationL:
                rigidbody.angularVelocity = new Vector3(0, rotationSpeed, 0);
                break;
            //case MotionType.Reciprocate:
            //    if (transform.position.y < -1)
            //    {
            //        dir_y = 1;
            //    }
            //    else if (transform.position.y > 1) {
            //        dir_y = -1;
            //    }
            //    rigidbody.velocity = new Vector3(0, dir_y * reciprocateSpeed, 0);
            //    break;
            case MotionType.CombinationR:
                rigidbody.angularVelocity = new Vector3(0, -rotationSpeed, 0);
                if (transform.position.y < -1)
                {
                    dir_y = 1;
                }
                else if (transform.position.y > 1)
                {
                    dir_y = -1;
                }
                rigidbody.velocity = new Vector3(0, dir_y * reciprocateSpeed, 0);
                break;
            case MotionType.CombinationL:
                rigidbody.angularVelocity = new Vector3(0, rotationSpeed, 0);
                if (transform.position.y < -1)
                {
                    dir_y = 1;
                }
                else if (transform.position.y > 1)
                {
                    dir_y = -1;
                }
                rigidbody.velocity = new Vector3(0, dir_y * reciprocateSpeed, 0);
                break;

            default:
                break;
        }
	}


}
