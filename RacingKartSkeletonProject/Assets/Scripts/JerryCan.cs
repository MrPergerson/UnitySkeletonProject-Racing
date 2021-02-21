using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JerryCan : MonoBehaviour
{
    bool canConsume = true;
    float consumeCoolDown = 10f;
    int jerryCanFuelValue = 10;
    GameObject jerryCanModel;

    private void Awake()
    {
        // Get model
        jerryCanModel = transform.GetChild(0).gameObject;
    }

    public int ConsumeFuel()
    {
        if(canConsume)
        {
            jerryCanModel.SetActive(false);

            StartCoroutine(HideJerryCan()); // start corountine

            return jerryCanFuelValue;
        }
        else
        {
            return 0;
        }
    }

    /* 
     * Corountines are helful for creating timers and asynchronous functions
     */
    IEnumerator HideJerryCan()
    {
        // 1 do nothing

        yield return new WaitForSeconds(consumeCoolDown); // 2 wait for X time

        // 3 reset jerrycan

        canConsume = true;
        jerryCanModel.SetActive(true);


    }
}
