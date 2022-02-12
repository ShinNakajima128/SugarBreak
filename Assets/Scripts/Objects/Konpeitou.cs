using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Konpeitou : MonoBehaviour
{
    [SerializeField] float m_arrivalTime = 2.0f;
    [SerializeField] PlayerData playerData = default;
    [SerializeField] SphereCollider m_sphereCollider = default;
    [SerializeField] float m_startMovingTimer = 2;
    public Vector3 m_position;
    Rigidbody m_rb;
    float period;
    Vector3 velocity;
    bool isSearched = false;
    bool isUpdated = false;
    public static int playSeCount = 0;

    public Transform Target { get; set; }

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        period = m_arrivalTime;
        StartCoroutine(Stopping());
    }

    void OnDisable()
    {
        if (isSearched)
        {
            isSearched = false;
            m_rb.useGravity = true;
        }
    }

    private void Update()
    {
        if (isSearched)
        {
            m_rb.isKinematic = false;
            StartMoving();
        }
    }

    public void StartMoving()
    {
        if (isUpdated)
        {
            m_position = transform.position;
            isUpdated = false;
        }

        var acceleration = m_rb.velocity;
        var diff = Target.position - m_position;
        acceleration += (diff - velocity * period) * 2.0f / (period * period);
        period -= Time.deltaTime;

        if (period <= 0f)
        {
            playerData.TotalKonpeitou++;
            SoundManager.Instance.PlaySeByName("Gain");
            this.gameObject.SetActive(false);
        }
        velocity += acceleration * Time.deltaTime;
        m_position += velocity * Time.deltaTime;
        transform.position = m_position;
    }

    IEnumerator Stopping()
    {
        yield return new WaitForSeconds(0.1f);

        velocity = Vector3.zero;
        m_sphereCollider.enabled = true;
        var a = Random.Range(0f, 1.5f);
        
        yield return new WaitForSeconds(m_startMovingTimer + a);

        isSearched = true;
        m_sphereCollider.enabled = false;
        m_rb.useGravity = false;
        Vector3 force = new Vector3(0, 15, 0);
        isUpdated = true;
        m_rb.AddForce(force, ForceMode.Impulse);
    }
}
