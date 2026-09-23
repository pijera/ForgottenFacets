using ForgottenFacets.Content.ModPlayers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ForgottenFacets.Content.Projectiles.Ruby.VanillaChanges
{
    internal class ImpStaffChanges : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public const int OverheatDuration = 180;
        public const int OverheatFireRate = 10;

        public bool ImpOverheated;
        public int OverheatTimer;
        public int OverheatFireTimer;

        public override void AI(Projectile projectile)
        {
            if (projectile.type != ProjectileID.FlyingImp)
                return;

            if (!ImpOverheated)
                return;

            OverheatTimer--;

            if (OverheatTimer <= 0)
            {
                ImpOverheated = false;
                OverheatFireTimer = 0;
                projectile.netUpdate = true;
                return;
            }

            OverheatFireTimer--;

            if (OverheatFireTimer > 0)
                return;

            OverheatFireTimer = OverheatFireRate;

            if (Main.myPlayer != projectile.owner)
                return;

            FireRapidFireball(projectile);
        }

        private void FireRapidFireball(Projectile imp)
        {
            NPC target = FindTarget(imp);

            if (target == null)
                return;

            Vector2 direction = target.Center - imp.Center;

            if (direction == Vector2.Zero)
                return;

            direction.Normalize();

            Vector2 velocity = direction * 8f;

            Projectile.NewProjectile(imp.GetSource_FromAI(), imp.Center, velocity, ProjectileID.ImpFireball, imp.damage, imp.knockBack, imp.owner);
        }

        private NPC FindTarget(Projectile imp)
        {
            NPC closestTarget = null;
            float closestDistance = 900f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.active)
                    continue;
                if (npc.friendly)
                    continue;
                if (npc.dontTakeDamage)
                    continue;
                if (npc.life <= 0)
                    continue;

                float distance = Vector2.Distance(imp.Center, npc.Center);

                if (distance >= closestDistance)
                    continue;

                if (!Collision.CanHit(imp.Center, 1, 1, npc.Center, 1, 1))
                    continue;

                closestDistance = distance;
                closestTarget = npc;
            }

            return closestTarget;
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.type != ProjectileID.ImpFireball)
                return;

            Player player = Main.player[projectile.owner];

            if (player == null || !player.active)
                return;

            SuperHeatingWeapons heat = player.GetModPlayer<SuperHeatingWeapons>();

            if (!Main.projectile.Any(p =>
                p.active &&
                p.owner == projectile.owner &&
                p.type == ProjectileID.FlyingImp &&
                p.GetGlobalProjectile<ImpStaffChanges>().ImpOverheated))
            {
                heat.AddHeat(1f);
            }
        }



        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
            ImpOverheated = binaryReader.ReadBoolean();
            OverheatTimer = binaryReader.ReadInt32();
            OverheatFireTimer = binaryReader.ReadInt32();
        }

        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(ImpOverheated);
            binaryWriter.Write(OverheatTimer);
            binaryWriter.Write(OverheatFireTimer);
        }
    }
}
