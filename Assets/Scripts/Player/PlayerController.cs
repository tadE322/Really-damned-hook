using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f; // сила прыжка
    [Range(0, 0.3f)][SerializeField] private float m_MovementSmoothing = 0.05f; // насколько нужно сгладить движение
    [SerializeField] private bool m_AirControl = false; // может ли игрок управляться в воздухе или нет
    [SerializeField] private LayerMask m_WhatIsGround; // слой, обозначающий что является землей для игрока
    [SerializeField] private Transform m_GroundCheck; // точка, для проверки приземления игрока, нужная для механики прыжка
    [SerializeField] private GrapplingRope m_GrapplingRope;

    const float k_GroundedRadius = 0.1f; // радиус окружности, определяющей наличие заземления
    private bool m_Grounded; // находится ли или нет игрок на земле
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;

    [Header("События")]
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
        // игрок считается на земле, если круг для проверки позиции на земле касается чего либо, обозначенного как земля
        // это можно сделать, используя слои, но Sample Assets не будут перезаписывать настройки проекта

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

        // управление персонажем только если он на земле или в воздухе, но включено управление в воздухе(airControl)
        if (m_Grounded && !m_GrapplingRope.enabled || m_AirControl && !m_GrapplingRope.enabled)
        {

            // двигаем персонажа путем нахождения целевой скорости
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // затем сглаживаем и применяем к персонажу
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        }    // если игрок должен прыгнуть
        if (m_Grounded && jump)
        {
            // даем ему вертикальную силу 
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
