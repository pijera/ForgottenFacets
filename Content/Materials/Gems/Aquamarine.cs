using ForgottenFacets.Content.Tiles.Gems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Materials.Gems
{
    internal class Aquamarine : ModItem
    {
        public override string Texture => "ForgottenFacets/Assets/Materials/Gems/Aquamarine";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 20;
            ItemID.Sets.SortingPriorityMaterials[Type] = 58;
        }
        public override void SetDefaults() 
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<AquamarineExposed>());
            Item.rare = ItemRarityID.Blue;
            Item.sellPrice(0, 0, 12, 75);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Sapphire, 1)
                .AddIngredient(ModContent.ItemType<AncientIceOre>(), 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
