﻿/******************************************************************************
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
    class SetRecCommand : Command
    {
        private uint _status;

        public SetRecCommand(ref CameraModel model, uint status) : base(ref model)
        {
            _status = status;
        }

        // Execute command	
        public override bool Execute()
        {
            // movieMode 0 : Disable , 1 : Enable
            uint movieMode;
            // Get movie mode.
            uint err = EDSDKLib.EDSDK.EdsGetPropertyData(_model.Camera, EDSDKLib.EDSDK.PropID_FixedMovie, 0, out movieMode);

            // Start/End movie recording
            if (movieMode == 1)
            {
                err = EDSDKLib.EDSDK.EdsSetPropertyData(_model.Camera, EDSDKLib.EDSDK.PropID_Record, 0, sizeof(uint), _status);
            }

            return true;
        }
    }
}
