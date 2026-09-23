using ForgottenFacets.Content.Dusts;
using ForgottenFacets.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles.Ruby
{
    internal class CinderSickleFullHeatProjectile : ModProjectile
    {
        private const float SWINGRANGE = 1.67f * (float)Math.PI;
        private const float FIRSTHALFSWING = 0.45f;
        private const float WINDUP = 0.1f;
        private const float UNWIND = 0.2f;

        private const float DASHSPEED = 15f;

        public override string Texture => "ForgottenFacets/Assets/Projectiles/CinderSickleFullHeatProjectile";

        private enum AttackType
        {
            Swing
        }

        private enum AttackStage
        {
            Prepare,
            Execute,
            Unwind
        }

        private AttackType CurrentAttack
        {
            get => (AttackType)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }

        private AttackStage CurrentStage
        {
            get => (AttackStage)Projectile.localAI[0];
            set
            {
                Projectile.localAI[0] = (float)value;
                Timer = 0;
            }
        }

        private ref float InitialAngle => ref Projectile.ai[1];
        private ref float Timer => ref Projectile.ai[2];
        private ref float Progress => ref Projectile.localAI[1];
        private ref float Size => ref Projectile.localAI[2];

        private float prepTime => 12f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
        private float execTime => 12f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
        private float hideTime => 12f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
        private Player Owner => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.timeLeft = 1000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.spriteDirection = Main.MouseWorld.X > Owner.MountedCenter.X ? 1 : -1;
            float targetAngle = (Main.MouseWorld - Owner.MountedCenter).ToRotation();

            if (Projectile.spriteDirection == 1)
            targetAngle = MathHelper.Clamp(targetAngle, (float)-Math.PI * 1 / 3, (float)Math.PI * 1 / 6);
            else
            {
                if (targetAngle < 0)
                    targetAngle += 2 * (float)Math.PI;
                targetAngle = MathHelper.Clamp(targetAngle, (float)Math.PI * 5 / 6, (float)Math.PI * 4 / 3);
            }

            InitialAngle = targetAngle - FIRSTHALFSWING * SWINGRANGE * Projectile.spriteDirection;
            
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((sbyte)Projectile.spriteDirection);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.spriteDirection = reader.ReadSByte();
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0f, 0f);

            Owner.itemAnimation = 2;
            Owner.itemTime = 2;


            if (!Owner.active || Owner.dead || Owner.noItems || Owner.CCed)
            {
                Projectile.Kill();
                return;
            }

            switch (CurrentStage)
            {
                case AttackStage.Prepare:
                    Dash();
                    PrepareStrike();
                    break;
                case AttackStage.Execute:
                    ExecuteStrike();
                    break;
                default:
                    UnwindStrike();
                    break;
            }

            SetSwordPosition();
            Timer++;


        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 origin;
            float rotationOffset;
            SpriteEffects effects;
            Texture2D swoosh = ModContent.Request<Texture2D>("ForgottenFacets/Assets/Misc/Smears/VerticalSmearLarge").Value;


            if (Projectile.spriteDirection > 0)
            {
                origin = new Vector2(0, Projectile.height);
                rotationOffset = MathHelper.ToRadians(45f);

                float FinalRotation = Projectile.rotation + rotationOffset;

                effects = SpriteEffects.None;

                if (CurrentStage == AttackStage.Execute)
                {
                    Main.EntitySpriteDraw(swoosh, Projectile.Center - Main.screenPosition + new Vector2(0, Owner.gfxOffY), null, Color.OrangeRed with { A = 0 } * 0.5f,
                        (FinalRotation + MathHelper.ToRadians(45)) + MathHelper.ToRadians(Projectile.ai[1] == 1 ? -70 : 70) * -Owner.direction, swoosh.Size() * 0.5f, 
                        Projectile.scale * 0.25f, SpriteEffects.None);
                }
            }
            else
            {
                origin = new Vector2(Projectile.width, Projectile.height);
                rotationOffset = MathHelper.ToRadians(135f);
                effects = SpriteEffects.FlipHorizontally;

                float FinalRotation = Projectile.rotation + rotationOffset;

                if (CurrentStage == AttackStage.Execute)
                {
                    Main.EntitySpriteDraw(swoosh, Projectile.Center - Main.screenPosition + new Vector2(0, Owner.gfxOffY), null, Color.OrangeRed with { A = 0 } * 0.5f,
                        (FinalRotation + MathHelper.ToRadians(45)) + MathHelper.ToRadians(Projectile.ai[1] == 1 ? -70 : 70) * -Owner.direction, swoosh.Size() * 0.5f, 
                        Projectile.scale * 0.25f, SpriteEffects.FlipVertically);
                }
            }


            Texture2D texture = TextureAssets.Projectile[Type].Value;

            

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, default, lightColor * Projectile.Opacity, Projectile.rotation + rotationOffset, origin, Projectile.scale, effects, 0);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Owner.MountedCenter;
            Vector2 end = start + Projectile.rotation.ToRotationVector2() * 180f;
            float collisonPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 15f * Projectile.scale, ref collisonPoint);
        }

        public override void CutTiles()
        {
            Vector2 start = Owner.MountedCenter;
            Vector2 end = start + Projectile.rotation.ToRotationVector2() * (Projectile.Size.Length() * Projectile.scale);
            Utils.PlotTileLine(start, end, 15 * Projectile.scale, DelegateMethods.CutTiles);
        }

        public override bool? CanDamage()
        {
            if (CurrentStage.Equals(AttackStage.Prepare))
                return false;
            return base.CanDamage();
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride = target.position.X > Owner.MountedCenter.X ? 1 : -1;
        }

        public void SetSwordPosition()
        {
            Projectile.rotation = InitialAngle + Projectile.spriteDirection * Progress;

            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(90f));
            Vector2 armPosition = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - (float)Math.PI / 2);

            if (Owner.gravDir == -1f)
            {
                Projectile.rotation = 0f - Projectile.rotation;
                armPosition.Y = Owner.Bottom.Y + (Owner.position.Y - armPosition.Y);
            }

            armPosition.Y += Owner.gfxOffY;

            Vector2 gripOffset = Projectile.rotation.ToRotationVector2() * -30f;
            armPosition += gripOffset;

            Projectile.Center = armPosition;
            Projectile.scale = Size * 3.5f * Owner.GetAdjustedItemScale(Owner.HeldItem);
            Owner.heldProj = Projectile.whoAmI;
        }


        private void PrepareStrike()
        {
            Progress = WINDUP * SWINGRANGE * (1f - Timer / prepTime);
            Size = MathHelper.SmoothStep(0, 1, Timer / prepTime);

            if (Timer >= prepTime)
            {
                SoundEngine.PlaySound(SoundID.Item1);
                CurrentStage = AttackStage.Execute;
            }
        }

        private void Dash()
        {
            for (int i = 0; i < 7; i++)
            {
                Dust.NewDustPerfect(Owner.Center, DustID.Torch, Main.rand.NextVector2Circular(3, 3), 250, default, Main.rand.NextFloat(1f, 1.4f));
            }
            for(int i=0; i < 5; i++)
            {
                Dust.NewDustPerfect(Owner.Center, DustID.GemRuby, Main.rand.NextVector2Circular(3, 3), 200, default, Main.rand.NextFloat(1f, 1.4f));
            }

            Dust.NewDustPerfect(Owner.Center, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(3, 3), 100, Color.OrangeRed, Main.rand.NextFloat(0.7f, 1f));
            Dust.NewDustPerfect(Owner.Center, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(3, 3), 100, Color.OrangeRed, Main.rand.NextFloat(0.6f, 0.9f));
            if (Timer == 0)
            {
                SoundEngine.PlaySound(SoundID.Item74, Owner.Center);
                Vector2 dashDirection = (Main.MouseWorld - Owner.MountedCenter).SafeNormalize(Vector2.UnitX);
                Owner.velocity = dashDirection * DASHSPEED;
            }
            if (Timer >= prepTime)
            {
                Owner.velocity *= 0.1f;
            }
        }


        private void ExecuteStrike()
        {
            Vector2 dustPosition = Projectile.Center + Projectile.rotation.ToRotationVector2() * 180f;

            Progress = MathHelper.SmoothStep(0, SWINGRANGE, (1f - UNWIND) * Timer / execTime);

            for (int i = 0; i < 30; i++)
            {
                Dust.NewDustPerfect(dustPosition, DustID.Torch, Main.rand.NextVector2Circular(3, 3), 250, default, Main.rand.NextFloat(1f, 1.2f));
                Dust.NewDustPerfect(dustPosition, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(3, 3), 100, Color.OrangeRed, Main.rand.NextFloat(0.6f, 1.2f));
                Dust.NewDustPerfect(dustPosition, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(3, 3), 100, Color.OrangeRed, Main.rand.NextFloat(0.8f, 1.2f));
            }

            for (int i = 0; i < 4; i++)
                Dust.NewDustPerfect(dustPosition, DustID.GemRuby, Main.rand.NextVector2Circular(3, 3), 120, default, Main.rand.NextFloat(1.5f, 2f));

            if (Timer >= execTime)
                CurrentStage = AttackStage.Unwind;
        }

        private void UnwindStrike()
        {

            Progress = MathHelper.SmoothStep(0, SWINGRANGE, (1f - UNWIND) + UNWIND * Timer / hideTime);
            Size = 1f - MathHelper.SmoothStep(0, 1, Timer / hideTime);

            if (Timer >= hideTime)
                Projectile.Kill();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ScreenShake screen = Main.player[Projectile.owner].GetModPlayer<ScreenShake>();
            screen.AddShake(15);

            target.AddBuff(BuffID.OnFire, 180);
        }



    }
}
