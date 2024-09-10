using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Input_Manager : SimulationBehaviour, INetworkRunnerCallbacks
{
    private NetInput _AccumulatedInputs;
    private bool _ResetInputs;
    private InputType _CurrentInput;
    [SerializeField] Vector2 _MoveDir;

    private enum InputType
    {
        Keyboard,
    }

    public void Update()
    {
        if (_ResetInputs)
        {
            _ResetInputs= false;
            _AccumulatedInputs = default;
        }
 
        if(_CurrentInput == InputType.Keyboard)
        {
            #region KeyboardLogic
            //lock de mouse 
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }

            //no quiero tomar input si el juego no esta loqueado
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                return;
            }

            //obtengo move direction
            NetworkButtons buttons = default;
             _MoveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (_MoveDir != null)
            {
                _AccumulatedInputs.InputDirection += _MoveDir;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                buttons.Set(InputButton.jump, true);
            }
            else
            {
                buttons.Set(InputButton.jump, false);
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                buttons.Set(InputButton.Sprint, true);
            }
            else
            {
                buttons.Set(InputButton.Sprint, false);
            }
            _AccumulatedInputs.Buttons = new NetworkButtons(_AccumulatedInputs.Buttons.Bits | buttons.Bits);
            #endregion
        }
     
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
      
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        _AccumulatedInputs.InputDirection.Normalize();
        input.Set(_AccumulatedInputs);
        _ResetInputs = true;
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if(player == runner.LocalPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }
}
