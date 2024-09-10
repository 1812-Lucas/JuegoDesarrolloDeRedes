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
    [SerializeField] float _Speed, _MaxSpeed, _JumpStrenght;
    float _SpeedModifier = 1; // internal speed modifier => if 1, then move at walk speed, if > 1 move at sprint speed
    [SerializeField] float _SprintSpeed; // static speed modifier, set this to modify sprint speed

    [SerializeField] private Vector2 _MoveDirection;
    private bool _grounded;
    [SerializeField] bool _Running;
    public void Grounded(bool IsGrounded)
    {
        _grounded = IsGrounded;
    }

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
        if (GetInput(out NetInput input))
        {
            _MoveDirection = input.InputDirection;
            if(input.Buttons.WasPressed(PreviousButtons,InputButton.Sprint)) 
            {
                _Running = true;
                _SpeedModifier = _SprintSpeed;
            }
            else if(input.Buttons.WasReleased(PreviousButtons,InputButton.Sprint))
            {
                _Running = false;
                _SpeedModifier = 1; // walking speed
            }
            MovementLogic();
            RotateObject();
            if (input.Buttons.WasPressed(PreviousButtons, InputButton.jump) && _grounded)
            {
                JumpLogic();
            }
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
        _TargetVelocity *= _Speed * _SpeedModifier;


        //alinear direccion
        _TargetVelocity = transform.TransformDirection(_TargetVelocity);

        //calcular la fuerza de movimiento
        Vector3 _VelocityChange = (_TargetVelocity - _CurrentVel);
        _VelocityChange = new Vector3(_VelocityChange.x, 0, _VelocityChange.z);

        //clamp fuerza
        Vector3.ClampMagnitude(_VelocityChange, _MaxSpeed * _SpeedModifier);

        _NetRb.AddForce(_VelocityChange, ForceMode.VelocityChange);

    }

    private void JumpLogic()
    {
        Vector3 JumpForce = Vector3.up * _JumpStrenght;

        _NetRb.AddForce(JumpForce, ForceMode.VelocityChange);
    }

}
