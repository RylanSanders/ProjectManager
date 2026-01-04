using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectManager.Controls
{
    /// <summary>
    /// Interaction logic for TODONote.xaml
    /// </summary>
    public partial class TODONote : UserControl
    {
        private bool isUpdatingStyle  = false;
        public TODONote()
        {
            InitializeComponent();
            isUpdatingStyle = true;
            LoadXamlPackage("RichTextTest.xaml");
            isUpdatingStyle = false;
        }

        private void MainRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //MessageBox.Show(GetRichTextBoxSettings());
            if (!isUpdatingStyle)
            {
                ApplyStyling(e);
                SaveXamlPackage("RichTextTest.xaml");
            }
            
        }

        private void ApplyStyling(TextChangedEventArgs e)
        {
            isUpdatingStyle = true;
            //var st = GetRichTextBoxSettings();

            //if (st.Contains("- "))
            //{
            processChanges(e);
            //    List a = new List();
            //    a.ListItems.Add(new ListItem(new Paragraph(new Run("Test1"))));
            //    MainRichTextBox.Document.Blocks.Add(a);
            //}
            isUpdatingStyle = false;
        }

        private void processChanges(TextChangedEventArgs e)
        {
         foreach (var change in e.Changes)
            {
                if (change.AddedLength > 0 || change.RemovedLength > 0)
                {
                    TextPointer start = MainRichTextBox.Document.ContentStart.GetPositionAtOffset(change.Offset);
                    TextPointer contentStart = MainRichTextBox.Document.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward);
                    var nextStart = contentStart.GetLineStartPosition(1);
                    TextPointer end = MainRichTextBox.Document.ContentStart.GetPositionAtOffset(change.Offset + change.AddedLength);
                    TextRange affectedRange = new TextRange(start, end);
                    string editedLine = affectedRange.Text;
                    //TODO
                    //Read this https://learn.microsoft.com/en-us/dotnet/api/system.windows.documents.textpointer?view=windowsdesktop-8.0#remarks
                    //I think that I need to insert a paragraph and maintain each run to be inside a paragraph
                    //https://stackoverflow.com/questions/62074093/how-to-get-the-next-line-with-textpointer-getlinestartposition-in-a-richtextbo
                    //affectedRange.Text = "";
                    //TODO if a line starts with - + space then it is a list
                    // Now you have the edited line in the 'editedLine' variable.
                    // You can process or analyze it further.
                }
            }
        }

        // Save XAML in RichTextBox to a file specified by _fileName
        private string GetRichTextBoxSettings()
        {
            TextRange range;
            range = new TextRange(MainRichTextBox.Document.ContentStart, MainRichTextBox.Document.ContentEnd);
            return range.Text;
        }

        void SaveXamlPackage(string _fileName)
        {
            TextRange range;
            FileStream fStream;
            range = new TextRange(MainRichTextBox.Document.ContentStart, MainRichTextBox.Document.ContentEnd);
            fStream = new FileStream(_fileName, FileMode.Create);
            range.Save(fStream, DataFormats.XamlPackage);
            fStream.Close();
        }

        void LoadXamlPackage(string _fileName)
        {
            TextRange range;
            FileStream fStream;
            if (File.Exists(_fileName))
            {
                range = new TextRange(MainRichTextBox.Document.ContentStart, MainRichTextBox.Document.ContentEnd);
                fStream = new FileStream(_fileName, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.XamlPackage);
                fStream.Close();
            }
        }
    }
}
