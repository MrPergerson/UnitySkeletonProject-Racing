using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelTank : MonoBehaviour
{
    
    [SerializeField] float fuelCount = 100;
    [SerializeField] Text ui_fuelNumber;
    [SerializeField] KartController kartController;

    private void Start()
    {
        ui_fuelNumber.text = fuelCount.ToString();
    }

    private void Update()
    {
        if(kartController.isDriving)
        {
            fuelCount -= Time.deltaTime;
            ui_fuelNumber.text = ((int)fuelCount).ToString();

            if (fuelCount < 20)
                kartController.acceleration -= Time.deltaTime * 4;

            if(kartController.acceleration <= 0)
            {
                kartController.acceleration = 0f;
            }

            if (fuelCount <= 0)
            {
                fuelCount = 0f;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "JerryCan")
        {
            fuelCount += other.GetComponent<JerryCan>().ConsumeFuel();
        }
    }
}
