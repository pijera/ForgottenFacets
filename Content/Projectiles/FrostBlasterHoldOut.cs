using ForgottenFacets.Content.Items.Weapons.Ranged;
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

namespace ForgottenFacets.Content.Projectiles
{
    internal class FrostBlasterHoldOut : ModProjectile
    {
        private int maxCharge = 120;
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
            if (!CanHold && !Dying)//call when player lifts their finger of left click
            {
                Dying = true;
                Projectile.timeLeft = 20;

                SpawnProjectiles(ChargeTimer);
            }

            if (recoilTimer > 0)
            {
                int offset = (int)MathHelper.Min(60, recoilTimer);

                armOffset = new Vector2(-15 * (offset / 60f), 0).RotatedBy(Projectile.rotation);
                recoilTimer--;
            }else
                armOffset = Vector2.Zero;

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

                UpdateHeldProjectile();
            }
            else
                UpdateHeldProjectile(false,false);

            if (ChargeTimer == maxCharge + 38)
            {
                SpawnProjectiles(ChargeTimer);
                ChargeTimer = 0;
            }

            Main.NewText(ChargeTimer);
        }

        public void SpawnProjectiles(float chargeAmount)
        {
            if (chargeAmount < maxCharge)//PUN KURAC EFFEKTOVA FIJUFIJUFIJUUUUUUUUUU
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Projectile.velocity * 10), ProjectileID.WaterBolt, Projectile.damage, Projectile.knockBack, Projectile.owner);
                return;
            }
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Projectile.velocity * 15), ProjectileID.MolotovCocktail, Projectile.damage, Projectile.knockBack, Projectile.owner);
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
            // var outline;

            SpriteEffects se = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Vector2 offset;

            if (Projectile.spriteDirection == -1)
            {
                offset = new Vector2(25f, 4f);
                offset = offset.RotatedBy(Projectile.rotation);
            }
            else
            {
                offset = new Vector2(0f, -8f);
                offset = offset.RotatedBy(Projectile.rotation);
            }

            Vector2 position = ArmPostion + offset - Main.screenPosition;

            float rotation = Projectile.rotation + (se == SpriteEffects.FlipHorizontally ? MathHelper.Pi : 0f);

            int frameCount = 4;

            int frameHeight = tex.Height / frameCount;
            int frame = (int)(ChargeTimer / 6f) % frameCount;

            Rectangle sourceRectangle = new Rectangle(0, frame * frameHeight, tex.Width, frameHeight);

            float fadeIn = 1f;

            if(Dying)
                fadeIn = Projectile.timeLeft / 10f;

            Vector2 origin = new Vector2(tex.Width / 4f,frameHeight / 4f);


            Main.spriteBatch.Draw(tex, position, sourceRectangle, lightColor * fadeIn, rotation, origin, Projectile.scale, se, 0f);

            if (recoilTimer > 0)
            {
                //outline ovde ide
            }
            return false;
        }

    }

}
