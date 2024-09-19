using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Base : NetworkBehaviour
{
    [SerializeField] private VariablesTest[] _RupturePoints;
    [SerializeField] private float _DamageTimer;
    [SerializeField] private NetworkRunner _NetRunner;

    [Networked] TickTimer RuptureTimer { get => default; set { } }
    [Networked] public int RuptureID { get => default; set { } }

    private void Awake()
    {
        foreach(var point in _RupturePoints)
        {
            point.gameObject.SetActive(false);
        }
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            ResetRuptureTimer();
        }
    }


    private void ResetRuptureTimer()
    {
        RuptureTimer = TickTimer.CreateFromSeconds(Runner, _DamageTimer);
    }

    public override void FixedUpdateNetwork()
    {
        if(HasStateAuthority == false) return;
        if (RuptureTimer.ExpiredOrNotRunning(Runner))
        {
            RandomizeDMGPoint();
            Rpc_CreateRupture();
            ResetRuptureTimer();
        }
    }

    private void RandomizeDMGPoint()
    {
        RuptureID=Random.Range(0,_RupturePoints.Length);
    }

    [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
    public void Rpc_CreateRupture()
    {
        print("OHNO, WE HIT A ROCK " + RuptureID);
        _RupturePoints[RuptureID].gameObject.SetActive(true);
    }
}
