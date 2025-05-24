using System;
using UnityEngine;

public class Chest : MonoBehaviour, ICollectable
{
    [Header("Action")]
    public static Action OnCollected;
    public void Collect(Player player)
    {
        // Logica diferente aqui som diferente, particulas.. etc

        OnCollected?.Invoke();
        Destroy(gameObject); // Pode usar pooling, porem n�o vamos ter tantos chest
    }
}
