using ForgottenFacets.Content.Materials;
using ForgottenFacets.Content.Projectiles;
using ForgottenFacets.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Items.Weapons.Ranged
{
    internal class TwinFrostArbalest : ModItem
    {
        public override string Texture => "ForgottenFacets/Assets/Items/Weapons/Ranged/TwinFrostArbalest";
        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 24;
            Item.damage = 24;
            Item.DamageType = DamageClass.Ranged;

            Item.useTime = 12;
            Item.useAnimation = 24;// 2 shots per use
            Item.useStyle = ItemUseStyleID.Shoot;
            
            Item.knockBack = 3f;
            Item.sellPrice(silver: 50);

            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Arrow;

            Item.noMelee = true;

            Item.reuseDelay = 10;
            Item.scale *= 0.9f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 spawnPostion = position + velocity.SafeNormalize(Vector2.Zero) * 20f;

            if (ModUtils.CheckWoodenArrow(type, player))
                Projectile.NewProjectile(source, spawnPostion, velocity, ModContent.ProjectileType<TwinFrostArbalestArrow>(), damage, knockback, player.whoAmI);
            else
                Projectile.NewProjectile(source, spawnPostion, velocity, type, damage, knockback, player.whoAmI);

            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-4, 0);

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AncientIceBar>(), 10)
                .AddIngredient(ItemID.Sapphire, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }

    }
}
