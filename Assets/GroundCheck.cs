using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private Player_Test PlayerScript;

    private Collider2D Trigger;

    private void Start()
    {
        PlayerScript = GetComponentInParent<Player_Test>();

        if (!PlayerScript.HasStateAuthority)
        {
            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == PlayerScript.gameObject)
        {
            return;
        }
        else
        {
            PlayerScript.Grounded(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerScript.gameObject)
        {
            return;
        }
        else
        {
            PlayerScript.Grounded(false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == PlayerScript.gameObject)
        {
            return;
        }
        else
        {
            PlayerScript.Grounded(true);
        }
    }


}
