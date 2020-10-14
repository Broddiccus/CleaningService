using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentHandler : MonoBehaviour
{
    public enum Difficulty { Empty, Easy, Medium, Hard };
    public Difficulty difficulty;
    public List<Dust> dusts;
    public List<InteractableObstacle> interacts;
    public List<HardObstacle> hard;
    public bool transition;
    public bool transitioningPiece;
    public void Refresh()
    {
        foreach(Dust d in dusts)
        {
            d.DustReset();
        }
        foreach(InteractableObstacle i in interacts)
        {
            i.UnAnim();
        }
        foreach(HardObstacle h in hard)
        {
            h.Reset();
        }
    }
}
