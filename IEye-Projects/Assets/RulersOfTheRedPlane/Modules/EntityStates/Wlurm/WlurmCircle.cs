using System.Collections;
using System.Collections.Generic;
using EntityStates;
using UnityEngine;
using RoR2;

namespace EnitityStates.Wlurm
{
    public class WlurmCircle : BaseState
    {
        public static GameObject laserProjectile;
        public static GameObject[] tentacleEnds;
        public static float duration;
        public static float baseTimeBetweenShots;

        private float timer;
        private float timeBetweenShots;
        private Animator animator;
        private int tentacleAmount;

        public override void OnEnter()
        {
            base.OnEnter();
            timeBetweenShots = baseTimeBetweenShots / attackSpeedStat;
            animator = GetModelAnimator();
            tentacleAmount = tentacleEnds.Length;
        }

        public override void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;
            if (timer >= timeBetweenShots)
            {
                Transform location = tentacleEnds[(int) Random.Range(0, tentacleAmount)].transform;
                FireProjectile(location);
            }
            base.FixedUpdate();
        }

        public void FireProjectile(Transform location)
        {
            Util.GetEnemyEasyTarget(characterBody, GetAimRay(), 60, 180);
        }
    }
}
