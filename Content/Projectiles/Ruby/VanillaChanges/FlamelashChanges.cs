using ForgottenFacets.Content.ModPlayers;
using ForgottenFacets.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles.Ruby.VanillaChanges
{
    internal class FlamelashChanges : ModProjectile
    {
        private NPC victim;

        private int damageTimer;
        private const int DamageDuration = 180;
        private const int DamageCooldown = 15;

        private Vector2 hitOffset;
        public Player Owner => Main.player[Projectile.owner];

        public override string Texture => "ForgottenFacets/Assets/Misc/Invisible";

        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;

            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;

            Projectile.DamageType = DamageClass.MeleeNoSpeed;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = DamageCooldown;

            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            if (!Owner.active || Owner.dead)
            {
                Projectile.Kill();
                return;
            }

            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(3, 3), 20, default, Main.rand.NextFloat(2.5f, 3.5f)).noGravity = true;
            }

            if (victim != null)
            {
                if (!victim.active || victim.life <= 0)
                {
                    Projectile.Kill();
                    return;
                }

                if (Vector2.Distance(Owner.MountedCenter, victim.Center) > 353f)
                {
                    Projectile.Kill();
                    return;
                }

                Projectile.Center = victim.Center + hitOffset;
                Projectile.velocity = Vector2.Zero;

                damageTimer++;

                if (damageTimer >= DamageDuration)
                {
                    Projectile.Kill();
                    return;
                }

                return;
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Vector2.Distance(Projectile.Center, Owner.Center) > 353f)
            {
                Projectile.Kill();
                return;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            victim = target;
            hitOffset = Projectile.Center - target.Center;

            Projectile.velocity = Vector2.Zero;
            Projectile.tileCollide = false;

            damageTimer = 0;

            Projectile.netUpdate = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D chainTex = ModContent.Request<Texture2D>("ForgottenFacets/Assets/Projectiles/Chains/SunfuryChain").Value;

            Vector2 start = Owner.MountedCenter;
            Vector2 end = Projectile.Center;

            Vector2 direction = end - start;
            float distance = direction.Length();

            if (distance > 0f)
            {
                direction.Normalize();
                float rotation = direction.ToRotation() - MathHelper.PiOver2;

                while (distance > 8f)
                {
                    start += direction * 8f * 1.3f;
                    distance -= 8f * 1.3f;

                    Color color = Lighting.GetColor((int)start.X / 16, (int)start.Y / 16);
                    Vector2 origin = new Vector2(chainTex.Width / 2f, chainTex.Height / 2f);

                    Main.EntitySpriteDraw(chainTex, start - Main.screenPosition, null, color, rotation, origin, 1.3f, SpriteEffects.None, 0);
                }
            }
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            if (victim == null)
                return;

            Player player = Main.player[Projectile.owner];

            ScreenShake shake = player.GetModPlayer<ScreenShake>();
            shake.AddShake(10);

            SoundEngine.PlaySound(SoundID.Item14 with { Volume = Main.rand.NextFloat(1f, 1.5f), PitchRange = (-0.6f, 0.3f) });
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<EnemyChainExplosion>(), 40, 7, Projectile.owner);
        }

    }

    internal class EnemyChainExplosion : ModProjectile
    {
        public override string Texture => "ForgottenFacets/Assets/Misc/Invisible";

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            for (int i = 0; i < 30; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, Main.rand.NextVector2Circular(3f, 3f), 0, Color.DarkGray, Main.rand.NextFloat(0.5f, 1.5f));
                Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(5f, 5f), 100, Color.DarkGray, Main.rand.NextFloat(0.5f, 1.5f));
            }
                
        }
    }

    public class FlamelashProjectileChanges : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool FlamelashShot;
        public bool NoHeat;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource &&
                itemSource.Item.type == ItemID.Flamelash)
            {
                FlamelashShot = true;
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!FlamelashShot)
                return;

            if (NoHeat)
                return;

            Player player = Main.player[projectile.owner];

            if (player == null || !player.active)
                return;

            player.GetModPlayer<SuperHeatingWeapons>().AddHeat(7f);
        }
    }

}
