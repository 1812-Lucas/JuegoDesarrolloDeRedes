using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevelScript : NetworkBehaviour
{
    [Networked] float _WaterLevel { get => default; set {}}
    [SerializeField]float _RaiseAmount;

    public static WaterLevelScript Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void ChangeWaterAmount(float ValueToChange)
    {
        if (!HasStateAuthority) return;
        _RaiseAmount = ValueToChange;
        _WaterLevel = _RaiseAmount;
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        //if(_RaiseAmount == 0) return;
        Rpc_RunLogic();
    }

    //[Rpc(RpcSources.StateAuthority,RpcTargets.All)]
    private void Rpc_RunLogic()
    {
        transform.position += Vector3.up * (_WaterLevel * Runner.DeltaTime);
        if (this.transform.position.y <= -7)
        {
            this.transform.position = new Vector3(this.transform.position.x,-7,this.transform.position.z);
        }
    }
}
