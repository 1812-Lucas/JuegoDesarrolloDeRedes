using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger_Component : MonoBehaviour
{
    public UnityEvent TriggerButton;

    bool PlAuth = false;


    private void OnTriggerStay(Collider other)
    {
        var Pscript = other.GetComponent<Player_Test>();
        if(Pscript != null && Input.GetKeyDown(KeyCode.E) && PlAuth == false)
        {
            TriggerButton.Invoke();
            PlAuth = true;
        }
    }
}
