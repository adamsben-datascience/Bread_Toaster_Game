using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagelScript : BreadScript
{
    protected override int PointScore()
    {
        return 10; //this is the lowest score you can get.
    }

    protected override int ToastTime()
    {
        return 7; //this is the lowest time you can have.
    }
}
