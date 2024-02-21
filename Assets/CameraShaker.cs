using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Vector3 _positionStrength;
    [SerializeField] private Vector3 _rotationStrength;
    [SerializeField] private Transform playerTransform;
    private GameController gameController;

    private void Awake()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    private static event Action Shake;

    public static void Invoke()
    {
        Shake?.Invoke();
    }

    private void OnEnable() => Shake += CameraShake;
    private void OnDisable() => Shake -= CameraShake;
    private void CameraShake()
    {
        _camera.DOComplete();
        
        _camera.DOShakePosition(0.3f, _positionStrength);
        _camera.DOShakeRotation(0.3f, _rotationStrength);
    }

    private void Update()
    {
        if (gameController.nadePos.Count != 0)
        {
            float shakeStrength = Mathf.Max(1 - Vector3.Distance(gameController.nadePos.Peek().position, playerTransform.position) * 17 / 800, 0);
            gameController.explosionSFX.volume = shakeStrength;
            _positionStrength = new Vector3(shakeStrength, shakeStrength, shakeStrength);
        }
    }
}
