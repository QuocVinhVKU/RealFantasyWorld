using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    public Vector3 change = new Vector3(0,0,0);
    private Animator animator;

    public float timeAttack_Sycthe;

    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    public VectorValue startingPos;
    public Inventory playerInventory;
    public SpriteRenderer receiveItemSprite;

    public Signal reduceMana;
    public Signal playerHit;
    

    [Header("Weapon Anim")]
    public Animator animSycthe;
    public Animator weaponCover;

    [Header("Bow Anim")]
    public Animator animBow;
    public Animator Bow_Cover;
    public float timeAttack_Bow;

    [Header("projectile stuff")]
    public GameObject playerProjectile;
    public Item bow;

    [Header("Fast Animator")]
    public Animator playerAnim;
    public Animator weponAnim;
    public Animator coverAnim;
    public Animator animBow2;
    public Animator Bow_Cover2;
    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();

        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);

        transform.position = startingPos.initialValue;
        fastAnimator();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack && currentState != PlayerState.stagger && currentState != PlayerState.interact)
        {
            StartCoroutine(weaponSyctheAttackCo());

        }
        else if(Input.GetButtonDown("Second Weapon") && currentState != PlayerState.attack && currentState != PlayerState.stagger && currentState != PlayerState.interact)
        {
            if (playerInventory.CheckItem(bow))
            {
                StartCoroutine(secondWeaponAttackCo());
            }
        }
        
    }
    void FixedUpdate()
    {
        //is the player in an interaction
        if (currentState == PlayerState.interact)
        {
            return;
        }
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        //else
        if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }


    }
    public void PlayerFreeze()
    {
        currentState = PlayerState.interact;
        animator.SetBool("moving", false);
        if (Input.GetButtonDown("interac"))
        {
            currentState = PlayerState.walk;
        }
    }
    private IEnumerator weaponSyctheAttackCo()
    {
        //set attacking àn update animation
        animator.SetBool("attacking", true);
        animSycthe.SetBool("attacking", true);
        weaponCover.SetBool("attacking", true);

        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        animSycthe.SetBool("attacking", false);
        weaponCover.SetBool("attacking", false);

        yield return new WaitForSeconds(timeAttack_Sycthe);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }

    }
    private IEnumerator secondWeaponAttackCo()
    {
        animator.SetBool("bow_attack", true);
        animBow.SetBool("attacking", true);
        Bow_Cover.SetBool("attacking", true);
        MakeArrow();
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("bow_attack", false);
        animBow.SetBool("attacking", false);
        Bow_Cover.SetBool("attacking", false);
        
        yield return new WaitForSeconds(timeAttack_Bow);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
        
    }

    private void MakeArrow()
    {
        if (playerInventory.currentMana > 0)
        {
            Vector3 bowPos = new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z);
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            Arrow arrow = Instantiate(playerProjectile, bowPos, Quaternion.identity).GetComponent<Arrow>();
            arrow.Setup(temp, ChooseArrowDirection());
            playerInventory.ReduceMana(arrow.manaCost);
            reduceMana.Raise();
        }
    }
    Vector3 ChooseArrowDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("moveY"), animator.GetFloat("moveX")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }
    public void RaiseItem()
    {
        if (currentState != PlayerState.interact)
        {
            animator.SetBool("receive item", true);
            currentState = PlayerState.interact;
            receiveItemSprite.sprite = playerInventory.currentItem.itemSprite;
        }
        else
        {
            animator.SetBool("receive item", false);
            currentState = PlayerState.idle;
            receiveItemSprite.sprite = null;
        }
    }
    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            change.x = Mathf.Round(change.x);
            change.y = Mathf.Round(change.y);
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }
    void MoveCharacter()
    {
        change.Normalize();
        myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
    }

    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {
            
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        
    }
    private IEnumerator KnockCo(float knockTime)
    {
        playerHit.Raise();
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }
    private void fastAnimator()
    {
        playerAnim.speed = 1.5f;
        weponAnim.speed = 1.5f;
        coverAnim.speed = 1.5f;
        animBow2.speed = 1.5f;
        Bow_Cover2.speed = 1.5f;
    }
}