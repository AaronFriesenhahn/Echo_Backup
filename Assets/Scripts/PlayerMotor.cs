using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    public event Action Land = delegate { };

    [SerializeField] GroundDetector _groundDetector = null;
    [SerializeField] GameObject ArtToRotate;

    [Header("Jump Ability Particles")]
    [SerializeField] GameObject FireJumpParticles;
    [SerializeField] GameObject IceJumpParticles;
    [SerializeField] GameObject GroundJumpParticles;
    [SerializeField] GameObject ElectricJumpParticles;

    Vector3 _movementThisFrame = Vector3.zero;
    float _turnAmountThisFrame = 0;
    float _lookAmountThisFrame = 0;

    //tracking our own camera angle, to avoid weird 0-360 angle conversion
    private float _currentCameraRotationX = 0;
    public bool _isGrounded = false;

    Rigidbody _rigidbody = null;

    [Header("Player Bools")]
    public bool isFacingRight = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        isFacingRight = true;
    }

    private void FixedUpdate()
    {
        ApplyMovement(_movementThisFrame);
    }

    private void Update()
    {
        if (GameManager.GameIsPaused == false)
        {
            RotateArt();
        }
    }

    public void Move(Vector3 requestedMovement)
    {
        //store this movement for next FixedUpdate tick
        _movementThisFrame = requestedMovement;
    }

    public void Jump(float jumpForce)
    {
        //only allow us to jump if we're on the ground
        if (_isGrounded == false)
            return;
        _rigidbody.AddForce(Vector3.up * jumpForce);
    }

    void ApplyMovement(Vector3 moveVector)
    {
        //confirms that we actually have movement, exit early if we don't
        if (moveVector == Vector3.zero)
            return;
        //move the rigidbody
        _rigidbody.MovePosition(_rigidbody.position + moveVector);
        //clear out movement, until we get a new move request
        _movementThisFrame = Vector3.zero;
    }

    private void OnEnable()
    {
        _groundDetector.GroundDetected += OnGroundDetected;
        _groundDetector.GroundVanished += OnGroundVanished;
    }

    private void OnDisable()
    {
        _groundDetector.GroundDetected -= OnGroundDetected;
        _groundDetector.GroundVanished -= OnGroundVanished;
    }

    void OnGroundDetected()
    {
        _isGrounded = true;
        //notify others that we have landed (animations, etc.)
        Land?.Invoke();
    }
    void OnGroundVanished()
    {
        _isGrounded = false;
    }

    void RotateArt()
    {
        if (Input.GetKey(KeyCode.A) && isFacingRight == true || (Input.GetKey(KeyCode.LeftArrow)) && isFacingRight == true)
        {
            ArtToRotate.transform.Rotate(0, -180, 0);
            GameObject Head = GameObject.Find("Player/Echo_Model_With_Upgrades/Head");
            Head.transform.Rotate(0, -90, 0);
            isFacingRight = false;
        }
        else if (Input.GetKey(KeyCode.D) && isFacingRight == false && !Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.RightArrow)) && isFacingRight == false && !Input.GetKey(KeyCode.RightArrow))
        {
            ArtToRotate.transform.Rotate(0, 180, 0);
            GameObject Head = GameObject.Find("Player/Echo_Model_With_Upgrades/Head");
            Head.transform.Rotate(0, 90, 0);
            isFacingRight = true;
        }
    }

    IEnumerator DelaySecondJumpActivation()
    {
        yield return new WaitForSeconds(1f);
    }
}
