using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Konpeitou : MonoBehaviour
{
    [SerializeField] float m_arrivalTime = 2.0f;
    public static int totalKonpeitou = 0;
    public Transform m_target;
    public Vector3 m_position;
    Rigidbody m_rb;
    float period;
    Vector3 velocity;
    PlayerData player;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        period = m_arrivalTime;
    }
    private void Update()
    {
        var acceleration = m_rb.velocity;
        var diff = m_target.position - m_position;
        acceleration += (diff - velocity * period) * 2.0f / (period * period);
        period -= Time.deltaTime;
        
        if (period <= 0f)
        {
            totalKonpeitou++;
            Destroy(this.gameObject);
        }
        velocity += acceleration * Time.deltaTime;
        m_position += velocity * Time.deltaTime;
        transform.position = m_position;
    }
}
