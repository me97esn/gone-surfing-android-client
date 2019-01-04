using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using EasyWiFi.Core;

namespace EasyWiFi.ClientControls
{
    [AddComponentMenu("EasyWiFiController/Client/UserControls/Accelerometer")]
    public class AccelerometerClientController : MonoBehaviour, IClientController
    {
        public string controlName = "Accelerometer";

        AccelerometerControllerType accelerometer;
        string accelerometerKey;

#if UNITY_STANDALONE || UNITY_EDITOR
    Vector3 initialRotation = new Vector3(0f, 0f, 0f);
#endif

    // Use this for initialization
    void Awake()
        {
            accelerometerKey = EasyWiFiController.registerControl(EasyWiFiConstants.CONTROLLERTYPE_ACCELEROMETER, controlName);
            accelerometer = (AccelerometerControllerType)EasyWiFiController.controllerDataDictionary[accelerometerKey];
        }

    //here we grab the input and map it to the data list
    void Update()
        {
          #if UNITY_STANDALONE || UNITY_EDITOR

          if (Input.GetKey(KeyCode.LeftArrow))
          {
            initialRotation.x -= 0.01f;
          }

          if (Input.GetKey(KeyCode.RightArrow))
          {
            initialRotation.x += 0.01f;
          }

          #endif
      mapInputToDataStream();
        }

        public void mapInputToDataStream()
        {
            Vector3 accel;
            accel.x = 0f; accel.y = 0f; accel.z = 0f;

            #if UNITY_STANDALONE || UNITY_EDITOR
                accel = initialRotation;
            #else
                accel = Input.acceleration;
            #endif

            accelerometer.ACCELERATION_X = accel.x;
            accelerometer.ACCELERATION_Y = accel.y;
            accelerometer.ACCELERATION_Z = accel.z;
        }

    }

}