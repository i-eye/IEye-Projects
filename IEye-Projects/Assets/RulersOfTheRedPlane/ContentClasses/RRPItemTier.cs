using MSU;
using R2API.ScriptableObjects;
using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RoR2.ContentManagement;
using IEye.RRP;

namespace IEye.RRP
{
    /// <summary>
    /// <inheritdoc cref="IItemTierContentPiece"/>
    /// </summary>
    public abstract class RRPItemTier : IItemTierContentPiece, IContentPackModifier
    {
        public ItemTierAssetCollection AssetCollection { get; private set; }
        public NullableRef<SerializableColorCatalogEntry> colorIndex { get; protected set; }
        public NullableRef<SerializableColorCatalogEntry> darkColorIndex { get; protected set; }
        public GameObject pickupDisplayVFX { get; protected set; }
        public List<ItemIndex> itemsWithThisTier { get; set; } = new List<ItemIndex>();
        public List<PickupIndex> availableTierDropList { get; set; } = new List<PickupIndex>();
        ItemTierDef IContentPiece<ItemTierDef>.asset => ItemTierDef;
        public ItemTierDef ItemTierDef { get; protected set; }

        public abstract RRPAssetRequest<ItemTierAssetCollection> AssetRequest { get; }
        public abstract void Initialize();
        public abstract bool IsAvailable(ContentPack contentPack);

        public virtual IEnumerator LoadContentAsync()
        {
            RRPAssetRequest<ItemTierAssetCollection> request = AssetRequest;

            request.StartLoad();
            while (!request.isComplete)
                yield return null;

            AssetCollection = request.asset;
            ItemTierDef = AssetCollection.itemTierDef;

            if (AssetCollection.colorIndex)
                colorIndex = AssetCollection.colorIndex;
            if (AssetCollection.darkColorIndex)
                darkColorIndex = AssetCollection.darkColorIndex;

            pickupDisplayVFX = AssetCollection.pickupDisplayVFX;
        }

        public void ModifyContentPack(ContentPack contentPack)
        {
            contentPack.AddContentFromAssetCollection(AssetCollection);
        }
    }
}