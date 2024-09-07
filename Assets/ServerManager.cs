using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private GameObject _NetworkPlayerPref;
   

    public static ServerManager instance;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(_NetworkPlayerPref, Vector3.up, Quaternion.identity, player);
           // NetworkObject PlayerObj = Runner.Spawn(_NetworkPlayerPref,Vector3.up,Quaternion.identity,player);
           // Players.Add(player,PlayerObj.GetComponent<Player_Test>());
        }
    }

    public void PlayerLeft(PlayerRef player)
    {

    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }


}
