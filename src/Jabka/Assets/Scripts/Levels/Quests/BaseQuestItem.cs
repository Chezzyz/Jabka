using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class BaseQuestItem : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<PlayerTransformController>(out _))
        {
            GetComponent<BoxCollider>().enabled = false;
            SendEvent();
        }
    }

    protected abstract void SendEvent();

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
