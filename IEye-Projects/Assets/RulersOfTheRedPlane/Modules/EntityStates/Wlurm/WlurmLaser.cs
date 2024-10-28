
using IL.RoR2.CharacterAI;
using IL.RoR2.EntityLogic;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using EntityStates.EngiTurret.EngiTurretWeapon;
using UnityEngine;
using System;

namespace EntityStates.Wlurm
{
    public class WlurmLaser : FireBeam
    {
        [SerializeField]
        public float duration;

        [SerializeField]
        public float aimMaxSpeed;


        private AimAnimator.DirectionOverrideRequest overrideRequest;
        private Vector3 aimDirection;
        private Vector3 aimVelocity;

        public override void OnEnter()
        {
            base.OnEnter();
            PlayAnimation("FullBody","LaserShoot");
            AimAnimator component = GetComponent<AimAnimator>();
            if (component)
            {
                overrideRequest = component.RequestDirectionOverride(GetAimDirection);
            }

            aimDirection = GetTargetDirection();
        }

        public override void FixedUpdate()
        {
            aimDirection = Vector3.RotateTowards(aimDirection, GetTargetDirection(), aimMaxSpeed * (MathF.PI / 180f) * GetDeltaTime(), float.PositiveInfinity);
        }

        public override bool ShouldFireLaser()
        {
            return duration > base.fixedAge;
        }

        private Vector3 GetAimDirection()
        {
            return aimDirection;
        }
        private Vector3 GetTargetDirection()
        {
            if (base.inputBank)
            {
                return inputBank.aimDirection;
            }
            return base.transform.forward;
        }

        public override Ray GetLaserRay()
        {
            if (base.inputBank)
            {
                return new Ray(base.inputBank.aimOrigin, aimDirection);
            }
            return new Ray(base.transform.position, aimDirection);
        }
        
    }
}

