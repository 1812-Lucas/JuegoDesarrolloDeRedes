using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Room_Base : NetworkBehaviour
{
    [SerializeField] private List<VariablesTest> _RupturePoints = new List<VariablesTest>(20);
    [SerializeField] private float _DamageTimer;
    [SerializeField] private NetworkRunner _NetRunner;

    [Networked] TickTimer RuptureTimer { get => default; set { } }
    [Networked] public int RuptureID { get => default; set { } }

    [Networked] private bool _GameStarted { get => default; set { } }

    [Networked] private float FloodAmount { get => default; set { } }

    public override void Spawned()
    {
        GameManager.Instance.AddRoomToList(this);
    }

    public void InitLogic()
    {
        if (HasStateAuthority)
        {
            ResetRuptureTimer();
            _GameStarted = true;
        }
    }

    private void ResetRuptureTimer()
    {
        RuptureTimer = TickTimer.CreateFromSeconds(Runner, _DamageTimer);
    }

    public override void FixedUpdateNetwork()
    {
        if(HasStateAuthority == false) return;
        if (RuptureTimer.ExpiredOrNotRunning(Runner) && _GameStarted)
        {
            RandomizeDMGPoint();
            Rpc_CreateRupture();
            ResetRuptureTimer();
        }
    }

    private void RandomizeDMGPoint()
    {
        RuptureID=Random.Range(0,_RupturePoints.Count);
    }

    [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
    public void Rpc_CreateRupture()
    {
        if (FloodAmount < 0 && HasStateAuthority)
        {
            FloodAmount = 0;
        }
        print("OHNO, WE HIT A ROCK " + RuptureID);
        _RupturePoints[RuptureID]._InGameRunning = true;
        _RupturePoints[RuptureID].gameObject.SetActive(true);

    }

    public void AddFlood(float WaterAdd)
    {
        if(HasStateAuthority)
        {
            FloodAmount += WaterAdd;
            WaterRise();
        }
    }

    private void WaterRise()
    {
        if (!HasStateAuthority) return;
        if(FloodAmount <= 0)
        {
            FloodAmount = -1;
        }
        WaterLevelScript.Instance.ChangeWaterAmount(FloodAmount);
    }

    public void AddBreakingPoint(VariablesTest NewPoint)
    {
        if (!_RupturePoints.Contains(NewPoint))
        {
            _RupturePoints.Add(NewPoint);
        }
    }
}
