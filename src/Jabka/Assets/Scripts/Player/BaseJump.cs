using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseJump : MonoBehaviour
{
    private bool _isInJump;

    protected Coroutine _currentJump;

    public bool IsInJump()
    {
        return _isInJump;
    }

    public void SetIsInJump(bool value)
    {
        _isInJump = value;
    }

    protected virtual void OnEnable()
    {
        PlayerTransformController.Collided += OnCollision;
    }

    protected virtual IEnumerator JumpCoroutine(
        PlayerTransformController playerTransformController,
        float duration,
        float height,
        float length,
        AnimationCurve curve,
        int layerToMask = PlayerTransformController.playerLayerMask)
    {
        SetIsInJump(true);

        var wait = new WaitForFixedUpdate();

        float expiredTime = 0.0f;

        Vector3 originPosition = playerTransformController.GetPosition();
        Vector3 originDirection = playerTransformController.GetForwardDirection();
        Vector3 colliderSize = playerTransformController.GetBoxColliderSize();

        float progress = 0;
        float maxProgress = curve.keys[curve.keys.Length - 1].time;

        while (IsInJump() && (progress < maxProgress))
        {
            expiredTime += Time.deltaTime;
            //����� �������� ������ �������, ������ ���������� �������, ��� ���������
            progress = expiredTime / duration;

            float nextHeight = height * curve.Evaluate(progress);
            float nextLength = length * progress;
            Vector3 nextPosition = originPosition + new Vector3((originDirection * nextLength).x, nextHeight, (originDirection * nextLength).z);

            if (!IsCollideWithSomething(nextPosition, colliderSize, playerTransformController.GetQuaternion(), layerToMask))
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

    public static bool IsCollideWithSomething(Vector3 pos, Vector3 size, Quaternion rot, params int[] layersToMask)
    {
        //��������� �� �������� ��� ������������ ����
        int layerMask = ~layersToMask.Select(layer => (1 << layer)).Sum();
        return Physics.CheckBox(pos, size / 2, rot, layerMask);
    }

    protected virtual void OnCollision(Collision collision, PlayerTransformController playerTransformController)
    {
        if (IsInJump())
        {
            SetIsInJump(false);
        }
    }

    protected virtual void OnDisable()
    {
        PlayerTransformController.Collided -= OnCollision;
    }
}
