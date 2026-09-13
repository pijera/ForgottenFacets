using ForgottenFacets.Content.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles
{
    internal class CinderLanceFullHeatProjectile : ModProjectile
    {

        public bool IsStuck
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }
        public int StuckTargetIndex
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        private int explosinTimer = 0;
        private const int TimeBeforeExplosion = 180;
        private Vector2 drawDirection;

        public override string Texture => "ForgottenFacets/Assets/Projectiles/CinderLanceFullHeatProjectile";

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;

            Projectile.width = 15;
            Projectile.height = 15;

            Projectile.scale *= 1.3f;

        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0f, 0f);

            for (int i = 0; i < 3; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(3, 3), 200, default, Main.rand.NextFloat(0.8f, 1.2f));
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(2, 2), 100, Color.Red, Main.rand.NextFloat(0.3f, 0.8f));
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(2, 2), 100, Color.Red, Main.rand.NextFloat(0.9f, 1f));
            }
            if (IsStuck)
            {
                explosinTimer++;

                if (StuckTargetIndex >= 0)
                {
                    NPC target = Main.npc[StuckTargetIndex];
                    if (!target.active)
                    {
                        Projectile.Kill();
                        return;
                    }

                    Projectile.velocity = Vector2.Zero;
                }
                else
                    Projectile.velocity = Vector2.Zero;

                if (explosinTimer >= TimeBeforeExplosion)
                    Projectile.Kill();
                return;
            }

            Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(2, 2), 100, default, Main.rand.NextFloat(0.7f, 1f));
            Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(2, 2), 100, Color.Red, Main.rand.NextFloat(0.9f, 1.4f));
            Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(2, 2), 100, Color.Red, Main.rand.NextFloat(0.9f, 1.4f));


            Lighting.AddLight(Projectile.Center, 1f, 0f, 0f);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.PiOver4;
        }

        

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!IsStuck)
            {
                drawDirection = Projectile.velocity.SafeNormalize(Vector2.UnitX);

                IsStuck = true;
                StuckTargetIndex = target.whoAmI;
                Projectile.tileCollide = false;


            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!IsStuck)
            {
                drawDirection = oldVelocity.SafeNormalize(Vector2.UnitX);
                IsStuck = true;
                StuckTargetIndex = -1;
                Projectile.tileCollide = false;
            }

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            for (int i = 0; i < 40; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 120, default, 3f);
                dust.velocity *= 1.5f;
            }

            for (int i = 0; i < 120; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center,DustID.Torch,Main.rand.NextVector2Circular(3,3),120,default,2f);
                dust.noGravity = false;
                dust.velocity *= 2.5f;
            }

            Projectile.position = Projectile.Center;
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.Center = Projectile.position;

            Projectile.damage = 65;
            Projectile.Damage();
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;

            Vector2 direction = IsStuck ? drawDirection : Projectile.velocity.SafeNormalize(Vector2.UnitX);

            Vector2 drawPosition = Projectile.Center - direction * 60f - Vector2.UnitY * 3f;

            Main.EntitySpriteDraw(texture, drawPosition - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

    }
}
