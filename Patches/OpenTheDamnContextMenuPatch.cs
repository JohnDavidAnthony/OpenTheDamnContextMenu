using EFT.InventoryLogic;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OpenTheDamnContextMenu
{
    public class OpenTheDamnContextMenuPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ItemUiContext), nameof(ItemUiContext.ShowContextMenu));
        }

        // Use reflection to get the private fields
        private static readonly FieldInfo itemInfoInteractionsField = typeof(ItemUiContext).GetField("itemInfoInteractionsAbstractClass", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo dictionaryField = typeof(ItemUiContext).GetField("dictionary_0", BindingFlags.NonPublic | BindingFlags.Instance);

        [PatchPrefix]
        static bool Prefix(ItemUiContext __instance, ItemContextAbstractClass itemContext, Vector2 position)
        {
            // Get the private dictionary field
            var dictionary = (Dictionary<EItemInfoButton, string>)dictionaryField.GetValue(__instance);

            // Get item interactions
            var itemInfoInteractions = __instance.GetItemContextInteractions(itemContext, null);

            // Set itemInfoInteractions field
            if (itemInfoInteractionsField != null)
            {
                itemInfoInteractionsField.SetValue(__instance, itemInfoInteractions);
            }

            // Check and update the AddOffer interaction
            if (itemInfoInteractions.AllInteractions.Contains(EItemInfoButton.AddOffer))
            {
                var ragFair = __instance.Session?.RagFair;
                if (ragFair != null && ragFair.Available)
                {
                    int myOffersCount = ragFair.MyOffersCount;
                    int maxOffersCount = ragFair.MaxOffersCount;
                    string text = string.Format("AddOfferButton{0}/{1}".Localized(null), myOffersCount, maxOffersCount);
                    dictionary[EItemInfoButton.AddOffer] = text;
                }
            }

            // Show the context menu
            __instance.ContextMenu.Show<EItemInfoButton>(position, itemInfoInteractions, dictionary, itemContext.Item);

            // Skip the original method
            return false;
        }
    }
}
