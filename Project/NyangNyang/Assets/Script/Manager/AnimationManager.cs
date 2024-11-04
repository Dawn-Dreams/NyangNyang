using UnityEditor.EditorTools;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;

    // 애니메이션 상태 Enum
    public enum AnimationState
    {
        // IdleA부터 시계방향
        None,
        IdleA,
        Alert,
        Victory,
        Block,
        Damage,
        DieA,
        DieB,
        Dash,
        Angry,
        Cute1, //10
        Yes,
        No,
        Hi,
        Tired,
        Move,
        Move_R,
        Move_L,
        Run,
        Run_L,
        Run_R, //20
        Walk,
        Walk_L,
        Walk_R, 
        Sit,
        Sleep,
        Swoon,
        Worship,
        DigA,
        DigB,
        DigC, //30
        Bow,
        Idle02,
        Idle03,
        Idle05,
        IdleB,
        IdleC,
        ATK1,
        ATK2,
        ATK3,
        NormalATK, //40
        Shoot,
        MakeA,
        MakeB,
        Quadruped_Idle,
        Quadruped_Eating,
        Quadruped_Chilling,
        Quadruped_Run,
        Quadruped_Walk,
        Swimming,
        Jump //50
    }


    void Awake()
    {
        // Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();
    }

    // 애니메이션 실행 메서드
    public void PlayAnimation(AnimationState state)
    {
        if (animator != null)
        {
            animator.SetInteger("animation", (int)state);
        }
        else
        {
            Debug.LogWarning("Animator가 설정되지 않았습니다.");
        }
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public void SetIdleAnimAfterAttack()
    {
        if (animator != null)
        {
            animator.SetInteger("animation", (int)AnimationState.IdleA);
        }
    }
}
