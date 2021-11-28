using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSuperJump : MonoBehaviour, ISuperJump
{
    [SerializeField]
    private AnimationCurve _jumpCurve;
    [SerializeField]
    private float _length;
    [SerializeField]
    private float _height;
    [SerializeField]
    private float _duration;

    private bool _isInJump;

    private void OnCollisionEnter(Collision collision)
    {
        if (IsInJump())
        {
           SetIsInJump(false);
        }
    }

    public bool IsInJump()
    {
        return _isInJump;
    }

    public string GetJumpName()
    {
        return "Long";
    }

    private void SetIsInJump(bool value)
    {
        _isInJump = value;
    }

    public void SuperJump(PlayerTransformController playerTransformController)
    {
        Coroutine jump = StartCoroutine(SuperJumpUpdate(playerTransformController, _length, _height, _duration, _jumpCurve));
    }

    private IEnumerator SuperJumpUpdate(PlayerTransformController playerTransformController, float length, float height, float duration, AnimationCurve curve)
    {
        SetIsInJump(true);

        float expiredTime = 0.0f;

        float progress = expiredTime / duration;

        Vector3 originPosition = playerTransformController.GetPosition();
        Vector3 originDirection = playerTransformController.GetForwardDirection();

        while (progress < 1 && IsInJump())
        {
            yield return new WaitForFixedUpdate();
            expiredTime += Time.deltaTime;
            progress = Mathf.Clamp01(expiredTime / duration);

            float currentHeight = height * curve.Evaluate(progress);
            float currentLength = length * progress;

            playerTransformController.SetPosition(originPosition + new Vector3((originDirection * currentLength).x, currentHeight, (originDirection * currentLength).z));
        }

        SetIsInJump(false);
    }
}
