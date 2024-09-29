using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelController : NetworkBehaviour
{
  [SerializeField] private NetworkMecanimAnimator _MecAnim;

    public override void Spawned()
    {
        _MecAnim = GetComponent<NetworkMecanimAnimator>();
        var PlayerScript = GetComponentInParent<Player_Test>();

        PlayerScript.OnMovementEvent += MoveAnimation;
    }


    private void MoveAnimation(float Xaxis)
    {
        _MecAnim.Animator.SetFloat("axi", Mathf.Abs(Xaxis));
    }
}
