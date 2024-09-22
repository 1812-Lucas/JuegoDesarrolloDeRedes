using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    #region PantallasVyD
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject winImage;
    [SerializeField]
    private GameObject loseImage;
    [SerializeField] private List<Room_Base> _RoomsInShip = new List<Room_Base>();

    public delegate void StartAllObjects();
    public event StartAllObjects Event_InitAllObjects;
    private List<PlayerRef> playerList;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        playerList = new List<PlayerRef>();
    }

    public void AddToList(Player_Test player)
    {
        var playerRef = player.Object.StateAuthority;

        if (playerList.Contains(playerRef)) return;

        playerList.Add(playerRef);
    }
    void RemoveFromList(PlayerRef player)
    {
        playerList.Remove(player);
    }

    public void Win()
    {
        winImage.SetActive(true);
    }

    public void Defeat()
    {
        loseImage.SetActive(true);
    }

    [Rpc]
    public void RPC_Defeat(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Defeat();
        }

        RemoveFromList(player);

        if (playerList.Count == 1 && HasStateAuthority)
        {
            RPC_Win(playerList[0]);
        }
    }


    [Rpc]
    void RPC_Win([RpcTarget] PlayerRef player)
    {
        Win();
    }
    #endregion

    public void StartGame()
    {
        var PlayerCount = Runner.SessionInfo.PlayerCount;
        if(this.HasStateAuthority)
        {
            if(PlayerCount > 0)
            {
                Rpc_StartGame();
            }
        }
        else
        {
            print("MissingPlayers to start game");
        }
    }

    [Rpc]
    private void Rpc_StartGame()
    {
        print("Starting Game");
        foreach(Room_Base _Room in _RoomsInShip)
        {
            _Room.InitLogic();
        }
        Event_InitAllObjects();
    }



    public void AddRoomToList(Room_Base NewRoom)
    {
        _RoomsInShip.Add(NewRoom);
    }
}
