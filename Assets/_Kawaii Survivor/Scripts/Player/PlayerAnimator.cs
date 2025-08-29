using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Animator animator;
    private Rigidbody2D rig;

    // Check Whenever we move -> Change the animation based in this
    // if magnitude != 0 player is moving other player is in Idle Animation

    private void Awake() => rig = GetComponent<Rigidbody2D>();


    private void FixedUpdate() // Because is Physics
    {
        if (rig.linearVelocity.magnitude < 0.001f)
        {
            animator.Play("Idle");
        }
        else
            animator.Play("Move");
    }
}
