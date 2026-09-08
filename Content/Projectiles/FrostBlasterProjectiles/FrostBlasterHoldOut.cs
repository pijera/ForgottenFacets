using ForgottenFacets.Content.Dusts;
using ForgottenFacets.Content.Items.Weapons.Ranged;
using ForgottenFacets.Core;
using Humanizer;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Social.Base;

namespace ForgottenFacets.Content.Projectiles.FrostBlasterProjectiles
{
    internal class FrostBlasterHoldOut : ModProjectile
    {
        private int chargeDust = -1;
        private int maxCharge = 90;
        public bool Dying;
        public int recoilTimer;
        public int ChargeTimer
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override string Texture => "ForgottenFacets/Assets/Items/Weapons/Ranged/FrostBlaster";
        public bool CanHold => Owner.HeldItem.ModItem is FrostBlaster && Owner.channel && !Owner.CCed && !Owner.noItems;
        public Vector2 ArmPostion => Owner.RotatedRelativePoint(Owner.MountedCenter, true) + new Vector2(16,2f).RotatedBy(Projectile.rotation) + armOffset;

        public Vector2 armOffset;
        public Player Owner => Main.player[Projectile.owner];

        public override bool? CanDamage() => false;

        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 16;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        

        public override void AI()
        {
            Vector2 pointingDir = Main.MouseWorld - Projectile.Center;
            pointingDir.Normalize();

            Vector2 barrelPosition = Projectile.Center + pointingDir * 38f + new Vector2(0, -5);



            if (!CanHold && !Dying)//call when player lifts their finger of left click
            {
                Dying = true;
                Projectile.timeLeft = 20;

                SpawnProjectiles(ChargeTimer);

            }


            if (ChargeTimer == 0f)
            {
                if(Main.myPlayer == Projectile.owner)
                    Projectile.velocity = Owner.DirectionTo(Main.MouseWorld) * 0.1f; //* 10f

                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.netUpdate = true;
            }

            if (!Dying)
            {
                ChargeTimer++;

                if (ChargeTimer == 30)
                {
                    SoundEngine.PlaySound(SoundID.Item30 with { Volume = 2, Pitch = 0.9f });
                    SoundEngine.PlaySound(SoundID.Item29 with { Volume = 0.5f, Pitch = 1f });

                    for (int i = 0; i < 20; i++)
                    {
                        Dust.NewDustPerfect(Projectile.Center, DustID.Frost, Main.rand.NextVector2CircularEdge(1f,1f), 0, default, 1f).noGravity = true;
                    }
                }
                    
                else if (ChargeTimer == 60)
                {
                    SoundEngine.PlaySound(SoundID.Item30 with { Volume = 4, Pitch = 0.7f });
                    SoundEngine.PlaySound(SoundID.Item29 with { Volume = 0.8f, Pitch = 0.9f });

                    for (int i = 0; i < 25; i++)
                    {
                        Dust.NewDustPerfect(Projectile.Center, DustID.Frost, Main.rand.NextVector2CircularEdge(1.5f, 1.5f), 0, default, 1.5f).noGravity = true;
                    }
                }
                    
                else if (ChargeTimer == 90)
                {
                    SoundEngine.PlaySound(SoundID.Item30 with { Volume = 6, Pitch = 0.5f });
                    SoundEngine.PlaySound(SoundID.Item29 with { Volume = 1f, Pitch = 1f });

                    for (int i = 0; i < 30; i++)
                    {
                        Dust.NewDustPerfect(Projectile.Center, DustID.Frost, Main.rand.NextVector2CircularEdge(2f, 2f), 0, default, 2f).noGravity = true;
                    }
                }
                else if(ChargeTimer > 90)
                {
                    Owner.velocity.X = MathHelper.Clamp(Owner.velocity.X, -6, 6);

                    if (chargeDust == -1 || !Main.dust[chargeDust].active)
                    {
                        Dust dust = Dust.NewDustPerfect(barrelPosition, DustID.IceTorch, Vector2.Zero, 0, default, 1.5f);

                        dust.noGravity = true;
                        chargeDust = dust.dustIndex;
                    }
                    else
                    {
                        Main.dust[chargeDust].position = barrelPosition;
                        Main.dust[chargeDust].velocity = Vector2.Zero;
                    }

                    // Extra particles
                    if (Main.rand.NextBool(1))
                    {
                        Dust dust = Dust.NewDustPerfect(barrelPosition, DustID.IceTorch, Main.rand.NextVector2Circular(1f, 1f), 50, default, Main.rand.NextFloat(1.8f, 2f));

                        dust.noGravity = true;
                    }

                }
                else
                {
                    if (chargeDust != -1)
                    {
                        Main.dust[chargeDust].active = false;
                        chargeDust = -1;
                    }
                }


                UpdateHeldProjectile();
            }
            else
                UpdateHeldProjectile(false,false);

        }

        public void SpawnProjectiles(float chargeAmount)    
        {

            Vector2 direction = Main.MouseWorld - Projectile.Center;
            direction.Normalize();

            Vector2 barrelPosition = Projectile.Center + direction * 38f + new Vector2(0, -5);


            if (chargeAmount < maxCharge)
            {
                if (chargeAmount < 31)//very weak shot
                {
                    SoundEngine.PlaySound(SoundID.Item28 with { Volume = Main.rand.NextFloat(0.8f, 1.2f), PitchRange = (0.8f, 1.2f) });

                    for (int i = 0; i < 8; i++)
                    {
                        Dust.NewDustPerfect(barrelPosition, DustID.Frost, Main.rand.NextVector2Circular(3, 3), 150, default, Main.rand.NextFloat(0.5f,0.8f));
                    }

                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), barrelPosition, (Projectile.velocity * 10), ModContent.ProjectileType<FrostBlasterVeryWeakProjectile>(), (int)Projectile.damage / 5, Projectile.knockBack * 0, Projectile.owner);
                }
                if (chargeAmount >= 31 && chargeAmount <= 60)//weak shot
                {
                    SoundEngine.PlaySound(SoundID.Item28 with { Volume = Main.rand.NextFloat(1f, 1.5f), Pitch = 0.6f});
                    //SoundEngine.PlaySound(SoundID.Item50 with { Volume = Main.rand.NextFloat(1.6f, 1.9f), PitchRange = (1f, 1.3f) });

                    for (int i = 0; i < 14; i++)
                    {
                        Dust.NewDustPerfect(barrelPosition, DustID.Frost, Main.rand.NextVector2Circular(3, 3), 150, default, Main.rand.NextFloat(0.8f, 1f));
                        Dust.NewDustPerfect(barrelPosition, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(3, 3), 150, default, Main.rand.NextFloat(1f, 1.3f));
                    }

                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Projectile.velocity * 10) * 1.2f, ModContent.ProjectileType<FrostBlasterWeakProjectile>(), Projectile.damage / 3, Projectile.knockBack / 2, Projectile.owner);
                }
                if (chargeAmount >= 61 && chargeAmount < 90)//medium shot
                {
                    SoundEngine.PlaySound(SoundID.Item28 with { Volume = Main.rand.NextFloat(1.6f, 1.9f), PitchRange = (0.5f, 0.8f) });
                    SoundEngine.PlaySound(SoundID.Item50 with { Volume = Main.rand.NextFloat(2f, 2.2f), PitchRange = (0.8f, 1f) });

                    for (int i = 0; i < 20; i++)
                    {
                        Dust.NewDustPerfect(barrelPosition, DustID.Frost, Main.rand.NextVector2Circular(3, 3), 150, default, Main.rand.NextFloat(1f, 1.3f));
                    }
                    for (int i = 0; i < 15; i++)
                    {
                        Dust.NewDustPerfect(barrelPosition, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(4, 4), 100, default, Main.rand.NextFloat(0.8f, 1f));

                        Dust.NewDustPerfect(barrelPosition, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(5, 5), 50, Color.LightCyan, Main.rand.NextFloat(0.3f, 0.4f));
                    }

                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Projectile.velocity * 10) * 1.6f, ModContent.ProjectileType<FrostBlasterMediumProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }

                return;
            }

            //strong shot
            SoundEngine.PlaySound(SoundID.Item28 with { Volume = Main.rand.NextFloat(1.6f, 1.9f), PitchRange = (0.5f, 0.8f) });
            SoundEngine.PlaySound(SoundID.Item50 with { Volume = Main.rand.NextFloat(2f, 2.2f), PitchRange = (0.8f, 1f) });
            SoundEngine.PlaySound(SoundID.Item38 with { Volume = Main.rand.NextFloat(0.2f, 0.3f), PitchRange = (0.2f, 0.3f) });

            Owner.GetModPlayer<ScreenShake>().AddShake(6);

            for (int i = 0; i < 30; i++)
            {
                Dust.NewDustPerfect(barrelPosition, DustID.Frost, Main.rand.NextVector2Circular(5, 5), 200, default, Main.rand.NextFloat(1f, 1.3f)).noGravity = false;
                Dust.NewDustPerfect(barrelPosition, DustID.IceTorch, Main.rand.NextVector2CircularEdge(3, 3), 0, default, Main.rand.NextFloat(0.8f, 1.2f)).noGravity = false;
            }
            for (int i = 0; i < 25; i++)
            {
                Dust.NewDustPerfect(barrelPosition, ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(3, 3), 50, Color.SkyBlue, Main.rand.NextFloat(1f, 1.3f));
                Dust.NewDustPerfect(barrelPosition, ModContent.DustType<GlowDust>(), Main.rand.NextVector2Circular(5, 5), 50, Color.Cyan, Main.rand.NextFloat(1f, 1.3f));
            }

            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Projectile.velocity * 15), ModContent.ProjectileType<FrostBlasterStrongProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            return;
        }

        public void UpdateHeldProjectile(bool updateTimeLeft = true,bool updateVelocity = true)
        {
            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;

            if (updateTimeLeft)
                Projectile.timeLeft = 2;

            Projectile.rotation = Projectile.velocity.ToRotation();
            Owner.itemRotation = Utils.ToRotation(Projectile.velocity * Projectile.direction);

            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(90));
            Projectile.position = ArmPostion - Projectile.Size * 0.5f;

            if (Main.myPlayer == Projectile.owner && updateVelocity)
            {
                Vector2 oldVelocity = Projectile.velocity;

                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Owner.DirectionTo(Main.MouseWorld),1f);

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
            var tex = ModContent.Request<Texture2D>("ForgottenFacets/Assets/Items/Weapons/Ranged/FrostBlaster").Value;
            var outline = ModContent.Request<Texture2D>("ForgottenFacets/Assets/Items/Weapons/Ranged/FrostBlaster_Outline").Value;

            SpriteEffects se = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Vector2 offset;

            Vector2 origin;

            if (Projectile.spriteDirection == -1)
            {
                offset = new Vector2(25f, 4f);
                offset = offset.RotatedBy(Projectile.rotation);
                origin = new Vector2(40, 10);
            }
            else
            {
                offset = new Vector2(0f, -8f);
                offset = offset.RotatedBy(Projectile.rotation);
                origin = new Vector2(13, 12);
            }

            Vector2 position = ArmPostion - Main.screenPosition;

            float rotation = Projectile.rotation + (se == SpriteEffects.FlipHorizontally ? MathHelper.Pi : 0f);

            int frameCount = 4;

            int frameHeight = tex.Height / frameCount;
            int frame = (int)(ChargeTimer / 6f) % frameCount;

            Rectangle sourceRectangle = new Rectangle(0, frame * frameHeight, tex.Width, frameHeight);

            float fadeIn = 1f;

            if(Dying)
                fadeIn = Projectile.timeLeft / 10f;


            float normalizedValue = (ChargeTimer - 1f) / (maxCharge - 1f);
            float chargeProgress = MathHelper.Clamp(normalizedValue, 0f, 1f);


            if (Dying && Projectile.timeLeft < 20)
            {
                float recoilAngle = MathHelper.ToRadians(25) * chargeProgress;

                float recoilRotation = Projectile.rotation;

                if (Projectile.spriteDirection == -1)
                    recoilRotation += MathHelper.Pi;

                // Rotate upward relative to the weapon's facing direction
                recoilRotation -= recoilAngle * Projectile.spriteDirection;

                Main.spriteBatch.Draw(tex, position, sourceRectangle, lightColor * fadeIn, recoilRotation, origin, Projectile.scale, se, 0f);

                //Main.spriteBatch.Draw(outline, position + new Vector2(-3, -2), null, lightColor * chargeProgress * 0.5f, recoilRotation, origin, Projectile.scale, se, 0f);

            }
            else
            {
                Main.spriteBatch.Draw(tex, position, sourceRectangle, lightColor * fadeIn, rotation, origin, Projectile.scale, se, 0f);

                Main.spriteBatch.Draw(outline, position + new Vector2(-3, -2), null, lightColor * chargeProgress * 0.5f, rotation, origin, Projectile.scale, se, 0f);
            }

            

            return false;
        }

    }

}
