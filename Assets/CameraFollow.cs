 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [Header("Variables")]
    [SerializeField] Vector2 MouseSens;
    public Transform targetObj;

    private static CameraFollow _Instance;
    public static CameraFollow Instance

    {
        get => _Instance;
        set 
        {
             if(value == null)
             {
                    _Instance = null;
             }
             else if(_Instance == null)
             {
                _Instance = value;
             }
             else if( _Instance != value)
             {
                Destroy(value);
             }
        }
    }

    private void Awake()
    {
        _Instance = this;
    }
    private void OnDestroy()
    {
        if(_Instance == this)
        {
            _Instance = null;
        }
    }

    float Yrotation;
    float Xrotation;
    private void LateUpdate()
    {
        if (targetObj == null) return;

        this.transform.position = targetObj.transform.position;
        //camera inputs
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector2 LookRotationDelta = new Vector2(mouseX, mouseY);

        Yrotation += mouseX * MouseSens.y;
        Xrotation -= mouseY * MouseSens.x;

        Xrotation = Mathf.Clamp(Xrotation, -90, 90);

        this.transform.rotation = Quaternion.Euler(Xrotation, Yrotation, 0);
        targetObj.rotation = Quaternion.Euler(0, Yrotation, 0);

    }

}
