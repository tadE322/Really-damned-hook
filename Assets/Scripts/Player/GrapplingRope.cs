using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    [Header("General References:")]
    public GrapplingGun GrapplingGun;
    public LineRenderer m_LineRenderer;

    [Header("General Settings:")]
    [SerializeField] private int _percision = 40;
    [Range(0, 20)][SerializeField] private float _straightenLineSpeed = 5;

    [Header("Rope Animations Settings:")]
    public AnimationCurve RopeAnimationCurve;
    [Range(0.01f, 4)][SerializeField] private float _startWaveSize = 2;
    private float _waveSize = 0;

    [Header("Rope Progression:")]
    public AnimationCurve RopeProgressionCurve;
    [SerializeField][Range(1, 50)] private float _ropeProgressionSpeed = 1;
    private float _moveTime = 0;

    [HideInInspector] public bool IsGrappling = true;

    private bool _strightLine = true;

    private void OnEnable()
    {
        _moveTime = 0;
        m_LineRenderer.positionCount = _percision;
        _waveSize = _startWaveSize;
        _strightLine = false;

        LinePointsToFirePoint();

        m_LineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        m_LineRenderer.enabled = false;
        IsGrappling = false;
    }

    private void LinePointsToFirePoint()
    {
        for (int i = 0; i < _percision; i++)
        {
            m_LineRenderer.SetPosition(i, GrapplingGun.firePoint.position);
        }
    }

    private void Update()
    {
        _moveTime += Time.deltaTime;
        DrawRope();
    }
    
    private void DrawRope()
    {
        if(!_strightLine)
        {
            if(m_LineRenderer.GetPosition(_percision - 1).x == GrapplingGun.grapplePoint.x)
            {
                _strightLine = true;
            }
            else
            {
                DrawRopeWaves();
            }
        }
        else
        {
            if(!IsGrappling)
            {
                GrapplingGun.Grapple();
                IsGrappling = true;
            }
            if(_waveSize > 0)
            {
                _waveSize -= Time.deltaTime * _straightenLineSpeed;
                DrawRopeWaves();
            }
            else
            {
                _waveSize = 0;

                if(m_LineRenderer.positionCount != 2)
                {
                    m_LineRenderer.positionCount = 2;
                }
                DrawRopeNoWaves();
            }
        }
    }

    private void DrawRopeWaves()
    {
        for (int i = 0; i < _percision; i++)
        {
            float delta = (float)i / ((float)_percision - 1f);
            Vector2 offset = Vector2.Perpendicular(GrapplingGun.grappleDistanceVector).normalized * RopeAnimationCurve.Evaluate(delta) * _waveSize;
            Vector2 targerPosition = Vector2.Lerp(GrapplingGun.firePoint.position, GrapplingGun.grapplePoint, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(GrapplingGun.firePoint.position, targerPosition, RopeProgressionCurve.Evaluate(_moveTime) * _ropeProgressionSpeed);

            m_LineRenderer.SetPosition(i, currentPosition);
        }
    }

    private void DrawRopeNoWaves()
    {
        m_LineRenderer.SetPosition(0, GrapplingGun.firePoint.position);
        m_LineRenderer.SetPosition(1, GrapplingGun.grapplePoint);
    }
}
