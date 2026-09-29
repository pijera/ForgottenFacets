using ForgottenFacets.Content.Buffs;
using ForgottenFacets.Content.Dusts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles.Topaz
{
    internal class StoneBreakerAOEProjectile : ModProjectile
    {
        public override string Texture => "ForgottenFacets/Assets/Misc/Invisible";

        public override void SetDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 5;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void AI()
        {

            for (int i = 0; i < 10; i++)
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<GlowDust>(), 0, -3, 130, Color.Orange).noGravity = true;

            for (int i = 0; i < 3; i++)
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SparkleDust>(), 0, -1, 130, Color.Orange).noGravity = true;

            for (int i = 0; i < 15; i++)
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Stone).noGravity = true;

        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride = target.Center.X > Projectile.Center.X ? 1 : -1;
            target.velocity.Y -= 10;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ArmorBreak1>(), 300);
        }
    }
}
