using ForgottenFacets.Content.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Items.Armor.AncientIce
{
    [AutoloadEquip(EquipType.Body)]
    internal class AncientIceChestpiece : ModItem
    {
        public override string Texture => "ForgottenFacets/Assets/Items/Armor/AncientIce/AncientIceChestpiece";

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;

            Item.rare = ItemRarityID.Blue;
            Item.sellPrice(silver: 60);
            Item.defense = 3; 
        }
        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<RangedDamageClass>() += 5f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AncientIceBar>(), 25)
                .AddIngredient(ItemID.Sapphire, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }

    }
}
