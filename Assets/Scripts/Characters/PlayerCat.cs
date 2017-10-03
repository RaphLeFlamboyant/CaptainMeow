using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;

public class PlayerCat : MovableObject
{
	public float humanDisturbedCoef = 5f; // how much the human disturbs compared to other disturbers

    public float moveTime = 0.22f;           //Time it will take object to move, in seconds.
    public float energyStack = 50f;
    public float energyLostPerMoveUnit = 1.2f;
    public float energyLostPerSecDisturbed = 2f;
    public float energyGainPerSec = 6f;
    public bool moving;
    public bool disturbed;
    public LayerMask blockingLayer;         //Layer on which collision will be checked.

    private HashSet<ItemScript> itemsCollided = new HashSet<ItemScript>();
    private PlayerGhost ghost;
	
    private MovingObject movingModule;

	private float roundStart;
	public float regularTime = 20f; // time before sudden death
	public float gameAccelerationCoef = 1.0f;
	
    public void Start ()
	{
        var rb2D = GetComponent <Rigidbody2D> ();

        movingModule = new MovingObject(rb2D, transform, moveTime);
		movingModule.blockingLayer = blockingLayer;

		roundStart = Time.time;
		gameAccelerationCoef = 1.0f;

	}
			
	private void OnDisable ()
	{
	}
			
	private void Update ()
	{
		float horizontal = 0;  	//Used to store the horizontal move direction.
        float vertical = 0;		//Used to store the vertical move direction.
        float energyRatio = 0;

		horizontal = Input.GetAxisRaw ("Horizontal");
		vertical = Input.GetAxisRaw ("Vertical");

        MovingVector = new Vector2(horizontal, vertical);
        moving = horizontal != 0 || vertical != 0;

        if (moving)
        {
            energyRatio = Vector3.Distance(movingModule.AttemptMove(horizontal, vertical), new Vector3 ());
        }
                
        UpdateDisturbed();
        UpdateEnergy(energyRatio);

		var dt = (Time.time - roundStart)/regularTime;
		gameAccelerationCoef = dt < 1.0f ? 1.0f : 1.0f + Mathf.Pow (dt - 1, 2);
	}

    private void UpdateDisturbed()
    {
        disturbed = ghost != null || itemsCollided.Any(i => i.isTurnedOn);
    }

    private void UpdateEnergy (float movementRatio)
    {
		var lostByDisturb = disturbed ? Time.deltaTime * energyLostPerSecDisturbed * (ghost == null ? 1 : humanDisturbedCoef) : 0;
        var lostByMove = movementRatio * energyLostPerMoveUnit;
        var gainByTime = disturbed || lostByMove != 0 ? 0 : Time.deltaTime * energyGainPerSec;

		var energyDelta = -lostByMove -lostByDisturb + gainByTime;
		energyDelta *= gameAccelerationCoef;

        energyStack = Mathf.Max(0, Mathf.Min(100, energyStack + energyDelta));
	}

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "DisturbingZone"){
            var item = coll.gameObject.GetComponentInParent<ItemScript>();

            if (item == null)
            {
                ghost = coll.gameObject.GetComponentInParent<PlayerGhost>();
            }
            else
                itemsCollided.Add(item);
        }
    }

    void  OnTriggerExit2D(Collider2D coll){
        if (coll.gameObject.tag == "DisturbingZone"){
            var item = coll.gameObject.GetComponentInParent<ItemScript>();

            if (item == null)
            {
                ghost = null;
            }
            else
                itemsCollided.Remove(item);
        }
    }
}

