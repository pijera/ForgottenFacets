using ForgottenFacets.Content.ModPlayers;
using ForgottenFacets.Content.Projectiles.Ruby.VanillaChanges;
using Microsoft.Xna.Framework;
using System.IO;
using System.Linq;
using Terraria;
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
            heat.ConsumeHeat();
        }

        private static void ActivateFlamarangOverheat(Player player)
        {
            SuperHeatingWeapons heat = player.GetModPlayer<SuperHeatingWeapons>();
            Vector2 direction = Main.MouseWorld - player.Center;

            if (direction == Vector2.Zero)
                return;

            direction.Normalize();

            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, direction.RotatedBy(MathHelper.ToRadians(10f)) * 13f,
                ModContent.ProjectileType<FlamarangChanges>(), player.GetWeaponDamage(player.HeldItem) + 5, player.GetWeaponKnockback(player.HeldItem));

            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, direction * 13f,
                ModContent.ProjectileType<FlamarangChanges>(), player.GetWeaponDamage(player.HeldItem) + 5, player.GetWeaponKnockback(player.HeldItem));

            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, direction.RotatedBy(MathHelper.ToRadians(-10f)) * 13f,
                ModContent.ProjectileType<FlamarangChanges>(), player.GetWeaponDamage(player.HeldItem) + 5, player.GetWeaponKnockback(player.HeldItem));


            heat.ConsumeHeat();
        }
    }

    
}

