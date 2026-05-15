using System;
using System.Collections.Generic;
using GTA;
using GTA.Chrono;
using static MMI_SP.Agency.ItemsManager;

namespace MMI_SP.Agency.Office.Ambient
{
    internal static class CollectionPicker
    {
        // ==========================================
        // BLOQUE 1: Funciones
        // ==========================================
        public static OfficeItemsCollection Pick()
        {
            Random rnd = new Random(Game.GameTime);
            List<OfficeItemsCollection> pool = new List<OfficeItemsCollection>();

            int hour = GameClock.Hour;

            if (hour >= 2 && hour < 12)
            {
                pool.AddRange(ItemsManager.GetItemsCollection(CollectionType.Normal));
            }
            else if (hour >= 12 && hour < 14)
            {
                pool.AddRange(ItemsManager.GetItemsCollection(CollectionType.Midday));
                pool.AddRange(ItemsManager.GetItemsCollection(CollectionType.Normal));
            }
            else if (hour >= 14)
            {
                pool.AddRange(ItemsManager.GetItemsCollection(CollectionType.Normal));
            }
            else
            {
                pool.AddRange(ItemsManager.GetItemsCollection(CollectionType.Normal));
                pool.AddRange(ItemsManager.GetItemsCollection(CollectionType.Night));
            }

            return pool[rnd.Next(0, pool.Count)];
        }
    }
}