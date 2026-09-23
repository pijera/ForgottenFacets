using ForgottenFacets.Content.Dusts;
using ForgottenFacets.Content.ModPlayers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles.Ruby.VanillaChanges
{
    internal class FlamarangChanges : ModProjectile
    {

        public ref float State => ref Projectile.ai[0];
        public ref float Timer => ref Projectile.ai[1];
        public static float Speed = 14f;

        public override string Texture => "ForgottenFacets/Assets/Projectiles/FlamarangFullHeatProjectile";

        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 25;
            Projectile.penetrate = 30;
            Projectile.timeLeft = 360;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;

            Projectile.scale *= 1.2f;
        }


        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0f, 0f);
            Player Owner = Main.player[Projectile.owner];

            Projectile.rotation += 0.3f;

            for (int i = 0; i < 7; i++)
                Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(5, 5), 200, default, Main.rand.NextFloat(0.8f, 1.2f)).noGravity = true;

            for (int i = 0; i < 5; i++)
                Dust.NewDustPerfect(Projectile.Center, DustID.GemRuby, Main.rand.NextVector2CircularEdge(5, 5), 120, default, Main.rand.NextFloat(0.8f, 1.2f)).noGravity = true;

            for (int i = 0; i < 2; i++)
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<GlowDust>(), Main.rand.NextVector2CircularEdge(5, 5), 200, Color.Red, Main.rand.NextFloat(0.8f, 1.2f)).noGravity = true;

            if (Main.rand.NextBool(5))
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2CircularEdge(5, 5), 120, Color.Red, Main.rand.NextFloat(0.8f, 1.2f)).noGravity = true;


            if (State == 0f)
            {
                Timer++;

                if (Timer == 3f)
                    Projectile.tileCollide = true;

                if (Timer >= 40f)
                {
                    State = 1f;
                    Timer = 0f;
                    Projectile.netUpdate = true;
                }

            }
            else
            {
                Projectile.tileCollide = false;
                float returnSpeed = ContentSamples.ItemsByType[ItemID.Flamarang].shootSpeed * 1.5f;
                float acceleration = 3.2f;
                Player owner = Main.player[Projectile.owner];

                Vector2 playerCenter = owner.Center;
                float xDist = playerCenter.X - Projectile.Center.X;
                float yDist = playerCenter.Y - Projectile.Center.Y;
                float dist = (float)Math.Sqrt((double)(xDist * xDist + yDist * yDist));
                if (dist > 3000f)
                    Projectile.Kill();

                dist = returnSpeed / dist;
                xDist *= dist;
                yDist *= dist;

                if (Projectile.velocity.X < xDist)
                {
                    Projectile.velocity.X = Projectile.velocity.X + acceleration;
                    if (Projectile.velocity.X < 0f && xDist > 0f)
                        Projectile.velocity.X += acceleration;
                }
                else if (Projectile.velocity.X > xDist)
                {
                    Projectile.velocity.X = Projectile.velocity.X - acceleration;
                    if (Projectile.velocity.X > 0f && xDist < 0f)
                        Projectile.velocity.X -= acceleration;
                }
                if (Projectile.velocity.Y < yDist)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y + acceleration;
                    if (Projectile.velocity.Y < 0f && yDist > 0f)
                        Projectile.velocity.Y += acceleration;
                }
                else if (Projectile.velocity.Y > yDist)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y - acceleration;
                    if (Projectile.velocity.Y > 0f && yDist < 0f)
                        Projectile.velocity.Y -= acceleration;
                }

                if (Main.myPlayer == Projectile.owner)
                    if (Projectile.Hitbox.Intersects(owner.Hitbox))
                        Projectile.Kill();
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            State = 1f;
            target.AddBuff(BuffID.OnFire, 180);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            State = 1f;
            return false;
        }

    }

    public class FlamarangGlobal : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.type != ProjectileID.Flamarang)
                return;

            Player player = Main.player[projectile.owner];

            if (player == null || !player.active)
                return;

            SuperHeatingWeapons heat =
                player.GetModPlayer<SuperHeatingWeapons>();

            heat.AddHeat(10f);
        }
    }

}
