using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DD_DVR.Converter
{
    class VideoConverter : IDisposable
    {
        public VideoConverter(string inPath, string outPath)
        {
            this.inPath = inPath;
            this.outPath = outPath;
            m_ffmpegProcess = new Process();
            m_ffmpegProcess.StartInfo.FileName = Directory.GetCurrentDirectory() + "\\lib\\ffmpeg.exe"; // путь до ffmpeg.exe
            m_ffmpegProcess.StartInfo.UseShellExecute = false;
            m_ffmpegProcess.StartInfo.RedirectStandardError = true;
            m_ffmpegProcess.StartInfo.RedirectStandardOutput = true;
            m_ffmpegProcess.StartInfo.CreateNoWindow = true; // скрываем процесс в панели
        }

        private string inPath, outPath;
        private Process m_ffmpegProcess = null;
        private StreamReader m_streamReader = null;
        private CultureInfo m_culture = new CultureInfo(0x0409);

       

        public async void StartConvertAsync()
        {
            await Task.Run(() =>
            {
                if (!Directory.Exists(inPath)) throw new DirectoryNotFoundException(inPath);

                var fileArray = Directory.GetFiles(inPath, "*.*264", SearchOption.TopDirectoryOnly);

                if (fileArray.Count() == 0) throw new FileNotFoundException("0 файлов '264' в папке - "+inPath);

                for (int i = 0; i < fileArray.Length; i++)
                {
                    fileArray[i] = Path.GetFileName(fileArray[i]);
                }

                for (int i = 0; i < fileArray.Length; i++)
                {
                    if(Converting(fileArray[i]))
                    {
                         // Событие один сконвертировался
                    }
                    else
                    {
                        if(Converting(fileArray[i], true))
                        {
                            // Событие один сконвертировался с востановлением
                        }
                        else
                        {
                            // Событие один не востановился
                        }
                    }
                }

                // Событие конец конвертации

            });
        }

        private bool Converting(string fileName, bool repair = false)
        {
            if (repair)
            {
                m_ffmpegProcess.StartInfo.Arguments = @" -r 12 -y -i " + inPath + "\\" + fileName
                + @" -vcodec mpeg4 -acodec copy " + outPath + "\\" + fileName + ".mkv"; // параметры конвертации  -preset ultrafast
            }
            else
            {
                m_ffmpegProcess.StartInfo.Arguments = @" -r 12 -y -i " + inPath + "\\" + fileName
                + @" -vcodec copy -acodec copy " + outPath + "\\" + fileName + ".mkv"; // параметры конвертации
            }

            try
            {
                m_ffmpegProcess.Start();
                m_streamReader = m_ffmpegProcess.StandardError;

                string outString = null;
                bool ret = false;
                while (!m_streamReader.EndOfStream)
                {
                    outString = m_streamReader.ReadLine();

                    if (outString.Contains("Conversion failed!")) return false;

                    if (outString.Contains("headers:0kB"))
                    {
                        ret = true;
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                m_ffmpegProcess.WaitForExit();
                m_ffmpegProcess.Close();
            }

        }

        public void Dispose()
        {
            m_ffmpegProcess.Dispose();
        }
    }
}
