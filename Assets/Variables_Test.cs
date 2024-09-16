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
            if (Worker.HasStateAuthority)
            {
                _TestVariable += 1;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other == Worker)
        {
            if (Worker.HasStateAuthority)
            {
                Worker = null;
            }
        }
    }
}
