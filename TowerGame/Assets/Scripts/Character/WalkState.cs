using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : ACharacterState
{
    #region Fields
    #endregion Fields

    #region Properties
    #endregion Properties

    #region Methods
    public override void EnterState()
    {
        Debug.Log("Enter Walk State");
        InputManager.Instance.OnJumpPressed += Jump;
        _controller.JumpCount = 0;
    }

    public override void UpdateState()
    {
        _controller.Walk();
        if (InputManager.Instance.MoveDir == Vector3.zero)
            _controller.ChangeState(ECharacterState.IDLE);
    }

    public override void ExitState()
    {
        Debug.Log("Exit Walk State");
        InputManager.Instance.OnJumpPressed -= Jump;
    }

    private void Jump()
    {
        _controller.ChangeState(ECharacterState.JUMP);
    }
    #endregion Methods
}
