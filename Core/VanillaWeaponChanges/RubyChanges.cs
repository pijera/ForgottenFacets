using ForgottenFacets.Content.Buffs;
using ForgottenFacets.Content.Dusts;
using ForgottenFacets.Content.ModPlayers;
using ForgottenFacets.Content.Projectiles.Ruby.VanillaChanges;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ForgottenFacets.Core.VanillaWeaponChanges
{
    internal class RubyChanges : GlobalItem
    {
        public override bool AltFunctionUse(Item item, Player player)
        {
            if (item.type == ItemID.ImpStaff)
                return true;
            if (item.type == ItemID.MoltenFury)
                return true;
            if (item.type == ItemID.Flamarang)
                return true;
            if (item.type == ItemID.HellwingBow)
                return true;
            if (item.type == ItemID.FlowerofFire)
                return true;
            if (item.type == ItemID.PhoenixBlaster)
                return true;
            if (item.type == ItemID.Flamelash)
                return true;
            if (item.type == ItemID.Sunfury)
                return true;
            if (item.type == ItemID.FieryGreatsword)
                return true;

            return base.AltFunctionUse(item, player);
        }

        public override bool? UseItem(Item item, Player player)
        {
            SuperHeatingWeapons heat = player.GetModPlayer<SuperHeatingWeapons>();
            if (item.type == ItemID.ImpStaff && player.altFunctionUse == 2 && heat.IsSuperHeated)
            {
                ActivateImpStaffOverheat(player);
                return true;
            }
            if (item.type == ItemID.FieryGreatsword && player.altFunctionUse == 2 && heat.IsSuperHeated)
            {
                ActivateVolcanoOverheat(player);
                return false;
            }
            return base.UseItem(item, player);
        }


        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SuperHeatingWeapons heat = player.GetModPlayer<SuperHeatingWeapons>();
            if (player.altFunctionUse == 2 && heat.IsSuperHeated)
            {
                switch (item.type)
                {
                    case ItemID.ImpStaff:
                        ActivateImpStaffOverheat(player);
                        return false;
                    case ItemID.MoltenFury:
                        ActivateMoltenFuryOverheat(player);
                        return false;
                    case ItemID.Flamarang:
                        ActivateFlamarangOverheat(player);
                        return false;
                    case ItemID.HellwingBow:
                        ActiveateHellwingOverheat(player);
                        return false;
                    case ItemID.FlowerofFire:
                        ActivateFlowerOfFireOverheat(player);
                        return false;
                    case ItemID.PhoenixBlaster:
                        ActivatePhoenixBlasterOverheat(player);
                        return false;
                    case ItemID.Flamelash:
                        ActivateFlamelashOverheat(player);
                        return false;
                    case ItemID.Sunfury:
                        ActivateSunfuryOverheat(player);
                        return false;

                }
            }

            return true;
        }

        private static void ActivateImpStaffOverheat(Player player)
        {
            SuperHeatingWeapons heat = player.GetModPlayer<SuperHeatingWeapons>();

            bool activatedImp = false;


            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];

                if (!projectile.active)
                    continue;
                if (projectile.owner != player.whoAmI)
                    continue;
                if (projectile.type != ProjectileID.FlyingImp)
                    continue;



                ImpStaffChanges imp = projectile.GetGlobalProjectile<ImpStaffChanges>();

                imp.ImpOverheated = true;
                imp.OverheatTimer = ImpStaffChanges.OverheatDuration;
                imp.OverheatFireTimer = 0;

                projectile.netUpdate = true;
                activatedImp = true;
            }

            SoundEngine.PlaySound(SoundID.Item45 with { Volume = Main.rand.NextFloat(0.6f, 0.8f), PitchRange = (0.5f, 1.5f) });

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustPerfect(player.Center, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(0.5f, 1f));
                Dust.NewDustPerfect(player.Center, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(0.5f, 1f));
            }

            if (activatedImp)
                heat.ConsumeHeat();
        }
        private static void ActivateMoltenFuryOverheat(Player player)
        {
            SuperHeatingWeapons heat = player.GetModPlayer<SuperHeatingWeapons>();
            Vector2 direction = Main.MouseWorld - player.Center;


            if (direction == Vector2.Zero)
                return;

            direction.Normalize();

            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, direction, 
                ModContent.ProjectileType<MoltenFuryChanges>(), player.GetWeaponDamage(player.HeldItem), player.GetWeaponKnockback(player.HeldItem));

            SoundEngine.PlaySound(SoundID.Item42 with { Volume = Main.rand.NextFloat(0.6f, 0.8f), PitchRange = (-1f, 0.5f) });

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustPerfect(player.Center, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(0.5f, 1f));
                Dust.NewDustPerfect(player.Center, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(0.5f, 1f));
            }

            heat.ConsumeHeat();
        }
        private static void ActivateFlamarangOverheat(Player player)
        {
            SuperHeatingWeapons heat = player.GetModPlayer<SuperHeatingWeapons>();
            Vector2 direction = Main.MouseWorld - player.Center;

            if (direction == Vector2.Zero)
                return;

            direction.Normalize();


            SoundEngine.PlaySound(SoundID.DD2_JavelinThrowersAttack with { Volume = Main.rand.NextFloat(0.8f, 1f), PitchRange = (0.2f, 1.1f) });

            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, direction.RotatedBy(MathHelper.ToRadians(10f)) * 13f,
                ModContent.ProjectileType<FlamarangChanges>(), player.GetWeaponDamage(player.HeldItem) + 5, player.GetWeaponKnockback(player.HeldItem));

            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, direction * 13f,
                ModContent.ProjectileType<FlamarangChanges>(), player.GetWeaponDamage(player.HeldItem) + 5, player.GetWeaponKnockback(player.HeldItem));

            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, direction.RotatedBy(MathHelper.ToRadians(-10f)) * 13f,
                ModContent.ProjectileType<FlamarangChanges>(), player.GetWeaponDamage(player.HeldItem) + 5, player.GetWeaponKnockback(player.HeldItem));


            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustPerfect(player.Center, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(0.5f, 1f));
                Dust.NewDustPerfect(player.Center, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(0.5f, 1f));
            }

            heat.ConsumeHeat();
        }
        private static void ActiveateHellwingOverheat(Player player)
        {
            SuperHeatingWeapons heat = player.GetModPlayer<SuperHeatingWeapons>();
            Vector2 direction = Main.MouseWorld - player.Center;

            if (direction == Vector2.Zero)
                return;

            direction.Normalize();

            Vector2[] offsets =
        {
            new Vector2(-35f, -55f),
            new Vector2(-15f, -30f),
            new Vector2(-10f, 0f),
            new Vector2(-15f, 30f),
            new Vector2(-35f, 55f)
        };

            SoundEngine.PlaySound(SoundID.Shatter with { Volume = Main.rand.NextFloat(0.6f, 0.8f), PitchRange = (0.5f, 1.5f) });

            float rotation = direction.ToRotation();

            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center + offsets[0].RotatedBy(rotation), direction * player.HeldItem.shootSpeed * 1.5f,
                ModContent.ProjectileType<HellwingBowChanges>(), player.GetWeaponDamage(player.HeldItem) + 5, player.GetWeaponKnockback(player.HeldItem));

            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center + offsets[1].RotatedBy(rotation), direction * player.HeldItem.shootSpeed * 1.5f,
                ModContent.ProjectileType<HellwingBowChanges>(), player.GetWeaponDamage(player.HeldItem) + 5, player.GetWeaponKnockback(player.HeldItem));

            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center + offsets[2].RotatedBy(rotation), direction * player.HeldItem.shootSpeed * 1.5f,
                ModContent.ProjectileType<HellwingBowChanges>(), player.GetWeaponDamage(player.HeldItem) + 5, player.GetWeaponKnockback(player.HeldItem));

            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center + offsets[3].RotatedBy(rotation), direction * player.HeldItem.shootSpeed * 1.5f,
                ModContent.ProjectileType<HellwingBowChanges>(), player.GetWeaponDamage(player.HeldItem) + 5, player.GetWeaponKnockback(player.HeldItem));

            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center + offsets[4].RotatedBy(rotation), direction * player.HeldItem.shootSpeed * 1.5f,
                ModContent.ProjectileType<HellwingBowChanges>(), player.GetWeaponDamage(player.HeldItem) + 5, player.GetWeaponKnockback(player.HeldItem));


            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustPerfect(player.Center, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(0.5f, 1f));
                Dust.NewDustPerfect(player.Center, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(0.5f, 1f));
            }

            heat.ConsumeHeat();
        }
        private static void ActivateFlowerOfFireOverheat(Player player)
        {
            SuperHeatingWeapons heat = player.GetModPlayer<SuperHeatingWeapons>();
            Vector2 direction = Main.MouseWorld - player.Center;

            if (direction == Vector2.Zero)
                return;

            direction.Normalize();


            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, direction * 13f,
                ModContent.ProjectileType<FlowerOfFireChanges>(), player.GetWeaponDamage(player.HeldItem) + 5, player.GetWeaponKnockback(player.HeldItem));

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustPerfect(player.Center, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(0.5f, 1f));
                Dust.NewDustPerfect(player.Center, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(0.5f, 1f));
            }

            heat.ConsumeHeat();
        }
        private static void ActivatePhoenixBlasterOverheat(Player player)
        {
            SuperHeatingWeapons heat = player.GetModPlayer<SuperHeatingWeapons>();
            Vector2 direction = Main.MouseWorld - player.Center;

            if (direction == Vector2.Zero)
                return;

            direction.Normalize();


            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, direction * 13f,
                ModContent.ProjectileType<PhoenixBlasterChanges>(), player.GetWeaponDamage(player.HeldItem) + 5, player.GetWeaponKnockback(player.HeldItem));

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustPerfect(player.Center, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(0.5f, 1f));
                Dust.NewDustPerfect(player.Center, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(0.5f, 1f));
            }

            heat.ConsumeHeat();
        }
        private static void ActivateFlamelashOverheat(Player player)
        {
            SuperHeatingWeapons heat = player.GetModPlayer<SuperHeatingWeapons>();
            Vector2 direction = Main.MouseWorld - player.Center;

            if (direction == Vector2.Zero)
                return;

            direction.Normalize();

            SoundEngine.PlaySound(SoundID.Item20 with { Volume = Main.rand.NextFloat(1f, 1.9f), PitchRange = (0.5f, 1.5f) });
            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, direction * 22f,
                ModContent.ProjectileType<FlamelashChanges>(), player.GetWeaponDamage(player.HeldItem), 0);

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustPerfect(player.Center, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(0.5f, 1f));
                Dust.NewDustPerfect(player.Center, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(0.5f, 1f));

                Dust.NewDustPerfect(player.Center, DustID.Torch, Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(1f, 2f));
            }

            heat.ConsumeHeat();
        }
        private static void ActivateSunfuryOverheat(Player player)
        {
            SuperHeatingWeapons heat = player.GetModPlayer<SuperHeatingWeapons>();
            Vector2 direction = Main.MouseWorld - player.Center;

            if (direction == Vector2.Zero)
                return;

            direction.Normalize();
            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, direction * 13f,
                ModContent.ProjectileType<SunfuryChanges>(), player.GetWeaponDamage(player.HeldItem) + 5, player.GetWeaponKnockback(player.HeldItem));

            SoundEngine.PlaySound(SoundID.DD2_JavelinThrowersAttack with { Volume = Main.rand.NextFloat(0.8f, 1f), PitchRange = (0.2f, 1.1f) });

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustPerfect(player.Center, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(0.5f, 1f));
                Dust.NewDustPerfect(player.Center, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(0.5f, 1f));
            }

            heat.ConsumeHeat();
        }
        private static void ActivateVolcanoOverheat(Player player)
        {
            SuperHeatingWeapons heat = player.GetModPlayer<SuperHeatingWeapons>();
            ScreenShake shake = player.GetModPlayer<ScreenShake>();

            player.itemAnimation = 0;
            player.itemTime = 0;

            Vector2 direction = Main.MouseWorld - player.Center;

            if (direction == Vector2.Zero)
                return;

            direction.Normalize();

            shake.AddShake(12);
            player.AddBuff(ModContent.BuffType<VolcanoBuff>(), 630);
            SoundEngine.PlaySound(SoundID.Item62 with { Volume = Main.rand.NextFloat(3f, 3.8f), PitchRange = (-1.1f, -0.2f) });
            for (int i = 0; i < 12; i++)
            {
                Dust.NewDustPerfect(player.Center, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(1.5f, 2.5f));
                Dust.NewDustPerfect(player.Center, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(3, 3), 120, Color.OrangeRed, Main.rand.NextFloat(1f, 1.5f));

            }
            for (int i = 0; i < 50; i++)
            {
                Dust.NewDustPerfect(player.Center, DustID.Torch, Main.rand.NextVector2Circular(5, 5), 30, Color.OrangeRed, Main.rand.NextFloat(1.5f, 2.8f));
            }

            heat.ConsumeHeat();
        }
    }    
}

