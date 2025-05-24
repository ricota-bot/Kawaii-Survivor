using System;
using System.Collections;
using UnityEngine;

public abstract class DroppableCurrency : MonoBehaviour, ICollectable
{
    protected bool _collected;

    private void OnEnable()
    {
        _collected = false;
    }

    public void Collect(Player player)
    {
        if (_collected) return;
        _collected = true;

        StartCoroutine(MoveTowardsPlayer(player));
    }

    IEnumerator MoveTowardsPlayer(Player player)
    {
        float timer = 0;
        Vector2 initialPosition = transform.position; // Represent Candy position


        while (timer < 1) // Represent 1 seconds
        {
            Vector2 targetPosition = player.GetCenter();
            transform.position = Vector2.Lerp(initialPosition, targetPosition, timer);

            timer += Time.deltaTime;
            yield return null;
        }

        Collected();
    }

    protected abstract void Collected();
}
