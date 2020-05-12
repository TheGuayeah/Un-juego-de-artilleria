using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PEC3
{
    public class PlayerControl : MonoBehaviour
    {
        [HideInInspector]
        public bool facingRight = true;
        [HideInInspector]
        public bool jump;

        public float moveForce = 365f;
        public float maxSpeed = 5f;
        public AudioClip[] jumpClips;
        public float jumpForce = 1000f;
        public AudioClip[] taunts;
        public float tauntProbability = 50f;
        public float tauntDelay = 1f;

        private int tauntIndex;
        private Transform groundCheck;
        private bool grounded = false;
        private Animator anim;

        protected float tilt;
        private readonly List<KeyCode> actions = new List<KeyCode>();
        private Transform pivot;

        public Gun gun;
        public bool IAmAnEnemy;

        [HideInInspector]
        public bool HasTurn;

        private void Awake()
        {
            groundCheck = transform.Find("groundCheck");
            anim = GetComponent<Animator>();
            pivot = transform.Find("Pivot");
        }
        
        private void FixedUpdate()
        {
            var h = Input.GetAxis("Horizontal");

            if (h == 0)
            {
                if (actions.Contains(KeyCode.LeftArrow))
                    h = anim.GetFloat("Speed") - 0.1f;

                if (actions.Contains(KeyCode.RightArrow))
                    h = anim.GetFloat("Speed") + 0.1f;
            }

            if (actions.Contains(KeyCode.UpArrow) || actions.Contains(KeyCode.W))
                tilt += 1.0f;
            if (actions.Contains(KeyCode.DownArrow) || actions.Contains(KeyCode.S))
                tilt -= 1.0f;

            tilt = Mathf.Clamp(tilt, 0, 75);
            pivot.rotation = Quaternion.Euler(0, 0, tilt);
        }

        private void ActualizarAccionDown(KeyCode code)
        {
            if (!actions.Contains(code))
                actions.Add(code);
        }

        private void ActualizarAccionUp(KeyCode code)
        {
            if (actions.Contains(code))
                actions.Remove(code);
        }

        private void ActualizarAccionTeclado(KeyCode code)
        {
            if (Input.GetKeyDown(code))
                ActualizarAccionDown(code);

            if (Input.GetKeyUp(code))
                ActualizarAccionUp(code);
        }

        public void MueveDerechaDown()
        {
            ActualizarAccionDown(KeyCode.RightArrow);
        }

        public void MueveIzquierdaDown()
        {
            ActualizarAccionDown(KeyCode.LeftArrow);
        }

        public void RotaDerechaDown()
        {
            ActualizarAccionDown(KeyCode.DownArrow);
        }

        public void RotaIzquierdaDown()
        {
            ActualizarAccionDown(KeyCode.UpArrow);
        }

        public void MueveDerechaUp()
        {
            ActualizarAccionUp(KeyCode.RightArrow);
        }

        public void MueveIzquierdaUp()
        {
            ActualizarAccionUp(KeyCode.LeftArrow);
        }

        public void RotaDerechaUp()
        {
            ActualizarAccionUp(KeyCode.DownArrow);
        }

        public void RotaIzquierdaUp()
        {
            ActualizarAccionUp(KeyCode.UpArrow);
        }

    }
}