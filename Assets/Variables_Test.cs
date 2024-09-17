using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariablesTest : NetworkBehaviour
{
    [Networked] public float _TestVariable { get; private set; }
    [SerializeField]Player_Test Worker;

    private void OnTriggerEnter(Collider other)
    {
        Worker  = other.GetComponent<Player_Test>();
        if(Worker!= null)
        {
            if (Worker.HasStateAuthority && this.HasStateAuthority)
            {
                _TestVariable += 1;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other == Worker?.GetComponent<Collider>())
        {
           Worker = null;
        }
    }
}
