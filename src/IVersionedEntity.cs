// --------------------------------------------------------------------------
//  <copyright file="IVersionedEntity.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    public interface IVersionedEntity
    {
        string EntityVersion { get; }
    }
}