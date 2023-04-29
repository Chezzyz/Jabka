using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

//Класс хардкодим так как мы точно знаем когда нам надо разблокировать прыжки
public class SuperJumpUnlocker : MonoBehaviour
{
    [SerializeField]
    private SuperJumpButton _longSuperJump;
    [SerializeField]
    private SuperJumpButton _dashSuperJump;
    [SerializeField]
    private SuperJumpButton _highSuperJump;
    [SerializeField]
    private SuperJumpPicker _superJumpPicker;

    public static event System.Action<ISuperJump> SuperJumpUnlocked;
    public static event System.Action SuperJumpsUnlocked;

    private void OnEnable()
    {
        SuperJumpCollectable.SuperJumpCollected += OnSuperJumpCollected;
    }

    private void Start()
    {
        int stage = SceneStatus.GetCurrentMetaData().GetStageNumber();
        int level = SceneStatus.GetCurrentMetaData().GetLevelNumber();
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
        if ((stage == 2 && level > 9) || stage > 2)
        {
            UnlockHighSuperJump();
        }
        SuperJumpsUnlocked?.Invoke();
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
            case "High":
                UnlockHighSuperJump();
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
    
    private void UnlockHighSuperJump()
    {
        SuperJumpUnlocked?.Invoke(_highSuperJump.GetComponent<ISuperJump>());
    }

    private void OnDisable()
    {
        SuperJumpCollectable.SuperJumpCollected -= OnSuperJumpCollected;
    }
}
