
using IL.RoR2.CharacterAI;
using IL.RoR2.EntityLogic;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityStates.Wlurm
{
    public class Laser : BaseSkillState
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
            }
            base.FixedUpdate();
        }

        public void FireProjectile(Transform location)
        {
            Util.GetEnemyEasyTarget(characterBody, GetAimRay(), 60, 180);
        }
    }
}

