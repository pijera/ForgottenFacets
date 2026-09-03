using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Materials
{
    internal class AncientIceBar : ModItem
    {
        public override string Texture => "ForgottenFacets/Assets/Materials/Bars/AncientIceBar";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.SortingPriorityMaterials[Type] = 59;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.sellPrice(silver: 15);
            Item.rare = ItemRarityID.Blue;
        }
    }
}
