using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariablesTest : NetworkBehaviour
{
    [Networked] public float TestVariable { get => default; private set { } }
    [Networked] public PlayerRef Worker { get => default; private set { } }
    [Networked] public bool IsBeingWorkedOn { get => default; private set { } }

    private void OnTriggerEnter(Collider other)
    {
        var Pscript = other.GetComponent<Player_Test>();
        if(Pscript != null)
        {
            print(other.gameObject.name);
            var PlayerMachine = Pscript.Object.StateAuthority;
            if(PlayerMachine != null && IsBeingWorkedOn == false)
            {
                Rpc_Lockworker(PlayerMachine);
            }
        }

    }

    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    private void Rpc_Lockworker([RpcTarget] PlayerRef PlayerWorking)
    {
        if (HasStateAuthority)
        {
            print("Player: " + PlayerWorking.PlayerId + "Is working the machinery");
            Worker = PlayerWorking;
            IsBeingWorkedOn = true;
            TestVariable++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var OtherComp = other.GetComponent<Player_Test>();
        if (OtherComp != null)
        {
            print(other.gameObject.name + " Has left trigger");
            if(OtherComp.Object.StateAuthority == Worker)
            {
                Rpc_UnlockWorker();
            }
        }
    }

    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    private void Rpc_UnlockWorker()
    {
        if(HasStateAuthority)
        {
            Worker = default;
            IsBeingWorkedOn = false;
            print("Resetting Work load");
        }
    }
}
