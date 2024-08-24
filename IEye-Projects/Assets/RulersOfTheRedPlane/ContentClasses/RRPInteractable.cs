using IEye.RRP;
using MSU;
using RoR2;
using UnityEngine;
﻿using MSU;
using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RoR2.ContentManagement;

namespace IEye.RRP
{
    /// <summary>
    /// <inheritdoc cref="IInteractableContentPiece"/>
    /// </summary>
    public abstract class RRPInteractable : IInteractableContentPiece, IContentPackModifier
    {
        public InteractableAssetCollection AssetCollection { get; private set; }
        public InteractableCardProvider CardProvider { get; protected set; }
        IInteractable IGameObjectContentPiece<IInteractable>.Component => InteractablePrefab.GetComponent<IInteractable>();
        GameObject IContentPiece<GameObject>.Asset => InteractablePrefab;
        public GameObject InteractablePrefab { get; protected set; }

        public abstract RRPAssetRequest<InteractableAssetCollection> AssetRequest { get; }

        NullableRef<InteractableCardProvider> IInteractableContentPiece.CardProvider => CardProvider;

        public abstract void Initialize();
        public abstract bool IsAvailable(ContentPack contentPack);
        public virtual IEnumerator LoadContentAsync()
        {
            RRPAssetRequest<InteractableAssetCollection> request = AssetRequest;

            request.StartLoad();
            while (!request.isComplete)
                yield return null;

            AssetCollection = request.asset;

            CardProvider = AssetCollection.interactableCardProvider;
            InteractablePrefab = AssetCollection.interactablePrefab;

        }

        public void ModifyContentPack(ContentPack contentPack)
        {
            contentPack.AddContentFromAssetCollection(AssetCollection);
        }
    }
}