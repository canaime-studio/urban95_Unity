// Copyright 2017 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace GoogleVR.Demos
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.XR;

    // Ensures correct app and scene setup.
    public class DemoSceneManager : MonoBehaviour
    {
        void Start()
        {
            Input.backButtonLeavesApp = true;
        }

        void Update()
        {
            // Exit when (X) is tapped.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //mudarTo2D();
                //StartCoroutine("BackMenu");
                //SceneManager.LoadScene("Menu");
                Application.Quit();
            }
        }
        //IEnumerator BackMenu()
        //{
        //    yield return new WaitForSeconds(2.0f);
        //    SceneManager.LoadScene("Menu");
        //}

        //IEnumerator SwitchTo2D()
        //{        
        //    XRSettings.LoadDeviceByName("");        
        //    yield return null;
        //    ResetCameras();
        //}

        //void ResetCameras()
        //{

        //    for (int i = 0; i < Camera.allCameras.Length; i++)
        //    {
        //        Camera cam = Camera.allCameras[i];
        //        if (cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None)
        //        {        
        //            cam.transform.localPosition = Vector3.zero;

        //            cam.transform.localRotation = Quaternion.identity;
        //        }
        //    }
        //}
        //public void mudarTo2D()
        //{

        //    StartCoroutine(SwitchTo2D());
        //}
    }
}
