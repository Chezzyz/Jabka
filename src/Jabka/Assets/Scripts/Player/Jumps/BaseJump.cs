using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public abstract class BaseJump : MonoBehaviour
{
    private bool _isInJump;

    protected Coroutine _currentJump;

    public static event Action<BaseJump> JumpEnded;

    public bool IsInJump()
    {
        return _isInJump;
    }

    public abstract ScriptableJumpData GetJumpData();

    protected virtual void SetIsInJump(bool value)
    {
        _isInJump = value;
    }

    protected virtual IEnumerator JumpCoroutine(
        PlayerTransformController playerTransformController,
        JumpData jumpData,
        int layerToMask = PlayerTransformController.playerLayerMask)
    {
        SetIsInJump(true);

        float expiredTime = 0.0f;

        Vector3 originPosition = playerTransformController.GetTransformPosition();
        Vector3 originDirection = playerTransformController.GetForwardDirection();
        Vector3 colliderSize = playerTransformController.GetBoxColliderSize();

        float maxProgress = jumpData.HeightCurve.keys.Last().time;
        float progress = 0;

        while (IsInJump() && progress < maxProgress)
        {
            expiredTime += Time.deltaTime;
            
            //когда прогресс больше единицы, значит происходит падение, все нормально
            progress = expiredTime / jumpData.Duration;
            
            float nextHeight = jumpData.Height * jumpData.HeightCurve.Evaluate(progress);
            float nextLength = jumpData.Length * jumpData.LengthCurve.Evaluate(progress);
            Vector3 nextPosition = originPosition + new Vector3((originDirection * nextLength).x, nextHeight, (originDirection * nextLength).z);

            if (!IsCollideWithSomething(nextPosition, colliderSize, playerTransformController.GetQuaternion(), layerToMask))
            {
                playerTransformController.SetPosition(nextPosition);
            }
            else
            {
                break;
            }
            yield return null;
        }

        JumpEnded?.Invoke(this);
        SetIsInJump(false);
    }

    public static bool IsCollideWithSomething(Vector3 pos, Vector3 size, Quaternion rot, params int[] layersToMask)
    {
        //исключаем из проверки все передаваемые слои
        int layerMask = ~layersToMask.Select(layer => (1 << layer)).Sum();
        return Physics.CheckBox(pos, size / 2, rot, layerMask);
    }
}
