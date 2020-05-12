using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PEC3
{
    public class Gun : MonoBehaviour
    {
        public Rigidbody2D Rocket;
        public float Speed = 20f;

        private Animator anim;
        public Sprite AttackBarSprite;

        private Transform attackBar;


        private enum States
        {
            Down,
            Fire,
            Up
        }
        private States state = States.Up;

        private void Awake()
        {
            anim = transform.root.gameObject.GetComponent<Animator>();

            var attackBarObject = new GameObject("Power");
            attackBar = attackBarObject.transform;
            attackBar.SetParent(transform);
            attackBar.localPosition = Vector3.zero;
            attackBar.localRotation = Quaternion.identity;
            attackBar.localScale = Vector3.up * 2 + Vector3.forward;

            var spriteRenderer = attackBarObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = AttackBarSprite;
            spriteRenderer.sortingLayerID = transform.root.GetComponentInChildren<SpriteRenderer>().sortingLayerID;
        }


        private void Update()
        {
            switch (state)
            {
                case States.Down:
                    Speed += Time.deltaTime * 30;
                    attackBar.localScale += Vector3.right * 0.01f;
                    attackBar.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.green, Color.red, attackBar.localScale.x);
                    break;

                case States.Fire:
                    Fire();
                    state = States.Up;
                    break;

                default:
                    break;
            }
        }


        public void FireUp()
        {
            state = States.Fire;
        }

        public void FireDown()
        {
            state = States.Down;
        }

        private void Fire()
        {
            anim.SetTrigger("Shoot");
            GetComponent<AudioSource>().Play();

            var bulletInstance = Instantiate(Rocket, transform.position, transform.rotation);
            bulletInstance.velocity = transform.right.normalized * Speed;

            Speed = 0;
            attackBar.localScale = Vector3.up * 2 + Vector3.forward;
        }
    }

}
