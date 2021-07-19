using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Konpeitou : MonoBehaviour
{
    [SerializeField] float m_arrivalTime = 2.0f;
    [SerializeField] PlayerData playerData = default;
    [SerializeField] SphereCollider m_sphereCollider = default;
    public Transform m_target;
    public Vector3 m_position;
    Rigidbody m_rb;
    float period;
    Vector3 velocity;
    bool isSearched = false;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        period = m_arrivalTime;
    }
    private void Update()
    {
        if (isSearched)
        {
            StartMoving();
        }
    }

    public void StartMoving()
    {
        var acceleration = m_rb.velocity;
        var diff = m_target.position - m_position;
        acceleration += (diff - velocity * period) * 2.0f / (period * period);
        period -= Time.deltaTime;

        if (period <= 0f)
        {
            playerData.TotalKonpeitou++;
            Destroy(this.gameObject);
        }
        velocity += acceleration * Time.deltaTime;
        m_position += velocity * Time.deltaTime;
        transform.position = m_position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isSearched = true;
            m_sphereCollider.enabled = false;
            m_rb.useGravity = false;
            Vector3 force = new Vector3(0, 10, 0);

            m_rb.AddForce(force, ForceMode.Impulse);
        }
    }
}
