using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour {
    public PlayerCat cat;
    public Image lifeBar;

    public bool isDebugMode = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        lifeBar.fillAmount = cat.energyStack / 100f;

        if (cat.energyStack >= 100)
        {
            OverallGameConstants.PlayerWin = 1;
            SceneManager.LoadScene ("EndScene");
        }
        if (cat.energyStack <= 0)
        {
            OverallGameConstants.PlayerWin = 2;
            SceneManager.LoadScene ("EndScene");
        }
    }

}
