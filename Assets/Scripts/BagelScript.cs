using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INHERITANCE
public class BagelScript : BreadScript
{
    //POLYMORPHISM
    protected override int PointScore()
    {
        return 10; //this is the lowest score you can get.
    }

    protected override int ToastTime()
    {
        return 7; //this is the lowest time you can have.
    }
}
