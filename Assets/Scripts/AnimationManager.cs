using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    private int[] conditions = new int[3];

    void Start()
    {
        animator = GetComponent<Animator>();

        conditions[(int)CharacterMovement.Animations.WALK] = Animator.StringToHash("IsWalking");
        conditions[(int)CharacterMovement.Animations.RUN] = Animator.StringToHash("IsRunning");
        conditions[(int)CharacterMovement.Animations.JUMP] = Animator.StringToHash("IsJumping");
    }

    public void Change(CharacterMovement.Animations animation)
    {
        switch(animation)
        {
            case CharacterMovement.Animations.IDLE:
                animator.SetBool(conditions[(int)CharacterMovement.Animations.WALK], false);
                animator.SetBool(conditions[(int)CharacterMovement.Animations.RUN], false);
                break;
            case CharacterMovement.Animations.WALK:
                animator.SetBool(conditions[(int)CharacterMovement.Animations.WALK], true);
                animator.SetBool(conditions[(int)CharacterMovement.Animations.RUN], false);
                break;
            case CharacterMovement.Animations.RUN:
                animator.SetBool(conditions[(int)CharacterMovement.Animations.WALK], true);
                animator.SetBool(conditions[(int)CharacterMovement.Animations.RUN], true);
                break;
            case CharacterMovement.Animations.JUMP:
                animator.SetTrigger(conditions[(int)CharacterMovement.Animations.JUMP]);
                break;
        }
    }
}
