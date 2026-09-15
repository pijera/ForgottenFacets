using ForgottenFacets.Content.Dusts;
using ForgottenFacets.Content.Items.Weapons.Magic;
using ForgottenFacets.Content.ModPlayers;
using ForgottenFacets.Core;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Projectiles.Ruby
{
    internal class CinderbranchHoldOut : ModProjectile
    {
        public override string Texture => "ForgottenFacets/Assets/Items/Weapons/Magic/Cinderbranch";
        public bool Dying;
        public int Timer
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public int UseTime
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public int AnimationTime
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[2] = value;
        }

        public int manaCost = 5;

        public float recoilTimer;

        public Player Owner => Main.player[Projectile.owner];

        public bool canHold => Owner.HeldItem.ModItem is Cinderbranch && Owner.channel && !Owner.CCed && !Owner.noItems;
        public Vector2 ArmPosition => Owner.RotatedRelativePoint(Owner.MountedCenter, true) + new Vector2(16f, 2f).RotatedBy(Projectile.rotation);

        public override bool? CanDamage() => false;

        public override void SetDefaults()
        {
            Projectile.width = 27;
            Projectile.height = 27;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {

            if (!canHold && !Dying)
            {
                Dying = true;
                Projectile.timeLeft = 20;
            }

            if (AnimationTime > 0)
                AnimationTime--;

            if (Timer == 0f)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    if (Main.myPlayer == Projectile.owner)
                        Projectile.velocity = Owner.DirectionTo(Main.MouseWorld);

                    Projectile.rotation = Projectile.velocity.ToRotation();
                    Projectile.netUpdate = true;
                    UseTime = CombinedHooks.TotalUseTime(Owner.itemTime, Owner, Owner.HeldItem);
                }
            }

            if (!Dying)
            {
                if (Timer % UseTime == 0)
                    AnimationTime = 20;

                UpdateHeldProjectile();
                Timer++;

                const int ticks = 3;
                if (AnimationTime > 0 && AnimationTime % ticks == 0)
                {
                    SpawnProjectiles();
                    Projectile.velocity = Projectile.velocity.RotatedByRandom(0.1f);
                    Dust.NewDustPerfect(Projectile.position, DustID.Torch, Main.rand.NextVector2Circular(4, 4), 120, default, Main.rand.NextFloat(0.8f, 1.2f));
                }


            }
            else
                UpdateHeldProjectile(false, false);
        }

        public void SpawnProjectiles()
        {
            if (Owner.statMana < Owner.HeldItem.mana)
            {
                Dying = true;
                Projectile.timeLeft = 10;
                return;
            }
            else
                Owner.statMana -= 5;

            SoundEngine.PlaySound(SoundID.Item34 with { Volume = Main.rand.NextFloat(0.3f, 0.5f), Pitch = Main.rand.NextFloat(-1.2f, 0.4f) });

            ScreenShake screen = Main.player[Projectile.owner].GetModPlayer<ScreenShake>();
            SuperHeatingWeapons HeatStacking = Main.player[Projectile.owner].GetModPlayer<SuperHeatingWeapons>();

            if (HeatStacking.IsSuperHeated)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Owner.Center, Vector2.Zero, ModContent.ProjectileType<CinderbranchFullHeatExplosion>(), 80, 5, Owner.whoAmI, 360f);
                screen.AddShake(15);
                SoundEngine.PlaySound(SoundID.Item62 with { Volume = 0.7f, PitchRange = (-0.5f, 1f) });
                SoundEngine.PlaySound(SoundID.DD2_BetsyFlameBreath with { Volume = 0.2f, PitchRange = (-0.8f, 0.8f) });
                SoundEngine.PlaySound(SoundID.DD2_BetsyWindAttack with { Volume = 0.2f, PitchRange = (-0.8f, 0.8f) });
                HeatStacking.ConsumeHeat();
            }

            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedByRandom(0.25f) * Main.rand.NextFloat(8f, 12f), ModContent.ProjectileType<CinderbranchProjectile>(), 0, 0, Projectile.owner);
                }

                if (Main.rand.NextBool(1))
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedByRandom(0.25f) * Main.rand.NextFloat(8f, 12f), ModContent.ProjectileType<CinderbranchProjectile>(), Owner.HeldItem.damage, Owner.HeldItem.knockBack, Projectile.owner);


                for (int i = 0; i < Main.rand.Next(7); i++)
                {
                    Dust.NewDustPerfect(Projectile.Center + new Vector2(30f, 0f).RotatedBy(Projectile.rotation) + Main.rand.NextVector2Circular(20f, 20f), ModContent.DustType<SparkleDust>(),
                        Projectile.velocity.RotatedByRandom(0.25f) * Main.rand.NextFloat(8f, 12f), 0,
                        Color.Lerp(Color.Yellow, Color.OrangeRed with { A = 0 }, Main.rand.NextFloat()), Main.rand.NextFloat(0.5f, 0.6f)).customData = true;

                    Dust.NewDustPerfect(Projectile.Center + new Vector2(30f, 0f).RotatedBy(Projectile.rotation) + Main.rand.NextVector2Circular(20f, 20f), ModContent.DustType<GlowDust>(),
                        Projectile.velocity.RotatedByRandom(0.25f) * Main.rand.NextFloat(8f, 12f), 0,
                        Color.Lerp(Color.Yellow, Color.OrangeRed with { A = 0 }, Main.rand.NextFloat()), Main.rand.NextFloat(0.5f, 0.6f)).customData = true;

                }
            }
            

        }

        public void UpdateHeldProjectile(bool updateTimeLeft = true, bool updateVelocity = true)
        {
            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;

            Owner.itemTime = 2;
            Owner.itemAnimation = 2;

            if (updateTimeLeft)
                Projectile.timeLeft = 2;

            Projectile.rotation = Projectile.velocity.ToRotation();
            Owner.itemRotation = Utils.ToRotation(Projectile.velocity * Projectile.direction);

            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
            Projectile.position = ArmPosition - Projectile.Size * 0.5f;

            if (Main.myPlayer == Projectile.owner && updateVelocity)
            {
                Vector2 oldVelocity = Projectile.velocity;

                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Owner.DirectionTo(Main.MouseWorld), 0.1f);

                if (Projectile.velocity != oldVelocity)
                {
                    Projectile.netSpam = 0;
                    Projectile.netUpdate = true;
                }
            }


            Projectile.spriteDirection = Projectile.direction;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var tex = ModContent.Request<Texture2D>("ForgottenFacets/Assets/Items/Weapons/Magic/Cinderbranch").Value;
            var outline = ModContent.Request<Texture2D>("ForgottenFacets/Assets/Items/Weapons/Magic/Cinderbranch_Outline").Value;

            SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Vector2 positon = ArmPosition - Main.screenPosition;

            float rotation = Projectile.rotation + (spriteEffects == SpriteEffects.FlipHorizontally ? MathHelper.Pi : 0f) + MathHelper.PiOver4 * Projectile.spriteDirection;

            float fadeIn = 1f;

            if (Timer < 8f)
                fadeIn = Timer / 8f;

            else if (Dying)
                fadeIn = Projectile.timeLeft / 10f;

            Main.spriteBatch.Draw(tex, positon, null, lightColor * fadeIn, rotation, tex.Size() / 2f, Projectile.scale, spriteEffects, 0f);

            float opacity = MathHelper.Min(60, Timer) / 60f;

            Main.spriteBatch.Draw(outline, positon, null, lightColor * opacity * fadeIn, rotation, outline.Size() / 2f, Projectile.scale * Projectile.timeLeft/60 * 0.8f, spriteEffects, 0f);

            return false;

        }

    }
}
