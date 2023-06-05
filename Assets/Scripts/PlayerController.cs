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
    public int IdStumbleLow                        { get => _IdStumbleLow; set => _IdStumbleLow = value; }
    public int IdDeathLower                        { get => _IdDeathLower; set => _IdDeathLower = value; }
    public int IdDeathMovingTrain                  { get => _IdDeathMovingTrain; set => _IdDeathMovingTrain = value; }
    public int IdDeathBounce                       { get => _IdDeathBounce; set => _IdDeathBounce = value; }
    public int IdDeathUpper                        { get => _IdDeathUpper; set => _IdDeathUpper = value; }
    public bool IsRolling                          { get => _isRolling; set => _isRolling = value; }
    public int IdStumbleCornerLeft                 { get => _IdStumbleCornerLeft; set => _IdStumbleCornerLeft = value; }
    public int IdStumbleCornerRight                { get => _IdStumbleCornerRight; set => _IdStumbleCornerRight = value; }
    public int IdStumbleSideLeft                   { get => _IdStumbleSideLeft; set => _IdStumbleSideLeft = value; }
    public int IdStumbleSideRight                  { get => _IdStumbleSideRight; set => _IdStumbleSideRight = value; }
    public int IdStumbleOffLeft { get => _IdStumbleOffLeft; set => _IdStumbleOffLeft = value; }
    public int IdStumbleOffRight { get => _IdStumbleOffRight; set => _IdStumbleOffRight = value; }

    private CharacterController _characterController;
    private float newXPosition; 
    private float xPosition; 
    private float yPosition;
    private float rollTimer;
    private bool swipeLeft, swipeRight, swipeDown, swipeUp;
    private bool isJumping, _isRolling;

    private Animator playerAnimator;
    private int IdJump                   = Animator.StringToHash("Jump");
    private int IdFall                   = Animator.StringToHash("Fall");
    private int IdRoll                   = Animator.StringToHash("Roll");
    private int IdLanding                = Animator.StringToHash("Landing");
    private int IdDodgeLeft              = Animator.StringToHash("DodgeLeft");
    private int IdDodgeRight             = Animator.StringToHash("DodgeRight");
    private int _IdStumbleLow            = Animator.StringToHash("StumbleLow");
    private int _IdStumbleCornerLeft     = Animator.StringToHash("StumbleCornerLeft");
    private int _IdStumbleCornerRight    = Animator.StringToHash("StumbleCornerRight");
    private int _IdStumbleFall           = Animator.StringToHash("StumbleFall");
    private int _IdStumbleOffLeft        = Animator.StringToHash("StumbleOffLeft");
    private int _IdStumbleOffRight       = Animator.StringToHash("StumbleOffRight");
    private int _IdStumbleSideLeft       = Animator.StringToHash("StumbleSideLeft");
    private int _IdStumbleSideRight      = Animator.StringToHash("StumbleSideRight");
    private int _IdDeathBounce           = Animator.StringToHash("DeathBounce");
    private int _IdDeathLower            = Animator.StringToHash("DeathLower");
    private int _IdDeathUpper            = Animator.StringToHash("DeathUpper");
    private int _IdDeathMovingTrain      = Animator.StringToHash("DeathMovingTrain");
    private int _IdDogDeathMovingTrain   = Animator.StringToHash("DogDeathMovingTrain");
    private int _IdGuardDeathMovingTrain = Animator.StringToHash("GuardDeathMovingTrain");

    private PlayerCollision playerCollision;
    private CapsuleCollider playerCollider;


    void Start()
    {
        position = Side.Middle;
        m_Transform = GetComponent<Transform>();
        playerAnimator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        yPosition = -1;
        playerCollision = GetComponent<PlayerCollision>();
        playerCollider = GetComponentInChildren<CapsuleCollider>();
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
        if (swipeLeft && !IsRolling)
        {
            if (position != Side.Left)
            {
                UpdatePlayerXPosition(Side.Left);
                SetPlayerAnimator(IdDodgeLeft, false);
            }
        }
        else if (swipeRight && !IsRolling)
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

    public void SetPlayerAnimator(int id, bool isCroosFade, float fixedTime = 0.1f)
    {
        playerAnimator.SetLayerWeight(0, 1);
        if (isCroosFade)
        {
            playerAnimator.CrossFadeInFixedTime(id, fixedTime);
        }
        else
        {
            playerAnimator.Play(id);
        }

        playerCollision.ResetCollision();
    }

    public void SetPlayerAnimatorWithLayer(int id)
    {
        playerAnimator.SetLayerWeight(1, 1);
        
        playerAnimator.Play(id);

        playerCollision.ResetCollision();
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
            if (swipeUp && !IsRolling)
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
            IsRolling = false;
            rollTimer = 0;
            _characterController.center = new Vector3(0, .45f, 0);
            _characterController.height = .9f;
            playerCollider.center = new Vector3(0, .45f, 0);
            playerCollider.height = .9f;
        }

        if (swipeDown && !isJumping)
        {
            IsRolling = true;
            rollTimer = .5f;
            SetPlayerAnimator(IdRoll, true);
            _characterController.center = new Vector3(0, .2f, 0);
            _characterController.height = .4f;
            playerCollider.center = new Vector3(0, .2f, 0);
            playerCollider.height = .4f;
        }
    }
}
