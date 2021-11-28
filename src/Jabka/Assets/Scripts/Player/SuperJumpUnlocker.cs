using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

//����� ��������� ��� ��� �� ����� ����� ����� ��� ���� �������������� ������
public class SuperJumpUnlocker : MonoBehaviour
{
    private LevelMetaData _currentLevelMeta;

    [SerializeField]
    private SuperJumpButton _longSuperJump;
    [SerializeField]
    private SuperJumpButton _dashSuperJump;

    public static event System.Action<ISuperJump> SuperJumpUnlocked;

    [Inject]
    public void Construct(LevelMetaData levelMeta)
    {
        _currentLevelMeta = levelMeta;
    }

    private void Awake()
    {
        int stage = _currentLevelMeta.GetStageNumber();
        int level = _currentLevelMeta.GetLevelNumber();
        //stage = -1 ���� ����� ��������
        if (stage == -1 || (stage == 1 && level >=2) || stage > 1)
        {
            UnlockLongSuperJump();
        }
    }

    private void UnlockLongSuperJump()
    {
        SuperJumpUnlocked?.Invoke(_longSuperJump.GetComponent<ISuperJump>());
    }
}
