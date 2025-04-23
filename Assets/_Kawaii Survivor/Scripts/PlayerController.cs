using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private MobileJoystick _mobileJoystick;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed;

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        _rigidbody2D.velocity = _mobileJoystick.GetMoveVector() * _moveSpeed * Time.deltaTime;
    }
}
