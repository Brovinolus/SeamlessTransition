using System.Collections;
using UnityEngine;

public class SphereMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float moveSpeed = 5.0f;
    private Vector3 _targetPosition; 
    private bool _isMoving = false;

    public bool IsSphereClicked { get; set; }

    private void Start()
    {
        if (Camera.main != null)
            _targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        _targetPosition.z = transform.position.z;
    }

    private void Update()
    {
        if (!IsSphereClicked)
        {
            Rotation();
        }
    }

    public void StartMoveToCenterCoroutine()
    {
        if (!_isMoving)
        {
            StartCoroutine(MoveToCenter());
        }
    }

    private void Rotation()
    {
        transform.RotateAround(target.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private IEnumerator MoveToCenter()
    {
        _isMoving = true;

        float distance = Vector3.Distance(transform.position, _targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, _targetPosition) > 0.01f)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed * Time.deltaTime;
            float fraction = distanceCovered / distance;
            transform.position = Vector3.Lerp(transform.position, _targetPosition, fraction);
            yield return null;
        }
        
        transform.position = _targetPosition;
    }
}
