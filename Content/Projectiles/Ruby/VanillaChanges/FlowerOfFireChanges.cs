using ForgottenFacets.Content.ModPlayers;
using Microsoft.Xna.Framework;
using rail;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Math;

namespace ForgottenFacets.Content.Projectiles.Ruby.VanillaChanges
{
    internal class FlowerOfFireChanges : ModProjectile
    {
        private int bounces = 5;
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Fireball;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;

            Projectile.scale *= 2f;
        }

        public override void AI()
        {
            Main.projectile[Projectile.whoAmI].GetGlobalProjectile<FlowerOfFireProjectileChanges>().NoHeat = true;

            for (int i = 0; i < 10; i++)
                Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(5, 5), 250, default, Main.rand.NextFloat(2f, 3f)).noGravity = true;

            for (int i = 0; i < 2; i++)
                Dust.NewDustPerfect(Projectile.Center, DustID.GemRuby, Main.rand.NextVector2Circular(5, 5), 150, default, Main.rand.NextFloat(0.5f, 0.8f));

            Projectile.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * 0.05f;

            if (Projectile.velocity.Y < 8f)
                Projectile.velocity.Y += 0.25f;

            Projectile.rotation += MathHelper.ToRadians(10f) * Sign(Projectile.velocity.X);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxModifier)
        {
            fallThrough = false;
            return true;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 hitboxCenter = Projectile.Center +
                new Vector2(15f, 0f).RotatedBy(Projectile.rotation);

            Rectangle hitbox = new Rectangle((int)(hitboxCenter.X - 10f), (int)(hitboxCenter.Y - 10f), 20, 20);

            return hitbox.Intersects(targetHitbox);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (bounces == 0)
                Split();
            else
            {
                bounces--;
                if (Projectile.velocity.X != oldVelocity.X)
                    Projectile.velocity.X = -oldVelocity.X * 0.9f;

                if (Projectile.velocity.Y != oldVelocity.Y)
                    Projectile.velocity.Y = -oldVelocity.Y * 0.9f;
            }

            SoundEngine.PlaySound(SoundID.Item10 with { Volume = Main.rand.NextFloat(0.8f, 1f), PitchRange = (-1f,0f)});
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Split();
        }

        private void Split()
        {
            if (Projectile.ai[0] == 1f)
                return;

            Projectile.ai[0] = 1f;
            for (int i = 0; i < 5; i++)
            {
                float angle = MathHelper.TwoPi / 8f * i;

                Vector2 velocity = angle.ToRotationVector2() * 7f;

                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ProjectileID.BallofFire, Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            Projectile.Kill();
        }

    }

    public class FlowerOfFireProjectileChanges : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool FlowerofFireShot;
        public bool NoHeat;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource && itemSource.Item.type == ItemID.FlowerofFire)
                FlowerofFireShot = true;
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!FlowerofFireShot)
                return;

            if (NoHeat)
                return;

            Player player = Main.player[projectile.owner];

            if (player == null || !player.active)
                return;

            player.GetModPlayer<SuperHeatingWeapons>().AddHeat(10f);
        }
    }



}
