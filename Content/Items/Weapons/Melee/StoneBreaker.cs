using ForgottenFacets.Content.Projectiles.Topaz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Items.Weapons.Melee
{
    internal class StoneBreaker : ModItem
    {
        public override string Texture => "ForgottenFacets/Assets/Items/Weapons/Meele/StoneBreaker";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.rare = ItemRarityID.Blue;

            Item.useStyle = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.SetWeaponValues(30, 15);
            Item.autoReuse = false;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<StoneBreakerProjectile>();
        }

        public override bool? CanAutoReuseItem(Player player)
        {
            if (player.autoReuseGlove)
                return true;

            return false;
        }

    }
}
