
using ForgottenFacets.Content.ModPlayers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles.Ruby.VanillaChanges
{
    internal class PhoenixBlasterChanges : ModProjectile
    {
        private const int bulletAmount = 12;
        private const int bulletDelay = 4;
        private const float bulletSpeed = 16f;
        private const float spread = 5f;

        private int bulletTimer;
        public override string Texture => "ForgottenFacets/Assets/Misc/Invisible";

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.timeLeft = bulletAmount * bulletDelay + 10;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            SuperHeatingWeapons heat = player.GetModPlayer<SuperHeatingWeapons>();

            player.itemAnimation = 2;
            player.itemTime = 2;

            Projectile.Center = player.Center;

            Vector2 direction = Main.MouseWorld - player.Center;

            if (direction != Vector2.Zero)
                direction.Normalize();

            player.direction = direction.X >= 0f ? 1 : -1;

            player.itemRotation = direction.ToRotation();

            if (player.direction == -1)
                player.itemRotation -= MathHelper.Pi;

            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = 1;
                Projectile.velocity.Normalize();
            }

            bulletTimer++;

            if (bulletTimer >= bulletDelay)
            {
                bulletTimer = 0;

                FireBullet(player);
                Projectile.ai[1]++;

                if (Projectile.ai[1] >= bulletAmount)
                    Projectile.Kill();
            }
        }

        private void FireBullet(Player player)
        {
            Vector2 direction = Main.MouseWorld - player.Center;

            if (direction == Vector2.Zero)
                return;

            direction.Normalize();

            Vector2 velocity =
                direction.RotatedBy(
                    MathHelper.ToRadians(Main.rand.NextFloat(-spread, spread))
                ) * bulletSpeed;
                
            int bullet = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ProjectileID.ExplosiveBullet,Projectile.damage,Projectile.knockBack,Projectile.owner);

            Main.projectile[bullet].GetGlobalProjectile<PhoenixBlasterBulletChanges>().NoHeat = true;
        }

    }

    public class PhoenixBlasterBulletChanges : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool PhoenixBlasterShot;
        public bool NoHeat;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource &&
                itemSource.Item.type == ItemID.PhoenixBlaster)
            {
                PhoenixBlasterShot = true;
            }
        }

        public override void OnHitNPC(Projectile projectile,NPC target,NPC.HitInfo hit,int damageDone)
        {
            if (!PhoenixBlasterShot)
                return;

            if (NoHeat)
                return;

            Player player = Main.player[projectile.owner];

            if (player == null || !player.active)
                return;

            player.GetModPlayer<SuperHeatingWeapons>().AddHeat(4f);
        }
    }

}
