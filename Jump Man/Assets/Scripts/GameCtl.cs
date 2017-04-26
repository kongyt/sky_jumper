using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtl : MonoBehaviour {

    public GameObject hero;
    public GameObject firstPlatform;
    public GameObject objs;

    public GameObject startLayer;
    public GameObject gameoverLayer;


    // Use this for initialization
    void Start () {
        startLayer.SetActive(true);

        hero.SetActive(false);
        firstPlatform.SetActive(false);
        objs.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GameStart() {
        hero.SetActive(true);
        firstPlatform.SetActive(true);
        objs.SetActive(true);

        startLayer.SetActive(false);

        hero.GetComponent<HeroController>().Reset();
    }

    public void GamePause() {
        hero.GetComponent<HeroController>().Pause();
    }

    public void GameResume() {
        hero.GetComponent<HeroController>().Resume();
    }

    public void GameRestart() {
        hero.GetComponent<HeroController>().Reset();
        gameoverLayer.SetActive(false);
    }

    public void GameOver() {
        gameoverLayer.SetActive(true);
    }


    public void GameQuit() {
        Application.Quit();
    }
}
