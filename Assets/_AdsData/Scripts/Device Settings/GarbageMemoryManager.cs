using System;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Scripting;

public class GarbageMemoryManager : MonoBehaviour
{
    // Perform an incremental collection every time we allocate more than 8 MB
    const long kCollectAfterAllocating = 8 * 1024 * 1024;

    // Perform an instant, full GC if we have more than 128 MB of managed heap.
    const long kHighWater = 128 * 1024 * 1024;

    long lastFrameMemory = 0;
    long nextCollectAt = 0;

    void Start()
    {
        // Set GC to manual collections only.
        GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
    }

    void Update()
    {
        long mem = Profiler.GetMonoUsedSizeLong();
        if (mem < lastFrameMemory)
        {
            // GC happened.
            nextCollectAt = mem + kCollectAfterAllocating;
        }
        if (mem > kHighWater)
        {
            // Trigger immediate GC
            System.GC.Collect(0);
        }
        else if (mem >= nextCollectAt)
        {
            // Trigger incremental GC
            UnityEngine.Scripting.GarbageCollector.CollectIncremental((GarbageCollector.incrementalTimeSliceNanoseconds / 1000 / 1000));
            lastFrameMemory = mem + kCollectAfterAllocating;
        }
        lastFrameMemory = mem;
    }

}
