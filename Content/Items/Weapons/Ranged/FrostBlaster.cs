using ForgottenFacets.Content.Materials;
using ForgottenFacets.Content.Projectiles;
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
    internal class FrostBlaster : ModItem
    {
        public override string Texture => "ForgottenFacets/Assets/Items/Weapons/Ranged/FrostBlaster";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(12, 4));

        }

        public override void SetDefaults()
        {
            Item.Size = new Vector2(52, 16);
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<FrostBlasterHoldOut>();
            Item.shootSpeed = 1f;

            
            Item.knockBack = 6f;
            Item.rare = ItemRarityID.Blue;
            Item.sellPrice(silver: 50);
            Item.scale *= 0.9f;
        }

        public override bool CanUseItem(Player player) => player.HasAmmo(Item);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<FrostBlasterHoldOut>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AncientIceBar>(), 10)
                .AddIngredient(ItemID.BorealWood, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
