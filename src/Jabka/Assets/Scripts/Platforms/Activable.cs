using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class Activable : MonoBehaviour
{
    [SerializeField]
    private float _activatingDuration;

    private void OnEnable()
    {
        MoveOnButton.ButtonPressed += OnButtonPressed;    
    }

    private void OnButtonPressed(GenericPlatformMove platform)
    {
        GenericPlatformMove platformParent = GetComponentInParent<GenericPlatformMove>();
        if (platform != null && platformParent == platform)
        {
            GetComponent<MeshRenderer>().material.DOFade(1, _activatingDuration);
            GetComponent<Collider>().enabled = true;
        }
    }

    private void OnDisable()
    {
        MoveOnButton.ButtonPressed -= OnButtonPressed;
    }
}
