using RoR2;
using UnityEngine;

namespace EntityStates.Wlurm
{
    public class WlurmSpawn : BaseState
    {
        private float stopwatch;
        public static float duration = 4f;


        public override void OnEnter()
        {
            base.OnEnter();
            PlayAnimation("Body", "Spawn", "Spawn.playbackRate", duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch > duration && base.isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}
