using ForgottenFacets.Content.Projectiles;
using ForgottenFacets.Content.Projectiles.Ruby;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Items.Weapons.Magic
{
    internal class Cinderbranch : ModItem
    {
        public override string Texture => "ForgottenFacets/Assets/Items/Weapons/Magic/Cinderbranch";
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;

            Item.Size = new Vector2(27, 27);
            Item.SetWeaponValues(26,3);
            Item.mana = 5;

            Item.UseSound = SoundID.Item34 with { Volume = Main.rand.NextFloat(0.3f, 0.5f), Pitch = Main.rand.NextFloat(-1.2f, 0.4f) };
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<CinderbranchHoldOut>();

            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;

            Item.scale = 0.9f;
        }
    }
}
