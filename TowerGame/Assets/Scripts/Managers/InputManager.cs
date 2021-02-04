using DeligoEngine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    #region Fields
    private Vector3 _moveDir = Vector3.zero;
    #endregion Fields

    #region Properties
    public Vector3 MoveDir => _moveDir;
    #endregion Properties

    #region Events
    private event Action _onJumpPressed = null;
    public event Action OnJumpPressed
    {
        add
        {
            _onJumpPressed -= value;
            _onJumpPressed += value;
        }
        remove
        {
            _onJumpPressed -= value;
        }
    }
    #endregion Events

    #region Methods
    public void Initialize()
    {

    }

    protected override void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (_onJumpPressed != null)
                _onJumpPressed();
        }

        _moveDir.x = Input.GetAxis("Horizontal");
        //_moveDir.y = Input.GetAxis("Vertical");
    }
    #endregion Methods
}
