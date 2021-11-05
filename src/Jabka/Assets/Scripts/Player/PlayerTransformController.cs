using UnityEngine;

public class PlayerTransformController : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="rotation"></param>
    public void SetRotation(Vector3 rotation)
    {
        transform.rotation = Quaternion.Euler(rotation);
    }

    public void SetRotationY(float y)
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, y, transform.rotation.z);
    }
}

