﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemScript : MonoBehaviour {
    public bool isTurnedOn;
    public float activationDuration = 8f;
    public float cooldown = 0f;

    public AudioClip soundEffect1;
    public AudioClip soundEffect2;
    public AudioClip soundEffect3;
    public AudioClip soundEffect4;
    public AudioClip soundEffect5;


    private List<AudioClip> effects;
    private List<AudioClip> Effects 
    {
        get
        {
            if (effects == null)
            {
                effects = new List<AudioClip>();

                if (soundEffect1 != null)
                    effects.Add(soundEffect1);
                if (soundEffect2 != null)
                    effects.Add(soundEffect2);
                if (soundEffect3 != null)
                    effects.Add(soundEffect3);
                if (soundEffect4 != null)
                    effects.Add(soundEffect4);
                if (soundEffect5 != null)
                    effects.Add(soundEffect5);
            }

            return effects;
        }
    }

    private bool askingActivation;
    private float lastActivation = float.MinValue;

	// Use this for initialization
	void Start () {
        var effect = GetComponent<AudioSource>();

        if (Effects.Count == 0)
        {
            effect.volume = 0;
        }
        else
        {
            effect.volume = 0.5f;
            effect.Stop();
            effect.loop = false;
        }
	}
	
	// Update is called once per frame
    void Update () {
        var effect = GetComponent<AudioSource>();
        var frameTime = Time.time;
        var animator = GetComponent<Animator> ();
        var zoneAnim = transform.Find("Zone").Find("ZoneAnim");
        var childAnimator = zoneAnim.GetComponent<Animator>();

        if (frameTime - lastActivation > activationDuration)
        {
            effect.volume = 0;
            
            isTurnedOn = false;
        }

        if (askingActivation && ((frameTime - lastActivation) > activationDuration))
        {
            lastActivation = frameTime;
            isTurnedOn = true;

            if (Effects.Count > 0)
            {
                effect.clip = Effects[Random.Range(0, Effects.Count - 1)];
                effect.Play(); 
            }

            if (Effects.Count == 0)
                effect.volume = 1;
            else
                effect.volume = 0.5f;
        }

        if (GameObject.Find("GameMaster").GetComponent<GameHandler>().isDebugMode)
            askingActivation = true;
        else
            askingActivation = false;

        animator.SetBool ("Activate", isTurnedOn);
        childAnimator.SetBool ("Activate", isTurnedOn);
    }

    public void AskActivation()
    {
        askingActivation = true;
    }
}
