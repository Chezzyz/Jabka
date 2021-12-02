using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractJump : MonoBehaviour
{
    private bool _isInJump;

    public bool IsInJump()
    {
        return _isInJump;
    }

    public void SetIsInJump(bool value)
    {
        _isInJump = value;
    }

    virtual protected IEnumerator JumpCoroutine(
        PlayerTransformController playerTransformController,
        float duration,
        float height,
        float length,
        AnimationCurve curve,
        int layerToMask = 3)
    {
        SetIsInJump(true);

        var wait = new WaitForFixedUpdate();

        float expiredTime = 0.0f;

        float progress = expiredTime / duration;

        Vector3 originPosition = playerTransformController.GetPosition();
        Vector3 originDirection = playerTransformController.GetForwardDirection();
        Vector3 colliderSize = playerTransformController.GetBoxColliderSize();

        while (progress < 1 && IsInJump())
        {
            expiredTime += Time.deltaTime;
            progress = Mathf.Clamp01(expiredTime / duration);

            float nextHeight = height * curve.Evaluate(progress);
            float nextLength = length * progress;
            Vector3 nextPosition = originPosition + new Vector3((originDirection * nextLength).x, nextHeight, (originDirection * nextLength).z);

            if (IsCollideWithSomething(nextPosition, colliderSize, playerTransformController.GetQuaternion(), layerToMask) == false)
            {
                playerTransformController.SetPosition(nextPosition);
            }
            else
            {
                break;
            }

            yield return wait;
        }

        SetIsInJump(false);
    }

    private bool IsCollideWithSomething(Vector3 pos, Vector3 size, Quaternion rot, int layerToMask)
    {
        int layerMask = ~(1 << layerToMask);
        return Physics.CheckBox(pos, size / 2, rot, layerMask);
    }
}
