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
    class SwitchMirrorUpCommand : Command
    {
        private uint _status;

        public SwitchMirrorUpCommand(ref CameraModel model, uint status) : base(ref model)
        {
            _status = status;
        }

        // Execute command	
        public override bool Execute()
        {
            uint err = EDSDKLib.EDSDK.EDS_ERR_OK;

            // mirrorUpStatus 0 : Disable , 1 : Enable , 2 : During shooting
            uint mirrorLockUpState;
            // Get Mirror Lockup Status.
            err = EDSDKLib.EDSDK.EdsGetPropertyData(_model.Camera, EDSDKLib.EDSDK.PropID_MirrorLockUpState, 0, out mirrorLockUpState);
            // 0x02 During mirror up shooting
            if (mirrorLockUpState == (uint)EDSDKLib.EDSDK.EdsMirrorLockupState.DuringShooting)
            {
                return true;
            }

            // Mirror up ON
            if (_status == 1 && mirrorLockUpState == (uint)EDSDKLib.EDSDK.EdsMirrorLockupState.Disable)
            {
                // mirrorUpSetting 1 : ON
                uint mirrorUpSetting = (uint)EDSDKLib.EDSDK.EdsMirrorUpSetting.On;
                // Set Mirror Lockup Setting.
                err = EDSDKLib.EDSDK.EdsSetPropertyData(_model.Camera, EDSDKLib.EDSDK.PropID_MirrorUpSetting, 0, sizeof(uint), mirrorUpSetting);
            }
            // Mirror up OFF
            else if (_status == 0 && mirrorLockUpState == (uint)EDSDKLib.EDSDK.EdsMirrorLockupState.Enable)
            {
                // mirrorUpSetting 0 : OFF
                uint mirrorUpSetting = (uint)EDSDKLib.EDSDK.EdsMirrorUpSetting.Off;
                // Set Mirror Lockup Setting.
                err = EDSDKLib.EDSDK.EdsSetPropertyData(_model.Camera, EDSDKLib.EDSDK.PropID_MirrorUpSetting, 0, sizeof(uint), mirrorUpSetting);
            }
            else
            {
                //NOP
            }

            return true;
        }
    }
}
