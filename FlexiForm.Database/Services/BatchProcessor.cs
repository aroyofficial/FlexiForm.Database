namespace FlexiForm.Database.Services
{
    /// <summary>
    /// Provides functionality to split a collection of items into batches represented as queues.
    /// Supports optional sorting by filename before batching.
    /// </summary>
    public static class BatchProcessor<T>
    {
        /// <summary>
        /// Batches a collection of items into a queue of queues.
        /// Optionally sorts items by filename before batching if a selector is provided.
        /// </summary>
        /// <param name="items">The collection of items to batch.</param>
        /// <param name="batchSize">Number of items per batch (must be greater than zero).</param>
        /// <param name="fileNameSelector">
        /// A function that extracts the filename from an item.
        /// If provided, enables sorting by filename; if null, original order is preserved.
        /// </param>
        /// <param name="sortAscending">
        /// If <c>true</c>, file names are sorted ascending.
        /// If <c>false</c>, file names are sorted descending.
        /// Ignored if <paramref name="fileNameSelector"/> is <c>null</c>.
        /// </param>
        /// <returns>A queue of batches, where each batch is a queue of items.</returns>
        public static Queue<Queue<T>> Batchify(
            IEnumerable<T> items,
            int batchSize,
            Func<T, string>? fileNameSelector = null,
            bool sortAscending = true)
        {
            if (batchSize <= 0)
                throw new ArgumentException("Batch size must be greater than zero.", nameof(batchSize));

            var ordered = fileNameSelector != null
                ? sortAscending
                    ? items.OrderBy(fileNameSelector)
                    : items.OrderByDescending(fileNameSelector)
                : items;

            var list = ordered.ToList();
            var result = new Queue<Queue<T>>();

            for (int i = 0; i < list.Count; i += batchSize)
            {
                result.Enqueue(new Queue<T>(list.Skip(i).Take(batchSize)));
            }

            return result;
        }
    }
}
