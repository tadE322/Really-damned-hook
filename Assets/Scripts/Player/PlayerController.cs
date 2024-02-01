using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [Range(0, 0.3f)][SerializeField] private float m_MovementSmoothing = 0.05f; // насколько нужно сгладить движение
    [SerializeField] private bool m_AirControl = false; // может ли игрок управляться в воздухе или нет
    [SerializeField] private GrapplingRope m_GrapplingRope;
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    private Animator _anim;

    [Header("Jump")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float m_JumpForce = 8f; // сила прыжка
    private bool isGrounded;
    private bool canJump;


    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }
    private void Update()
    {
        CheckInput();
        CheckIfCanJump();
        //if (Input.GetKeyDown(KeyCode.Space) && m_GrapplingRope.enabled)
        //{
        //    Debug.Log("Алё");
        //    m_GrapplingRope.GrapplingGun.NotGrapple();
        //    Jump();
        //}
    }

    private void FixedUpdate()
    {
        ChechSurroundings();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (canJump)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
        }
        if(!canJump && m_GrapplingRope.enabled)
        {
            m_GrapplingRope.GrapplingGun.NotGrapple();
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
        }
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && m_Rigidbody2D.velocity.y <= 0)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    private void ChechSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }



    public void Move(float move)
    {
        // управление персонажем только если он на земле или в воздухе, но включено управление в воздухе(airControl)
        if (isGrounded && !m_GrapplingRope.enabled || m_AirControl && !m_GrapplingRope.enabled)
        {

            // двигаем персонажа путем нахождения целевой скорости
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // затем сглаживаем и применяем к персонажу
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
