using UnityEngine;

namespace PEC3
{
    public class EnemyController : PlayerControl
    {
        private Transform hero;
        private Gun gun;
        private float shootingAngle = 30.0f;

        private void Start()
        {
            hero = GameObject.FindGameObjectWithTag("Player").transform;
            gun = GetComponentInChildren<Gun>();
        }

        private void LateUpdate()
        {
            if (HasTurn)
            {
                if (facingRight)
                    Flip();

                if (tilt != shootingAngle)
                {
                    if (tilt > shootingAngle)
                        RotaDerechaDown();
                    else
                        RotaIzquierdaDown();
                }
                else
                {
                    gun.Fire(CalculateVelocity(hero, shootingAngle));
                    RotaDerechaUp();
                    RotaIzquierdaUp();
                }
            }
            else
            {
                shootingAngle = Random.Range(25, 45);
            }
        }

        private float CalculateVelocity(Transform target, float angle)
        {
            var dir = target.position - transform.position;
            var h = dir.y;
            dir.y = 0;
            var dist = dir.magnitude;
            var a = angle * Mathf.Deg2Rad;

            dist += h / Mathf.Tan(a);

            return Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a)) * Random.Range(1.2f, 1.8f) * 3;
        }

    }

}