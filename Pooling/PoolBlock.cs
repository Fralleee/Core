using UnityEngine;

namespace Fralle.Core.Pooling
{
  [System.Serializable]
  public class PoolBlock
  {
    [Tooltip("Initial number of object in the pool.")]
    public int Size;
    [Tooltip("Behavior when an object is requested and the pool is empty.\n\n" +
      "Grow - Will add a new object to the pool, \nlimited by Max Size.\n\n" +
      "Fail - No object will be spawned.\n\n" +
      "Reuse Oldest - Will reuse the oldest active object.")] // becomes slower than grow for large pools, but faster with small pools
    public EmptyBehavior EmptyBehavior;
    [Tooltip("When using Grow behaviour, this is the absolute max size the pool can grow to.")]
    public int MaxSize; // absolut limit on pool size, used with EmptyBehavior Grow mode
    [Tooltip("Behavior when an object is requested and the pool is empty, and the max size of the pool has been reached.\n\n" +
      "Fail - No object will be spawned.\n\n" +
      "Reuse Oldest - Will reuse the oldest active object.")]
    public MaxEmptyBehavior MaxEmptyBehavior; // mode when pool is at the max size
    [Tooltip("Object that this pool contains.")]
    public GameObject Prefab;
    [Tooltip("When the scene is stopped, creates a report showing pool usage:\n\n" +
      "Start Size - Size of the pool when the scene started.\n\n" +
      "End Size - Size of the pool when the scene ended.\n\n" +
      "Added Objects - Number of objects added to the pool beyond the Start Size.\n\n" +
      "Failed Spawns - Number of spawns failed due to no objects available in the pool.\n\n" +
      "Reused Objects - Number of objects reused before they were added back to the pool.\n\n" +
      "Most Objects Active - The most pool objects ever active at the same time.")]
    public bool PrintLogOnQuit;

    public PoolBlock(int size, EmptyBehavior emptyBehavior, int maxSize, MaxEmptyBehavior maxEmptyBehavior, GameObject prefab, bool printLogOnQuit)
    {
      this.Size = size;
      this.EmptyBehavior = emptyBehavior;
      this.MaxSize = maxSize;
      this.MaxEmptyBehavior = maxEmptyBehavior;
      this.Prefab = prefab;
      this.PrintLogOnQuit = printLogOnQuit;
    }
  }
}
