using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObstacle : HardObstacle
{
    //VARIABLES
    public bool interacted = false;
    public Animator animCon;
    public ParticleSystem closer;
    //===================================================
    //VARIABLE FUNCTIONS
    protected override void HitPlayer()
    {
        if (!interacted)
        {
            base.HitPlayer();
        }
    }
    //protected override void EndHit()
    //{
    //    InfiniteScroll.Instance.SetScrollSpeed(InfiniteScroll.Instance.scrollSpeed);
    //}
    //===================================================
    //ANIMATION FUNCTIONS
    public void Anim()
    {
        interacted = true;
        animCon.SetBool("Hit", true);
        GameManager.Instance.fixInteract();
        closer.Play();
    }
    public void UnAnim()
    {

        interacted = false;
        animCon.SetBool("Hit", false);
    }
}
