﻿using UnityEngine;
using Microsoft.Azure.Kinect.Sensor;

namespace AzureKinect4Unity
{
    [RequireComponent(typeof(PointCloudRenderer))]
    public class AzureKinectPointCloudVisualizer : MonoBehaviour
    {
        [SerializeField] AzureKinectManager _AzureKinectManager;

        AzureKinectSensor _KinectSensor;

        PointCloudRenderer _PointCloudRenderer;
        Texture2D _TransformedColorImageTexture;

        void Start()
        {
            _KinectSensor = _AzureKinectManager.Sensor;
            if (_KinectSensor != null)
            {
                Debug.Log("ColorResolution: " + _KinectSensor.ColorImageWidth + "x" + _KinectSensor.ColorImageHeight);
                Debug.Log("DepthResolution: " + _KinectSensor.DepthImageWidth + "x" + _KinectSensor.DepthImageHeight);

                _TransformedColorImageTexture = new Texture2D(_KinectSensor.DepthImageWidth, _KinectSensor.DepthImageHeight, TextureFormat.BGRA32, false);

                _PointCloudRenderer = GetComponent<PointCloudRenderer>();
                _PointCloudRenderer.GenerateMesh(_KinectSensor.DepthImageWidth, _KinectSensor.DepthImageHeight);
            }
        }

        void Update()
        {
            if (_KinectSensor.TransformedColorImage != null)
            {
                _TransformedColorImageTexture.LoadRawTextureData(_KinectSensor.TransformedColorImage);
                _TransformedColorImageTexture.Apply();
            }

            if (_KinectSensor.PointCloud != null)
            {
                Short3[] pointCloud = _KinectSensor.PointCloud;
                
                Vector3[] vertices = new Vector3[pointCloud.Length];
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = new Vector3(pointCloud[i].X * 0.001f, pointCloud[i].Y * -0.001f, pointCloud[i].Z * 0.001f);
                }

                _PointCloudRenderer.UpdateVertices(vertices);
                _PointCloudRenderer.UpdateColorTexture(_KinectSensor.TransformedColorImage);
            }
        }
    }
}