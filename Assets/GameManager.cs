using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class GameManager : NetworkBehaviour
{
    #region PantallasVyD
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject winImage;
    [SerializeField]
    private GameObject loseImage;

    private List<PlayerRef> playerList;

    private void Awake()
    {
        Instance = this;

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
}
