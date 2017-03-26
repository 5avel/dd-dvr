using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            double speed = 64;

            MediaPlayer player1 = new MediaPlayer() { SpeedRatio = speed };
            player1.Open(new Uri(@"D:\TEST\128\201-03-212606-215606-00c000.h264.mkv", UriKind.RelativeOrAbsolute));
            v1.Fill = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = player1 });
            player1.Play();
            

            MediaPlayer player2 = new MediaPlayer() { SpeedRatio = speed };
            player2.Open(new Uri(@"D:\TEST\128\201-04-182607-185606-00c000.h264.mkv", UriKind.RelativeOrAbsolute));
            v2.Fill = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = player2 });
            player2.Play();

            MediaPlayer player3 = new MediaPlayer() { SpeedRatio = speed };
            player3.Open(new Uri(@"D:\TEST\128\201-03-225606-232606-00c000.h264.mkv", UriKind.RelativeOrAbsolute));
            v3.Fill = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = player3 });
            player3.Play();

            MediaPlayer player4 = new MediaPlayer() { SpeedRatio = speed };
            player4.Open(new Uri(@"D:\TEST\128\201-03-202606-205605-00c000.h264.mkv", UriKind.RelativeOrAbsolute));
            v4.Fill = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = player4 });
            player4.Play();
        }
    }
}
