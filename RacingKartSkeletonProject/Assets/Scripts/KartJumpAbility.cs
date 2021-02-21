using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartJumpAbility : MonoBehaviour
{

    [Header("Parameters")]
    public float jumpForce = 100f;

    private KartController kartController;

    private void Awake()
    {
        kartController = GetComponent<KartController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump") && kartController.isGrounded)
        {
            var sphere = kartController.sphere;
            var kartModel = kartController.kartModel;

            sphere.AddForce(kartModel.transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}
