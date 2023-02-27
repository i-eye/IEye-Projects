/*
Generated from a ROR2EK Template. Feel free to remove this comment section.
0 = modName; 1 = Nicified mod name; 2 = authorName; 3 = using clauses; 4 = attributes; 
*/

using BepInEx;
using R2API;
using R2API.ScriptableObjects;
using R2API.Utils;
using R2API.ContentManagement;
using UnityEngine;

namespace IEye.RulersOfTheRedPlane
{
    [BepInDependency("com.TeamMoonstorm.MoonstormSharedUtils")]
    [BepInDependency("com.bepis.r2api.artifactcode")]
    [BepInDependency("com.bepis.r2api.colors")]
    [BepInDependency("com.bepis.r2api.commandhelper")]
    [BepInDependency("com.bepis.r2api.content_management")]
    [BepInDependency("com.bepis.r2api")]
    [BepInDependency("com.bepis.r2api.damagetype")]
    [BepInDependency("com.bepis.r2api.deployable")]
    [BepInDependency("com.bepis.r2api.difficulty")]
    [BepInDependency("com.bepis.r2api.director")]
    [BepInDependency("com.bepis.r2api.dot")]
    [BepInDependency("com.bepis.r2api.elites")]
    [BepInDependency("com.bepis.r2api.items")]
    [BepInDependency("com.bepis.r2api.language")]
    [BepInDependency("com.bepis.r2api.loadout")]
    [BepInDependency("com.bepis.r2api.lobbyconfig")]
    [BepInDependency("com.bepis.r2api.networking")]
    [BepInDependency("com.bepis.r2api.orb")]
    [BepInDependency("com.bepis.r2api.prefab")]
    [BepInDependency("com.bepis.r2api.recalculatestats")]
    [BepInDependency("com.bepis.r2api.rules")]
    [BepInDependency("com.bepis.r2api.sceneasset")]
    [BepInDependency("com.bepis.r2api.sound")]
    [BepInDependency("com.bepis.r2api.tempvisualeffect")]
    [BepInDependency("com.bepis.r2api.unlockable")]

	[BepInPlugin(GUID, MODNAME, VERSION)]
	public class RulersOfTheRedPlaneMain : BaseUnityPlugin
	{
		public const string GUID = "com.I_Eye.RulersOfTheRedPlane";
		public const string MODNAME = "Rules of the Red Plane";
		public const string VERSION = "0.0.1";

		public static RulersOfTheRedPlaneMain Instance { get; private set; }
        public static PluginInfo pluginInfo;

		private void Awake()
		{
			Instance = this;
            pluginInfo = Info;
            DefNotSS2Log.logger = Logger;

            new RRPAssets().Init();
		}	
	}
}