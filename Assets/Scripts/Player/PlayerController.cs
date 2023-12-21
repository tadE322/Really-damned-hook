using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f; // ���� ������
    [Range(0, 0.3f)][SerializeField] private float m_MovementSmoothing = 0.05f; // ��������� ����� �������� ��������
    [SerializeField] private bool m_AirControl = false; // ����� �� ����� ����������� � ������� ��� ���
    [SerializeField] private LayerMask m_WhatIsGround; // ����, ������������ ��� �������� ������ ��� ������
    [SerializeField] private Transform m_GroundCheck; // �����, ��� �������� ����������� ������, ������ ��� �������� ������
    [SerializeField] private GrapplingRope m_GrapplingRope;

    const float k_GroundedRadius = 0.1f; // ������ ����������, ������������ ������� ����������
    private bool m_Grounded; // ��������� �� ��� ��� ����� �� �����
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;

    [Header("�������")]
    [Space]

    public UnityEvent OnLandEvent;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }

    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;
        // ����� ��������� �� �����, ���� ���� ��� �������� ������� �� ����� �������� ���� ����, ������������� ��� �����
        // ��� ����� �������, ��������� ����, �� Sample Assets �� ����� �������������� ��������� �������

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                }
            }
        }
    }

    public void Move(float move, bool jump)
    {

        // ���������� ���������� ������ ���� �� �� ����� ��� � �������, �� �������� ���������� � �������(airControl)
        if (m_Grounded && !m_GrapplingRope.enabled || m_AirControl && !m_GrapplingRope.enabled)
        {

            // ������� ��������� ����� ���������� ������� ��������
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // ����� ���������� � ��������� � ���������
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        }    // ���� ����� ������ ��������
        if (m_Grounded && jump)
        {
            // ���� ��� ������������ ���� 
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
        else if (m_GrapplingRope.IsGrappling && jump)
        {
            m_GrapplingRope.GrapplingGun.NotGrapple();
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce * 0.75f));
        }
    }
}
