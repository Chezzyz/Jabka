using System.Collections;
using UnityEngine;
using Zenject;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private bool _isActivable;
    [SerializeField]
    private int _orderNumber;
    [SerializeField]
    private GameObject _model;
    [SerializeField]
    [Tooltip("Высота, на которой надо заспаунить игрока относительно объекта чекпоинта")]
    private float _heightForUp;
    [SerializeField]
    private float _upDuration;

    public static event System.Action<Checkpoint> CheckpointActivated;
    public static event System.Action<PlayerTransformController> PlayerSpawned;

    private PlayerTransformController _playerTransformController;

    private Tween _animationTween;

    [Inject]
    public void Construct(PlayerTransformController playerTransform)
    {
        _playerTransformController = playerTransform;
    }

    public int GetOrderNumber()
    {
        return _orderNumber;
    }

    private void OnEnable()
    {
        CheckpointActivated += OnCheckpointActivated;
    }

    public void SpawnPlayer()
    {
        _playerTransformController.SetPosition(transform.position + new Vector3(0, _heightForUp, 0));
        _playerTransformController.SetRotation(transform.rotation.eulerAngles);
        PlayerSpawned?.Invoke(_playerTransformController);
    }

    public IEnumerator SpawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerTransformController>(out var player) && _isActivable)
        {
            CheckpointActivated?.Invoke(this);
        }
    }

    private void OnCheckpointActivated(Checkpoint checkpoint)
    {
        if(_orderNumber == 1)
        {
            return;
        }

        //Если активирован чекпоинт, который явлется одним из предыдущих, то деактивируем его 
        if(checkpoint.GetOrderNumber() > _orderNumber)
        {
            _isActivable = false;
            CheckpointActivated -= OnCheckpointActivated;
            _animationTween.Kill();
            DownFlag();
        }
        if(checkpoint.GetOrderNumber() == _orderNumber)
        {
            _isActivable = false;
            CheckpointActivated -= OnCheckpointActivated;
            UpFlag();
            StartCoroutine(DoFlagAnimationAfterDelay(_upDuration));
        }
    }

    private void UpFlag()
    {
        _model.transform.DOLocalRotate(new Vector3(10, 0, 0), _upDuration).SetEase(Ease.OutCubic);
    }

    private IEnumerator DoFlagAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _animationTween = _model.transform.DOLocalRotate(new Vector3(-10,0,0), _upDuration).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
    }

    private void DownFlag()
    {
        _model.transform.DOLocalRotate(new Vector3(90, 0 ,0), _upDuration).SetEase(Ease.OutCubic);
    }

    private void OnDisable()
    {
        CheckpointActivated -= OnCheckpointActivated;
    }
}
