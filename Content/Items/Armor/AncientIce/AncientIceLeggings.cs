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
    [AutoloadEquip(EquipType.Legs)]
    internal class AncientIceLeggings : ModItem
    {
        public override string Texture => "ForgottenFacets/Assets/Items/Armor/AncientIce/AncientIceLeggings";

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;

            Item.rare = ItemRarityID.Blue;
            Item.sellPrice(silver: 50);
            Item.defense = 4;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) *= 1.04f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AncientIceBar>(), 12)
                .AddIngredient(ItemID.Amethyst, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
