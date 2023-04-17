using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    public Rigidbody2D myRigidbody;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Animator anim;
    public bool attacked;
    private SpriteRenderer spriteRen;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        spriteRen = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;

    }

    void FixedUpdate()
    {
        checkDistance();
    }

    public virtual void checkDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius 
            && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle 
                || currentState == EnemyState.walk 
                && currentState != EnemyState.stagger)
            {
                Vector3 dir = (target.position - transform.position).normalized;
                
                Vector3 temp = Vector3.MoveTowards(transform.position, 
                                                   target.position, 
                                                   moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);

                myRigidbody.MovePosition(temp);
                if(Vector3.Distance(target.position, transform.position) <= chaseRadius*0.05f)
                {

                    myRigidbody.AddForce(dir*2500f);

                }

                ChangeState(EnemyState.walk);
            }
 
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            Vector3 temp = Vector3.MoveTowards(transform.position, homePos, moveSpeed * Time.deltaTime);

            changeAnim(temp - transform.position);
            myRigidbody.MovePosition(temp);
            ChangeState(EnemyState.walk);
        }
    }
    public void changeAnim(Vector2 direction)
    {
        if (direction.x < 0)
        {
            spriteRen.flipX = true;
        }
        else
        {
            spriteRen.flipX = false;
        }
    }
    public void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;

        }
    }
}
