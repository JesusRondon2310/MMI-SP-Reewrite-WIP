using static MMI_SP.Agency.ItemsManager;

namespace MMI_SP.Agency.Office.Ambient
{
    internal class Handler
    {
        // ==========================================
        // BLOQUE 1: Variables de la clase y creación del objeto
        // ==========================================
        internal OfficeItemsCollection ItemsCollection { get; private set; }

        public Handler()
        {
            ItemsCollection = new OfficeItemsCollection(CollectionPicker.Pick());
            Builder.Build(ItemsCollection);
        }

        public Handler(OfficeItemsCollection collection)
        {
            ItemsCollection = new OfficeItemsCollection(collection);
            Builder.Build(ItemsCollection);
        }

        // ==========================================
        // BLOQUE 2: Funciones
        // ==========================================
        internal void CleanOffice()
        {
            CleanUp.Execute();
        }
    }
}