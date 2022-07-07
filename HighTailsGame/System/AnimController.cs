using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{

    public Animator animator;
    SpriteRenderer spriteRen;

    // Use this for initialization
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        spriteRen = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setIdle() //Default State
    {
        if (animator == null)
        {
            animator = this.gameObject.GetComponent<Animator>();
            spriteRen = this.gameObject.GetComponent<SpriteRenderer>();
        }
        animator.SetBool("RunFront", false);
        animator.SetBool("RunBack", false);
        animator.SetTrigger("Idle");
    }

    public void faceRight() //Sets Sprite  To Face Right
    {
        spriteRen.flipX = false;
    }

    public void faceLeft() //Sets Sprite To Face Left
    {
        spriteRen.flipX = true;
    }

    public void setAttacking() //Attacks
    {
        animator.SetTrigger("Attacking");
    }

    public void setHit() //Get Hit
    {
        animator.SetTrigger("Hit");
    }

    public void runFront() //Run Forward
    {
        animator.SetBool("RunBack", false);
        animator.SetBool("RunFront", true);

    }

    public void runBack() // Run Backward
    {
        animator.SetBool("RunFront", false);
        animator.SetBool("RunBack", true);

    }
}
