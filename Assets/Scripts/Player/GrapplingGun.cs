using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public GrapplingRope GrappleRope;

    [Header("Layers Settings:")]
    [SerializeField] private int _grappableLayerNumber = 3;
    [SerializeField] private LayerMask _selectLayerMask;

    [Header("Main Camera:")]
    public Camera m_Camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_SpringJoint2D;
    public Rigidbody2D m_Rigidbody2D;

    [Header("Rotation:")]

    [Header("Distance:")]
    [SerializeField] private bool _hasMaxDistance = false;
    [SerializeField] private float _maxDistance = 20;

    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool _launchToPoint = true;
    [SerializeField] private LaunchType _launchType = LaunchType.Physics_Launch;
    [SerializeField] private float _launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool _autoConfigureDistance = false;
    [SerializeField] private float _targetDistance = 3;
    [SerializeField] private float _targetFrequncy = 1;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;


    [Header("Hook Cooldown:")]
    [SerializeField] private float time;
    [SerializeField] private Image _hookCooldownFill;

    private float _timeLeft = 0;
    private bool _timerOn = true;
    private bool _canThrowHook = true;

    private void Start()
    {
        _timeLeft = time;

        GrappleRope.enabled = false;
        m_SpringJoint2D.enabled = false;
    }

    private void Update()
    {
        Timer();

        if (Input.GetKeyDown(KeyCode.E) && _canThrowHook)
        {
            SetGrapplePoint();
            _timerOn = true;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            if (GrappleRope.enabled)
            {
                RotateGun(grapplePoint);
            }
            else
            {
                Vector2 mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos);
            }

            if (_launchToPoint && GrappleRope.IsGrappling)
            {
                if (_launchType == LaunchType.Transform_Launch)
                {
                    Vector2 firePointDistance = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint - firePointDistance;
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * _launchSpeed);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            NotGrapple();
        }
        else
        {
            Vector2 mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            RotateGun(mousePos);
        }
    }

    public void NotGrapple()
    {
        GrappleRope.enabled = false;
        m_SpringJoint2D.enabled = false;
        m_Rigidbody2D.gravityScale = 3;
    }

    private void RotateGun(Vector3 lookPoint)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;

    }

    private void SetGrapplePoint()
    {
        Vector2 distanceVector = m_Camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
        RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized, _maxDistance, _selectLayerMask);
        if (_hit)
        {
            if (_hit.transform.gameObject.layer == _grappableLayerNumber)
            {

                grapplePoint = _hit.point;
                grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                GrappleRope.enabled = true;

            }
            else
            {
                Debug.Log("Условие не работает" + _hit.collider.name);
            }
        }
        else
        {
            Debug.Log("Путь перегражден" + _hit.collider.name);
        }

    }

    public void Grapple()
    {
        m_SpringJoint2D.autoConfigureDistance = false;
        if (!_launchToPoint && !_autoConfigureDistance)
        {
            m_SpringJoint2D.distance = _targetDistance;
            m_SpringJoint2D.frequency = _targetFrequncy;
        }
        if (!_launchToPoint)
        {
            if (_autoConfigureDistance)
            {
                m_SpringJoint2D.autoConfigureDistance = true;
                m_SpringJoint2D.frequency = 0;
            }
            m_SpringJoint2D.connectedAnchor = grapplePoint;
            m_SpringJoint2D.enabled = true;
        }
        else
        {
            switch (_launchType)
            {
                case LaunchType.Physics_Launch:
                    m_SpringJoint2D.connectedAnchor = grapplePoint;

                    Vector2 distanceVector = firePoint.position - gunHolder.position;

                    m_SpringJoint2D.distance = distanceVector.magnitude;
                    m_SpringJoint2D.frequency = _launchSpeed;
                    m_SpringJoint2D.enabled = true;
                    break;
                case LaunchType.Transform_Launch:
                    m_Rigidbody2D.gravityScale = 0;
                    m_Rigidbody2D.velocity = Vector2.zero;
                    break;
            }
        }
    }

    private void Timer()
    {
        if (_timerOn)
        {
            if (_timeLeft < time)
            {
                _canThrowHook = false;
                _timeLeft += Time.deltaTime;
                _hookCooldownFill.fillAmount = _timeLeft / time;
            }
            else if (_timeLeft >= time)
            {
                _canThrowHook = true;
                _timerOn = false;
                _timeLeft = 0;
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && _hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, _maxDistance);
        }
    }
}
