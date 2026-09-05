using ForgottenFacets.Content.Materials;
using ForgottenFacets.Core;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace ForgottenFacets.Content.Tiles
{
    internal class AncientIceOreTile : ModTile
    {
        public override string Texture => "ForgottenFacets/Assets/Tiles/AncientIce/AncientIceOreTile";

        public override void SetStaticDefaults()
        {
            this.SetUpOre(ModContent.ItemType<AncientIceOre>(), Color.DarkGreen, "Ancient Ice");
            Main.tileOreFinderPriority[Type] = 410;
            DustType = DustID.Silver;
            MineResist = 2.5f;

            MinPick = 65;//demonite and crimtane
            Main.tileLighted[Type] = true;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.09f;
            b = 0.09f;
            g = 0.09f;
        }

    }

    public class AncientIceOreSystem : ModSystem
    {
        public static LocalizedText AncientIceOreText { get; private set; }

        public override void SetStaticDefaults()
        {
            AncientIceOreText = Mod.GetLocalization($"WorldGen.{nameof(AncientIceOreText)}");
        }

        public static void SpawnOre()
        {

            if (Main.netMode == NetmodeID.SinglePlayer || Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(AncientIceOreText.ToNetworkText(), new Color(130, 200, 255));
            }

            for (int i = 0; i < (int)(Main.maxTilesX * Main.maxTilesY * 0.001); i++)
            {
                int x = Main.rand.Next(0, Main.maxTilesX);
                int y = Main.rand.Next((int)Main.worldSurface + 350, Main.maxTilesY);

                Tile tile = Framing.GetTileSafely(x, y);

                if ((tile.HasTile && tile.TileType == TileID.IceBlock) && Main.rand.NextBool(2))
                {
                    WorldGen.TileRunner(x, y, Main.rand.Next(6, 8), Main.rand.Next(4, 7), ModContent.TileType<AncientIceOreTile>());
                }
            }
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
    }

    public class AncientIceGlobalNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.EaterofWorldsHead ||
                npc.type == NPCID.EaterofWorldsBody ||
                npc.type == NPCID.EaterofWorldsTail)
            {
                bool anotherSegmentAlive = false;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC otherNpc = Main.npc[i];

                    if (i != npc.whoAmI && otherNpc.active)
                    {
                        if (otherNpc.type == NPCID.EaterofWorldsHead ||
                            otherNpc.type == NPCID.EaterofWorldsBody ||
                            otherNpc.type == NPCID.EaterofWorldsTail)
                        {
                            anotherSegmentAlive = true;
                            break;
                        }
                    }
                }
                if (!anotherSegmentAlive && !NPC.downedBoss2)
                {
                    NPC.downedBoss2 = true;
                    AncientIceOreSystem.SpawnOre();
                }
            }
            if (npc.type == NPCID.BrainofCthulhu && !NPC.downedBoss2)
            {
                NPC.downedBoss2 = true;
                AncientIceOreSystem.SpawnOre();
            }


        }
    }

}
