using System;
using UnityEngine;

namespace Player.Jumps
{
    public class HighSuperJump : BaseJump, ISuperJump
    {
        [SerializeField]
        private ScriptableJumpData _jumpData;
        
        public static event Action<float> HighJumpStarted;
        
        public override ScriptableJumpData GetJumpData()
        {
            return _jumpData;
        }

        public void SuperJump(PlayerTransformController playerTransformController)
        {
            _currentJump = StartCoroutine(JumpCoroutine(playerTransformController, _jumpData.GetJumpData()));

            HighJumpStarted?.Invoke(_jumpData.GetJumpData().Duration);
        }

        public string GetJumpName()
        {
            return "High";
        }
    }
}