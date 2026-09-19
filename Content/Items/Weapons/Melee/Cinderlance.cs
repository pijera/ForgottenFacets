using ForgottenFacets.Content.ModPlayers;
using ForgottenFacets.Content.Projectiles.Ruby;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Items.Weapons.Melee
{
    internal class Cinderlance : ModItem
    {

        public override string Texture => "ForgottenFacets/Assets/Items/Weapons/Meele/Cinderlance";

        public override void SetStaticDefaults()
        {
            ItemID.Sets.SkipsInitialUseSound[Type] = true;
            ItemID.Sets.Spears[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0,1,0,0);

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.UseSound = SoundID.Item1 with { Volume = 0.8f, PitchRange = (0.1f, 0.3f) };

            Item.damage = 33;
            Item.knockBack = 6.5f;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;

            Item.shootSpeed = 3.7f;
            Item.shoot = ModContent.ProjectileType<CinderLanceProjectile>();
            Item.noMelee = true;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < 1;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SuperHeatingWeapons HeatStacking = player.GetModPlayer<SuperHeatingWeapons>();
            Vector2 spawnPostion = position + velocity.SafeNormalize(Vector2.Zero) * 20f;

            if (HeatStacking.IsSuperHeated)
            {
                SoundEngine.PlaySound(SoundID.Item20 with { Volume = 1.2f, PitchRange=(-1.2f,1.2f)});
                SoundEngine.PlaySound(SoundID.Item42 with { Volume = 0.8f, PitchRange = (-0.8f, 0.8f) });
                Projectile.NewProjectile(source, spawnPostion - new Vector2(-5,-5), velocity * 8, ModContent.ProjectileType<CinderLanceFullHeatProjectile>(), damage/2, knockback + 3, player.whoAmI);
                HeatStacking.ConsumeHeat();
            }
            else
                Projectile.NewProjectile(source, spawnPostion, velocity, ModContent.ProjectileType<CinderLanceProjectile>(), damage, knockback, player.whoAmI);

            return false;
        }

        public override bool? UseItem(Player player)
        {
            if (!Main.dedServ && Item.UseSound.HasValue)
                SoundEngine.PlaySound(Item.UseSound.Value, player.Center);

            return null;
        }

    }
}
