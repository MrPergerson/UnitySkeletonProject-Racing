using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapScore : MonoBehaviour
{
    [SerializeField] int numberOfLaps = 1;
    [SerializeField] int currentLap = -1;
    [SerializeField] Text lapText;


    private void Awake()
    {
        if (lapText == null) Debug.LogError("LapScore is missing a reference to lapText. Goto Canvas -> LapCount -> [Lap Text]. " +
            "Drag \"Lap Text\" GameObject into lapText slot in LapScore inspector");

    }

    /* On the first frame of impact
     * 
     * Look at the Box collider component, you see that Trigger is checked
     * Trigger means that kart can move through it. 
     * 
     */
    private void OnTriggerEnter(Collider other)
    {
        // If a kart passses through, add to the lap count
        if(other.gameObject.tag == "Kart")
        {
            currentLap++;
            lapText.text = currentLap.ToString();
        }
    }

    /*
     * Try unchecking Trigger. The kart will hit the collider like a wall.
     * 
     * This is function isn't useful lol. But it's here for you to see the 
     * difference between Trigger and Collision
     * 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Kart")
        {
            currentLap++;
            lapText.text = currentLap.ToString();
        }
    }
    */

}
