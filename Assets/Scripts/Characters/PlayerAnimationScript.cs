using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour {
    public MovableObject player;

    void Start()
    {
    }

    // Update is called once per frame
    void Update () {
        var animator = GetComponent<Animator> ();
        var spr = GetComponent<SpriteRenderer> ();

        var playerMove = player.MovingVector;
        var horMove = Mathf.Abs(playerMove.x) > float.Epsilon;
        var verMove = Mathf.Abs(playerMove.y) > float.Epsilon;

        if (!horMove && !verMove)
        {
            animator.SetBool("isSide", false);
            animator.SetBool("isFront", false);
            animator.SetBool("isBack", false);
        }
        else
        {
            var horAbs = Mathf.Abs(playerMove.x);
            var verAbs = Mathf.Abs(playerMove.y);

            if (verAbs < horAbs)
            {
                animator.SetBool("isSide", true);
                animator.SetBool("isBack", false);
                animator.SetBool("isFront", false);

                if (playerMove.x < 0)
                    spr.flipX = true;
                else
                    spr.flipX = false;                    
            }
            else if (playerMove.y < 0)
            {
                animator.SetBool("isFront", true);
                animator.SetBool("isBack", false);
                animator.SetBool("isSide", false);
            }
            else
            {
                animator.SetBool("isBack", true);
                animator.SetBool("isSide", false);
                animator.SetBool("isFront", false);
            }
        }
    }
}
