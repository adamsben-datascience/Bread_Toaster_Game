using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//INHERITANCE
public class WheatScript : BreadScript
{
    //POLYMORPHISM
    protected override int PointScore()
    {
        return 3; //this is the lowest score you can get.
    }

    protected override int ToastTime()
    {
        return 5; //this is the lowest time you can have.
    }
}
