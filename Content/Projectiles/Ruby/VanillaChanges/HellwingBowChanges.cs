
using ForgottenFacets.Content.Dusts;
using ForgottenFacets.Content.ModPlayers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles.Ruby.VanillaChanges
{
    internal class HellwingBowChanges : ModProjectile
    {
        private const float HOMING_RANGE = 600f;
        private const float HOMING_SPEED = 12f;
        private const float HOMING_STRENGHT = 0.12f;

        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Hellwing;

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;

            Main.projFrames[Projectile.type] = 5;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0.27f, 0f);

            if (Main.rand.NextBool(1))
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.GemRuby, Main.rand.NextVector2CircularEdge    (2, 2), 100, default, Main.rand.NextFloat(0.7f, 0.8f));
                Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(2, 2), 100, default, Main.rand.NextFloat(0.9f, 1.4f)).noGravity = true;
            }

            if (Main.rand.NextBool(3))
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(2, 2), 100, Color.OrangeRed, Main.rand.NextFloat(0.4f, 0.8f)).noGravity = true;


            Projectile.frameCounter++;

            if (Projectile.frameCounter >= 2)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }


            NPC target = FindNearestEnemy();
            Main.projectile[Projectile.whoAmI].GetGlobalProjectile<HellwingbowyArrowChanges>().NoHeat = true;
            Projectile.ai[0]++;

            Projectile.tileCollide = false;
            if (target != null && Projectile.ai[0] >= 15)
            {
                Projectile.tileCollide = true;
                Vector2 desiredVelocity = Projectile.DirectionTo(target.Center) * HOMING_SPEED;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, HOMING_STRENGHT);
            }

            if (Projectile.velocity.LengthSquared() > 0.01f)
                Projectile.rotation = Projectile.velocity.ToRotation();
        }
        private NPC FindNearestEnemy()
        {
            NPC closest = null;
            float closestDistance = HOMING_RANGE * HOMING_RANGE;

            foreach(NPC npc in Main.npc)
            {
                if (!npc.CanBeChasedBy())
                    continue;

                float distance = Vector2.DistanceSquared(Projectile.Center, npc.Center);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = npc;
                }
            }
            return closest;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];

            Rectangle sourceRectangle = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, sourceRectangle, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

    }

    public class HellwingbowyArrowChanges : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool HellwingShot;
        public bool NoHeat;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource && itemSource.Item.type == ItemID.HellwingBow)
                HellwingShot = true;
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!HellwingShot)
                return;

            if (NoHeat)
                return;

            Player player = Main.player[projectile.owner];

            if (player == null || !player.active)
                return;

            player.GetModPlayer<SuperHeatingWeapons>().AddHeat(1.5f);
        }
    }

}
