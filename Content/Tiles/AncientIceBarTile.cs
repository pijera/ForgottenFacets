using ForgottenFacets.Content.Materials;
using ForgottenFacets.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Tiles
{
    internal class AncientIceBarTile : ModTile
    {
        public override string Texture => "ForgottenFacets/Assets/Tiles/AncientIce/AncientIceBarTile";

        public override void SetStaticDefaults()
        {
            this.SetUpBar(ModContent.ItemType<AncientIceBar>(), Color.LightBlue, "Ancient Ice Bar");
            DustType = DustID.Silver;
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            type = Main.rand.NextBool() ? 187 : 16;
            return true;
        }

    }
}
