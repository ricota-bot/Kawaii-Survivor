using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform _targetTransform;

    [SerializeField] private Vector2 minMaxXY; // Values to Clampo the position of Camera


    private void LateUpdate()
    {
        CameraFollowTarget();
    }

    private void CameraFollowTarget()
    {
        if (_targetTransform == null)
        {
            Debug.LogError("No target has been specified.... 'Komoonnnnnnn''");
            return;
        }

        Vector3 targetPosition = _targetTransform.position;
        targetPosition.z = -10.0f;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -minMaxXY.x, minMaxXY.y);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -minMaxXY.y, minMaxXY.y);

        transform.position = targetPosition;
    }
}
