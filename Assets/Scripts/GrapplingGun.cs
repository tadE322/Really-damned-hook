using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public GrapplingRope GrappleRope;

    [Header("Layers Settings:")]
    [SerializeField] private bool _grappleToAll = false;
    [SerializeField] private int _grappableLayerNumber = 9;

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
    [SerializeField] private bool _rotateOverTime = true;
    [Range(0, 60)][SerializeField] private float _rotationSpeed = 4;

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

    private void Start()
    {
        GrappleRope.enabled = false;
        m_SpringJoint2D.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetGrapplePoint();
        }
        else if (Input.GetKey(KeyCode.E))
        {
            if(GrappleRope.enabled)
            {
                RotateGun(grapplePoint, false);
            }
            else
            {
                Vector2 mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }

            if(_launchToPoint && GrappleRope.IsGrappling)
            {
                if(_launchType == LaunchType.Transform_Launch)
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
            RotateGun(mousePos, true);
        }
    }

    public void NotGrapple()
    {
        GrappleRope.enabled = false;
        m_SpringJoint2D.enabled = false;
        m_Rigidbody2D.gravityScale = 3;
    }

    private void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if(_rotateOverTime && allowRotationOverTime)
        {
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * _rotationSpeed);
        }
        else
        {
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void SetGrapplePoint()
    {
        Vector2 distanceVector = m_Camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
        if(Physics2D.Raycast(firePoint.position, distanceVector.normalized))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
            if(_hit.transform.gameObject.layer == _grappableLayerNumber || _grappleToAll)
            {
                if(Vector2.Distance(_hit.point, firePoint.position) <= _maxDistance || !_hasMaxDistance)
                {
                    grapplePoint = _hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                    GrappleRope.enabled = true;
                }
            }
        }
    }

    public void Grapple()
    {
        m_SpringJoint2D.autoConfigureDistance = false;
        if(!_launchToPoint && !_autoConfigureDistance)
        {
            m_SpringJoint2D.distance = _targetDistance;
            m_SpringJoint2D.frequency = _targetFrequncy;
        }
        if(!_launchToPoint)
        {
            if(_autoConfigureDistance)
            {
                m_SpringJoint2D.autoConfigureDistance = true;
                m_SpringJoint2D.frequency = 0;
            }
            m_SpringJoint2D.connectedAnchor = grapplePoint;
            m_SpringJoint2D.enabled = true;
        }
        else
        {
            switch(_launchType)
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

    private void OnDrawGizmosSelected()
    {
        if(firePoint != null && _hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, _maxDistance);
        }
    }
}
