//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.Azure.Cosmos
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a contract for generating float32 vector embeddings from input text strings.
    /// The SDK invokes this when a query plan includes an embedding parameter (e.g. hybrid or
    /// vector-search queries with literal text such as
    /// <c>ORDER BY RANK VectorDistance(c.text, 'big brown cat')</c>).
    /// Set an instance on <see cref="CosmosClientOptions.EmbeddingGenerator"/> for a client-wide
    /// default. Implementations MUST be thread-safe and are responsible for any caching, retries,
    /// and authentication required to call the underlying embedding service.
    /// </summary>
#if PREVIEW
    public
#else
    internal
#endif
    interface ICosmosEmbeddingGenerator
    {
        /// <summary>
        /// Generates an embedding vector for each of the supplied input strings.
        /// </summary>
        /// <param name="text">
        /// The collection of input strings to embed. The implementation MUST
        /// return one vector per input, in the same order.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> propagated from the originating
        /// SDK call (for example <c>FeedIterator.ReadNextAsync</c>).
        /// Implementations should honor cancellation.
        /// </param>
        /// <returns>
        /// A task that resolves to a sequence of float32 embedding vectors with the same
        /// cardinality and ordering as <paramref name="text"/>. Embedding models always produce
        /// float32 vectors. The <c>datatype</c> in a container's <see cref="VectorEmbeddingPolicy"/>
        /// (e.g. <c>int8</c>, <c>uint8</c>, <c>float16</c>) describes how vectors are stored and
        /// indexed — the service applies any quantization at write time. Query vectors are always
        /// sent as float32 regardless of the container's stored datatype.
        /// </returns>
        Task<IEnumerable<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(
            IEnumerable<string> text,
            CancellationToken cancellationToken = default);
    }
}
