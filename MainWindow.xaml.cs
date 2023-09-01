using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace StarfieldWideScreenPatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                txtEditor.Text = openFileDialog.FileName;
        }

        private void btnPatch_Click(object sender, RoutedEventArgs e)
        {
            if(txtEditor.Text.Contains("Starfield.exe"))
            {
                byte[] originalBytes = { 0x8E, 0xE3, 0x18, 0x40 };
                byte[] replacementBytes = { 0x39, 0x8E, 0x63, 0x40 };

                byte[] fileBytes = File.ReadAllBytes(txtEditor.Text);

                List<int> positions = FindByteSequence(fileBytes, originalBytes);

                if (positions.Count > 0)
                {
                    foreach (var position in positions)
                    {
                        for (int i = 0; i < replacementBytes.Length; i++)
                        {
                            fileBytes[position + i] = replacementBytes[i];
                        }
                    }

                    File.WriteAllBytes(txtEditor.Text, fileBytes);
                    MessageBox.Show("Starfield.exe is now patched", "SUCCESS", MessageBoxButton.OK);
                } else
                {
                    MessageBox.Show("Starfield is already patched", "ERROR", MessageBoxButton.OK);
                }
            } else
            {
                MessageBox.Show("You must select your Starfield.exe in the Steam folders", "ERROR", MessageBoxButton.OK);
            }
        }

        private void btnUnPatch_Click(object sender, RoutedEventArgs e)
        {
            if (txtEditor.Text.Contains("Starfield.exe"))
            {
                byte[] originalBytes = { 0x39, 0x8E, 0x63, 0x40 };
                byte[] replacementBytes = { 0x8E, 0xE3, 0x18, 0x40 };

                byte[] fileBytes = File.ReadAllBytes(txtEditor.Text);

                List<int> positions = FindByteSequence(fileBytes, originalBytes);

                if (positions.Count > 0)
                {
                    foreach (var position in positions)
                    {
                        for (int i = 0; i < replacementBytes.Length; i++)
                        {
                            fileBytes[position + i] = replacementBytes[i];
                        }
                    }

                    File.WriteAllBytes(txtEditor.Text, fileBytes);
                    MessageBox.Show("Starfield.exe is now unpatched", "SUCCESS", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("Starfield is already unpatched", "ERROR", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("You must select your Starfield.exe in the Steam folders", "ERROR", MessageBoxButton.OK);
            }
        }

        static List<int> FindByteSequence(byte[] fileBytes, byte[] sequence)
        {
            List<int> positions = new List<int>();
            for (int i = 0; i < fileBytes.Length - sequence.Length + 1; i++)
            {
                bool match = true;
                for (int j = 0; j < sequence.Length; j++)
                {
                    if (fileBytes[i + j] != sequence[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    positions.Add(i);
                }
            }
            return positions;
        }
    }
}
