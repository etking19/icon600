using Session.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Windows;

namespace WindowsFormClient.Command
{
    class ClientKeyboardCmdImpl : BaseImplementer
    {
        public override void ExecuteCommand(string userId, string command)
        {
            ClientKeyboardCmd keyboardData = deserialize.Deserialize<ClientKeyboardCmd>(command);
            if (keyboardData == null)
            {
                return;
            }

            InputConstants.KEYBOARDINPUT keyboardInput = new InputConstants.KEYBOARDINPUT();
            keyboardInput.wScan = keyboardData.data.wScan;
            keyboardInput.wVk = keyboardData.data.wVk;
            keyboardInput.dwFlags = keyboardData.data.dwFlags;
            keyboardInput.time = keyboardData.data.time;
            keyboardInput.dwExtraInfo = IntPtr.Zero;

            // create input object
            InputConstants.INPUT input = new InputConstants.INPUT();
            input.type = InputConstants.KEYBOARD;
            input.ki = keyboardInput;

            // send input to Windows
            InputConstants.INPUT[] inputArray = new InputConstants.INPUT[] { input };
            uint result = NativeMethods.SendInput(1, inputArray, System.Runtime.InteropServices.Marshal.SizeOf(input));
        }
    }
}
