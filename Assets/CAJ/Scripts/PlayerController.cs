using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float moveH, moveV;
    [SerializeField] private float moveSpeed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        moveH = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveV = Input.GetAxisRaw("Vertical") * moveSpeed;

        rb.velocity = new Vector2(moveH, moveV);
        //rb.velocity = new Vector3(moveH, 0, moveV);

        Vector3 direction = new Vector2(moveH, moveV);

    }
    

}