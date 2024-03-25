using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Rigidbody2D rb;
    private bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 move = Vector3.zero;
  
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                move.y += moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                move.y -= moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                move.x -= moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                move.x += moveSpeed * Time.deltaTime;
            }

            transform.position += move;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name.Contains("Walls"))
        {
            canMove = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name.Contains("Walls"))
        {
            canMove = true;
        }
    }

}
