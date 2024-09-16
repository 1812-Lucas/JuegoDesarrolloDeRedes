using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthComponent : NetworkBehaviour
{
    [SerializeField] float _Health, _MaxHealth;
    [SerializeField] Player_Test _PT;
    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            _Health = _MaxHealth;
        }
    }

    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    public void Rpc_TakeDamage(float DMG)
    {
        _Health -= DMG / Runner.ActivePlayers.Count();// esto es para evitar duplicar la cantidad de salud perdida 

        if (_Health <= 0)
        {
            Death();
        }
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_HealHealth(float Heal)
    {
        _Health += Heal / Runner.ActivePlayers.Count(); // esto es para evitar duplicar la cantidad de salud ganada
    }

    private void Death()
    {
        Runner.Despawn(_PT._NetworkObject);
    }
}
