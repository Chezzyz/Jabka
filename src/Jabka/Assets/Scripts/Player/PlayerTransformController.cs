using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerTransformController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public Rigidbody PlayerRigidbody {
        private set { _rigidbody = value; }
        get
        {
            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }

            return _rigidbody;
        }
    }

    public void SetRotation(Vector3 rotation)
    {
        PlayerRigidbody.rotation = Quaternion.Euler(rotation);
        //transform.rotation = Quaternion.Euler(rotation);
    }

    public void SetRotationY(float y)
    {
        Vector3 rot = PlayerRigidbody.rotation.eulerAngles;
        PlayerRigidbody.MoveRotation(Quaternion.Euler(rot.x, y, rot.z));
        //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, y, transform.localEulerAngles.z);
    }

    public void SetPosition(Vector3 position)
    {
        PlayerRigidbody.MovePosition(position);
        //transform.position = position;
    }

    public Vector3 GetRotation()
    {
        return PlayerRigidbody.rotation.eulerAngles;
    }
}

