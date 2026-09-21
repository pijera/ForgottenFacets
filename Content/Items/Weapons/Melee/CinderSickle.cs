using ForgottenFacets.Content.ModPlayers;
using ForgottenFacets.Content.Projectiles.Ruby;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Items.Weapons.Melee
{
    internal class CinderSickle : ModItem
    {

        public override string Texture => "ForgottenFacets/Assets/Items/Weapons/Meele/CinderSickle";

        public int attackType = 0;
        public int comboExpireTimer = 0;

        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 44;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.knockBack = 7;
            Item.autoReuse = true;
            Item.damage = 35;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.shoot = ModContent.ProjectileType<CinderSickleProjectile>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SuperHeatingWeapons HeatStacking = player.GetModPlayer<SuperHeatingWeapons>();
            Vector2 spawnPostion = position + velocity.SafeNormalize(Vector2.Zero) * 20f;

            if (HeatStacking.IsSuperHeated)
            {
                SoundEngine.PlaySound(SoundID.Item20 with { Volume = 1.2f, PitchRange = (-1.2f, 1.2f) });
                SoundEngine.PlaySound(SoundID.Item42 with { Volume = 0.8f, PitchRange = (-0.8f, 0.8f) });
                Projectile.NewProjectile(source, spawnPostion - new Vector2(-5, -5), velocity * 8, ModContent.ProjectileType<CinderSickleFullHeatProjectile>(), damage, knockback + 3, player.whoAmI);
                HeatStacking.ConsumeHeat();
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, attackType);
                attackType = (attackType + 1) % 3;
                comboExpireTimer = 0;
            }
            return false;
        }

        public override void UpdateInventory(Player player)
        {
            if (comboExpireTimer++ >= 120)
                attackType = 0;
        }

        public override bool MeleePrefix() => true;


    }
}
