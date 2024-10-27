using System.Collections;
using System.Linq;
using System.Collections.Generic;
using EntityStates;
using RoR2;
using RoR2.Navigation;
using UnityEngine;


namespace EntityStates.Wlurm
{

    public class WlurmIdle : BaseState
    {

        private Transform modelTransform;
        private CharacterModel model;
        private HurtBoxGroup hurtBoxGroup;
        private ChildLocator childLocator;

        private int originalLayer;

        public override void OnEnter()
        {
            base.OnEnter();
            modelTransform = GetModelTransform();
            if (modelTransform)
            {
                model = modelTransform.GetComponent<CharacterModel>();
                hurtBoxGroup = modelTransform.GetComponent<HurtBoxGroup>();
                childLocator = modelTransform.GetComponent<ChildLocator>();
            }

            if (model)
            {
                model.invisibilityCount++;
            }

            if (hurtBoxGroup)
            {
                HurtBoxGroup hurtboxes = hurtBoxGroup;
                int deactivatorCount = hurtboxes.hurtBoxesDeactivatorCounter + 1;
                hurtboxes.hurtBoxesDeactivatorCounter = deactivatorCount;
            }

            if (base.characterMotor)
            {
                base.characterMotor.enabled = false;
            }

            originalLayer = base.gameObject.layer;
            base.gameObject.layer = LayerIndex.GetAppropriateFakeLayerForTeam(base.teamComponent.teamIndex).intVal;
            base.characterMotor.Motor.RebuildCollidableLayers();
        }

        public override void OnExit()
        {
            base.OnExit();
            ExitCleanup();
        }

        private void ExitCleanup()
        {
            if (model)
            {
                model.invisibilityCount--;
            }

            if (hurtBoxGroup)
            {
                HurtBoxGroup group = hurtBoxGroup;
                int hurtBoxesDeactivatorCounter = group.hurtBoxesDeactivatorCounter - 1;
                group.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
            }

            if (base.characterMotor)
            {
                base.characterMotor.enabled = true;
            }
        }
    }
}
