using ForgottenFacets.Content.Dusts;
using ForgottenFacets.Content.ModPlayers;
using ForgottenFacets.Content.Projectiles.Aquamarine.FrostBlasterProjectiles;
using ForgottenFacets.Content.Projectiles.Ruby;
using ForgottenFacets.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Items.Weapons.Ranged
{
    internal class CinderFlintlock : ModItem
    {
        public override string Texture => "ForgottenFacets/Assets/Items/Weapons/Ranged/CinderFlintlock";

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;

            Item.SetWeaponValues(63,8);
            Item.Size = new Vector2(46, 22);

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 70;
            Item.useAnimation = 70;

            Item.shootSpeed = 20f;
            Item.shoot = ProjectileID.WaterBolt;
            Item.useAmmo = AmmoID.Bullet;

            Item.noMelee = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            SuperHeatingWeapons HeatStacking = player.GetModPlayer<SuperHeatingWeapons>();
            Vector2 spawnPostion = position + velocity.SafeNormalize(Vector2.Zero) * 20f;

            if (HeatStacking.IsSuperHeated && player.altFunctionUse == 2)
            {
                player.velocity += -velocity / 2f;
                float maxSpeed = 42f;
                ScreenShake screen = player.GetModPlayer<ScreenShake>();

                if (player.velocity.Length() > maxSpeed)
                {
                    player.velocity = Vector2.Normalize(player.velocity) * maxSpeed;
                }
                Vector2 muzzlePosition = position + velocity.SafeNormalize(Vector2.UnitX) * 30f;
                Vector2 smokePos = muzzlePosition - new Vector2(player.direction * 10f, 0f);
                screen.AddShake(6);

                SoundEngine.PlaySound(SoundID.Item74 with { Volume = Main.rand.NextFloat(0.9f, 1.2f), PitchRange = (0.8f, 1.2f) });

                for (int i = 0; i < 25; i++)
                {
                    Dust.NewDustPerfect(muzzlePosition, DustID.Torch, Main.rand.NextVector2Circular(3, 3), 100, default, Main.rand.NextFloat(1f, 1.4f));
                    Dust.NewDustPerfect(muzzlePosition, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(3, 3), 100, Color.OrangeRed, Main.rand.NextFloat(1f, 1.4f));
                    Dust.NewDustPerfect(muzzlePosition, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(3, 3), 100, Color.OrangeRed, Main.rand.NextFloat(1f, 1.4f));
                }
                for (int i = 0; i < 15; i++)
                {
                    Dust.NewDustPerfect(muzzlePosition, DustID.Smoke, Main.rand.NextVector2Circular(2, 2), 100, Color.DimGray, Main.rand.NextFloat(1f, 1.2f));
                }

                Projectile.NewProjectile(source, spawnPostion - new Vector2(-5, -5), velocity, ModContent.ProjectileType<CinderFlintlockFullHeatProjectile>(), damage + 50, knockback + 3, player.whoAmI);
                HeatStacking.ConsumeHeat();
            }
            else
            {
                player.velocity += -velocity / 3f;

                SoundEngine.PlaySound(SoundID.Item38 with { Volume = Main.rand.NextFloat(0.8f, 1.2f), Pitch = Main.rand.NextFloat(0.8f, 1.2f) });

                float maxSpeed = 10f;
                if (player.velocity.Length() > maxSpeed)
                {
                    player.velocity = Vector2.Normalize(player.velocity) * maxSpeed;
                }
                Vector2 muzzlePosition = position + velocity.SafeNormalize(Vector2.UnitX) * 30f;
                Vector2 smokePos = muzzlePosition - new Vector2(player.direction * 10f, 0f);

                for (int i = 0; i < 15; i++)
                {
                    Dust.NewDustPerfect(muzzlePosition, DustID.Torch, Main.rand.NextVector2Circular(3, 3), 100, default, Main.rand.NextFloat(1f, 1.4f));
                    Dust.NewDustPerfect(muzzlePosition, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(3, 3), 100, Color.OrangeRed, Main.rand.NextFloat(1f, 1.4f));
                }
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDustPerfect(muzzlePosition, DustID.Smoke, Main.rand.NextVector2Circular(2, 2), 100, Color.DimGray, Main.rand.NextFloat(1f, 1.2f));
                }


                Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<CinderFlintlockProjectile>(), damage, knockback, player.whoAmI);
            }


            return false;
        }
    }
}
