using Platforms.Interactable;
using UnityEngine;

namespace Levels.Quests.Special
{
    [CreateAssetMenu(fileName = "UseTeleportQuest", menuName = "ScriptableObjects/Quest/UseTeleportQuest", order = 5)]
    public class UseTeleportQuest : BaseQuest
    {
        protected override void OnSceneLoaded(string sceneName)
        {
            base.OnSceneLoaded(sceneName);

            TeleportPlatform.TeleportStarted -= OnTeleportStarted;
            TeleportPlatform.TeleportStarted += OnTeleportStarted;
        }

        private void OnTeleportStarted()
        {
            if (SceneStatus.GetCurrentLevelNumber() == GetLevelNumber())
            {
                ReadyForComplete();
            }
        }
        
        protected override void OnLevelCompleted(CompletePlace completePlace)
        {
            if (SceneStatus.GetCurrentLevelNumber() == GetLevelNumber())
            {
                if (_isReadyForComplete)
                {
                    Complete();
                }
            }
        }
    }
}