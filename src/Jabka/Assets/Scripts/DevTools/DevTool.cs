using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTool : MonoBehaviour
{
    [SerializeField]
    private float _timeScale;
    void Start()
    {
        Time.timeScale = _timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
