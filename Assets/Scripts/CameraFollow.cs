using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private Transform m_Transform;
    private Vector3 cameraOffset;
    private Vector3 followPosition;
    [SerializeField] private float rayDistance;
    [SerializeField] private float speedOffset;
    private float y;
    
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        m_Transform = GetComponent<Transform>();
        cameraOffset = m_Transform.position;
    }

    void LateUpdate()
    {
        followPosition = target.position + cameraOffset;
        m_Transform.position = followPosition;
        UpdateCameraOffset();
    }

    private void UpdateCameraOffset()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(target.position, Vector3.down, out hitInfo, rayDistance))
        {
            y = Mathf.Lerp(y, hitInfo.point.y, Time.deltaTime * speedOffset);
        }
        // else y = Mathf.Lerp(m_Transform.position.y, target.position.y, Time.deltaTime * speedOffset);

        followPosition.y = cameraOffset.y + y;
        m_Transform.position = followPosition;
    }
}
