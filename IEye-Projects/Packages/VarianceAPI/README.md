# VarianceAPI

VarianceAPI  is a complete rewrite of Rob's original Monster Variants concept, the tools inside the API allows you to create Variants for existing Characters in the game, want a lemurian that's twice the size and spits out lasers? Want a golem that's blazing fast and spawns enemies when clapping? All of this and more are possible with VarianceAPI

## Features

### Thunderkit Ease of Development.

VarianceAPI was made with ThunderKit in mind, as such, almost all of it's systems are made with it in mind, The way how variants are defined and created are done via ScriptableObjects, While Variants can be created entirely in code (untested, report a bug if this isnt the case), VarianceAPI doesnt give any kind of official support for doing this.

### Lobby Rules and VariantPacks

VarianceAPI comes bundled with VariantPacks, each VariantPack contains the Variants that are going to be added to the game alongside other tidbits, thanks to this system, it is completely possible to use the Lobby's rule system to enable and disable entire Packs together, or Variants themselves with a config change.

![](https://cdn.discordapp.com/attachments/850538397647110145/1077369013656621116/image.png)

### Custom Tiers

With the advent of ItemTierDefs, VAPI 2.0 adds VariantTierDefs, which allows you to create your own Tiers for your custom variants, the tier def itself is not sealed and you're intended to subclass it to add more functionality to it

![](https://cdn.discordapp.com/attachments/850538397647110145/1061053456976183467/image.png)

### Less Tight Coupling

VarianceAPI's systems are now less coupled together, You can now get the VariantDefs from a body using the BodyVariantDefProvider, and create custom reward logic for tiers using the VariantReward class.

### Custom Spawning Rules

VarianceAPI 2.0 introduces the VariantSpawningCondition scriptable object, which allows you to specify a collection of criteria that need to be met for a Variant to spawn.

![](https://cdn.discordapp.com/attachments/850538397647110145/1061053863538466867/image.png)

### Complete Spawn Control

VarianceAPI 2.0 introduces the VariantSummon class, an extension of MasterSummon that allows you to carefully control a custom spawned variant, do you want an enemy that when killed spawns another enemy with a specific Variant? want a skill that spawns enemies but said enemies are never variants? VariantSummon allows you to do this.

### Console Commands

VAPI adds new Console commands to VarianceAPI, which allows you to debug test variants, the commands are the following:

| Command | Arguments | Effect |
|--|--|--|
| vapi_list_bodies | none | Lists all the Bodies that have VariantDefs associated with them |
| vapi_list_variants | body name | Lists all the variants associated with the specified body |
| vapi_spawn_ai | Argument 1: Master Name - Argument 2: Variants | Spawns the specified Master with the VariantDefs specified active  |
| vapi_spawn_as | Argument 1: Body Name - Argument 2: Variants | Spawns you as the specified body with the specified VariantDefs active |

### Editor Utilities

It is extremely recommended to use the Github repo version of the API for development on ThunderKit, as said version comes bundled with an Editor assembly that simplifies and gives utilities on creating new Variants

### MSU Implementation

VarianceAPI depends on MSU, this is done so the API can fully leverage from the existing MSU systems to create the custom variant items alongside other utilities found on MSU, despite this, it is not necesary to touch MSU in any way shape or form to create custom variants.

## Documentation

The current 2.0 version of VarianceAPI does not have Documentation on the github's wiki, however, VarianceAPI 2.0 does come with XML documentation alongside unity Tooltips. Any questions can be asked on nebby's discord server.

## Official Variant Packs (Variant Packs made by Nebby)

Below are the official VariantPacks that where made by Nebby. you can click the icon and a new tab will open showing the variant pack's thunderstore page.

### The Original 30

The original 30 is a complete port of Rob's original 30 Monster Variants, it includes a plethora of QoL changes such as using intrinsic items, better balancing and more.

[![TO30 Icon](https://media.discordapp.net/attachments/850538397647110145/1077361557295616051/icon.png)](https://thunderstore.io/package/Nebby/VariantPack_TheOriginal30/)

### Nebby's Wrath

Nebby's Wrath is a complete port of the Additional variants of the MonsterVariantsPlus addon.

[![NW Icon](https://cdn.discordapp.com/attachments/850538397647110145/1077366233994895431/icon.png)](https://thunderstore.io/package/Nebby/VariantPack_NebbysWrath/)


### Community Made Variants
<details><summary>(Click me!)</summary>
<p>

(Note: click the icon to open a new tab to the variant pack)
| Icon/URL | Name | Description |
|--|--|--|
|[![ShbonesIcon](https://gcdn.thunderstore.io/live/repository/icons/shbones-ShbonesVariants-0.0.2.png.128x128_q95.png)](https://thunderstore.io/package/shbones/ShbonesVariants/)| Shbones Variants | Adds new monster variants to the game using Nebby's VarianceAPI. |

</p>
</details>

### Official Nebby's Mods discord server.

* If you wish to contact me about my risk of rain mods, you can do so here in this Discord server.

https://discord.gg/kWn8T4fM5W

### Donations

If you like what i do, and wish to help me out, consider donating on my Ko-fi

[![ko-fi](https://media.discordapp.net/attachments/850538397647110145/994431434817273936/SupportNebby.png)](https://ko-fi.com/nebby1999)

### Special thanks.

* Kevin for the EntityStateDrawer, which was used as a base for the component drawer. (And making me not use thunderkit like an ape)
* IDeathHD and Harb, for making DebugToolkit and it's spawn_ai and spawn_as commands (used for the spawn_variant and spawn_as_variant)
* IDeathHD for helping me point towards a general direction with networking.
* Aaron, Gaforb, "come on and SLAM", & especially TheTimeSweeper ~~Love you habibi~~ for helping me with networking issues.
* Aaron for creating a weighted selection for the Unique variants.
* Dotflare for making the Variance artifact token and other tidbits from the official variant packs.
* PassivePicasso for Thunderkit and helping me a lot with certain editor scripts.
* Rob for creating MonsterVariants.
* Papa Zach for creating the Artifact & Expansion icon