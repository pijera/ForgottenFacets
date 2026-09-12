using ForgottenFacets.Content.Materials.Essences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ForgottenFacets.Core
{
    internal class VanillaEditedReciepes : GlobalItem
    {
        public override void AddRecipes()
        {
            //Amethyst
            Recipe amethystRecipe = Recipe.Create(ItemID.Amethyst);
            amethystRecipe.AddIngredient(ModContent.ItemType<ArcaneEssence>());
            amethystRecipe.AddIngredient(ItemID.FallenStar,5);
            amethystRecipe.AddTile(TileID.Anvils);
            amethystRecipe.Register();

            //Topaz
            Recipe topazRecipeGold = Recipe.Create(ItemID.Topaz);
            topazRecipeGold.AddIngredient(ModContent.ItemType<EarthEssence>());
            topazRecipeGold.AddIngredient(ItemID.GoldOre, 15);
            topazRecipeGold.AddTile(TileID.Anvils);
            topazRecipeGold.Register();


            Recipe topazRecipePlat = Recipe.Create(ItemID.Topaz);
            topazRecipePlat.AddIngredient(ModContent.ItemType<EarthEssence>());
            topazRecipePlat.AddIngredient(ItemID.PlatinumOre, 15);
            topazRecipePlat.AddTile(TileID.Anvils);
            topazRecipePlat.Register();

            //Sapphire
            Recipe sapphireRecipeCoral = Recipe.Create(ItemID.Sapphire);
            sapphireRecipeCoral.AddIngredient(ModContent.ItemType<WaterEssence>());
            sapphireRecipeCoral.AddIngredient(ItemID.Coral, 5);
            sapphireRecipeCoral.AddTile(TileID.Anvils);
            sapphireRecipeCoral.Register();

            Recipe sapphireRecipeSeashell = Recipe.Create(ItemID.Sapphire);
            sapphireRecipeSeashell.AddIngredient(ModContent.ItemType<WaterEssence>());
            sapphireRecipeSeashell.AddIngredient(ItemID.Seashell, 5);
            sapphireRecipeSeashell.AddTile(TileID.Anvils);
            sapphireRecipeSeashell.Register();

            //Emerald
            Recipe emeraldRecipe = Recipe.Create(ItemID.Emerald);
            emeraldRecipe.AddIngredient(ModContent.ItemType<LifeEssence>());
            emeraldRecipe.AddIngredient(ItemID.JungleSpores, 5);
            emeraldRecipe.AddTile(TileID.Anvils);
            emeraldRecipe.Register();

            //Ruby
            Recipe rubyRecipe = Recipe.Create(ItemID.Ruby);
            rubyRecipe.AddIngredient(ModContent.ItemType<FireEssence>());
            rubyRecipe.AddIngredient(ItemID.Hellstone, 10);
            rubyRecipe.AddTile(TileID.Anvils);
            rubyRecipe.Register();

            //Diamond
            Recipe diamondRecipe = Recipe.Create(ItemID.Diamond);
            diamondRecipe.AddIngredient(ModContent.ItemType<StormEssence>());
            diamondRecipe.AddIngredient(ItemID.Feather, 5);
            diamondRecipe.AddTile(TileID.Anvils);
            diamondRecipe.Register();

            //Amber
            Recipe amberRecipe = Recipe.Create(ItemID.Amber);
            amberRecipe.AddIngredient(ModContent.ItemType<AncientEssence>());
            amberRecipe.AddIngredient(ItemID.FossilOre, 3);
            amberRecipe.AddTile(TileID.Anvils);
            amberRecipe.Register();
        }
    }
}
