using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    private Transform m_Transform;
    [SerializeField] private float m_Speed;
    
    void Start()
    {
        m_Transform = GetComponent<Transform>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        m_Transform.Translate(0, 0, m_Speed * Time.deltaTime);
    }
}
