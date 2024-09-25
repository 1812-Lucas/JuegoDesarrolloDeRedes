using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VariablesTest : NetworkBehaviour
{
    [Networked] public PlayerRef Worker { get => default; private set { } }
    [Networked] public bool IsBeingWorkedOn { get => default; private set { } }

    [SerializeField] float _TimeToRepair,_TotalRepairAmount;

    public Room_Base _Room;

    public override void FixedUpdateNetwork()
    {
        if(!HasStateAuthority) return;

        if (IsBeingWorkedOn)
        {
            _TimeToRepair += Runner.DeltaTime;
        }

        if(_TimeToRepair >= _TotalRepairAmount)
        {
            Rpc_RepairTime();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority) return;
        var Pscript = other.GetComponent<Player_Test>();
        if (Pscript != null)
        {
            var PlayerMachine = Pscript.Object.StateAuthority;
            if (PlayerMachine != null && IsBeingWorkedOn == false)
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
            Worker = PlayerWorking;
            print("Player: " + PlayerWorking.PlayerId + "Is working the machinery");
            IsBeingWorkedOn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!HasStateAuthority) return;
        var OtherComp = other.GetComponent<Player_Test>();
        if (OtherComp != null)
        {
            if (OtherComp.Object.StateAuthority == Worker)
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
            IsBeingWorkedOn = false;
            Worker = default;
            print("Resetting Work load");
        }
    }

    [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
    void Rpc_RepairTime()
    {
        LocalDisableObject();
    }

    void LocalDisableObject()
    {
        _TimeToRepair = 0;
        if(HasStateAuthority)
        {
            _Room.AddFlood(-0.1f);
        }
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (!HasStateAuthority) return;
        _Room.AddFlood(0.1f);
    }
}
