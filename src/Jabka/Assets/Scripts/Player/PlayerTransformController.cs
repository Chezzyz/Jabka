using UnityEngine;

public class PlayerTransformController : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="rotation"></param>
    /// 
    public void SetRotation(Vector3 rotation)
    {
        transform.rotation = Quaternion.Euler(rotation);
    }

    public void SetRotationY(float y)
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, y, transform.localEulerAngles.z);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}

