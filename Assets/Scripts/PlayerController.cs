using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side { Left = -2 , Middle = 0 , Right = 2 }

public class PlayerController : MonoBehaviour
{
    private Side position;

    private Vector3 motionVector;
    private Transform m_Transform;
    [SerializeField] private float forwardSpeed = 10;
    [SerializeField] private float jumpForce = 7;
    [SerializeField] private float dodgeSpeed;
    public CharacterController CharacterController { get => _characterController; set => _characterController = value; }
    private CharacterController _characterController;
    private float newXPosition; 
    private float xPosition; 
    private float yPosition;
    private float rollTimer;
    private bool swipeLeft, swipeRight, swipeDown, swipeUp;
    private bool isJumping, isRolling;

    private Animator playerAnimator;
    private int IdJump = Animator.StringToHash("Jump");
    private int IdFall = Animator.StringToHash("Fall");
    private int IdRoll = Animator.StringToHash("Roll");
    private int IdLanding = Animator.StringToHash("Landing");
    private int IdDodgeLeft = Animator.StringToHash("DodgeLeft");
    private int IdDodgeRight = Animator.StringToHash("DodgeRight");

    void Start()
    {
        position = Side.Middle;
        m_Transform = GetComponent<Transform>();
        playerAnimator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        yPosition = -1;
    }

    void Update()
    {
        GetSwipeInput();
        SetPlayerPosition();
        MovePlayer();
        Jump();
        Roll();
    }

    private void GetSwipeInput()
    {
        swipeLeft = Input.GetKeyDown(KeyCode.LeftArrow);
        swipeRight = Input.GetKeyDown(KeyCode.RightArrow);
        swipeDown = Input.GetKeyDown(KeyCode.DownArrow);
        swipeUp = Input.GetKeyDown(KeyCode.UpArrow);
    }

    private void SetPlayerPosition()
    {
        if (swipeLeft && !isRolling)
        {
            if (position != Side.Left)
            {
                UpdatePlayerXPosition(Side.Left);
                SetPlayerAnimator(IdDodgeLeft, false);
            }
        }
        else if (swipeRight && !isRolling)
        {
            if (position != Side.Right)
            {
                UpdatePlayerXPosition(Side.Right);
                SetPlayerAnimator(IdDodgeRight, false);
            }
        }
    }

    private void UpdatePlayerXPosition(Side plPosition)
    {
        newXPosition = (int)position + (int)plPosition;
        position = (Side)newXPosition;
    }

    private void SetPlayerAnimator(int id, bool isCroosFade, float fixedTime = 0.1f)
    {
        if (isCroosFade)
        {
            playerAnimator.CrossFadeInFixedTime(id, fixedTime);
        }
        else
            playerAnimator.Play(id);
    }

    private void MovePlayer()
    {
        motionVector = new Vector3(xPosition - m_Transform.position.x, yPosition * Time.deltaTime, forwardSpeed * Time.deltaTime);
        xPosition = Mathf.Lerp(xPosition, newXPosition, Time.deltaTime * dodgeSpeed);
        _characterController.Move(motionVector);
    }

    private void Jump()
    {
        if (_characterController.isGrounded)
        {
            isJumping = false;
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
                SetPlayerAnimator(IdLanding, false);
            if (swipeUp && !isRolling)
            {
                isJumping = true;
                yPosition = jumpForce;
                SetPlayerAnimator(IdJump, true);
            }
        }
        else
        {
            yPosition -= jumpForce * 2 * Time.deltaTime;
            if (_characterController.velocity.y <= 0)
                SetPlayerAnimator(IdFall, false);
        }
    }

    private void Roll()
    {
        rollTimer -= Time.deltaTime;

        if (rollTimer <= 0)
        {
            isRolling = false;
            rollTimer = 0;
            _characterController.center = new Vector3(0, .45f, 0);
            _characterController.height = .9f;
        }

        if (swipeDown && !isJumping)
        {
            isRolling = true;
            rollTimer = .5f;
            SetPlayerAnimator(IdRoll, true);
            _characterController.center = new Vector3(0, .2f, 0);
            _characterController.height = .4f;
        }
    }
}
