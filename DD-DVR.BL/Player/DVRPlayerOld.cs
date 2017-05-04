using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.BL.Playr
{
    public class DVRPlayerOld
    {

        // Может исползовать массив стримов, удобней перебирать цыклом и делать однотипные операции!
        public List<StreamOld> Streams { set; get; }

        public string VideoSourcePath { get; set; } // путь к папке с файлами *.mkv

       
        public DVRPlayerOld()
        {
            // проверка папки на сушествование
            Streams = new List<StreamOld>();
           
        }

        public void LoadVideo(string videoFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(videoFolder);

            List<string> videoPath1 = new List<string>();
            foreach (var item in dir.GetFiles(@"*-01-*-*-*.mkv")) videoPath1.Add(item.FullName);
            if (videoPath1.Count > 0) Streams.Add(new StreamOld(videoPath1));

            List<string> videoPath2 = new List<string>();
            foreach (var item in dir.GetFiles(@"*-02-*-*-*.mkv")) videoPath2.Add(item.FullName);
            if (videoPath2.Count > 0) Streams.Add(new StreamOld(videoPath2));

            List<string> videoPath3 = new List<string>();
            foreach (var item in dir.GetFiles(@"*-03-*-*-*.mkv")) videoPath3.Add(item.FullName);
            if (videoPath3.Count > 0) Streams.Add(new StreamOld(videoPath3));

            List<string> videoPath4 = new List<string>();
            foreach (var item in dir.GetFiles(@"*-04-*-*-*.mkv")) videoPath4.Add(item.FullName);
            if (videoPath4.Count > 0) Streams.Add(new StreamOld(videoPath4));


            //foreach (var item in dir.GetFiles(@"channel1_*_*_*_*.mkv")) videoPath1.Add(item.FullName);
            //if (videoPath1.Count > 0) Streams.Add(new Stream(videoPath1));

            //List<string> videoPath2 = new List<string>();
            //foreach (var item in dir.GetFiles(@"channel2_*_*_*_*.mkv")) videoPath2.Add(item.FullName);
            //if (videoPath2.Count > 0) Streams.Add(new Stream(videoPath2));

            //List<string> videoPath3 = new List<string>();
            //foreach (var item in dir.GetFiles(@"channel3_*_*_*_*.mkv")) videoPath3.Add(item.FullName);
            //if (videoPath3.Count > 0) Streams.Add(new Stream(videoPath3));

            //List<string> videoPath4 = new List<string>();
            //foreach (var item in dir.GetFiles(@"channel4_*_*_*_*.mkv")) videoPath4.Add(item.FullName);
            //if (videoPath4.Count > 0) Streams.Add(new Stream(videoPath4));
        }



        public void Play()
        {
            foreach (StreamOld s in Streams) { s.Play(); } 
        }

        public void Stop()
        {
            foreach (StreamOld s in Streams) s.Stop();
        }

        public void Pause()
        {
            foreach (StreamOld s in Streams) s.Pause();
        }

        public void Step(bool isRightStep = true)
        {
            foreach (StreamOld s in Streams) s.Step(isRightStep);
        }

        public void SetSpeedRatio(double curspeedRatio)
        {
            foreach (StreamOld s in Streams) s.player.SpeedRatio = curspeedRatio;
        }
    }
}
