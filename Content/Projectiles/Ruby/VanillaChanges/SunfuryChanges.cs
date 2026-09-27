using ForgottenFacets.Content.ModPlayers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles.Ruby.VanillaChanges
{
    internal class SunfuryChanges : ModProjectile
    {
        private int victimIndex = -1;
        public Player Owner => Main.player[Projectile.owner];

        public const float MAX_HOOK_DISTANCE = 500f;
        public const float PULL_STRENGHT = 1.5f;
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Sunfury;
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.scale *= 0.8f;

        }
        public override void AI()
        {
            Projectile.position.Y += 1f;
            Projectile.rotation += MathHelper.PiOver4;

            if (Owner.dead)
            {
                Projectile.Kill();
                return;
            }
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(3, 3), 20, default, Main.rand.NextFloat(1.5f, 2.5f));
            }

            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] == 5f)
                Projectile.tileCollide = true;

            Vector2 vector = Owner.Center - Projectile.Center;

            NPC victim = GetVictim();

            if (victim != null)
            {
                Vector2 offset = new Vector2(Owner.direction * 30f, 0f);

                victim.Center = Projectile.Center + offset;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    victim.velocity = Vector2.Zero;
                    victim.netUpdate = true;
                }
            }

            if (Projectile.ai[0] == 0f && vector.Length() > 600f)
                Projectile.ai[0] = 1f;
            if (Projectile.ai[0] >= 1f)
            {
                Projectile.tileCollide = false;
                float currentLength = vector.Length();
                if (currentLength > 400f)
                    Projectile.ai[0] = 2f;
                if (currentLength > 800f)
                {
                    Projectile.Kill();
                    return;
                }
                float min = Projectile.ai[0] == 2f ? 17f : 15f;
                Projectile.velocity = Vector2.Normalize(vector) * min;
                if (vector.Length() < min)
                {
                    Projectile.Kill();
                    return;
                }
            }

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2CircularEdge(3, 3), 120, default, Main.rand.NextFloat(1.5f, 2.5f));

            if (Projectile.ai[1] >= 5f)
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                Projectile.ai[0] = 1f;
                Projectile.netUpdate = true;
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.spriteDirection = -1;

            Texture2D texture = ModContent.Request<Texture2D>("ForgottenFacets/Assets/Projectiles/Chains/SunfuryChain").Value;
            Texture2D sunfuryTex = ModContent.Request<Texture2D>("Terraria/Images/Projectile_" + ProjectileID.Sunfury).Value;

            for (int i = 0; i < 5; i++)
            {
                Vector2 trailPos = Projectile.Center - Projectile.velocity * (i * 0.6f);

                float opacity = 1f - i / 6f;

                Main.EntitySpriteDraw(sunfuryTex, trailPos - Main.screenPosition, null, lightColor * opacity, Projectile.rotation, sunfuryTex.Size() / 2f, Projectile.scale, SpriteEffects.None);
            }

            Vector2 center = Projectile.Center;
            float xVel = Projectile.velocity.X;
            float yVel = Projectile.velocity.Y;
            float velDis = (float)Math.Sqrt(xVel * xVel + yVel * yVel);
            velDis = 4f / velDis;

            if (Projectile.ai[0] == 0f)
            {
                center.X -= Projectile.velocity.X * velDis;
                center.Y -= Projectile.velocity.Y * velDis;
            }
            else
            {
                center.X += Projectile.velocity.X * velDis;
                center.Y += Projectile.velocity.Y * velDis;
            }
            xVel = Owner.MountedCenter.X - center.X;
            yVel = Owner.MountedCenter.Y - center.Y;
            float rotation = (float)Math.Atan2(yVel, xVel) - 1.57f;
            if (Projectile.alpha == 0)
            {
                int num115 = -1;
                if (Projectile.position.X + (float)(Projectile.width / 2) < Owner.MountedCenter.X)
                {
                    num115 = 1;
                }
                if (Main.player[Projectile.owner].direction == 1)
                {
                    Main.player[Projectile.owner].itemRotation = (float)Math.Atan2(yVel * (float)num115, xVel * (float)num115);
                }
                else
                {
                    Main.player[Projectile.owner].itemRotation = (float)Math.Atan2(yVel * (float)num115, xVel * (float)num115);
                }
            }
            bool flag20 = true;
            while (flag20)
            {
                float num116 = (float)Math.Sqrt(xVel * xVel + yVel * yVel);
                if (num116 < 16f)
                {
                    flag20 = false;
                    continue;
                }
                if (float.IsNaN(num116))
                {
                    flag20 = false;
                    continue;
                }
                num116 = 8f / num116;
                xVel *= num116;
                yVel *= num116;
                center.X += xVel;
                center.Y += yVel;
                xVel = Owner.MountedCenter.X - center.X;
                yVel = Owner.MountedCenter.Y - center.Y;
                Color color = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16f));
                Main.EntitySpriteDraw(texture, center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), color, rotation, new Vector2(texture.Width, texture.Height), 1f, SpriteEffects.None, 0);
            }
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            victimIndex = -1;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[0] = 1f;
            Projectile.netUpdate = true;

            if (target.whoAmI == victimIndex)
                hit.Damage = 0;

            if (target.boss && victimIndex != -1)
                return;

            if (target.boss)
                return;

            target.AddBuff(BuffID.OnFire, 120);
            Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2CircularEdge(3, 3), 140, default, Main.rand.NextFloat(1, 2));

            victimIndex = target.whoAmI;
            Projectile.netUpdate = true;

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                using (var packet = Mod.GetPacket())
                {
                    packet.Write((byte)0);
                    packet.Write(Projectile.whoAmI);
                    packet.Write(target.whoAmI);
                    packet.Send();
                }
            }
        }

        public NPC GetVictim()
        {
            if (victimIndex < 0 || victimIndex >= Main.maxNPCs)
                return null;

            NPC npc = Main.npc[victimIndex];

            if (!npc.active || npc.boss)
                return null;

            return npc;
        }

        public void SetVictim(int npcIndex)
        {
            if (npcIndex < 0 || npcIndex >= Main.maxNPCs)
                return;

            NPC npc = Main.npc[npcIndex];

            if (!npc.active || npc.boss)
                return;

            victimIndex = npcIndex;
            Projectile.netUpdate = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(victimIndex);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            victimIndex = reader.ReadInt32();
        }
    }

    public class SunfuryProjectileChanges : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool SunfuryShot;
        public bool NoHeat;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource &&
                itemSource.Item.type == ItemID.Sunfury)
            {
                SunfuryShot = true;
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!SunfuryShot)
                return;

            if (NoHeat)
                return;

            Player player = Main.player[projectile.owner];

            if (player == null || !player.active)
                return;

            player.GetModPlayer<SuperHeatingWeapons>().AddHeat(5f);
        }
    }

}