using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class PlayerTransformController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public static event System.Action<Collision, PlayerTransformController> Collided;

    public const int playerLayerMask = 3; 

    public bool IsGrounded { get; private set; }

    [SerializeField]
    [Tooltip("Угол на который можно отклониться находясь на поверхности")]
    private float _degreeToBeGrounded;

    private Rigidbody PlayerRigidbody {
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

    private void OnCollisionEnter(Collision collision)
    {
        Collided?.Invoke(collision, this);
    }

    public Rigidbody GetRigidbody()
    {
        return PlayerRigidbody;
    }

    public void SetRotation(Vector3 rotation)
    {
        PlayerRigidbody.rotation = Quaternion.Euler(rotation);
    }

    public void SetRotationY(float y)
    {
        Vector3 rot = PlayerRigidbody.rotation.eulerAngles;
        PlayerRigidbody.MoveRotation(Quaternion.Euler(rot.x, y, rot.z));
    }

    public void SetPosition(Vector3 position)
    {
        PlayerRigidbody.MovePosition(position);
    }

    public Vector3 GetRotation()
    {
        return PlayerRigidbody.rotation.eulerAngles;
    }

    public Quaternion GetQuaternion()
    {
        return PlayerRigidbody.rotation;
    }

    public Vector3 GetPosition()
    {
        return PlayerRigidbody.position;
    }

    public Vector3 GetTransformPosition()
    {
        return transform.position;
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
        PlayerRigidbody.velocity = value;
    }

    public Vector3 GetVelocity()
    {
        return PlayerRigidbody.velocity;
    }

    public Vector3 GetBoxColliderSize()
    {
        return GetComponent<BoxCollider>().size;
    }

    public void SetGravityAffection(bool value)
    {
        PlayerRigidbody.useGravity = value;
    }
}

