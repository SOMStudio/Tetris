using UnityEngine;

namespace SOMStudio.Tetris.Scripts.Base.Optimization
{
	[AddComponentMenu("SOMStudio/Tetris/Utility/FPS counter")]
	public class FpsCounter : MonoBehaviour
	{
		[Header("Settings")]
		[SerializeField] private UnityEngine.UI.Text fpsText;
		[SerializeField] private UnityEngine.UI.Text minFpsText;
		[SerializeField] private UnityEngine.UI.Text maxFpsText;

		private const float FPSMeasurePeriod = 0.5f;
		private int fpsAccumulator;
		private float fpsNextPeriod;
		private int currentFps;
		private int minFPS = -1;
		private int maxFPS = -1;

		private void Start()
		{
			fpsNextPeriod = Time.realtimeSinceStartup + FPSMeasurePeriod;
		}

		private void Update()
		{
			fpsAccumulator++;
			if (Time.realtimeSinceStartup > fpsNextPeriod)
			{
				currentFps = (int) (fpsAccumulator/FPSMeasurePeriod);

				if (Time.realtimeSinceStartup > 20) {
					if (minFPS == -1) {
						minFPS = currentFps;
						maxFPS = currentFps;
					} else {
						if (minFPS > currentFps)
							minFPS = currentFps;
						if (maxFPS < currentFps)
							maxFPS = currentFps;
					}
				}

				fpsAccumulator = 0;
				fpsNextPeriod += FPSMeasurePeriod;

				
				fpsText.text = $"FPS:{currentFps}";
				minFpsText.text = $"minFPS:{minFPS}";
				maxFpsText.text = $"maxFPS:{maxFPS}";
			}
		}
	}
}
