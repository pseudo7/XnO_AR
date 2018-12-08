using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
public class UDTManager : MonoBehaviour, IUserDefinedTargetEventHandler
{
    public Text text, quality;
    public Button captureButton;
    public ImageTargetBehaviour imageTarget;

    UserDefinedTargetBuildingBehaviour userDefinedTargetBuildingBehaviour;
    ObjectTracker objectTracker;
    DataSet dataSet;
    ImageTargetBuilder.FrameQuality m_FrameQuality;
    int targetCounter;

    void Start()
    {
        userDefinedTargetBuildingBehaviour = GetComponent<UserDefinedTargetBuildingBehaviour>();
        if (userDefinedTargetBuildingBehaviour)
            userDefinedTargetBuildingBehaviour.RegisterEventHandler(this);
    }

    public void OnFrameQualityChanged(ImageTargetBuilder.FrameQuality frameQuality)
    {
        quality.text = string.Format("{0}", frameQuality.ToString());
        m_FrameQuality = frameQuality;
    }

    public void OnInitialized()
    {
        objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        if (objectTracker != null)
        {
            dataSet = objectTracker.CreateDataSet();
            objectTracker.ActivateDataSet(dataSet);
        }
    }

    public void OnNewTrackableSource(TrackableSource trackableSource)
    {
        targetCounter++;
        objectTracker.DeactivateDataSet(dataSet);
        dataSet.CreateTrackable(trackableSource, imageTarget.gameObject);
        objectTracker.ActivateDataSet(dataSet);
        userDefinedTargetBuildingBehaviour.StartScanning();
    }

    public void BuildTarget()
    {
        if (m_FrameQuality == ImageTargetBuilder.FrameQuality.FRAME_QUALITY_HIGH)
        {
            if (!text.enabled)
                return;
            userDefinedTargetBuildingBehaviour.BuildNewTarget(targetCounter.ToString(), imageTarget.GetSize().x);
            text.enabled = false;
            quality.enabled = false;
            captureButton.interactable = false;
        }
    }
}
