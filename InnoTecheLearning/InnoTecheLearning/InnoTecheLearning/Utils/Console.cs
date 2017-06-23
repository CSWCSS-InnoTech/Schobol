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
            public StringBuilder Out { get; } = new StringBuilder();

            public event PropertyChangedEventHandler PropertyChanged;

            public void Execute(string Command)
            {
                Out.AppendLine(Command);
                Out.AppendLine();
                var Content = Command.Contains(" ") ? Command.Substring(Command.IndexOf(' ') + 1) : null;
                var SubHeader = Content == null ? null : Content.Contains(" ") ? Content.Remove(Content.IndexOf(' ')) : Content;
                var SubContent = Content == null || !Content.Contains(" ") ? null : Content.Substring(Content.IndexOf(' ') + 1);
                switch (Command.Contains(" ") ? Command.Remove(Command.IndexOf(' ')) : Command)
                {
                    case "temp":
                        switch (SubHeader)
                        {
                            case "show":
                                if (string.IsNullOrWhiteSpace(SubContent))
                                {
                                    foreach (var file in Directory.EnumerateFiles(Temp.TempPath)) Out.AppendLine(file);
                                    Out.AppendLine();
                                }
                                else { Out.Append(File.ReadAllText(SubContent)); Out.AppendLine(); }
                                break;
                            case "delete":
                                File.Delete(SubContent);
                                break;
                        }
                        break;
                    case "help":
                        Out.AppendLine(
@"clear: Clears the screen
temp show: Show all temp files
temp delete <file: string>: Delete a specific temp file
temp show <file: string>: Show a specific temp file"
                        );
                        break;
                    case "clear":
                        Out.Clear();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}