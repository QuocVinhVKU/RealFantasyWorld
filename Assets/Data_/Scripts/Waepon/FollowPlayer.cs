using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform Target;
    public PlayerMovement Player;
    private Vector3 change;
    public Animator animWeapon;
    public Animator weaponCover;
    
    //public Animator animHammer;
    void Start()
    {
        animWeapon.SetFloat("moveY", -1f);
        weaponCover.SetFloat("moveY", -1f);
        transform.position = Target.position;
    }

    // Update is called once per frame
    void Update()
    {
        change.x = Player.change.x;
        change.y = Player.change.y;
        UpdateAnimation();
    }
    public void UpdateAnimation()
    {
        if (change != Vector3.zero && Player.currentState != PlayerState.attack)
        {

            animWeapon.SetFloat("moveX", change.x);
            animWeapon.SetFloat("moveY", change.y);
            weaponCover.SetFloat("moveX", change.x);
            weaponCover.SetFloat("moveY", change.y);
            //animWeapon.SetBool("attacking", true);
        }
        
        
    }
}
