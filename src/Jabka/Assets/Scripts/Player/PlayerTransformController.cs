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

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collided?.Invoke(collision, this);
    }

    public void SetRotation(Vector3 rotation)
    {
        _rigidbody.rotation = Quaternion.Euler(rotation);
    }

    public void SetRotationY(float y)
    {
        Vector3 rot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(rot.x, y, rot.z);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetTransformParent(Transform parent)
    {
        transform.parent = parent;
    }

    public Vector3 GetRotation()
    {
        return transform.rotation.eulerAngles;
    }

    public Quaternion GetQuaternion()
    {
        return transform.rotation;
    }

    public Vector3 GetRigidbodyPosition()
    {
        return _rigidbody.position;
    }

    public Vector3 GetTransformPosition()
    {
        return transform.position;
    }

    public Vector3 GetForwardDirection()
    {
        return transform.forward;
    }

    public bool IsOnHorizontalSurface(Vector3 normal)
    {
        //смотрим косинус между векторами нормали контакта коллайдеров и осью Y
        float cosBetweenVectors = Vector3.Dot(normal.normalized, Vector3.up);
        float limitCos = Mathf.Cos(Mathf.Deg2Rad * _degreeToBeGrounded);

        return cosBetweenVectors >= limitCos;
    }

    public void SetIsGrounded(bool value)
    {
        IsGrounded = value;
    }

    public void SetVelocity(Vector3 value)
    {
        _rigidbody.velocity = value;
    }

    public Vector3 GetVelocity()
    {
        return _rigidbody.velocity;
    }

    public Vector3 GetBoxColliderSize()
    {
        return GetComponent<BoxCollider>().size;
    }

    public void SetGravityAffection(bool value)
    {
        _rigidbody.useGravity = value;
    }
}

