using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashID : MonoBehaviour {

    //Animator states
    public int dyingState;
    public int walkingState;
    public int attackingState;
    public int idleState;

    //Animator parameters
    public int isDeadBool;
    public int isWalkingBool;
    public int isAttackingBool;
    public int isIdleBool;

    private void Awake()
    {
        //states
        dyingState = Animator.StringToHash("Base Layer.Dead");
        walkingState = Animator.StringToHash("Base Layer.Walk");
        attackingState = Animator.StringToHash("Base Layer.Attack");
        idleState = Animator.StringToHash("Base Layer.Wait");

        //parameters
        isDeadBool = Animator.StringToHash("isDead");
        isWalkingBool = Animator.StringToHash("isWalking");
        isAttackingBool = Animator.StringToHash("isAttacking");
        isIdleBool = Animator.StringToHash("isIdle");
    }
}
