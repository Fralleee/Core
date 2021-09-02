using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEngine;

namespace Fralle.Core.Profiling
{
  public class ProfilerController : MonoBehaviour
  {
    ProfilerRecorder systemMemoryRecorder;
    ProfilerRecorder gcMemoryRecorder;
    ProfilerRecorder mainThreadTimeRecorder;
    ProfilerRecorder drawCallsCountRecorder;

    public float frameTime;
    public float fps;
    public float gcMemory;
    public float systemMemory;
    public float drawCalls;

    static double GetRecorderFrameAverage(ProfilerRecorder recorder)
    {
      int samplesCount = recorder.Capacity;
      if (samplesCount == 0)
        return 0;

      List<ProfilerRecorderSample> samples = new List<ProfilerRecorderSample>(samplesCount);
      recorder.CopyTo(samples);
      double r = samples.Aggregate<ProfilerRecorderSample, double>(0, (current, t) => current + t.Value);
      r /= samplesCount;

      return r;
    }

    void OnEnable()
    {
      systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
      gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
      mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15);
      drawCallsCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
    }

    void OnDisable()
    {
      systemMemoryRecorder.Dispose();
      gcMemoryRecorder.Dispose();
      mainThreadTimeRecorder.Dispose();
      drawCallsCountRecorder.Dispose();
    }

    void Update()
    {
      frameTime = (float)GetRecorderFrameAverage(mainThreadTimeRecorder) * (1e-6f);
      fps = 1000 / frameTime;
      gcMemory = gcMemoryRecorder.LastValue / (1024f * 1024f);
      systemMemory = systemMemoryRecorder.LastValue / (1024f * 1024f);
      drawCalls = drawCallsCountRecorder.LastValue;
    }
  }
}
