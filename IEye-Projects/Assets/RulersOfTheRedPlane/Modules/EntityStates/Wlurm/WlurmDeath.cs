using System.Collections;
using System.Collections.Generic;
using EntityStates;
using IEye.RRP.Monsters;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.Wlurm
{
    public class WlurmDeath : GenericCharacterDeath
    {
        public float duration;
        public override void PlayDeathAnimation(float crossfadeDuration = 0.1f)
        {
            PlayCrossfade("Body","Death","Death.playbackRate",duration,crossfadeDuration);
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration)
            {
                DestroyModel();
                if (NetworkServer.active)
                {
                    DestroyBodyAsapServer();
                }
            }
        }
        
        public override void OnExit()
        {
            DestroyModel();
            base.OnExit();
        }
    }
}
