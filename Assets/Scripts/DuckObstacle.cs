using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckObstacle : HardObstacle
{
    //VARIABLES

    //===================================================
    //VARIABLE FUNCTIONS
    protected override void HitPlayer()
    {
        if (!PlayerScript.Instance.dashing)
        {
            base.HitPlayer();
        }
    }
    //protected override void EndHit()
    //{
    //    InfiniteScroll.Instance.SetScrollSpeed(InfiniteScroll.Instance.scrollSpeed);
    //}
    //===================================================
}
