using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD_DVR.Converter;
namespace DD_DVR.Converter.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            VideoConverter vc = new VideoConverter(@"D:\Работа\video2\ОбразцыВидеофайлов\2017-05-01", @"C:\video");

            vc.ConvertingStarted += (s,e) => Console.WriteLine(DateTime.Now + " ConvertingStarted fileCount:"+ e.VideoFileCount);
            vc.OneFileConvertingComplete += (s, e) => Console.WriteLine(DateTime.Now + " OneFileConvertingComplete fileNum:"+e.VideoFileNum);
            vc.OneFileConvertingFiled += (s, e) => Console.WriteLine(DateTime.Now + " OneFileConvertingFiled fileNum:"+e.VideoFileNum+" fileName:"+e.VideoFileName);
            vc.ConvertingComplete += (s, e) => Console.WriteLine(DateTime.Now + " ConvertingComplete");

            Console.WriteLine(DateTime.Now + " StartConvertAsync");
            vc.StartConvertAsync();
            Console.WriteLine(DateTime.Now + " StartConvertAsync");
            Console.ReadKey();
        }

       
    }
}
