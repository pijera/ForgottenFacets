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
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ForgottenFacets.Content.Tiles
{
    internal class AncientIceOreTile : ModTile
    {
        public override string Texture => "ForgottenFacets/Assets/Tiles/AncientIce/AncientIceOreTile";

        public override void SetStaticDefaults()
        {
            this.SetUpOre(ModContent.ItemType<AncientIceOre>(), Color.HotPink, "Ancient Ice");
            Main.tileOreFinderPriority[Type] = 410;
            DustType = DustID.Silver;
            MineResist = 2f;

            MinPick = 65;//demonite and crimtane
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
                int y = Main.rand.Next((int)Main.worldSurface, Main.maxTilesY - 200);

                Tile tile = Framing.GetTileSafely(x, y);

                if (tile.HasTile && tile.TileType == TileID.IceBlock)
                {
                    WorldGen.TileRunner(x, y, Main.rand.Next(5, 12), Main.rand.Next(5, 10), ModContent.TileType<AncientIceOreTile>());
                }
            }

            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
    }

    public class ConditionForSpawning : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.BrainofCthulhu)
            {
                AncientIceOreSystem.SpawnOre();
            }
        }
    }


}
