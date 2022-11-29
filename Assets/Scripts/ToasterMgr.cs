using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToasterMgr : MonoBehaviour
{
    private bool isToasting = false;
    private int baseToastSpeed = 1;
    public bool IsToasting
    {
        get { return isToasting; }
    }
    
    public int StartToasting(int seconds_to_toast)
    {
        isToasting = true;
        //Debug.Log("This is the Toaster. I'm toasting for:" + seconds_to_toast);
        StartCoroutine(WaitForToastingProcess(seconds_to_toast));
        return baseToastSpeed;
    }
    IEnumerator WaitForToastingProcess(int toastingTime)
    {
        yield return new WaitForSecondsRealtime(toastingTime / baseToastSpeed);
        //Debug.Log("Done Toasting!");
        isToasting = false;

    }
}
