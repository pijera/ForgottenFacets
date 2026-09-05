using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace ForgottenFacets.Core
{
    internal static class FurnitureCommon
    {
        internal static void SetUpBar(this ModTile mt, int itemDropID, Color mapColor, string barName, bool lavaImmune = true)
        {
            mt.RegisterItemDrop(itemDropID);

            Main.tileShine[mt.Type] = 1100;
            Main.tileSolid[mt.Type] = true;
            Main.tileSolidTop[mt.Type] = true;
            Main.tileFrameImportant[mt.Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = !lavaImmune;
            TileObjectData.newTile.LavaPlacement = lavaImmune ? LiquidPlacement.Allowed : LiquidPlacement.NotAllowed;
            TileObjectData.addTile(mt.Type);

            // Vanilla bars are labeled as "Metal Bar" on the minimap
            mt.AddMapEntry(mapColor, Language.GetText(barName));
        }
        internal static void SetUpOre(this ModTile mt, int itemDropID, Color mapColor, string oreName)
        {
            mt.RegisterItemDrop(itemDropID);

            TileID.Sets.Ore[mt.Type] = true;
            TileID.Sets.FriendlyFairyCanLureTo[mt.Type] = true;
            Main.tileSpelunker[mt.Type] = true;

            Main.tileShine2[mt.Type] = true;
            Main.tileShine[mt.Type] = 975;

            Main.tileMergeDirt[mt.Type] = true;
            Main.tileSolid[mt.Type] = true;
            Main.tileBlockLight[mt.Type] = true;

            mt.AddMapEntry(mapColor, Language.GetText(oreName));
            mt.HitSound = SoundID.Tink;
            mt.VanillaFallbackOnModDeletion = TileID.Copper;
        }


    }
}
