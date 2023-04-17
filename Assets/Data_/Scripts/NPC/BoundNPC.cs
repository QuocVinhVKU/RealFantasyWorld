using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundNPC : Interactable
{
    private Vector3 directionVector;
    private Transform myTransform;
    [SerializeField] private float speed;
    private Rigidbody2D myRigidbody2D;
    private Animator anim;
    public Collider2D bounds;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("walking", true);
        myTransform = GetComponent<Transform>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        ChangeDirection();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        Vector3 temp = myTransform.position + directionVector * speed * Time.deltaTime;
        if (bounds.bounds.Contains(temp))
        {
            myRigidbody2D.MovePosition(temp);
        }
        else
        {
            ChangeDirection();
        }
    }
    void ChangeDirection()
    {
        int direction = Random.Range(0, 4);
        switch (direction)
        {
            case 0:
                directionVector = Vector3.right;
                break;
            case 1:
                directionVector = Vector3.up;
                break;
            case 2:
                directionVector = Vector3.left;
                break;
            case 3:
                directionVector = Vector3.down;
                break;
            case 4:
                break;
        }
        UpdateAmination();
    }
    void UpdateAmination()
    {
        anim.SetFloat("moveX", directionVector.x);
        anim.SetFloat("moveY", directionVector.y);
    }
}
