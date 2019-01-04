using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using EasyWiFi.Core;

namespace EasyWiFi.ClientControls
{
  [AddComponentMenu("EasyWiFiController/Client/UserControls/Gyro")]
  public class GyroClientController : MonoBehaviour, IClientController
  {

    public string controlName = "Gyro";

    GyroControllerType gyro;
    string gyroKey;


#if UNITY_STANDALONE || UNITY_EDITOR
    Vector3 initialRotation = new Vector3(90f, 90f, 0f);

    bool gotFirstValue = true;
#else
    bool gotFirstValue = false;
#endif

    // initial camera and sensor value
    private Quaternion initialCameraRotation = Quaternion.identity;

    void Awake(){
      gyroKey = EasyWiFiController.registerControl(EasyWiFiConstants.CONTROLLERTYPE_GYRO, controlName);
      gyro = (GyroControllerType)EasyWiFiController.controllerDataDictionary[gyroKey];
    }

    void Start()
    {
      // Used to enable standalone client to run beside server on same machine
      Screen.fullScreen = false;

      SensorHelper.ActivateRotation();
      //StartCoroutine(Calibration());
    }

    IEnumerator Calibration()
    {
      gotFirstValue = false;

      while (!SensorHelper.gotFirstValue)
      {
        SensorHelper.FetchValue();
        yield return null;
      }

      SensorHelper.FetchValue();

      // wait some frames
      yield return new WaitForSeconds(0.1f);

      //// Initialize rotation values
      //Quaternion initialSensorRotation = SensorHelper.rotation;
      //initialCameraRotation *= Quaternion.Euler(0, -initialSensorRotation.eulerAngles.y, 0);

      //// allow updates
      gotFirstValue = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
      mapInputToDataStream();
    }
    /**********************/

#if UNITY_STANDALONE || UNITY_EDITOR
    void Update(){
      // No rotation is fine, just use the acceleration to steer the board
      //if (Input.GetKey(KeyCode.LeftArrow))
      //{
      //  initialRotation.x -= 1f;
      //}

      //if (Input.GetKey(KeyCode.RightArrow))
      //{
      //  initialRotation.x += 1f;
      //}

      //if (Input.GetKey(KeyCode.UpArrow))
      //{
      //  initialRotation.z -= 1f;
      //}

      //if (Input.GetKey(KeyCode.DownArrow))
      //{
      //  initialRotation.y += 1f;
      //}
    }
#endif

    public void mapInputToDataStream()
    {
        // Send the Sensor values instead of the simple gyro rotation.
        //Quaternion quaternion = initialCameraRotation * SensorHelper.rotation;
        Quaternion quaternion = SensorHelper.rotation;

        //Debug.Log("Sending " + quaternion);

#if UNITY_STANDALONE || UNITY_EDITOR
        // Some rotation similar to a correct placed phone
        quaternion = Quaternion.Euler(initialRotation);
#endif
        gyro.GYRO_W = quaternion.w;
        gyro.GYRO_X = quaternion.x;
        gyro.GYRO_Y = quaternion.y;
        gyro.GYRO_Z = quaternion.z;
      
    }
  }
}
