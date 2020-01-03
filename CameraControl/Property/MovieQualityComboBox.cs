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

namespace CameraControl
{
    class MovieQualityComboBox : PropertyComboBox, IObserver
    {
        private ActionSource _actionSource;

        private EDSDKLib.EDSDK.EdsPropertyDesc _desc;

        public void SetActionSource(ref ActionSource actionSource) { _actionSource = actionSource; }

        public MovieQualityComboBox()
        {
            map.Add(0x00000200, "fhd_2398");
            map.Add(0x00000210, "fhd_2398_alli_editing");
            map.Add(0x00000230, "fhd_2398_ipb_standard");
            map.Add(0x00000310, "fhd_2400_alli_editing");
            map.Add(0x00000330, "fhd_2400_ipb_standard");
            map.Add(0x00000400, "fhd_2500");
            map.Add(0x00000410, "fhd_2500_alli_editing");
            map.Add(0x00000430, "fhd_2500_ipb_standard");
            map.Add(0x00000500, "fhd_2997");
            map.Add(0x00000510, "fhd_2997_alli_editing");
            map.Add(0x00000530, "fhd_2997_ipb_standard");
            map.Add(0x00000610, "fhd_5000_alli_editing");
            map.Add(0x00000630, "fhd_5000_ipb_standard");
            map.Add(0x00000710, "fhd_5994_alli_editing");
            map.Add(0x00000730, "fhd_5994_ipb_standard");
            map.Add(0x00001210, "fhd_2398_alli_editing");
            map.Add(0x00001230, "fhd_2398_ipb_standard");
            map.Add(0x00001310, "fhd_2400_alli_editing");
            map.Add(0x00001330, "fhd_2400_ipb_standard");
            map.Add(0x00001410, "fhd_2500_alli_editing");
            map.Add(0x00001430, "fhd_2500_ipb_standard");
            map.Add(0x00001431, "fhd_2500_ipb_light");
            map.Add(0x00001510, "fhd_2997_alli_editing");
            map.Add(0x00001530, "fhd_2997_ipb_standard");
            map.Add(0x00001531, "fhd_2997_ipb_light");
            map.Add(0x00001610, "fhd_5000_alli_editing");
            map.Add(0x00001630, "fhd_5000_ipb_standard");
            map.Add(0x00001710, "fhd_5994_alli_editing");
            map.Add(0x00001730, "fhd_5994_ipb_standard");
            map.Add(0x00010600, "hd_5000");
            map.Add(0x00010700, "hd_5994");
            map.Add(0x00010810, "hd_1000_alli_editing");
            map.Add(0x00010910, "hd_1199_alli_editing");
            map.Add(0x00011430, "hd_2500_ipb_standard");
            map.Add(0x00011431, "hd_5000_ipb_standard");
            map.Add(0x00011530, "hd_2997_ipb_standard");
            map.Add(0x00011531, "hd_2997_ipb_light");
            map.Add(0x00011610, "hd_5000_alli_editing");
            map.Add(0x00011630, "hd_5000_ipb_standard");
            map.Add(0x00011710, "hd_5994_alli_editing");
            map.Add(0x00011730, "hd_5994_ipb_standard");
            map.Add(0x00011810, "hd_1000_alli_editing");
            map.Add(0x00011830, "hd_1000_ipb_standard");
            map.Add(0x00011910, "hd_1199_alli_editing");
            map.Add(0x00011930, "hd_1199_ipb_standard");
            map.Add(0x00020400, "vga_2500");
            map.Add(0x00020500, "vga_2397");
            map.Add(0x00030240, "dci4k_2398_motion_jp");
            map.Add(0x00030340, "dci4k_2400_motion_jp");
            map.Add(0x00030440, "dci4k_2500_motion_jp");
            map.Add(0x00030540, "dci4k_2997_motion_jp");
            map.Add(0x00051210, "4k_2398_alli_editing");
            map.Add(0x00051230, "4k_2398_ipb_standard");
            map.Add(0x00051310, "4k_2400_alli_editing");
            map.Add(0x00051330, "4k_2400_ipb_standard");
            map.Add(0x00051410, "4k_2500_alli_editing");
            map.Add(0x00051430, "4k_2500_ipb_standard");
            map.Add(0x00051510, "4k_2997_alli_editing");
            map.Add(0x00051530, "4k_2997_ipb_standard");
            map.Add(0xffffffff, "unknown");
        }

        protected override void OnSelectionChangeCommitted(EventArgs e)
        {
            if (this.SelectedItem != null)
            {
                uint key = (uint)_desc.PropDesc[this.SelectedIndex];

                _actionSource.FireEvent(ActionEvent.Command.SET_MOVIEQUALITY, (IntPtr)key);
            }
        }

        public void Update(Observable from, CameraEvent e)
        {

            CameraModel model = (CameraModel)from;
            CameraEvent.Type eventType = CameraEvent.Type.NONE;

            if ((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED || eventType == CameraEvent.Type.PROPERTY_DESC_CHANGED)
            {
                uint propertyID = (uint)e.GetArg();

                if (propertyID == EDSDKLib.EDSDK.PropID_MovieParam)
                {
                    uint property = model.MovieQuality;

                    //Update property
                    switch (eventType)
                    {
                        case CameraEvent.Type.PROPERTY_CHANGED:
                            this.UpdateProperty(property);
                            break;

                        case CameraEvent.Type.PROPERTY_DESC_CHANGED:
                            _desc = model.MovieQualityDesc;
                            // Ignore PropertyDesc when shooting still images.
                            uint movieMode;
                            uint err = EDSDKLib.EDSDK.EdsGetPropertyData(model.Camera, EDSDKLib.EDSDK.PropID_FixedMovie, 0, out movieMode);
                            if (movieMode == 0)
                            {
                                _desc.NumElements = 0;
                            }
                            this.UpdatePropertyDesc(ref _desc);
                            this.UpdateProperty(property);
                            break;
                    }
                }
            }
        }
    }
}
