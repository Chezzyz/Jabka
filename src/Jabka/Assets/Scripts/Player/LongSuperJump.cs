using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSuperJump : BaseJump, ISuperJump
{
    [SerializeField]
    private AnimationCurve _jumpCurve;
    [SerializeField]
    private float _length;
    [SerializeField]
    private float _height;
    [SerializeField]
    private float _duration;

    private Coroutine _currentJump;

    private void OnEnable()
    {
        PlayerTransformController.Collided += StopCurrentSuperJump;
    }

    public void SuperJump(PlayerTransformController playerTransformController)
    {
        _currentJump = StartCoroutine(JumpCoroutine(playerTransformController, _duration, _height, _length, _jumpCurve));
    }

    public string GetJumpName()
    {
        return "Long";
    }

    private void StopCurrentSuperJump(Collision collision)
    {
        if (IsInJump())
        {
            SetIsInJump(false);
        }
    }

    private void OnDisable()
    {
        PlayerTransformController.Collided -= StopCurrentSuperJump;
    }
}
