using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YOrdered : MonoBehaviour {

	public Transform referenceY;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var r = (referenceY != null) ? referenceY : transform.parent;
        //Debug.Log(transform.parent.position.y);
		var spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sortingOrder = 10000 + Mathf.RoundToInt(9999*(1-(r.position.y/10)));
	}
}
