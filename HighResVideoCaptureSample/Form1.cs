/*
 * THIS CODE AND INFORMATION IS PROVIDED AS IS WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPIED.
 *
 * Copyright (c) Digital Doc. All rights reserved
 *
 * Author: James Petko (JPetko@Digi-Doc.com)
 * Purpose: Implementation sample of UHD Video sources with Game Controller Capture signal processing.
 * Date: 2/22/2022
 */

using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HighResVideoCaptureSample
{
    public partial class Form1 : Form
    {
        private Joystick controller1;
        private DirectInput DInput = new DirectInput();
        private bool isFirst = true;

        //GameController variables.
        private int yValue = 0;

        private int xValue = 0;
        private int zValue = 0;
        private int rotationZValue = 0;
        private int rotationXValue = 0;
        private int rotationYValue = 0;

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
            string defaultCamera = "";
            try
            {
                Console.WriteLine("Camera: " + cameras[0]);
                this.Text = "Camera Capture: " + cameras[0];
                defaultCamera = cameras[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please ensure that your camera is connected.", "Cannot retrieve Camera", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    controller1 = GetJoysticks()[0]; //assign game controller listener to default game con
                    gameConPollingTimer.Start(); //poll game con every 50ms
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not find Capture Button source", "Game Controller Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
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
            PollGameCon(controller1, 0);
        }

        private void PollGameCon(Joystick stick, int id)
        {
            // Creates an object from the class JoystickState.
            JoystickState state = new JoystickState();
            state = stick.GetCurrentState();
            //even if not occupied by hardware, each game controller defined in SlimDX requires Joystick States to be initialized.

            #region UnusedGameConVariables

            yValue = -state.Y;
            xValue = state.X;
            zValue = state.Z;
            rotationZValue = state.RotationZ;
            rotationXValue = state.RotationY;
            rotationYValue = state.RotationX;
            int th = 0;
            int[] z = state.GetSliders();
            th = z[0];
            if (z[0] == 0 && isFirst)
            {
                th = 0;
            }
            else
            {
                if (isFirst)
                    isFirst = false;

                if (th >= 0)
                    th = 50 - th / 2;
                else
                    th = -th / 2 + 50;
            }

            #endregion UnusedGameConVariables

            bool[] buttons = state.GetButtons(); // Stores the number of each button on the gamepad into the bool[] butons.
            //Here is an example on how to use this for the joystick in the first index of the array list
            if (id == 0)
            {
                // This is when button 0 of the gamepad is pressed, the label will change. Button 0 should be the square button.
                foreach (var btn in buttons)
                {
                    if (btn)//When the button is Pressed
                    {
                        dxCameraControl1.TakePhotoAsync();//Async capture to not interupt the stream. If pause feature is desired, implement it here.
                        System.Media.SystemSounds.Beep.Play();
                        Console.WriteLine("Button Pressed: " + btn);
                    }
                }
            }
            System.Threading.Thread.Sleep(50);
        }

        /// <summary>
        /// Populate the Game Controller List, we only listen for the default controller in this example app.
        /// </summary>
        /// <returns></returns>
        private Joystick[] GetJoysticks()
        {
            List<SlimDX.DirectInput.Joystick> sticks = new List<SlimDX.DirectInput.Joystick>(); // Creates the list of joysticks connected to the computer via USB.

            foreach (DeviceInstance device in DInput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                // Creates a joystick for each game device in USB Ports
                try
                {
                    controller1 = new SlimDX.DirectInput.Joystick(DInput, device.InstanceGuid);
                    controller1.Acquire();
                    // Gets the joysticks properties and sets the range for them.
                    foreach (DeviceObjectInstance deviceObject in controller1.GetObjects())
                    {
                        if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                        {
                            controller1.GetObjectPropertiesById((int)deviceObject.ObjectType).SetRange(-100, 100);
                        }
                    }

                    // Adds how ever many joysticks are connected to the computer into the sticks list.
                    sticks.Add(controller1);
                }
                catch (DirectInputException)
                {
                }
            }
            return sticks.ToArray();
        }
    }
}