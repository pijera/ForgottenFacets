using ForgottenFacets.Content.ModPlayers;
using ForgottenFacets.Content.Projectiles;
using ForgottenFacets.Content.Projectiles.Ruby;
using ForgottenFacets.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
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

            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 1, 0, 0);

            Item.scale = 0.9f;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            ScreenShake screen = player.GetModPlayer<ScreenShake>();
            SuperHeatingWeapons HeatStacking = player.GetModPlayer<SuperHeatingWeapons>();

            if (HeatStacking.IsSuperHeated && player.controlUseTile)
            {
                Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<CinderbranchFullHeatExplosion>(), 80, 5, player.whoAmI, 250f);
                screen.AddShake(15);
                SoundEngine.PlaySound(SoundID.Item62 with { Volume = 0.7f, PitchRange = (-0.5f, 1f) });
                SoundEngine.PlaySound(SoundID.DD2_BetsyFlameBreath with { Volume = 0.2f, PitchRange = (-0.8f, 0.8f) });
                SoundEngine.PlaySound(SoundID.DD2_BetsyWindAttack with { Volume = 0.2f, PitchRange = (-0.8f, 0.8f) });
                HeatStacking.ConsumeHeat();
            }

            return true;
        }
    }
}
