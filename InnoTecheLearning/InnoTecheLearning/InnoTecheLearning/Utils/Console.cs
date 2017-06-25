using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public static Commands Console = new Commands();
        public class Commands : INotifyPropertyChanged
        {
            public StringBuilder Out { get; } = new StringBuilder(
                $@"CSWCSS eLearn Utilities [Version {Version}]
(c) 2017 Innovative Technology Society of CSWCSS. All rights reserved.

");
            public string OutText => Out.ToString();

            public event PropertyChangedEventHandler PropertyChanged = delegate { };
            void Changed() => PropertyChanged(this, new PropertyChangedEventArgs(nameof(OutText)));

            public void Execute(string Command)
            {
                try
                {
                    Out.AppendFormat("> {0}\n", Command);
                    Changed();
                    var Content = Command.Contains(" ") ? Command.Substring(Command.IndexOf(' ') + 1) : null;
                    var SubHeader = (Content == null ? null :
                        Content.Contains(" ") ? Content.Remove(Content.IndexOf(' ')) : Content)?.ToLower();
                    var SubContent = Content == null || !Content.Contains(" ") ? null : Content.Substring(Content.IndexOf(' ') + 1);
                    switch ((Command.Contains(" ") ? Command.Remove(Command.IndexOf(' ')) : Command).ToLower())
                    {
                        case "lingual":
                            if (SubHeader == "server")
                            {
                                switch (SubContent?.ToLower())
                                {
                                    case "pearson":
                                        OnlineDict.UsePearson = true;
                                        Out.Append("Set Lingual server to Pearson.");
                                        break;
                                    case "pedosa":
                                        OnlineDict.UsePearson = false;
                                        Out.Append("Set Lingual server to Pedosa.");
                                        break;
                                    default:
                                        Out.Append("Invalid argument. Available arguments: pearson, pedosa");
                                        break;
                                }
                            }
                            break;
                        case "temp":
                            switch (SubHeader)
                            {
                                case "clear":
                                    foreach (var file in Directory.GetFiles(Temp.TempPath)) File.Delete(file);
                                    Out.Append("Successfully cleared all temporary files.");
                                    break;
                                case "delete":
                                    if (!string.IsNullOrWhiteSpace(SubContent))
                                    {
                                        File.Delete(SubContent);
                                        Out.AppendFormat("Successfully deleted {0}.", SubContent);
                                    }
                                    break;
                                case "show":
                                    if (string.IsNullOrWhiteSpace(SubContent))
                                    {
                                        foreach (var file in Directory.EnumerateFiles(Temp.TempPath))
                                            Out.AppendLine(Path.GetFileName(file));
                                    }
                                    else
                                    {
                                        Out.Append(File.ReadAllText(Path.Combine(Temp.TempPath, SubContent)));
                                    }
                                    break;
                                case "weigh":
                                    if (string.IsNullOrWhiteSpace(SubContent))
                                        foreach (var file in Directory.GetFiles(Temp.TempPath))
                                            Out.AppendFormat(
                                            "The size of {0} is {1} bytes.\n",
                                            file,
                                            new FileInfo(Path.Combine(Temp.TempPath, file)).Length
                                            );
                                    else Out.AppendFormat(
                                            "The size of {0} is {1} bytes.",
                                            SubContent,
                                            new FileInfo(Path.Combine(Temp.TempPath, SubContent)).Length
                                            );
                                    break;
                            }
                            break;
                        case "help":
                            Out.Append(@"clear: Clears the screen
help: Shows available commands
lingual dictionary <pearson|pedosa>: Switches between two Dictionary servers. Default: Pedosa
temp clear: Clears all temporary files
temp delete <file: string>: Deletes a specific temporary file
temp show: Shows all temporary files
temp show <file: string>: Shows the contents of a specific temporary file
temp weigh: Shows the size of every temporary file in bytes
temp weigh <file: string>: Shows the size of a specific temporary file in bytes"
                            );
                            break;
                        case "clear":
                            Out.Clear();
                            break;
                        case var s:
                            Out.AppendFormat("'{0}' is not recognized as a command.", s);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Out.Append(ex.Message);
                }
                if (Out.Length > 0)
                {
                    Out.AppendLine();
                    Out.AppendLine();
                }
                Changed();
            }
        }
    }
}