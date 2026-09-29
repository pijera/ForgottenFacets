using ForgottenFacets.Content.Buffs;
using ForgottenFacets.Content.Dusts;
using ForgottenFacets.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles.Topaz
{
    internal class StoneBreakerProjectile : ModProjectile
    {
        private const float SWINGRANGE = 1.67f * (float)Math.PI;
        private const float FIRSTHALFSWING = 0.45f;
        private const float WINDUP = 0.15f;
        private const float UNWIND = 0.4f;

        public override string Texture => "ForgottenFacets/Assets/Items/Weapons/Meele/StoneBreaker";

        private enum AttackType
        {
            Swing
        }

        private enum AttackStage
        {
            Prepare,
            Execute,
            Unwind,
            Stuck
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

        private Vector2 PreviousHeadPosition;
        private ref float InitialAngle => ref Projectile.ai[1];
        private ref float Timer => ref Projectile.ai[2];
        private ref float Progress => ref Projectile.localAI[1];
        private ref float Size => ref Projectile.localAI[2];

        private float prepTime => 60f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
        private float execTime => 12f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
        private float hideTime => 3f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
        private Player Owner => Main.player[Projectile.owner];

        private Vector2 StuckPosition;
        private float StuckRotation;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
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
            {
                targetAngle = MathHelper.Clamp(targetAngle, (float)-Math.PI * 1f / 3f, (float)Math.PI * 1f / 6f);
            }
            else
            {
                if (targetAngle < 0)
                    targetAngle += 2f * (float)Math.PI;

                targetAngle = MathHelper.Clamp(targetAngle, (float)Math.PI * 5f / 6f, (float)Math.PI * 4f / 3f);
            }

            InitialAngle = Projectile.spriteDirection == 1 ? MathHelper.ToRadians(20f) - FIRSTHALFSWING * SWINGRANGE * Projectile.spriteDirection :
                MathHelper.ToRadians(180f) - FIRSTHALFSWING * SWINGRANGE * Projectile.spriteDirection;
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
                    PrepareStrike();
                    break;
                case AttackStage.Execute:
                    ExecuteStrike();
                    break;
                case AttackStage.Unwind:
                    UnwindStrike();
                    break;
                case AttackStage.Stuck:
                    StuckInGround();
                    break;
            }


            if (CurrentStage != AttackStage.Stuck)
                SetSwordPosition();

            if (CurrentStage == AttackStage.Stuck)
                Owner.velocity.X = 0f;

            CheckTileCollision();
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
                effects = SpriteEffects.None;
                float finalRotation = Projectile.rotation + rotationOffset;

                if (CurrentStage == AttackStage.Execute)
                {
                    Main.EntitySpriteDraw(swoosh, Projectile.Center - Main.screenPosition + new Vector2(0, Owner.gfxOffY), null, Color.Orange with { A = 0 } * 0.3f, 
                        (finalRotation + MathHelper.ToRadians(45f)) + MathHelper.ToRadians(Projectile.ai[1] == 1 ? -70f : 70f) * -Owner.direction, swoosh.Size() * 0.5f, Projectile.scale * 0.25f, SpriteEffects.None);
                }
            }
            else
            {
                origin = new Vector2(Projectile.width, Projectile.height);
                rotationOffset = MathHelper.ToRadians(135f);
                effects = SpriteEffects.FlipHorizontally;
                float finalRotation = Projectile.rotation + rotationOffset;

                if (CurrentStage == AttackStage.Execute)
                {
                    Main.EntitySpriteDraw(swoosh, Projectile.Center - Main.screenPosition + new Vector2(0, Owner.gfxOffY), null, Color.Orange with { A = 0 } * 0.3f, 
                        (finalRotation + MathHelper.ToRadians(45f)) + MathHelper.ToRadians(Projectile.ai[1] == 1 ? -70f : 70f) * -Owner.direction, swoosh.Size() * 0.5f, Projectile.scale * 0.25f, SpriteEffects.FlipVertically);
                }
            }

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, default, lightColor * Projectile.Opacity, Projectile.rotation + rotationOffset, origin, Projectile.scale, effects, 0);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Owner.MountedCenter;
            Vector2 end = start + Projectile.rotation.ToRotationVector2();
            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Projectile.scale, ref collisionPoint);
        }

        public override void CutTiles()
        {
            Vector2 start = Owner.MountedCenter;
            Vector2 end = start + Projectile.rotation.ToRotationVector2();
            Utils.PlotTileLine(start, end, Projectile.scale, DelegateMethods.CutTiles);
        }

        public override bool? CanDamage()
        {
            if (CurrentStage == AttackStage.Prepare)
                return false;

            return base.CanDamage();
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride = target.Center.X > Projectile.Center.X ? 1 : -1;
        }

        public void SetSwordPosition()
        {
            Projectile.rotation = InitialAngle + Projectile.spriteDirection * Progress;
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(90f));
            Vector2 armPosition = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - (float)Math.PI / 2f);

            if (Owner.gravDir == -1f)
            {
                Projectile.rotation = 0f - Projectile.rotation;
                armPosition.Y = Owner.Bottom.Y + (Owner.position.Y - armPosition.Y);
            }

            armPosition.Y += Owner.gfxOffY;
            Vector2 gripOffset = Projectile.rotation.ToRotationVector2() * -15f;
            armPosition += gripOffset;
            Projectile.Center = armPosition;
            Projectile.scale = Size * 1.3f * Owner.GetAdjustedItemScale(Owner.HeldItem);
            Owner.heldProj = Projectile.whoAmI;
        }

        private void PrepareStrike()
        {
            Progress = WINDUP * (1f - Timer / prepTime);
            Size = 1f;

            if (Timer >= prepTime)
            {
                SoundEngine.PlaySound(SoundID.Item1);
                CurrentStage = AttackStage.Execute;
            }
        }

        private void ExecuteStrike()
        {
            Vector2 dustPosition = Projectile.Center + Projectile.rotation.ToRotationVector2() * 90f;
            Progress = MathHelper.SmoothStep(0.3f, SWINGRANGE, (1f - UNWIND) * Timer / execTime);

            for (int i = 0; i < 25; i++)
            {
                Dust.NewDustPerfect(dustPosition, DustID.Stone, Main.rand.NextVector2Circular(3, 3), 50, default, Main.rand.NextFloat(0.9f, 1.2f)).noGravity = true;
                Dust.NewDustPerfect(dustPosition, DustID.GemTopaz, Main.rand.NextVector2Circular(3, 3), 150, default, Main.rand.NextFloat(0.9f, 1.2f)).noGravity = true;
            }

            if (Timer >= execTime)
                CurrentStage = AttackStage.Unwind;
        }

        private void UnwindStrike()
        {
           // Progress = MathHelper.SmoothStep(0, SWINGRANGE, (1f - UNWIND) + UNWIND * Timer / hideTime);
            Size = 1f - MathHelper.SmoothStep(0, 1, Timer / hideTime);

            if (Timer >= hideTime)
                Projectile.Kill();
        }

        private void StuckInGround()
        {
            Projectile.Center = StuckPosition;
            Projectile.rotation = StuckRotation;
            Projectile.velocity = Vector2.Zero;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Owner.heldProj = -1;

            if (Timer >= 10f)
                CurrentStage = AttackStage.Unwind;
        }


        private void CheckTileCollision()
        {
            if (CurrentStage != AttackStage.Execute)
                return;

            Vector2 start = Owner.MountedCenter;
            Vector2 direction = Projectile.rotation.ToRotationVector2();
            Vector2 headPosition = start + direction * 90f;

            Point tilePosition = headPosition.ToTileCoordinates();

            if (!WorldGen.InWorld(tilePosition.X, tilePosition.Y, 1))
                return;

            Tile tile = Framing.GetTileSafely(tilePosition.X, tilePosition.Y);

            if (!tile.HasTile)
                return;

            if (!Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType])
                return;

            Rectangle tileRect = new Rectangle(tilePosition.X * 16, tilePosition.Y * 16, 16, 16);

            if (!tileRect.Contains(headPosition.ToPoint()))
                return;

            StuckPosition = Projectile.Center;
            StuckRotation = Projectile.rotation;

            ScreenShake screen = Main.player[Projectile.owner].GetModPlayer<ScreenShake>();
            screen.AddShake(15);

            Projectile.NewProjectile(Projectile.GetSource_FromAI(),Owner.Bottom,Vector2.Zero,ModContent.ProjectileType<StoneBreakerAOEProjectile>(),Owner.HeldItem.damage,8);

            SoundEngine.PlaySound(SoundID.Item70 with { Volume = Main.rand.NextFloat(0.8f, 2f), PitchRange = (-0.5f, 0.5f) });

            CurrentStage = AttackStage.Stuck;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ScreenShake screen = Main.player[Projectile.owner].GetModPlayer<ScreenShake>();
            target.AddBuff(ModContent.BuffType<ArmorBreak1>(), 300);
            screen.AddShake(10);
        }
    }
}
