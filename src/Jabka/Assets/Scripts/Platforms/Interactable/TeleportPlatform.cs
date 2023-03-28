using System;
using System.Collections;
using UnityEngine;

namespace Platforms.Interactable
{
    public class TeleportPlatform : MonoBehaviour
    {
        [SerializeField] private Transform _destination;
        [SerializeField] private float _heightForUp;
        [SerializeField] private float _teleportDelay;

        public static event Action TeleportStarted;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerTransformController>(out var player))
            {
                StartCoroutine(TeleportPlayer(player, _teleportDelay));
            }
        }

        private IEnumerator TeleportPlayer(PlayerTransformController controller, float delay)
        {
            TeleportStarted?.Invoke();
            yield return new WaitForSeconds(delay);
            Vector3 dest = _destination.transform.position;
            controller.SetPosition(new Vector3(dest.x, dest.y + _heightForUp, dest.z));
        }
    }
}
