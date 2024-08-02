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
            RRPMain.logger.LogMessage("aimYawCycle is " + animator.GetFloat("aimYawCycle"));
            RRPMain.logger.LogMessage("aimPitchCycle is " + animator.GetFloat("aimPitchCycle"));
            RRPMain.logger.LogMessage("aimYawCycle State is " + animator.GetCurrentAnimatorStateInfo(yawIndex).shortNameHash);
            RRPMain.logger.LogMessage("real aimYawCycle is " + Animator.StringToHash("YawControl"));
        }
    }
}