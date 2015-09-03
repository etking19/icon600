using Session.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Utils.Windows;

namespace WindowsFormClient.Command
{
    class ClientMouseCmdImpl : BaseImplementer
    {
        static uint counter = 0;

        public override void ExecuteCommand(string userId, string command)
        {
            ClientMouseCmd mouseData = deserialize.Deserialize<ClientMouseCmd>(command);
            if (mouseData == null)
            {
                return;
            }

            InputConstants.MOUSEINPUT mouseInput = new InputConstants.MOUSEINPUT();
            mouseInput.dx = mouseData.data.dx;
            mouseInput.dy = mouseData.data.dy;
            mouseInput.mouseData = mouseData.data.mouseData;
            mouseInput.dwFlags = mouseData.data.dwFlags;
            mouseInput.time = mouseData.data.time;
            mouseInput.dwExtraInfo = UIntPtr.Zero;

            // create input object
            InputConstants.INPUT input = new InputConstants.INPUT();
            input.type = InputConstants.MOUSE;
            input.mkhi = new InputConstants.MouseKeyboardHardwareInputUnion()
            {
                mi = mouseInput,
            };

            // send input to Windows
            InputConstants.INPUT[] inputArray = new InputConstants.INPUT[] { input };
            uint result = NativeMethods.SendInput(1, inputArray, System.Runtime.InteropServices.Marshal.SizeOf(input));
        }
    }
}
