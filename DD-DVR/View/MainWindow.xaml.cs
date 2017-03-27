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

            double speed = 2;

            MediaPlayer player1 = new MediaPlayer() { SpeedRatio = speed };
            player1.Open(new Uri(@"C:\testVide1\201-01-140854-142354-00c000.h264.mkv", UriKind.RelativeOrAbsolute));
            v1.Fill = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = player1 });
            player1.Play();
            

            MediaPlayer player2 = new MediaPlayer() { SpeedRatio = speed };
            player2.Open(new Uri(@"C:\testVide1\201-03-140856-142354-00c000.h264.mkv", UriKind.RelativeOrAbsolute));
            v2.Fill = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = player2 });
            player2.Play();

            MediaPlayer player3 = new MediaPlayer() { SpeedRatio = speed };
            player3.Open(new Uri(@"C:\testVide1\201-04-140856-142356-00c000.h264.mkv", UriKind.RelativeOrAbsolute));
            v3.Fill = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = player3 });
            player3.Play();

            MediaPlayer player4 = new MediaPlayer() { SpeedRatio = speed };
            player4.Open(new Uri(@"C:\testVide1\201-04-142356-143855-00c000.h264.mkv", UriKind.RelativeOrAbsolute));
            v4.Fill = new DrawingBrush(new VideoDrawing() { Rect = new Rect(0, 0, 300, 200), Player = player4 });
            player4.Play();
            
        }
    }
}
