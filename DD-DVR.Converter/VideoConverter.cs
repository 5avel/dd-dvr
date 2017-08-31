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
    public class VideoConverter : IDisposable
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

        public event EventHandler<ConvertingStartedEventArgs> ConvertingStarted = delegate { };
        public event EventHandler<OneFileConvertingCompleteEventArgs> OneFileConvertingComplete = delegate { };
        public event EventHandler<OneFileConvertingFiledEventArgs> OneFileConvertingFiled = delegate { };
        public event EventHandler<EventArgs> ConvertingComplete = delegate { };


        public async void StartConvertAsync()
        {
            await Task.Run(() =>
            {
                if (!Directory.Exists(inPath)) throw new DirectoryNotFoundException(inPath);
                //if (!Directory.Exists(outPath)) throw new DirectoryNotFoundException(outPath);

                var fileArray = Directory.GetFiles(inPath, "*.*264", SearchOption.TopDirectoryOnly);

                if (fileArray.Count() == 0) throw new FileNotFoundException("Нет файлов '264' в папке - "+inPath);

                // Событие начало конвертации
                ConvertingStarted(this, new ConvertingStartedEventArgs(fileArray.Count()));

                for (int i = 0; i < fileArray.Length; i++)
                {
                    fileArray[i] = Path.GetFileName(fileArray[i]);
                }

                for (int i = 0; i < fileArray.Length; i++)
                {
                    if(Converting(fileArray[i]))
                    {
                        // Событие один сконвертировался
                        OneFileConvertingComplete(this, new OneFileConvertingCompleteEventArgs(i));
                    }
                    else
                    {
                        if(Converting(fileArray[i], true))
                        {
                            // Событие один сконвертировался с востановлением
                            OneFileConvertingComplete(this, new OneFileConvertingCompleteEventArgs(i));
                        }
                        else
                        {
                            // Событие один не востановился
                            OneFileConvertingFiled(this, new OneFileConvertingFiledEventArgs(i, fileArray[i]));
                        }
                    }
                }

                // Событие конец конвертации
                ConvertingComplete(this, new EventArgs());

            });
        }

        /// <summary>
        /// Конвертирует один видеофайл из h264 в MKV.
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="repair">True восстановить поврежденный файл, полной конвертацией mpeg4</param>
        /// <returns> true - успешная конвертация</returns>
        private bool Converting(string fileName, bool repair = false)
        {
            if (repair)
            {
                m_ffmpegProcess.StartInfo.Arguments 
                    = $@" -r 12 -y -i ""{inPath}\{fileName}"" -vcodec mpeg4 -acodec copy ""{outPath}\{fileName}.mkv""";
            }
            else
            {
 
                m_ffmpegProcess.StartInfo.Arguments 
                    = $@" -r 12 -y -i ""{inPath}\{fileName}"" -vcodec copy -acodec copy ""{outPath}\{fileName}.mkv""";
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

    public class ConvertingStartedEventArgs : EventArgs
    {
       public int VideoFileCount { set; get; }

        public ConvertingStartedEventArgs(int videoFileCount)
        {
            VideoFileCount = videoFileCount;
        }
    }

    public class OneFileConvertingCompleteEventArgs : EventArgs
    {
        public int VideoFileNum { set; get; }

        public OneFileConvertingCompleteEventArgs(int videoFileNum)
        {
            VideoFileNum = videoFileNum;
        }
    }

    public class OneFileConvertingFiledEventArgs : EventArgs
    {
        public int VideoFileNum { set; get; }
        public int VideoFileName { set; get; }
        public OneFileConvertingFiledEventArgs(int videoFileNum, string videoFileName)
        {
            VideoFileNum = videoFileNum;
            VideoFileName = videoFileNum;
        }
    }

}

// TODO: Порядок конвертации 1,2,3,4 1,2,3,4 итд.