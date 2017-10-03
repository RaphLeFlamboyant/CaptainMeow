using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameScript : MonoBehaviour {
    public GameObject catWin;
    public GameObject guyWin;


	// Use this for initialization
	void Start () {
        if (OverallGameConstants.PlayerWin == 1)
        {
            catWin.SetActive(true);
        }
        else
        {
            guyWin.SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
