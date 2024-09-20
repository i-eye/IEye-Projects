using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoR2.ContentManagement;
using MSU;
using RoR2;

namespace IEye.RRP
{
    /// <summary>
    /// <inheritdoc cref="IItemContentPiece"/>
    /// </summary>
    public abstract class RRPItem : IItemContentPiece, IContentPackModifier
    {
        public ItemAssetCollection AssetCollection { get; private set; }
        public NullableRef<List<GameObject>> ItemDisplayPrefabs { get; protected set; } = new List<GameObject>();
        public ItemDef ItemDef { get; protected set; }

        ItemDef IContentPiece<ItemDef>.asset => ItemDef;
        NullableRef<List<GameObject>> IItemContentPiece.itemDisplayPrefabs => ItemDisplayPrefabs;

        public abstract RRPAssetRequest AssetRequest { get; }

        public abstract void Initialize();
        public abstract bool IsAvailable(ContentPack contentPack);
        public virtual IEnumerator LoadContentAsync()
        {
            RRPAssetRequest request = AssetRequest;

            request.StartLoad();
            while (!request.isComplete)
                yield return null;

            if (request.boxedAsset is ItemAssetCollection collection)
            {
                AssetCollection = collection;

                ItemDef = AssetCollection.itemDef;
                ItemDisplayPrefabs = AssetCollection.itemDisplayPrefabs;
            }
            else if (request.boxedAsset is ItemDef def)
            {
                ItemDef = def;
            }
            else
            {
                RRPLog.Error("Invalid AssetRequest " + request.assetName + " of type " + request.boxedAsset.GetType());
            }
        }

        public virtual void ModifyContentPack(ContentPack contentPack)
        {
            if (AssetCollection)
            {
                contentPack.AddContentFromAssetCollection(AssetCollection);
            }
        }
    }
}
