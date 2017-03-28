using DD_DVR.Video;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.Windows.Media;

namespace DD_DVR
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();


            DVRPlayer dvr = new DVRPlayer(@"C:\testVide1");
           
            dvr.Play();
            v1.Fill = dvr.Streams[0].VideoBrush;
            v2.Fill = dvr.Streams[1].VideoBrush;
            v3.Fill = dvr.Streams[2].VideoBrush;
            //v4.Fill = dvr.Streams[3].VideoBrush;

            
        }
    }
}


//TODO: разбить главное окно на части 1) плеер, 2) подсчет пасажыров, 3) отладочьная информация
