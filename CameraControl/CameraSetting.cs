/******************************************************************************
*                                                                             *
*   PROJECT : Eos Digital camera Software Development Kit EDSDK               *
*                                                                             *
*   Description: This is the Sample code to show the usage of EDSDK.          *
*                                                                             *
*                                                                             *
*******************************************************************************
*                                                                             *
*   Written and developed by Canon Inc.                                       *
*   Copyright Canon Inc. 2018 All Rights Reserved                             *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;

namespace CameraControl
{

    public partial class CameraSetting : Form, IObserver
    {
        CameraController _controller = null;

        public CameraSetting(ref CameraController controller, ref IntPtr[] volumes, ref int volume_count)
        {
            uint err = EDSDKLib.EDSDK.EDS_ERR_OK;
            EDSDKLib.EDSDK.EdsVolumeInfo outVolumeInfo;
            _controller = controller;
            _volumes = volumes;
            _volume_count = volume_count;

            InitializeComponent();



            comboBox1.DrawMode = DrawMode.Normal;
            comboBox1.Items.AddRange(new object[] { "Change Owner Name", "Change Artist", "Change Copyright" });
            comboBox1.SelectedIndex = 0;

            if (_volume_count > 0)
            {
                err = EDSDKLib.EDSDK.EdsGetChildAtIndex(_controller.GetModel().Camera, 0, out _volumes[0]);
                err = EDSDKLib.EDSDK.EdsGetVolumeInfo(_volumes[0], out outVolumeInfo);
                if (outVolumeInfo.StorageType != (uint)EDSDKLib.EDSDK.EdsStorageType.Non)
                {
                    this.button1.Enabled = true;
                    this.button1.Text = "Memory Card 1 (" + outVolumeInfo.szVolumeLabel + ")";
                }
                if (_volume_count > 1)
                {
                    err = EDSDKLib.EDSDK.EdsGetChildAtIndex(_controller.GetModel().Camera, 1, out _volumes[1]);
                    err = EDSDKLib.EDSDK.EdsGetVolumeInfo(_volumes[1], out outVolumeInfo);
                    if (outVolumeInfo.StorageType != (uint)EDSDKLib.EDSDK.EdsStorageType.Non)
                    {
                        this.button3.Enabled = true;
                        this.button3.Text = "Memory Card 2 (" + outVolumeInfo.szVolumeLabel + ")";
                    }
                }
            }

        }

        private IntPtr[] _volumes = new IntPtr[2];
        private int _volume_count = 0;

        public void ShowSettingDialog()
        {
            this.ShowDialog();
        }

        public void Update(Observable from, CameraEvent e)
        {
            Update(e);

        }


        private delegate void _Update(CameraEvent e);

        private void Update(CameraEvent e)
        {
            if (this.InvokeRequired)
            {
                //The update processing can be executed from another thread. 
                this.Invoke(new _Update(Update), new object[] { e });
                return;
            }

            CameraEvent.Type eventType = e.GetEventType();

            switch (eventType)
            {
                case CameraEvent.Type.PROPERTY_CHANGED:
                    UpdateItems((uint)e.GetArg());
                    break;
                default:
                    break;
            }
        }

        private uint UpdateItems(uint propertyID)
        {
            IntPtr camera = _controller.GetModel().Camera;
            uint err = EDSDKLib.EDSDK.EDS_ERR_OK;
            string stItem = string.Empty;

            switch (propertyID)
            {
                case EDSDKLib.EDSDK.PropID_OwnerName:
                    err = EDSDKLib.EDSDK.EdsGetPropertyData(camera, EDSDKLib.EDSDK.PropID_OwnerName, 0, out stItem);
                    if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
                    {
                        return err;
                    }
                    this.listView1.Items[3].SubItems[1].Text = stItem;
                    break;
                case EDSDKLib.EDSDK.PropID_Artist:
                    err = EDSDKLib.EDSDK.EdsGetPropertyData(camera, EDSDKLib.EDSDK.PropID_Artist, 0, out stItem);
                    if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
                    {
                        return err;
                    }
                    this.listView1.Items[4].SubItems[1].Text = stItem;
                    break;
                case EDSDKLib.EDSDK.PropID_Copyright:
                    err = EDSDKLib.EDSDK.EdsGetPropertyData(camera, EDSDKLib.EDSDK.PropID_Copyright, 0, out stItem);
                    if (err != EDSDKLib.EDSDK.EDS_ERR_OK)
                    {
                        return err;
                    }
                    this.listView1.Items[5].SubItems[1].Text = stItem;
                    break;

            }

            return err;

        }

        private void FormSetting_Load(object sender, EventArgs e)
        {
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Columns.Add("Key", (listView1.PreferredSize.Width / 2) - 21);
            this.listView1.Columns.Add("Value", (listView1.PreferredSize.Width / 2) );
            IntPtr camera = _controller.GetModel().Camera;
            string[] row = { string.Empty, string.Empty };
            string stItem = string.Empty;

            if (camera == IntPtr.Zero)
            {

                row[0] = "Product Name";
                row[1] = "No camera detected";
                this.listView1.Items.Add(new ListViewItem(row));
            }
            else
            {
                EDSDKLib.EDSDK.EdsDeviceInfo devinfo;
                EDSDKLib.EDSDK.EdsGetDeviceInfo(camera, out devinfo);
                //EDSDKLib.EDSDK.EdsTime dateTime;

                uint err = EDSDKLib.EDSDK.EdsGetPropertyData(camera, EDSDKLib.EDSDK.PropID_ProductName, 0, out stItem);
                row[0] = "Product Name";
                row[1] = stItem;
                this.listView1.Items.Add(new ListViewItem(row));

                err = EDSDKLib.EDSDK.EdsGetPropertyData(camera, EDSDKLib.EDSDK.PropID_BodyIDEx, 0, out stItem);
                row[0] = "Serial Number";
                row[1] = stItem;
                this.listView1.Items.Add(new ListViewItem(row));

                err = EDSDKLib.EDSDK.EdsGetPropertyData(camera, EDSDKLib.EDSDK.PropID_FirmwareVersion, 0, out stItem);
                row[0] = "Firmware Version";
                row[1] = stItem;
                this.listView1.Items.Add(new ListViewItem(row));

                err = EDSDKLib.EDSDK.EdsGetPropertyData(camera, EDSDKLib.EDSDK.PropID_OwnerName, 0, out stItem);
                row[0] = "Owner Name";
                row[1] = stItem;
                this.listView1.Items.Add(new ListViewItem(row));
                this.textBox3.Text = stItem;

                err = EDSDKLib.EDSDK.EdsGetPropertyData(camera, EDSDKLib.EDSDK.PropID_Artist, 0, out stItem);
                row[0] = "Artist";
                row[1] = stItem;
                this.listView1.Items.Add(new ListViewItem(row));

                err = EDSDKLib.EDSDK.EdsGetPropertyData(camera, EDSDKLib.EDSDK.PropID_Copyright, 0, out stItem);
                row[0] = "Copyright";
                row[1] = stItem;
                this.listView1.Items.Add(new ListViewItem(row));

                //err = EDSDKLib.EDSDK.EdsGetPropertyData(camera, EDSDKLib.EDSDK.PropID_DateTime, 0, out dateTime);
                //DateTime dt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0);
                //row[0] = "DateTime";
                //row[1] = dt.ToString("yy/MM/dd HH:mm");
                //this.listView1.Items.Add(new ListViewItem(row));
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _controller.FormatVolumeCommand(_volumes[0]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            uint[] property = { EDSDKLib.EDSDK.PropID_OwnerName, EDSDKLib.EDSDK.PropID_Artist, EDSDKLib.EDSDK.PropID_Copyright };
            IntPtr camera = _controller.GetModel().Camera;
            string str = textBox3.Text;
            // Confirm whether a multibyte character is not included.
            Encoding sjisEnc = Encoding.GetEncoding("EUC-JP");
            if (str.Length == sjisEnc.GetByteCount(str))
            {
                uint err = EDSDKLib.EDSDK.EdsSetPropertyData(camera, property[comboBox1.SelectedIndex], 0, textBox3.Text.Length + 1, str);
            }
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int index = 3 + comboBox1.SelectedIndex;
            if (index < this.listView1.Items.Count)
                this.textBox3.Text = this.listView1.Items[3 + comboBox1.SelectedIndex].SubItems[1].Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _controller.FormatVolumeCommand(_volumes[1]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var _dateTimeZoneSetting = new DateTimeZoneSetting(ref _controller);
            _dateTimeZoneSetting.ShowDialog(this);
            _dateTimeZoneSetting.Dispose();
        }
    }
}
