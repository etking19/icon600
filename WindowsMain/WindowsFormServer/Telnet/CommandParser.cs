using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormClient.Telnet.Command;

namespace WindowsFormClient.Telnet
{
    /// <summary>
    /// class to parse the command sent by telnet
    /// </summary>
    public class CommandParser
    {
        private ClearWall _clearWall;
        private CreatePreset _creatPreset;
        private GetInputSourceList _getInputSourceList;
        private GetPresetList _getPresetList;
        private GetRemoteList _getRemoteList;
        private GetWindowList _getWndList;
        private LaunchInputSource _launchInputSource;
        private LaunchPreset _launchPreset;
        private LaunchRemote _launchRemote;
        private MessageBox _messageBox;
        private RemovePreset _removePreset;

        private static CommandParser sInstance;

        public static CommandParser GetInstance()
        {
            if (sInstance == null)
            {
                sInstance = new CommandParser();
            }

            return sInstance;
        }

        public void Initialize(VncMarshall.Client vncClient)
        {
            _clearWall = new ClearWall();
            _creatPreset = new CreatePreset();
            _getInputSourceList = new GetInputSourceList();
            _getPresetList = new GetPresetList();
            _getRemoteList = new GetRemoteList();
            _getWndList = new GetWindowList();
            _launchInputSource = new LaunchInputSource();
            _launchPreset = new LaunchPreset();
            _launchRemote = new LaunchRemote(vncClient);
            _messageBox = new MessageBox();
            _removePreset = new RemovePreset();
        }

        public string parseCommand(string command)
        {
            string[] cmdList = command.Split(',');

            string reply = "Invalid command.";
            if(cmdList.Length != 0)
            {
                try
                {
                    switch (cmdList[0])
                    {                      
                        case ClearWall.COMMAND:
                            reply = _clearWall.executeCommand(cmdList);
                            break;

                        case CreatePreset.COMMAND:
                            reply = _creatPreset.executeCommand(cmdList);
                            break;

                        case GetInputSourceList.COMMAND:
                            reply = _getInputSourceList.executeCommand(cmdList);
                            break;

                        case GetPresetList.COMMAND:
                            reply = _getPresetList.executeCommand(cmdList);
                            break;

                        case GetRemoteList.COMMAND:
                            reply = _getRemoteList.executeCommand(cmdList);
                            break;

                        case GetWindowList.COMMAND:
                            reply = _getWndList.executeCommand(cmdList);
                            break;

                        case LaunchInputSource.COMMAND:
                            reply = _launchInputSource.executeCommand(cmdList);
                            break;

                        case LaunchPreset.COMMAND:
                            reply = _launchPreset.executeCommand(cmdList);
                            break;

                        case LaunchRemote.COMMAND:
                            reply = _launchRemote.executeCommand(cmdList);
                            break;

                        case MessageBox.COMMAND:
                            reply = _messageBox.executeCommand(cmdList);
                            break;

                        case RemovePreset.COMMAND:
                            reply = _removePreset.executeCommand(cmdList);
                            break;

                        default:
                            break;

                    }
                }
                catch (Exception)
                {
                    // formatting error.
                }
                
            }

            return Environment.NewLine + reply + Environment.NewLine;
        }
    }
}
