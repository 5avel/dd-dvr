using DD_DVR.Video;
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


            DVRPlayer dvr = new DVRPlayer(@"D:\TEST\128");
           
            dvr.Play();
            v1.Fill = dvr.Streams[0].VideoBrush;
            v2.Fill = dvr.Streams[1].VideoBrush;
            v3.Fill = dvr.Streams[2].VideoBrush;
            v4.Fill = dvr.Streams[3].VideoBrush;

            //double speed = 2;

            //MediaPlayer player1 = new MediaPlayer() { SpeedRatio = speed };
            //player1.Open(new Uri(@"D:\TEST\3.10\201-01-000000-003000-00c000.h264.mkv", UriKind.RelativeOrAbsolute));
            //v1.Fill = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = player1 });
            //player1.Play();


            //MediaPlayer player2 = new MediaPlayer() { SpeedRatio = speed };
            //player2.Open(new Uri(@"D:\TEST\3.10\201-01-000000-003000-00c000.h264.mkv", UriKind.RelativeOrAbsolute));
            //v2.Fill = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = player2 });
            //player2.Play();

            //MediaPlayer player3 = new MediaPlayer() { SpeedRatio = speed };
            //player3.Open(new Uri(@"D:\TEST\3.10\201-01-000000-003000-00c000.h264.mkv", UriKind.RelativeOrAbsolute));
            //v3.Fill = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = player3 });
            //player3.Play();

            //MediaPlayer player4 = new MediaPlayer() { SpeedRatio = speed };
            //player4.Open(new Uri(@"D:\TEST\3.10\201-01-000000-003000-00c000.h264.mkv", UriKind.RelativeOrAbsolute));
            //v4.Fill = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = player4 });
            //player4.Play();

        }
    }
}
