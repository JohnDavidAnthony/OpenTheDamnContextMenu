using EFT.InventoryLogic;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;


namespace OpenTheDamnContextMenu
{
    public class OpenTheContext : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ItemUiContext), nameof(ItemUiContext.ShowContextMenu));
        }

        // Use reflection to get the private fields
        private static readonly FieldInfo inventoryControllerField = typeof(ItemUiContext).GetField("inventoryControllerClass", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo itemInfoInteractionsField = typeof(ItemUiContext).GetField("itemInfoInteractionsAbstractClass", BindingFlags.NonPublic | BindingFlags.Instance);

        [PatchPrefix]
        static bool Prefix(ItemUiContext __instance, ItemContextAbstractClass itemContext, Vector2 position)
        {

            var inventoryController = (InventoryControllerClass)inventoryControllerField.GetValue(__instance);
            var itemInfoInteractions = __instance.GetItemContextInteractions(itemContext, null);

            if (itemInfoInteractionsField != null)
            {
                itemInfoInteractionsField.SetValue(__instance, itemInfoInteractions);
            }

            __instance.ContextMenu.Show<EItemInfoButton>(position, itemInfoInteractions, null, itemContext.Item);

            return false; // Skip the original method


        }

    }

}
