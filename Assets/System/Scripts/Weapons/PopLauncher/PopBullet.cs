using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopBullet : MonoBehaviour
{
    [SerializeField] GameObject m_ExplosionSfx = null;
    SoundManager soundManager;
    int m_attackDamage = 0;

    public int AttackDamage
    {
        get { return m_attackDamage; }
        set { m_attackDamage = value; }
    }

    private void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        EffectManager.PlayEffect(EffectType.Explosion, this.transform.position);
        soundManager.PlaySeByName("Explosion");
        Destroy(this.gameObject);
    }
}
