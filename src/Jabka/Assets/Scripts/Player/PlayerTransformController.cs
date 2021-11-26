using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class PlayerTransformController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public bool IsGrounded { get; private set; }

    [SerializeField]
    [Tooltip("Угол на который можно отклониться находясь на поверхности")]
    private float _degreeToBeGrounded;

    private Rigidbody _playerRigidbody {
        set { _rigidbody = value; }
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
        _playerRigidbody.rotation = Quaternion.Euler(rotation);
    }

    public void SetRotationY(float y)
    {
        Vector3 rot = _playerRigidbody.rotation.eulerAngles;
        _playerRigidbody.MoveRotation(Quaternion.Euler(rot.x, y, rot.z));
    }

    public void SetPosition(Vector3 position)
    {
        _playerRigidbody.MovePosition(position);
    }

    public Vector3 GetRotation()
    {
        return _playerRigidbody.rotation.eulerAngles;
    }

    public Vector3 GetPosition()
    {
        return _playerRigidbody.position;
    }

    public Vector3 GetForwardDirection()
    {
        return transform.forward;
    }

    public bool IsOnHorizontalSurface(Collision collision)
    {
        //смотрим косинус между векторами нормали контакта коллайдеров и осью Y
        float cosBetweenVectors = Vector3.Dot(collision.GetContact(0).normal.normalized, Vector3.up);
        float limitCos = Mathf.Cos(Mathf.Deg2Rad * _degreeToBeGrounded);

        return cosBetweenVectors >= limitCos;
    }

    public void SetIsGrounded(bool value)
    {
        IsGrounded = value;
    }

    public void SetVelocity(Vector3 value)
    {
        _playerRigidbody.velocity = value;
    }
}

