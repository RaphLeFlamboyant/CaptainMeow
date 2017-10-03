using UnityEngine;
using System.Collections;

public class MovingObject
{
	public float moveTime = 0.1f;			//Time it will take object to move, in seconds.
	public LayerMask blockingLayer;			//Layer on which collision will be checked.
	
	private Rigidbody2D rb2D;				//The Rigidbody2D component attached to this object.
    private Transform trf;
	private float inverseMoveTime;			//Used to make movement more efficient.
	
    public MovingObject (Rigidbody2D rb, Transform tf, float moveTime)
    {
        rb2D = rb;
        trf = tf;
        this.moveTime = moveTime;

        inverseMoveTime = 1f / moveTime;
    }
	
    private Vector3 Move (Vector2 movement)
	{
        Vector2 start = trf.position;
        Vector2 end = start + movement;

        return DoMovement(movement);			
	}

    private Vector3 DoMovement (Vector3 movementVector)
	{            
        float distance = Vector3.Distance(new Vector3(), movementVector);

        var normNector = (Vector2) (movementVector * 1 / Vector3.Distance(new Vector3(), movementVector));
        var lcMid = Physics2D.Linecast (rb2D.position, rb2D.position + normNector, blockingLayer);
		
        if(distance > float.Epsilon && lcMid.transform == null)
        {
            var movementFrame = movementVector * inverseMoveTime * Time.deltaTime;
            Vector3 newPostion = movementFrame + new Vector3(rb2D.position.x, rb2D.position.y, 0);
			
			rb2D.MovePosition(newPostion);

            return movementFrame;
		}

        return new Vector3();
	}
	
    public Vector3 AttemptMove(float xDir, float yDir)
	{
        var movement = new Vector2(xDir, yDir);
        return Move (movement);

		/*
		//Check if nothing was hit by linecast
		if(hit.transform == null)
			//If nothing was hit, return and don't execute further code.
			return;
		
		//Get a component reference to the component of type T attached to the object that was hit
		T hitComponent = hit.transform.GetComponent <T> ();
		
		//If canMove is false and hitComponent is not equal to null, meaning MovingObject is blocked and has hit something it can interact with.
		if(!canMove && hitComponent != null)
			
			//Call the OnCantMove function and pass it hitComponent as a parameter.
			OnCantMove (hitComponent);
        */
	}
}
