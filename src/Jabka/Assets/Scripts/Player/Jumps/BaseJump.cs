using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class BaseJump : MonoBehaviour
{
    private bool _isInJump;

    protected Coroutine _currentJump;

    public static event Action JumpEnded;

    public bool IsInJump()
    {
        return _isInJump;
    }

    //shtefan ����� ������� private
    public virtual void SetIsInJump(bool value)
    {
        _isInJump = value;
    }

    protected virtual void OnEnable()
    {
        PlayerTransformController.Collided += OnCollision;
    }

//shtefan ��� ���������, ����� ������ �������
    //shtefan: ����� ������������ JumpData ������ ���� ����������
    protected virtual IEnumerator JumpCoroutine(
        PlayerTransformController playerTransformController,
        float duration,
        float height,
        float length,
        float maxProgress,
        AnimationCurve heightCurve,
        AnimationCurve lengthCurve,
        int layerToMask = PlayerTransformController.playerLayerMask)
    {
        SetIsInJump(true);

        //shtefan: �������������� ����������
        var wait = new WaitForFixedUpdate();
        //shtefan: ����� ����� ������ ���������� ���� ������ var? �� �� �� � c++
        float expiredTime = 0.0f;

        Vector3 originPosition = playerTransformController.GetTransformPosition();
        Vector3 originDirection = playerTransformController.GetForwardDirection();
        Vector3 colliderSize = playerTransformController.GetBoxColliderSize();

        float progress = 0;

        //shtefan �� ����� ������ ��� ��������
        while (IsInJump() && (progress < maxProgress))
        {
            expiredTime += Time.deltaTime;
            
            //����� �������� ������ �������, ������ ���������� �������, ��� ���������
            progress = expiredTime / duration;
            //shtefan: ����� �������� �� var
            float nextHeight = height * heightCurve.Evaluate(progress);
            float nextLength = length * lengthCurve.Evaluate(progress);
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

        JumpEnded?.Invoke();
        SetIsInJump(false);
    }

    public static bool IsCollideWithSomething(Vector3 pos, Vector3 size, Quaternion rot, params int[] layersToMask)
    {
        //��������� �� �������� ��� ������������ ����
        int layerMask = ~layersToMask.Select(layer => (1 << layer)).Sum();
        return Physics.CheckBox(pos, size / 2, rot, layerMask);
    }

    //shtefan ��� ���������, ����� ������ �������
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
