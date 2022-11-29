using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BreadScript : MonoBehaviour
{
    private GameObject gameMgr;
    private int plateNumber;
    private GameManager gameMgrScript;
    
    //private readonly int timeToToast = 3;
    //private readonly int pointScore = 1;

    private int toastImprover = 1; //if you put this slice in a better toaster, this will go up.
    private bool isToasted = false;
    public int PlateNumber { 
        get { return plateNumber; }
        set { plateNumber = value; }
    }

    protected abstract int ToastTime();//gives back the time to toast this object.
    protected abstract int PointScore(); //returns the score for this object.

    // Start is called before the first frame update
    void Start()
    {
        gameMgr = GameObject.Find("Game Manager");
        gameMgrScript = gameMgr.GetComponent<GameManager>();
    }

    private void OnMouseDown()
    {
        if (!isToasted && gameMgrScript.isGameActive)
        {
            //check to see if toaster is available.
            if (gameMgrScript.ToasterScript.IsToasting == false)
            {
                isToasted = true;
                transform.SetPositionAndRotation(new Vector3(0f, 1.5f, 0f),
                                                 Quaternion.Euler(90, 0, 90)
                                                 );
                gameMgrScript.ReleasePlate(plateNumber);
                toastImprover = gameMgrScript.ToasterScript.StartToasting(ToastTime());
                Invoke(nameof(FinishMe), ToastTime() / toastImprover);
                //GetComponent<MeshRenderer>().material =

            }
            else { Debug.Log("Toaster is Busy!"); }
        } else { Debug.Log("Already Toasted!");}
    }

    private void FinishMe()
    {
        if (gameMgrScript.isGameActive)
        {
            float x = 1.5f + Random.Range(0f, 1f) + (Random.Range(0f, 100f) / 100);
            float y = 0.4f + (Random.Range(0f, 100f) / 100);
            float z = -0.29f + Random.Range(0f, 3f) + (Random.Range(0f, 100f) / 100);

            GetComponent<Renderer>().material = gameMgrScript.ToastedMaterial;
            transform.SetLocalPositionAndRotation(new Vector3(x, y, z),
                                                    Quaternion.Euler(0, 0, 0));
            gameMgrScript.ScorePoints(PointScore());
}
    }

}
