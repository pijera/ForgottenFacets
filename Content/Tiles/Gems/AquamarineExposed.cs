using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Content.Tiles.Gems
{
    internal class AquamarineExposed : ModTile
    {
        const int TileHeight = 18;
        const int RandomStyleCount = 3;
        const int StyleHeight = TileHeight * RandomStyleCount;

        Color gemColor = new Color(78, 213, 226);

        public override string Texture => "ForgottenFacets/Assets/Tiles/Gems/AquamarineExposed";
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;

            Main.tileShine2[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileSpelunker[Type] = true;

            DustType = DustID.GemDiamond;
            AddMapEntry(gemColor, CreateMapEntryName());
        }

        public override bool CanPlace(int i, int j)
        {
            if (WorldGen.SolidTile(i - 1, j, noDoors: true) || WorldGen.SolidTile(i + 1, j, noDoors: true) || WorldGen.SolidTile(i, j - 1) || WorldGen.SolidTile(i, j + 1))
            {
                return true;
            }

            return false;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Tile tile = Main.tile[i, j];

            bool left = WorldGen.SolidTile(i - 1, j, noDoors: true);
            bool right = WorldGen.SolidTile(i + 1, j, noDoors: true);
            bool up = WorldGen.SolidTile(i, j - 1);
            bool down = WorldGen.SolidTile(i, j + 1);

            int frameDirection;

            if (left)
                frameDirection = 2;
            else if (right)
                frameDirection = 3;
            else if (up)
                frameDirection = 1;
            else if (down)
                frameDirection = 0;
            else
            {
                WorldGen.KillTile(i, j);
                return false;
            }

            short randomStyleOffset = (short)(WorldGen.genRand.Next(RandomStyleCount) * TileHeight);

            int frameStart = StyleHeight * frameDirection;

            if (tile.TileFrameY < frameStart ||
                tile.TileFrameY >= frameStart + StyleHeight)
            {
                tile.TileFrameY = (short)(frameStart + randomStyleOffset);
            }

            return false;
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            if (Main.tile[i, j].TileFrameY < StyleHeight)
            {
                Main.tile[i, j].TileFrameY = (short)(WorldGen.genRand.Next(RandomStyleCount) * TileHeight);
            }
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            if (tileFrameY < StyleHeight)
            {
                offsetY = 2;
            }
        }

        public override void EmitParticles(int i, int j, Tile tile, short tileFrameX, short tileFrameY, Color tileLight, bool visible)
        {
            if (!visible)
            {
                return;
            }

            if (tileLight.R <= 20 && tileLight.B <= 20 && tileLight.G <= 20)
            {
                return;
            }

            int lightValue = tileLight.R;
            if (tileLight.G > lightValue)
            {
                lightValue = tileLight.G;
            }

            if (tileLight.B > lightValue)
            {
                lightValue = tileLight.B;
            }

            lightValue /= 30;

            const int ParticleRate = 500;
            if (Main.rand.Next(ParticleRate) >= lightValue)
            {
                return;
            }

            Color dustColor = gemColor;
            int dust = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.TintableDustLighted, 0f, 0f, 254, dustColor, 0.5f);
            Main.dust[dust].velocity *= 0f;
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            int dust = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, type, 0f, 0f, 75, gemColor, 0.75f);
            Main.dust[dust].noLight = true;

            return false;

        }
    }
}
