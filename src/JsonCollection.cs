// --------------------------------------------------------------------------
//  <copyright file="JsonCollection.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    using System.Collections;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    [JsonObject]
    internal class JsonCollection<T> : ICollection<T>, IReadOnlyCollection<T>
    {
        /// <summary>
        /// Contains a collection of items included in this response.
        /// </summary>
        [JsonProperty("value")]
        public ICollection<T> items;

        [JsonIgnore]
        public int Count
        {
            get { return this.items.Count; }
        }

        [JsonIgnore]
        public bool IsReadOnly
        {
            get { return this.items.IsReadOnly; }
        }

        /// <summary>
        /// The relative url to the remaining items in the collection.
        /// </summary>
        [JsonProperty("nextLink")]
        public string NextLink { get; set; }

        /// <summary>
        /// The total number of elements in the collection.
        /// </summary>
        [JsonProperty("count")]
        public long TotalCount { get; set; }

        public void Add(T item)
        {
            this.items.Add(item);
        }

        public void Clear()
        {
            this.items.Clear();
        }

        public bool Contains(T item)
        {
            return this.items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        public bool Remove(T item)
        {
            return this.items.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.items.GetEnumerator();
        }
    }
}