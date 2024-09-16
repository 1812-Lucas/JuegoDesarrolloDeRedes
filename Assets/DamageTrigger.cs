 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField] float DmgDealt;
    [SerializeField] bool IsHealing;

    private void OnTriggerEnter(Collider other)
    {
        var HealthCom = other.GetComponent<HealthComponent>();
        if (HealthCom != null)
        {
            if (!IsHealing)
            {
                HealthCom.Rpc_TakeDamage(DmgDealt);
                print("dealt damage to: " + other.name);
            }
            else
            {
                HealthCom.Rpc_HealHealth(DmgDealt);
                print("Healed hp of: " + other.name);
            }
        }
    }
}
