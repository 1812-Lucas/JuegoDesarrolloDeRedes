using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CaptainWheelScript : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI _DistanceCounter;

    [Header("Variables")]
    [SerializeField] private float _MaxDistance;
    [Networked,OnChangedRender(nameof(updateDisplay))] private float _DistanceTravelled { get => default; set { } }
    [Networked] public PlayerRef Worker { get => default; private set { } }
    [Networked] public bool IsBeingWorkedOn { get => default; private set { } }
    [SerializeField] private bool GameStarted;
    #region Triggers:
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
    private void Rpc_Lockworker(PlayerRef PlayerWorking)
    {
        if (HasStateAuthority)
        {
            Worker = PlayerWorking;
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
    private void Rpc_UnlockWorker()
    {
        if (HasStateAuthority)
        {
            IsBeingWorkedOn = false;
            Worker = default;
            print("Resetting Work load");
        }
    }
    #endregion

    private void Start()
    {
        GameManager.Instance.Event_InitAllObjects += StartGame;
    }
    public override void Spawned()
    {
        _DistanceTravelled = 0;
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
        if (IsBeingWorkedOn && GameStarted) 
        {
            RunLogic();
        }
    }

    private void RunLogic()
    {
        _DistanceTravelled = _DistanceTravelled + (1 * Runner.DeltaTime);

        if(_DistanceTravelled >= _MaxDistance)
        {
            GameManager.Instance.RPC_Win();
        }
    }
    public void StartGame()
    {
        GameStarted = true;
    }
    private void updateDisplay()
    {
        _DistanceCounter.SetText((int)_DistanceTravelled + " / " + _MaxDistance);
    }
}
