using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

//Класс хардкодим так как мы точно знаем когда нам надо разблокировать прыжки
public class SuperJumpUnlocker : MonoBehaviour
{
    private LevelMetaData _currentLevelMeta;

    [SerializeField]
    private SuperJumpButton _longSuperJump;
    [SerializeField]
    private SuperJumpButton _dashSuperJump;
    [SerializeField]
    private SuperJumpPicker _superJumpPicker;

    public static event System.Action<ISuperJump> SuperJumpUnlocked;

    [Inject]
    public void Construct(LevelMetaData levelMeta)
    {
        _currentLevelMeta = levelMeta;
    }

    private void OnEnable()
    {
        SuperJumpCollectable.SuperJumpCollected += OnSuperJumpCollected;
    }

    private void Start()
    {
        int stage = _currentLevelMeta.GetStageNumber();
        int level = _currentLevelMeta.GetLevelNumber();
        //stage = -1 если сцена тестовая
        if(stage == -1)
        {
            UnlockLongSuperJump();
            UnlockDashSuperJump();
        }
        if ((stage == 1 && level > 2) || stage > 1)
        {
            UnlockLongSuperJump();
        }
        if ((stage == 1 && level > 4) || stage > 1)
        {
            UnlockDashSuperJump();
        }
    }

    private void OnSuperJumpCollected(SuperJumpCollectable jumpCollectable)
    {
        ISuperJump superJump = jumpCollectable.GetSuperJump(); 
        switch (superJump.GetJumpName())
        {
            case "Long":
                UnlockLongSuperJump();
                break;
            case "Dash":
                UnlockDashSuperJump();
                break;
        }
    }

    private void UnlockLongSuperJump()
    {
        _superJumpPicker.gameObject.SetActive(true);
        SuperJumpUnlocked?.Invoke(_longSuperJump.GetComponent<ISuperJump>());
    }

    private void UnlockDashSuperJump()
    {
        SuperJumpUnlocked?.Invoke(_dashSuperJump.GetComponent<ISuperJump>());
    }

    private void OnDisable()
    {
        SuperJumpCollectable.SuperJumpCollected -= OnSuperJumpCollected;
    }
}
