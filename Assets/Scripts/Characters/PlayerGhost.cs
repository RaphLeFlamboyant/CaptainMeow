using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhost : MovableObject
{
    public float moveTime = 0.1f;           //Time it will take object to move, in seconds.
    public bool moving;
    public LayerMask blockingLayer;         //Layer on which collision will be checked.

    private HashSet<ItemScript> itemsCollided = new HashSet<ItemScript>();

    private MovingObject movingModule;

    public void Start ()
    {
        var rb2D = GetComponent <Rigidbody2D> ();

        movingModule = new MovingObject(rb2D, transform, moveTime);
        movingModule.blockingLayer = blockingLayer;
    }

    private void OnDisable ()
    {
    }

    private void Update ()
    {           
        float horizontal = 0;   //Used to store the horizontal move direction.
        float vertical = 0;     //Used to store the vertical move direction.

        horizontal = Input.GetAxisRaw ("Horizontal2");
        vertical = Input.GetAxisRaw ("Vertical2");

        MovingVector = new Vector2(horizontal, vertical);
        moving = horizontal != 0 || vertical != 0;

        if (moving)
        {
            movingModule.AttemptMove(horizontal, vertical);
        }

        foreach (var item in itemsCollided)
        {
            item.AskActivation();
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "DisturbingActivationZone"){
            var item = coll.gameObject.GetComponentInParent<ItemScript>();
            itemsCollided.Add(item);
        }
    }

    void  OnTriggerExit2D(Collider2D coll){
        if (coll.gameObject.tag == "DisturbingActivationZone"){
            var item = coll.gameObject.GetComponentInParent<ItemScript>();
            itemsCollided.Remove(item);
        }
    }
}
