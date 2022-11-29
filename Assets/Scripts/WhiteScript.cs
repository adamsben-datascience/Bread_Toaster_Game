using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteScript : BreadScript
{
    protected override int PointScore()
    {
        return 1; //this is the lowest score you can get.
    }

    protected override int ToastTime()
    {
        return 3; //this is the lowest time you can have.
    }
}
