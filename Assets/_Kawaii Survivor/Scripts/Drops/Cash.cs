using System;
using System.Collections;
using UnityEngine;

public class Cash : DroppableCurrency
{
    [Header("Actions")]
    public static Action<Cash> OnDropCollected;
    protected override void Collected()
    {
        // Logica diferente aqui som diferente, particulas.. etc

        OnDropCollected?.Invoke(this);
    }
}
