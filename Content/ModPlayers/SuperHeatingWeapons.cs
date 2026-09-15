using ForgottenFacets.Content.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.ModPlayers
{
    internal class SuperHeatingWeapons : ModPlayer
    {
        public int Heat { get; private set; }

        public bool IsSuperHeated => Heat >= 100;

        private int heatDecayTimer;


        private static readonly HashSet<int> heatWeapons = new()
        {
            ModContent.ItemType<Cinderlance>()
        };


        public void AddHeat(int amount)
        {
            int oldHeat = Heat;

            Heat += amount;

            if (Heat > 100)
                Heat = 100;

            heatDecayTimer = 0;

            if (oldHeat < 100 && Heat == 100)
            {
                SoundEngine.PlaySound(SoundID.DD2_BetsyFlameBreath with { Volume = 2f, PitchRange = (-0.8f, 0.8f) });
                SoundEngine.PlaySound(SoundID.DD2_BetsyWindAttack with { Volume = 2f, PitchRange = (-0.8f, 0.8f) });
            }
                

        }

        public bool ConsumeHeat()
        {
            if (!IsSuperHeated)
                return false;

            Heat = 0;
            heatDecayTimer = 0;

            return true;
        }

        public bool isHoldingHeatWeapon() => heatWeapons.Contains(Player.HeldItem.type);


        public override void PostUpdate()
        {
            if (Heat <= 0)
            {
                Heat = 0;
                return;
            }

            heatDecayTimer++;

            if (heatDecayTimer >= 120 && Heat != 100)
            {
                heatDecayTimer = 0;
                Heat-=5;

            }
            Vector2 position = Player.Center - new Vector2(Player.direction * 10f, 0f);
            if (Heat >= 25)
            {
                if (Main.rand.NextBool(5))
                {
                    Dust.NewDustPerfect(position, DustID.Smoke, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-4f, -1f)), 200, default, 1f);
                }
            }
            if (Heat >= 50)
            {

                if (Main.rand.NextBool(4))
                {
                    Dust.NewDustPerfect(position, DustID.Smoke, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-4f, -1f)), 150, Color.Gray, Main.rand.NextFloat(1f,1.5f));
                }
            }

            if (Heat >= 75)
            {

                if (Main.rand.NextBool(3))
                {
                    Dust.NewDustPerfect(position, DustID.Smoke, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-4f, -1f)), 150, Color.DarkGray, Main.rand.NextFloat(1f, 1.5f));
                    Dust.NewDustPerfect(position, DustID.Torch, Main.rand.NextVector2Circular(3, 3), 150, default, Main.rand.NextFloat(1f, 1.5f));
                }
            }
            if (Heat == 100)
            {
                if (Main.rand.NextBool(2))
                {
                    Dust.NewDustPerfect(position, DustID.Smoke, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-4f, -1f)), 150, Color.DarkGray, Main.rand.NextFloat(1f, 1.5f));
                    Dust.NewDustPerfect(position, DustID.Torch, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-4f, -1f)), 150, default, Main.rand.NextFloat(1f, 1.5f));
                }
                Player.AddBuff(BuffID.OnFire, 1);
            }

        }

        public override void UpdateDead()
        {
            Heat = 0;
            heatDecayTimer = 0;
        }



    }
}
