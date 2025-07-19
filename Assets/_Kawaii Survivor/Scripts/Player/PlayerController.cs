using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour, IPlayerStatsDepedency
{
    [Header("Elements")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private MobileJoystick _mobileJoystick;

    [Header("Settings")]
    [SerializeField] private float _baseMoveSpeed;
    private float _moveSpeed;

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        _rigidbody2D.linearVelocity = _mobileJoystick.GetMoveVector() * _moveSpeed * Time.deltaTime;
    }
    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float moveSpeedPercent = playerStatsManager.GetStatValue(Stat.MoveSpeed) / 100;
        //Debug.Log("MoveSpeed Percent" + moveSpeedPercent);
        _moveSpeed = _baseMoveSpeed * (1 + moveSpeedPercent);
    }
}
