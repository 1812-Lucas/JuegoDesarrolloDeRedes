using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private GameObject _NetworkPlayerPref;
    [Networked, Capacity(12)] private NetworkDictionary<PlayerRef, Player_Test> Players => default; 

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
        if(Players.TryGet(player,out Player_Test playerComp))
        {
            Players.Remove(player);
            Runner.Despawn(playerComp.Object);
        }
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
