using System.Linq;
using TMPro;
using UnityEngine;

namespace IEye.RRP
{
    public class AnimatorDebugger: MonoBehaviour
    {
        private Animator animator;
        int pitchIndex;
        int yawIndex;

        public void Start()
        {
            animator = GetComponent<Animator>();
            pitchIndex = animator.GetLayerIndex("AimPitch");
            yawIndex = animator.GetLayerIndex("AimYaw");
        }

        public void Update()
        {
            RRPLog.Message("aimYawCycle is " + animator.GetFloat("aimYawCycle"));
            RRPLog.Message("aimPitchCycle is " + animator.GetFloat("aimPitchCycle"));
            RRPLog.Message("aimYawCycle State is " + animator.GetCurrentAnimatorStateInfo(yawIndex).shortNameHash);
            RRPLog.Message("real aimYawCycle is " + Animator.StringToHash("YawControl"));
        }
    }
}