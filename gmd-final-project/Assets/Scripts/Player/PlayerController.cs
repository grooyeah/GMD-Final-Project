using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float pushBackDuration = 0.2f;
    [SerializeField] private float pushBackSpeed = 20f;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Vector2 smoothMovementInput;
    private Vector2 movementInputSmoothVelocity;
    private MeleeParent meleeParent;
    private bool isPushedBack;
    private float pushBackTimer;
    private Health playerHealth;
    private Coroutine walkSoundCoroutine;
    private bool isWalking;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        meleeParent = GetComponentInChildren<MeleeParent>();
        playerHealth = GetComponent<Health>();
    }

    private void FixedUpdate()
    {
        if (isPushedBack)
        {
            pushBackTimer -= Time.deltaTime;
            if (pushBackTimer <= 0)
            {
                isPushedBack = false;
            }
        }

        Move();
        RotateDirectionOnInput();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Enemy"))
        {
            playerHealth.Damage(5);
        }
        if(collision.gameObject.tag.Equals("Level Boss"))
        {
            playerHealth.Damage(10);
        }
    }

    private void RotateDirectionOnInput()
    {
        if (movementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, smoothMovementInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            rb.MoveRotation(rotation);
        }
    }

    private void Move()
    {
        smoothMovementInput = Vector2.SmoothDamp(
            smoothMovementInput,
            movementInput,
            ref movementInputSmoothVelocity,
            0.1f
        );

        rb.velocity = isPushedBack ? rb.velocity : smoothMovementInput * moveSpeed;
        HandleWalkingSound();
    }
    private void HandleWalkingSound()
    {
        if (movementInput != Vector2.zero)
        {
            if (walkSoundCoroutine == null)
            {
                walkSoundCoroutine = StartCoroutine(PlayWalkingSound());
            }
        }
        else
        {
            if (walkSoundCoroutine != null)
            {
                StopCoroutine(walkSoundCoroutine);
                walkSoundCoroutine = null;
                SoundManager.Instance.StopWalkSound();
            }
        }
    }

    private IEnumerator PlayWalkingSound()
    {
        SoundManager.Instance.PlayWalkSound();
        while (true)
        {
            yield return new WaitForSeconds(SoundManager.Instance.walkClip.length);
            SoundManager.Instance.PlayWalkSound();
        }
    }

    private void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }

    private void OnSwing(InputValue inputValue)
    {
        meleeParent.Attack();
    }

    public void ApplyPushBack(Vector2 pushDirection, float pushForce)
    {
        SoundManager.Instance.PlayPlayerHitSound();
        isPushedBack = true;
        pushBackTimer = pushBackDuration;
        rb.velocity = pushDirection * pushBackSpeed;
    }
}
