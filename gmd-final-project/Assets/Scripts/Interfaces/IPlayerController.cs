using UnityEngine;

public interface IPlayerController
{
    Transform PlayerTransform { get; }
    Rigidbody2D PlayerRigidbody { get; }
    void ApplyPushBack(Vector2 pushDirection, float pushForce);
}

