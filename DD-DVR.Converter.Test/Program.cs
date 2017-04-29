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
            VideoConverter vc = new VideoConverter(@"D:\video", @"C:\video");

            vc.ConvertingStarted += (s,e) => Console.WriteLine(DateTime.Now + " ConvertingStarted");
            vc.OneFileConvertingComplete += (s, e) => Console.WriteLine(DateTime.Now + " OneFileConvertingComplete");
            vc.OneFileConvertingFiled += (s, e) => Console.WriteLine(DateTime.Now + " OneFileConvertingFiled");
            vc.ConvertingComplete += (s, e) => Console.WriteLine(DateTime.Now + " ConvertingComplete");

            Console.WriteLine(DateTime.Now + " StartConvertAsync");
            vc.StartConvertAsync();
            Console.WriteLine(DateTime.Now + " StartConvertAsync");
            Console.ReadKey();
        }

       
    }
}
