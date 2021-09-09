using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class LockonController : MonoBehaviour
{
    [SerializeField] 
    CinemachineVirtualCamera m_lockonCamera = default;
    
    [SerializeField]
    CinemachineTargetGroup m_targetGroup = default;

    [SerializeField] 
    float m_searchRadius = 10f;

    [SerializeField]
    Transform m_playerLookAt = default;

    [SerializeField] 
    GameObject m_LockonPrefab = default;

    [SerializeField]
    float m_playerToEnemyDistance = 15f;

    GameObject nearTarget;
    GameObject lockImage;
    bool isLockon = false;

    void Start()
    {
        m_lockonCamera.Priority = 9;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            if (isLockon)
            {
                OffLock();
                
            }
            else
            {
                OnLock();
            }
        }

        if (isLockon && nearTarget == null)
        {
            OffLock();
        }

        if (isLockon)
        {
            var dist = Vector3.Distance(transform.position, nearTarget.transform.position);

            if (dist > m_playerToEnemyDistance)
            {
                OffLock();
            }
        }
    }

    void OnLock()
    {
        
        nearTarget = LockonNearTarget();

        if (nearTarget)
        {
            lockImage = Instantiate(m_LockonPrefab);

            lockImage.transform.position = nearTarget.transform.position;
            lockImage.transform.parent = nearTarget.transform;
            
            m_targetGroup.AddMember(nearTarget.transform, 1, 2);
        }
        else
        {
            Debug.Log("ロックする対象がいません");
            return;
        }

        isLockon = true;
        m_lockonCamera.Priority = 20;
    }

    void OffLock()
    {
        foreach (var m in m_targetGroup.m_Targets)
        {
            if (!nearTarget)
            {
                Debug.Log("ターゲット情報を削除した");
                m_targetGroup.RemoveMember(m.target);
            }
            else if (m.target == nearTarget.transform)
            {
                m_targetGroup.RemoveMember(nearTarget.transform);
                nearTarget = null;
                break;
            }
        }
        if (m_targetGroup.m_Targets.Length < 1) m_targetGroup.AddMember(m_playerLookAt, 2, 2);

        if (lockImage) Destroy(lockImage);
        isLockon = false;
        m_lockonCamera.Priority = 9;
        Debug.Log("ロック解除");
    }

    GameObject LockonNearTarget()
    {
        var hits = Physics.SphereCastAll(this.transform.position, m_searchRadius, transform.forward, 0.01f)
                          .Select(t => t.transform.gameObject).ToList();

        Debug.Log(hits.Count);

        if (0 < hits.Count)
        {
            float minDistance = 100f;
            GameObject target = default;

            foreach (var h in hits)
            {
                if (h.gameObject.CompareTag("Enemy"))
                {
                    var targetDistance = Vector3.Distance(transform.position, h.transform.position);

                    if (targetDistance < minDistance)
                    {
                        minDistance = targetDistance;
                        target = h.transform.gameObject;
                    }
                }
            }
            return target;
        }
        else
        {
            return null;
        }
    }
}
