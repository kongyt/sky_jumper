using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide : MonoBehaviour {

    public GameObject hero;
    public GameObject[] spheres;
    public float gravity = 9.8f;

    public float scaleH = 0.8f;
    public float scaleV = 2f;

    private bool isShowingGuide = false;
    private HeroController ctrl;
	// Use this for initialization
	void Start () {
        ctrl = hero.GetComponent<HeroController>();
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = hero.transform.position;
        if (ctrl.showGuide)
        {            
            this.calcCurve(ctrl.pos, ctrl.dir, ctrl.vh, ctrl.vv);
            if (isShowingGuide == false)
            {
                isShowingGuide = true;
                for (int i = 0; i < spheres.Length; i++)
                {
                    spheres[i].SetActive(true);
                }
            }
        }
        else {
            if (isShowingGuide) {
                isShowingGuide = false;
                for (int i = 0; i < spheres.Length; i++) {
                    spheres[i].SetActive(false);
                }
            }
        }


	}

    public void calcCurve(Vector3 startPos, Vector3 dir, float vh, float vv) {
        float time = vv / gravity;
        float timeClip = time * 2 / spheres.Length ;
        float dx = time;
        float dy = 0.5f * gravity * time * time;
        dir = dir.normalized * scaleH;
        for (int i = 0; i < spheres.Length; i++) {
            Vector3 v3 = new Vector3();
            v3 = startPos + dir * vh * timeClip * (i + 1);
            v3.y = calcY(timeClip * (i + 1), dx, dy);
            spheres[i].transform.position = v3;
        }
    }

    public float calcY(float x, float dx, float dy) {
       return (-0.5f * gravity * (x - dx) * (x - dx) + dy) * scaleV;
    }

}
