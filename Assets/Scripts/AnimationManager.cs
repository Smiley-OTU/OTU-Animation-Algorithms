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

        conditions[(int)Animations.WALK] = Animator.StringToHash("IsWalking");
        conditions[(int)Animations.RUN] = Animator.StringToHash("IsRunning");
        conditions[(int)Animations.JUMP] = Animator.StringToHash("IsJumping");
    }

    public enum Animations
    {
        WALK,
        RUN,
        JUMP,
        IDLE
    }

    public void Change(Animations animation)
    {
        switch(animation)
        {
            case Animations.IDLE:
                animator.SetBool(conditions[(int)Animations.WALK], false);
                animator.SetBool(conditions[(int)Animations.RUN], false);
                break;
            case Animations.WALK:
                animator.SetBool(conditions[(int)Animations.WALK], true);
                animator.SetBool(conditions[(int)Animations.RUN], false);
                break;
            case Animations.RUN:
                animator.SetBool(conditions[(int)Animations.WALK], true);
                animator.SetBool(conditions[(int)Animations.RUN], true);
                break;
            case Animations.JUMP:
                animator.SetTrigger(conditions[(int)Animations.JUMP]);
                break;
        }
    }
}
