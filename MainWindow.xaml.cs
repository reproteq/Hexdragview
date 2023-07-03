using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Hexdragview
{
    public partial class MainWindow : Window
    {
        private string filePath;
        public ObservableCollection<ByteRow> Bytes { get; set; }
        public string TextBoxBytes { get; set; }

        ///
 

        public MainWindow()
        {
            InitializeComponent();
            Bytes = new ObservableCollection<ByteRow>();
            DataContext = this;
        }

        private void Archivo_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length > 0)
                {
                    filePath = files[0];
                    CargarBytesArchivo();
                }
            }
        }

        private void Archivo_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void CargarArchivo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All|*.*|Bin|*.bin|Hex|*.hex|Dec|*.dec|E2prom|*.e2p|Eeprom|*.eep|Flash|*.fls|Micro|*mpc|Ori|*.ori|Mod|*.mod";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                CargarBytesArchivo();
            }
        }

        private void CargarBytesArchivo()
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(filePath);
                Bytes.Clear();

                for (int i = 0; i < bytes.Length; i += 16)
                {
                    ByteRow byteRow = new ByteRow();
                    byteRow.RowIndex = i / 16;
                    byteRow.Data = new byte[16];
                    for (int j = 0; j < 16; j++)
                    {
                        if (i + j < bytes.Length)
                            byteRow.Data[j] = bytes[i + j];
                    }
                    Bytes.Add(byteRow);
                }

                StringBuilder hexString = new StringBuilder();
                int segmentLength = 16;
                foreach (ByteRow row in Bytes)
                {
                    for (int i = 0; i < row.Data.Length; i++)
                    {
                        hexString.AppendFormat("{0:X2} ", row.Data[i]);
                    }
                    hexString.AppendLine();
                }

                TextBoxBytes = hexString.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el archivo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }






    }// end main

    public class ByteRow
    {
        public int RowIndex { get; set; }
        public byte[] Data { get; set; }

        public string D0 { get { return GetByteHex(0); } }
        public string D1 { get { return GetByteHex(1); } }
        public string D2 { get { return GetByteHex(2); } }
        public string D3 { get { return GetByteHex(3); } }
        public string D4 { get { return GetByteHex(4); } }
        public string D5 { get { return GetByteHex(5); } }
        public string D6 { get { return GetByteHex(6); } }
        public string D7 { get { return GetByteHex(7); } }
        public string D8 { get { return GetByteHex(8); } }
        public string D9 { get { return GetByteHex(9); } }
        public string DA { get { return GetByteHex(10); } }
        public string DB { get { return GetByteHex(11); } }
        public string DC { get { return GetByteHex(12); } }
        public string DD { get { return GetByteHex(13); } }
        public string DE { get { return GetByteHex(14); } }
        public string DF { get { return GetByteHex(15); } }

        private string GetByteHex(int index)
        {
            if (index < Data.Length)
                return Data[index].ToString("X2");
            else
                return "00";
        }
    }












}
