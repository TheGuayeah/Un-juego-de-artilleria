using UnityEngine;
using System.Collections;

namespace PEC3
{
    public class PlayerHealth : MonoBehaviour
    {
        public float health = 100f;          
        public float repeatDamagePeriod = 2f;
        public AudioClip[] ouchClips;        
        public float hurtForce = 10f;        
        public float damageAmount = 10f;     

        public SpriteRenderer healthBar;     
        private float lastHitTime;           
        private Vector3 healthScale;         
        private PlayerControl playerControl; 
        private Animator anim;               


        void Awake()
        {
            playerControl = GetComponent<PlayerControl>();
            anim = GetComponent<Animator>();

            healthScale = healthBar.transform.localScale;
        }


        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("ExplosionFX"))
            {
                if (Time.time > lastHitTime + repeatDamagePeriod)
                {
                    if (health > 0f)
                    {
                        TakeDamage(col.transform);
                        lastHitTime = Time.time;
                    }
                    else
                    {
                        Collider2D[] cols = GetComponents<Collider2D>();
                        foreach (Collider2D c in cols)
                        {
                            c.isTrigger = true;
                        }

                        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
                        foreach (SpriteRenderer s in spr)
                        {
                            s.sortingLayerName = "UI";
                        }

                        GetComponent<PlayerControl>().enabled = false;

                        GetComponentInChildren<Gun>().enabled = false;

                        anim.SetTrigger("Die");
                    }
                }
            }
        }


        void TakeDamage(Transform enemy)
        {
            playerControl.jump = false;

            Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;

            GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);

            health -= damageAmount;

            UpdateHealthBar();

            int i = Random.Range(0, ouchClips.Length);
            AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);
        }


        public void UpdateHealthBar()
        {
            healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);

            healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);
        }
    }
}