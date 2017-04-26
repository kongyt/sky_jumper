using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    List<GameObject> normalPlatforms = new List<GameObject>();
    List<GameObject> pricklyPlatforms = new List<GameObject>();
    List<GameObject> cylinderPlatforms = new List<GameObject>();

    private int maxNormalPlatformCount = 0;
    private int maxPricklyPlatformCount = 0;
    private int maxCylinderPlatformCount = 0;

    private float curPlatformX = 0;
    private float curPlatformZ = 0;
    private float baseDeltaX = 3f;

    private bool lastIsPrickly = true;
    private bool lastIsCylinder = false;
    private bool lastToX = false;
    private bool toX = true;

    public void Reset()
    {
        for (int i = 0; i < 10; i++)
        {
            normalPlatforms[i].SetActive(false);
            normalPlatforms[i].transform.position = new Vector3(0, 0, 0);
        }

        for (int i = 0; i < 3; i++)
        {
            pricklyPlatforms[i].SetActive(false);
            pricklyPlatforms[i].transform.position = new Vector3(0, 0, 0);
        }

        for (int i = 0; i < 4; i++) {
            cylinderPlatforms[i].SetActive(false);
            cylinderPlatforms[i].transform.position = new Vector3(0, 0, 0);
        }

        this.curPlatformX = 0;
        this.curPlatformZ = 0;
        this.maxNormalPlatformCount = 0;
        this.maxPricklyPlatformCount = 0;
        this.maxCylinderPlatformCount = 0;

        this.lastIsCylinder = false;
        this.lastIsPrickly = true;
        this.lastToX = true;
        this.toX = true;

        this.genNormalPlatform(0);
        this.genNormalPlatform(1);
        this.genNormalPlatform(2);
    

    }

    // Use this for initialization
    void Awake () {
        Random.InitState((int)(Time.time*1000));
        for (int i = 0; i < 10; i++) {
            normalPlatforms.Add((GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/NormalPlatform")));
            normalPlatforms[i].transform.parent = gameObject.transform;
        }

        for (int i = 0; i < 3; i++)
        {
            pricklyPlatforms.Add((GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/PricklyPlatform")));
            pricklyPlatforms[i].transform.parent = gameObject.transform;
        }

        for (int i = 0; i < 4; i++)
        {
            cylinderPlatforms.Add((GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/CylinderPlatform")));
            cylinderPlatforms[i].transform.parent = gameObject.transform;
        }
    }

    private void Start() {
        Reset();
    }

    public void genNormalPlatform(int idx) {
        Debug.Log("gen id=" + idx);
        if (idx >= maxNormalPlatformCount + maxCylinderPlatformCount) {
            if (this.lastIsPrickly == true  || this.lastIsCylinder == true || Random.Range(0f, 1f) < 0.8f)
            {

                float d = 0;
                if (lastIsCylinder) {
                    d = baseDeltaX;
                }

                if (lastIsPrickly)
                {
                    toX = lastToX;
                    if (toX)
                    {
                        curPlatformX += baseDeltaX + d;
                    }
                    else
                    {
                        curPlatformZ += baseDeltaX + d;
                    }
                }
                else
                {
                    if (Random.Range(0f, 1f) > 0.5f || idx == 0)
                    {
                        curPlatformX += baseDeltaX + d;
                        toX = true;
                    }
                    else
                    {
                        curPlatformZ += baseDeltaX + d;
                        toX = false;
                    }
                }
                


                Debug.Log("maxNormalPlatformCount=" + maxNormalPlatformCount + " idx=" + maxNormalPlatformCount % normalPlatforms.Count);
                GameObject np = normalPlatforms[maxNormalPlatformCount % normalPlatforms.Count];
                np.SetActive(true);
                np.transform.position = new Vector3(curPlatformX, 0, curPlatformZ);
                np.GetComponent<Platform>().idx = this.maxNormalPlatformCount + this.maxCylinderPlatformCount;
                maxNormalPlatformCount++;
                if (lastIsPrickly == false && lastIsCylinder == false && Random.Range(0f, 1f) < 0.4f && lastToX == toX)
                {
                    lastIsPrickly = true;
                    GameObject pp = pricklyPlatforms[maxPricklyPlatformCount % pricklyPlatforms.Count];
                    pp.SetActive(true);
                    pp.transform.position = new Vector3(curPlatformX, 0, curPlatformZ);
                    maxPricklyPlatformCount++;
                }
                else
                {
                    lastIsPrickly = false;
                }

                lastToX = toX;
                lastIsCylinder = false;
            } else {
                if (lastToX)
                {
                    curPlatformX += 2f * baseDeltaX;
                }
                else
                {
                    curPlatformZ += 2f * baseDeltaX;
                }

                GameObject cp = cylinderPlatforms[maxCylinderPlatformCount % cylinderPlatforms.Count];
                Debug.Log(""+cp);
                cp.SetActive(true);
                cp.transform.position = new Vector3(curPlatformX, 0, curPlatformZ);
                cp.transform.GetChild(0).GetComponent<Platform>().idx = this.maxNormalPlatformCount + this.maxCylinderPlatformCount;

                cp.transform.GetChild(0).GetComponent<CylinderPlatform>().Reset();
                cp.transform.GetChild(0).GetComponent<CylinderPlatform>().motionType = (MotionType)Random.Range(1, 4);
                maxCylinderPlatformCount++;
                Debug.Log("maxCylinderPlatform" + maxCylinderPlatformCount);
                lastIsCylinder = true;
            }         
        }
        
    }

	// Update is called once per frame
	void Update () {
		
	}
}
