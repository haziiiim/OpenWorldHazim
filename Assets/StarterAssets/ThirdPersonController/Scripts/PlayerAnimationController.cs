using UnityEngine;

namespace StarterAssets
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator _animator;

        // Animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            AssignAnimationIDs();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        public void SetGrounded(bool grounded)
        {
            if (_animator)
            {
                _animator.SetBool(_animIDGrounded, grounded);
            }
        }

        public void SetSpeed(float speed)
        {
            if (_animator)
            {
                _animator.SetFloat(_animIDSpeed, speed);
            }
        }

        public void SetMotionSpeed(float motionSpeed)
        {
            if (_animator)
            {
                _animator.SetFloat(_animIDMotionSpeed, motionSpeed);
            }
        }

        public void SetJump(bool isJumping)
        {
            if (_animator)
            {
                _animator.SetBool(_animIDJump, isJumping);
            }
        }

        public void SetFreeFall(bool isFreeFalling)
        {
            if (_animator)
            {
                _animator.SetBool(_animIDFreeFall, isFreeFalling);
            }
        }
    }
}