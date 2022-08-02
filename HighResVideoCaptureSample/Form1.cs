/*
 * THIS CODE AND INFORMATION IS PROVIDED AS IS WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPIED.
 *
 * Copyright (c) Digital Doc. All rights reserved
 *
 * Author: James Petko (JPetko@Digi-Doc.com)
 * Purpose: Implementation sample of UHD Video sources with Game Controller Capture signal processing.
 * Date: 2/22/2022
 */

using AForge.Video.DirectShow;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace HighResVideoCaptureSample
{
    public partial class Form1 : Form
    {
        //GameController variables.
        private DirectInput Input = new DirectInput();
        bool isHighRes = false;

        List<DeviceInstance> directInputList = new List<DeviceInstance>();
        private DirectInput directInput = new DirectInput();
        Guid joystickGuid = Guid.Empty;
        public Joystick joystick;

        private FilterInfoCollection _filterInfoCollection;
        private VideoCaptureDevice _videoDevice;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// On Load Populate Cameras, Only accounting for one video source connected in this example application. If no cameras are present, an exception will be raised.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            List<string> cameras = dxCameraControl1.GetAvailableCameras();
            if (cameras.Count == 0)
            {
                isHighRes = false;
                this.Size = new Size(640, 480);
                //init AForge Interface
                _filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                directInputList.AddRange(Input.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly));
                foreach (AForge.Video.DirectShow.FilterInfo x in _filterInfoCollection)
                {
                    if (x.Name.Contains("Iris") || x.Name.Contains("IRIS") || x.Name.Contains("HYBRIS"))
                    {
                        this.Text = "Digital Doc Camera Capture";
                    }
                    if (x.Name.Contains("X80"))
                    {
                        this.Text = "Iris X80 Camera Capture";
                    }
                }
                if (_filterInfoCollection.Count != 0)
                {
                    _videoDevice = new VideoCaptureDevice(_filterInfoCollection[0].MonikerString);
                    videoPlayer.VideoSource = _videoDevice;

                    videoPlayer.Size = new Size(640, 480);
                    videoPlayer.BringToFront();
                    videoPlayer.Start();
                }
                else 
                {
                    MessageBox.Show("Please Connect a supported camera.", "No Camera Connected");
                    this.Close();
                }
                try
                {
                    if (Init_JoyStick()) //Inialize the Joystick
                    {
                        gameConPollingTimer.Start(); //poll game con every 50ms
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not find Capture Button source" + ex.Message, "Game Controller Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                isHighRes = true;
                string defaultCamera = "";
                try
                {
                    Console.WriteLine("Camera: " + cameras[0]);
                    this.Text = "Camera Capture: " + cameras[0];
                    defaultCamera = cameras[0];

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please ensure that your camera is connected.\r\n" + ex.Message, "Cannot retrieve Camera", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                if (cameras.Count != 0)
                {
                    //available resolutions and renderers
                    List<string> types = dxCameraControl1.GetMediaTypeList(defaultCamera);
                    object[] resoultionIndex = types.ToArray();
                    string defaultResoltion = types[0];
                    dxCameraControl1.OnTakePhoto += DxCameraControl1_OnTakePhoto;
                    dxCameraControl1.StartPreview(defaultCamera, 0); //start video stream
                    try
                    {

                        if (Init_JoyStick()) //Inialize the Joystick
                        {
                            gameConPollingTimer.Start(); //poll game con every 50ms
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not find Capture Button source" + ex.Message, "Game Controller Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            try
            {
                if (gameConPollingTimer.Enabled &&!isHighRes)
                {
                    _videoDevice.Stop();

                    Thread.Sleep(1000);
                }

                Dispose();
                GC.Collect();
                Environment.Exit(0);
            }
            catch { Console.WriteLine("On Close Error"); }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            dxCameraControl1.Size = new Size(this.Size.Width - 5, this.Size.Height - 5);
        }

        /// <summary>
        /// Captures the image from the camera control
        /// </summary>
        /// <param name="imageData"></param>
        private void DxCameraControl1_OnTakePhoto(byte[] imageData)
        {
            string currentUsersDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            int fileIndex = 0;
            while (File.Exists(currentUsersDesktop + "\\capture" + fileIndex.ToString() + ".png"))
                fileIndex++;
            using (var ms = new System.IO.MemoryStream(imageData))
            {
                Image.FromStream(ms).Save(currentUsersDesktop + "\\capture" + fileIndex.ToString() + ".png");
            }
        }

        /// <summary>
        /// Assigning Game Con Button Listener function to Polling timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gameConPollingTimer_Tick(object sender, EventArgs e)
        {
            // Acquire the joystick
            joystick.Acquire();

            // Poll events from joystick

            joystick.Poll();
            var datas = joystick.GetBufferedData();
            foreach (var press in datas)
            {
                if (press.Value != 0)
                {
                    dxCameraControl1.TakePhotoAsync();//Async capture to not interupt the stream. If pause feature is desired, implement it here.
                    System.Media.SystemSounds.Beep.Play();
                }
            }
        }
        private bool Init_JoyStick()
        {
            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
                joystickGuid = deviceInstance.InstanceGuid;

            // If Gamepad not found, look for a Joystick
            if (joystickGuid == Guid.Empty)
                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                    joystickGuid = deviceInstance.InstanceGuid;

            // If Joystick not found, throws an error
            if (joystickGuid == Guid.Empty)
            {
                return false;
            }

            // Instantiate the joystick
            joystick = new Joystick(directInput, joystickGuid);

            Console.WriteLine("Found Joystick/Gamepad with GUID: {0}", joystickGuid);

            // Query all suported ForceFeedback effects
            var allEffects = joystick.GetEffects();
            foreach (var effectInfo in allEffects)
                Console.WriteLine("Effect available {0}", effectInfo.Name);

            // Set BufferSize in order to use buffered data.
            joystick.Properties.BufferSize = 128;
            return true;
        }



    }
}