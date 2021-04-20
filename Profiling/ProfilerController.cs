using System.Collections.Generic;
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

		public float FrameTime;
		public float Fps;
		public float GcMemory;
		public float SystemMemory;
		public float DrawCalls;

		static double GetRecorderFrameAverage(ProfilerRecorder recorder)
		{
			var samplesCount = recorder.Capacity;
			if (samplesCount == 0)
				return 0;

			double r = 0;
			var samples = new List<ProfilerRecorderSample>(samplesCount);
			recorder.CopyTo(samples);
			for (var i = 0; i < samples.Count; ++i)
				r += samples[i].Value;
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
			FrameTime = (float)GetRecorderFrameAverage(mainThreadTimeRecorder) * (1e-6f);
			Fps = 1000 / FrameTime;
			GcMemory = gcMemoryRecorder.LastValue / (1024f * 1024f);
			SystemMemory = systemMemoryRecorder.LastValue / (1024f * 1024f);
			DrawCalls = drawCallsCountRecorder.LastValue;
		}
	}
}
