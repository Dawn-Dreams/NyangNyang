#if UNITY_EDITOR

using System.IO;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;

namespace UnityEngine.Recorder.Examples
{
    public class CaptureScreenShotExample : MonoBehaviour
    {
        RecorderController m_RecorderController;

        void OnEnable()
        {
            var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
            m_RecorderController = new RecorderController(controllerSettings);

            var mediaOutputFolder = Path.Combine(Application.dataPath, "MyAsset");

            // Image
            var imageRecorder = ScriptableObject.CreateInstance<ImageRecorderSettings>();
            imageRecorder.name = "My Image Recorder";
            imageRecorder.Enabled = true;
            imageRecorder.OutputFormat = ImageRecorderSettings.ImageRecorderOutputFormat.PNG;
            imageRecorder.CaptureAlpha = false;

            imageRecorder.OutputFile = Path.Combine(mediaOutputFolder, "image_") + DefaultWildcard.Take;

            imageRecorder.imageInputSettings = new GameViewInputSettings
            {
                OutputWidth = 900,
                OutputHeight = 2000,
            };

            // Setup Recording
            controllerSettings.AddRecorderSettings(imageRecorder);
        }

        void OnGUI()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                m_RecorderController.PrepareRecording();
                m_RecorderController.StartRecording();
            }
        }
    }
}

#endif
