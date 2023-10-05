using System.Collections;
using UnityEngine;
using RoR2;
using RoR2.Items;
using Moonstorm;
using System.Collections.Generic;
using R2API.Networking;
using Moonstorm.Loaders;
using BepInEx.Logging;

namespace IEye.RRP.Items
{
    [DisabledContent]
    public class PoisonIvy : ItemBase
    {

        private const string token = "RRP_ITEM_IVY_DESC";
        public static float percentHealthNeeded = 15f;

        public static float halfDistance = 15f;
        public override ItemDef ItemDef => RRPAssets.LoadAsset<ItemDef>("PoisonIvy", RRPBundle.Items);

        public sealed class Behavior : BaseItemBodyBehavior, IOnDamageDealtServerReceiver
        {
            public static float damageNeeded;
            public static float healthTracker;
            public static List<BuffDef> buff;
            int count;

            private void Start()
            {
                
                buff = new List<BuffDef>
                {
                    RoR2Content.Buffs.SmallArmorBoost,
                    RRPContent.Buffs.IvyPower,
                    RRPContent.Buffs.IvyPower,


                };
                count = buff.Count;
            }
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => RRPContent.Items.PoisonIvy;

            public void OnDamageDealtServer(DamageReport report)
            {
                
                float damage = 0;
                float distance = Vector3.Distance(report.victimBody.transform.position, body.transform.position);
                damage = report.damageDealt / report.victim.fullCombinedHealth;
                if (distance > halfDistance) damage /= 2;
                if (report.victimIsChampion) damage *= 2f;
                if (report.victimIsElite) damage *= 1.25f;
                if (report.victimIsBoss) damage *= 5f;
                if (report.victimBody.baseNameToken == "Mithrix" || report.victimBody.baseNameToken == "Voidling") damage *= 8f;
                healthTracker += damage;
                if (healthTracker > percentHealthNeeded) BuffGive();
                
            }
            public void BuffGive()
            {
                int selection = Random.Range(0, count);
                body.AddTimedBuffAuthority(buff[selection].buffIndex, 5f);
            }
        }
    }
}


