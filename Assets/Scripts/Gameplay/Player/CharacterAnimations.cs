using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using UnityEngine;

public class CharacterAnimations
{
    private Animator animator;
    private ClientNetworkAnimator networkAnimator;

    public CharacterAnimations(Animator animator, ClientNetworkAnimator networkAnimator)
    {
        this.animator = animator;
        this.networkAnimator = networkAnimator;
    }

    [Header("Hashed Animations")]
    private readonly int move = Animator.StringToHash("movement");
    private readonly int death = Animator.StringToHash("death");
    private readonly int attack = Animator.StringToHash("meleeAttack");
    private readonly int getHit = Animator.StringToHash("getHit");
    private readonly int interact = Animator.StringToHash("interact");

    public void Movement(float velocity)
    {
        if (animator == null) return;

        animator.SetFloat(move, velocity);
    }

    public void Death()
    {
        if (animator == null) return;

        animator.SetTrigger(death);
        networkAnimator.SetTrigger(death);
    }

    public void Attack()
    {
        if (animator == null) return;

        animator.SetTrigger(attack);
        networkAnimator.SetTrigger(attack);
    }

    public void GetHit()
    {
        if (animator == null) return;

        animator.SetTrigger(getHit);
        networkAnimator.SetTrigger(getHit);
    }

    public void Interact()
    {
        if (animator == null) return;

        animator.SetTrigger(interact);
        networkAnimator.SetTrigger(interact);
    }
}