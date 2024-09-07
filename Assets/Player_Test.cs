using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Test : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody _NetRb;
    [SerializeField] private Transform _CameraPos;
    [SerializeField] private CameraFollow _CamHolder;

    [Header("MovementVariables")]
    [SerializeField] float _Speed,_MaxSpeed, _JumpStrenght;
    [SerializeField]private Vector2 _MoveDirection;

    [Networked] private NetworkButtons PreviousButtons { get; set; }

    public override void Spawned()
    {
        _NetRb = GetComponent<Rigidbody>();
        if (HasInputAuthority)
        {
            _CamHolder = FindObjectOfType<CameraFollow>();
            _CamHolder.targetObj = _CameraPos;
        }
    }


    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetInput input))
        {
            _MoveDirection = input.InputDirection;
            MovementLogic();
            RotateObject();
            PreviousButtons = input.Buttons;
        }
    }

    private void RotateObject()
    {
        this.transform.rotation = _CameraPos.transform.rotation;
    }

    private void MovementLogic()
    {
        Vector3 _CurrentVel = _NetRb.velocity;
        Vector3 _TargetVelocity = new Vector3(_MoveDirection.x, 0, _MoveDirection.y);
        _TargetVelocity *= _Speed;

        //alinear direccion
        _TargetVelocity = transform.TransformDirection(_TargetVelocity);

        //calcular la fuerza de movimiento
        Vector3 _VelocityChange = (_TargetVelocity - _CurrentVel);

        //clamp fuerza
        Vector3.ClampMagnitude(_VelocityChange, _MaxSpeed);

        _NetRb.AddForce(_VelocityChange, ForceMode.VelocityChange); 

    }
}
