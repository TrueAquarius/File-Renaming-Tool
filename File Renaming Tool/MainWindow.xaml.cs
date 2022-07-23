using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO;
using System.Threading;

namespace File_Renaming_Tool
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string root;
        string origName;
        string replaceName;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void Log(string text)
        {
            this.Dispatcher.Invoke(() => {
                LogArea.Text += text + "\n";
                LogArea.ScrollToEnd();
            });
        }
        private void EmptyLog()
        {
            this.Dispatcher.Invoke(() => {
                LogArea.Text = "";
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            root = Root.Text;
            origName = OrigName.Text;
            replaceName = ReplaceName.Text;
            EmptyLog();
            new Thread(Rename).Start();
        }


        private void Rename()
        {

            EmptyLog();
            Rename(root, origName, replaceName);
            Log("DONE");
        }
        private void Rename(string root, string origName, string replaceName)
        {
            char [] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            if (replaceName.IndexOfAny(invalidChars) >= 0)
            {
                Log("Replacement Name contains invalid characters. ");
                Log("ABANDONED. ");
                return;
            }

            if (replaceName == null || replaceName.Length == 0)
            {
                Log("Replacement Name must not be empty. ");
                Log("ABANDONED. ");
                return;
            }

            if (origName == null || origName.Length == 0)
            {
                Log("Original Name must not be empty. ");
                Log("ABANDONED. ");
                return;
            }

            if (root == null || root.Length == 0)
            {
                Log("Path must not be empty. ");
                Log("ABANDONED. ");
                return;
            }

            if (!Directory.Exists(root))
            {
                Log("Path does not seem to exist. ");
                Log("ABANDONED. ");
                return;
            }

            Log("Folder: " + root);

            var files = Directory.EnumerateFiles(root);

            foreach(string file in files)
            {
                RenameFile(file, origName, replaceName);
            }

            var dirs = Directory.EnumerateDirectories(root);

            foreach(string dir in dirs)
            {
                Rename(dir, origName, replaceName);
            }

        }

        private void RenameFile(string path, string origName, string replaceName)
        {
            File.Move(path, path.Replace(origName, replaceName));
            Log("   File: " + path);
            Log("         " + path.Replace(origName, replaceName));

        }
    }
}
