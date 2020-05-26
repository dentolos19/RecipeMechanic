using System.IO;

namespace SmRecipeModifier.Core
{

    public static class Constants
    {

        public const string CraftbotJsonPath = @"\Survival\CraftingRecipes\craftbot.json";
        public const string DispenserJsonPath = @"\Survival\CraftingRecipes\craftbot.json";
        public const string WorkbenchJsonPath = @"\Survival\CraftingRecipes\craftbot.json";

        public static string Match(string path, string constant)
        {
            return Path.Combine(path, constant);
        }

    }

}