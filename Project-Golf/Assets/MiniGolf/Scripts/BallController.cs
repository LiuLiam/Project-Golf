using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class BallController : MonoBehaviour
{
    public static BallController instance;

    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _areaAffector;
    [SerializeField] private float _maxForce, _forceModifier;
    [SerializeField] private LayerMask _rayLayer;

    private float _force;
    private Rigidbody _theRB;

    private Vector3 _startPos, _endPos;
    private Vector3 _direction;
    private bool _canShoot = false;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        _theRB = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CameraFollow.instance.SetTarget(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !_canShoot)
        {
            _startPos = ClickedPoint();
            _lineRenderer.gameObject.SetActive(true);
            _lineRenderer.SetPosition(0, _lineRenderer.transform.localPosition);
        }

        if (Input.GetMouseButton(0))
        {
            _endPos = ClickedPoint();
            _endPos.y = _lineRenderer.transform.position.y;
            _force = Mathf.Clamp(Vector3.Distance(_endPos, _startPos) * _forceModifier, 0, _maxForce);
            _lineRenderer.SetPosition(1, transform.InverseTransformPoint(_endPos));
        }

        if (Input.GetMouseButtonUp(0))
        {
            _canShoot = true;
            _lineRenderer.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (_canShoot)
        {
            _canShoot = false;
            _direction = _startPos - _endPos;
            _theRB.AddForce(_direction * _force, ForceMode.Impulse);
            _areaAffector.SetActive(false);
            _force = 0;
            _startPos = _endPos = Vector3.zero;
        }
    }

    Vector3 ClickedPoint()
    {
        Vector3 position = Vector3.zero;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _rayLayer))
            position = hit.point;

        return position;
    }
}
