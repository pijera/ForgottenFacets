using ForgottenFacets.Content.Materials.Essences;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Core
{
    internal class VanillaEditedDrops : GlobalNPC
    {
        private static readonly HashSet<int> WaterEssenceDroppers = new()//water essence,sapphire
        {
            //pre-hardmode
            NPCID.BlueJellyfish,
            NPCID.Crab,
            NPCID.ZombieEskimo,NPCID.ArmedZombieEskimo,
            NPCID.IceBat,
            NPCID.IceSlime,
            NPCID.BlueJellyfish,NPCID.PinkJellyfish,NPCID.GreenJellyfish, NPCID.BloodJelly,NPCID.FungoFish,
            NPCID.Piranha,
            NPCID.SeaSnail,
            NPCID.Shark,
            NPCID.SnowFlinx,
            NPCID.SpikedIceSlime,
            NPCID.Squid,
            NPCID.UndeadViking,

            //hardmode
            NPCID.AnglerFish,
            NPCID.Arapaima,
            NPCID.ArmoredViking,
            NPCID.BloodFeeder,
            NPCID.IceElemental,
            NPCID.IceMimic,
            NPCID.IceTortoise,
            NPCID.IcyMerman,
            NPCID.PigronCorruption,NPCID.PigronCrimson,NPCID.PigronHallow,
            NPCID.ZombieMerman

        };
        public static readonly HashSet<int> FireEssenceDroppers = new()//fire essence,ruby
        {
            //pre-hardmode
            NPCID.BoneSerpentHead,
            NPCID.Demon,
            NPCID.FireImp,
            NPCID.Hellbat,
            NPCID.LavaSlime,
            NPCID.VoodooDemon,
            NPCID.MeteorHead,

            //hardmode

            NPCID.HellArmoredBones,NPCID.HellArmoredBonesMace,NPCID.HellArmoredBonesSpikeShield,NPCID.HellArmoredBonesSword,
            NPCID.Lavabat,
            NPCID.RedDevil,
            534,
        };
        public static readonly HashSet<int> EarthEssenceDroppers = new()//earth essence,topaz
        {
            //pre-hardmode
            NPCID.BlackSlime,
            NPCID.CaveBat,
            NPCID.CochinealBeetle,
            NPCID.Crawdad,NPCID.Crawdad2,
            NPCID.CyanBeetle,
            NPCID.GiantShelly,NPCID.GiantShelly2,
            NPCID.GiantWormHead,
            NPCID.GraniteFlyer,
            NPCID.GraniteGolem,
            481,//hoplite
            NPCID.MotherSlime,
            NPCID.Nymph,
            NPCID.Salamander,NPCID.Salamander2,NPCID.Salamander3,NPCID.Salamander4,NPCID.Salamander5,NPCID.Salamander6,NPCID.Salamander7,NPCID.Salamander8,NPCID.Salamander9,
            NPCID.Skeleton,NPCID.BoneThrowingSkeleton,NPCID.SmallSkeleton,NPCID.BigSkeleton,
            NPCID.HeadacheSkeleton,NPCID.BoneThrowingSkeleton2,NPCID.SmallHeadacheSkeleton,NPCID.BigHeadacheSkeleton,
            NPCID.MisassembledSkeleton,NPCID.BoneThrowingSkeleton3,NPCID.SmallMisassembledSkeleton, NPCID.BigMisassembledSkeleton,
            NPCID.PantlessSkeleton,NPCID.BoneThrowingSkeleton4,NPCID.BigPantlessSkeleton,NPCID.SmallPantlessSkeleton,
            NPCID.UndeadMiner,
            NPCID.WallCreeper,

            //hardmode
            NPCID.ArmoredSkeleton, NPCID.HeavySkeleton,
            NPCID.BlackRecluse,
            NPCID.GiantBat,
            NPCID.Medusa,
            NPCID.RockGolem,
            NPCID.SkeletonArcher,
            NPCID.ToxicSludge


        };
        public static readonly HashSet<int> LifeEssenceDroppers = new()//life essence,emerald
        {
            //pre-hardmode
            NPCID.AnomuraFungus,
            NPCID.DoctorBones,
            NPCID.FungiBulb,
            NPCID.Hornet,NPCID.LittleStinger,NPCID.BigStinger,NPCID.HornetFatty,NPCID.LittleHornetFatty,NPCID.BigHornetFatty,NPCID.HornetHoney,NPCID.LittleHornetHoney,NPCID.BigHornetHoney,NPCID.HornetLeafy,NPCID.LittleHornetLeafy,
            NPCID.BigHornetLeafy,NPCID.HornetSpikey,NPCID.LittleHornetSpikey,NPCID.BigHornetSpikey,NPCID.HornetStingy,NPCID.BigHornetStingy,NPCID.LittleHornetStingy,
            NPCID.JungleBat,
            NPCID.JungleSlime,
            NPCID.LacBeetle,
            NPCID.ManEater,
            NPCID.MushiLadybug,
            NPCID.Snatcher,
            NPCID.SpikedJungleSlime,
            NPCID.SporeBat,
            NPCID.SporeSkeleton,
            NPCID.ZombieMushroom,NPCID.ZombieMushroomHat,

            //hardmode
            NPCID.AngryTrapper,
            NPCID.Derpling,
            NPCID.FlyingSnake,
            NPCID.GiantFlyingFox,
            NPCID.GiantTortoise,
            NPCID.JungleCreeper,
            NPCID.Lihzahrd,
            NPCID.MossHornet,NPCID.BigMossHornet,NPCID.GiantMossHornet,NPCID.LittleMossHornet,NPCID.TinyMossHornet,
            NPCID.Moth

        };
        public static readonly HashSet<int> StormEssenceDroppers = new()//storm essence,diamond
        {
            //pre-hardmode
            NPCID.Harpy,
            NPCID.Dandelion,

            //hardmode
            NPCID.WyvernHead,
            NPCID.AngryNimbus,
            NPCID.FlyingFish,
            NPCID.ZombieRaincoat,
            NPCID.UmbrellaSlime
        };
        public static readonly HashSet<int> ArcaneEssenceDroppers = new()//arcane essence,amethyst
        {
            //pre-hardmode
            NPCID.AngryBones, NPCID.AngryBonesBig, NPCID.AngryBonesBigHelmet, NPCID.AngryBonesBigMuscle, NPCID.BigBoned, NPCID.ShortBones,
            NPCID.CursedSkull,
            NPCID.DarkCaster,
            NPCID.DungeonGuardian,
            NPCID.DungeonSlime,
            NPCID.Ghost,
            NPCID.Gnome,
            NPCID.Tim,
            NPCID.GoblinSorcerer,

            //hardmode
            NPCID.BlueArmoredBones,NPCID.BlueArmoredBonesMace,NPCID.BlueArmoredBonesNoPants,NPCID.BlueArmoredBonesSword,
            NPCID.BoneLee,
            NPCID.BigMimicCorruption,NPCID.BigMimicCrimson,NPCID.BigMimicHallow,
            NPCID.CultistArcherBlue,
            NPCID.DiabolistRed,NPCID.DiabolistWhite,
            NPCID.DungeonSpirit,
            NPCID.GiantCursedSkull,
            NPCID.HoppinJack,
            NPCID.HellArmoredBones,NPCID.HellArmoredBonesMace,NPCID.HellArmoredBonesSpikeShield,NPCID.HellArmoredBonesSword,
            NPCID.CultistDevote,
            NPCID.Necromancer,NPCID.NecromancerArmored,
            NPCID.Paladin,
            NPCID.RaggedCaster,NPCID.RaggedCasterOpenCoat,
            NPCID.RuneWizard,
            NPCID.RustyArmoredBonesAxe,NPCID.RustyArmoredBonesFlail,NPCID.RustyArmoredBonesSwordNoArmor,NPCID.RustyArmoredBonesSword,
            NPCID.SkeletonCommando,
            NPCID.SkeletonSniper,
            NPCID.TacticalSkeleton,
            NPCID.GoblinSummoner
        };
        public static readonly HashSet<int> AncientEssenceDroppers = new()//ancient essence,amber
        {
            //pre-hardmode
            NPCID.Antlion,
            NPCID.GiantWalkingAntlion,NPCID.WalkingAntlion,
            NPCID.FlyingAntlion, NPCID.GiantFlyingAntlion,
            NPCID.SandSlime,
            NPCID.TombCrawlerHead,

            //hardmode
            532,//Basilisk
            NPCID.BloodMummy,NPCID.DarkMummy,NPCID.Mummy,NPCID.LightMummy,
            NPCID.DesertDjinn,
            NPCID.DesertGhoul, NPCID.DesertGhoulCorruption, NPCID.DesertGhoulCrimson, NPCID.DesertGhoulHallow,
            NPCID.DuneSplicerHead,
            NPCID.DesertLamiaDark,NPCID.DesertLamiaLight,
            530,531,//skorpijon bumass
            NPCID.Tumbleweed,
            NPCID.SandShark,NPCID.SandsharkCorrupt,NPCID.SandsharkCrimson,NPCID.SandsharkHallow
        };

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (WaterEssenceDroppers.Contains(npc.type))
            {
                IItemDropRule classicRule = ItemDropRule.Common(ModContent.ItemType<WaterEssence>(), 5, 1, 2);
                IItemDropRule expertRule = ItemDropRule.Common(ModContent.ItemType<WaterEssence>(), 4, 1, 2);

                IItemDropRule difficultyScaling = new DropBasedOnExpertMode(classicRule, expertRule);

                npcLoot.Add(difficultyScaling);
            }

            if (FireEssenceDroppers.Contains(npc.type))
            {
                IItemDropRule classicRule = ItemDropRule.Common(ModContent.ItemType<FireEssence>(), 5, 1, 2);
                IItemDropRule expertRule = ItemDropRule.Common(ModContent.ItemType<FireEssence>(), 4, 1, 2);

                IItemDropRule difficultyScaling = new DropBasedOnExpertMode(classicRule, expertRule);

                npcLoot.Add(difficultyScaling);
            }

            if (EarthEssenceDroppers.Contains(npc.type))
            {
                IItemDropRule classicRule = ItemDropRule.Common(ModContent.ItemType<EarthEssence>(), 5, 1, 2);
                IItemDropRule expertRule = ItemDropRule.Common(ModContent.ItemType<EarthEssence>(), 4, 1, 2);

                IItemDropRule difficultyScaling = new DropBasedOnExpertMode(classicRule, expertRule);

                npcLoot.Add(difficultyScaling);
            }

            if (LifeEssenceDroppers.Contains(npc.type))
            {
                IItemDropRule classicRule = ItemDropRule.Common(ModContent.ItemType<LifeEssence>(), 5, 1, 2);
                IItemDropRule expertRule = ItemDropRule.Common(ModContent.ItemType<LifeEssence>(), 4, 1, 2);

                IItemDropRule difficultyScaling = new DropBasedOnExpertMode(classicRule, expertRule);

                npcLoot.Add(difficultyScaling);
            }

            if (StormEssenceDroppers.Contains(npc.type))
            {
                IItemDropRule classicRule = ItemDropRule.Common(ModContent.ItemType<StormEssence>(), 5, 1, 2);
                IItemDropRule expertRule = ItemDropRule.Common(ModContent.ItemType<StormEssence>(), 4, 1, 2);

                IItemDropRule difficultyScaling = new DropBasedOnExpertMode(classicRule, expertRule);

                npcLoot.Add(difficultyScaling);
            }
            if (ArcaneEssenceDroppers.Contains(npc.type))
            {
                IItemDropRule classicRule = ItemDropRule.Common(ModContent.ItemType<ArcaneEssence>(), 5, 1, 2);
                IItemDropRule expertRule = ItemDropRule.Common(ModContent.ItemType<ArcaneEssence>(), 4, 1, 2);

                IItemDropRule difficultyScaling = new DropBasedOnExpertMode(classicRule, expertRule);

                npcLoot.Add(difficultyScaling);
            }
            if (AncientEssenceDroppers.Contains(npc.type))
            {
                IItemDropRule classicRule = ItemDropRule.Common(ModContent.ItemType<AncientEssence>(), 5, 1, 2);
                IItemDropRule expertRule = ItemDropRule.Common(ModContent.ItemType<AncientEssence>(), 4, 1, 2);

                IItemDropRule difficultyScaling = new DropBasedOnExpertMode(classicRule, expertRule);

                npcLoot.Add(difficultyScaling);
            }
        }
    }
}
