using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Media;
using System.Net.Http.Headers;

namespace AudioSteganographyConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("\nEnter wav file path: ");
            string sourcePath = Console.ReadLine();


            String ReadWavFile1 = ReadWavFile(sourcePath);
            //Console.WriteLine("Path: " + ReadWavFile1);
            Console.Write("Would you like to hide the information ? ");
            string answer;
            answer = Console.ReadLine();
            if (answer.ToLower()=="yes")
            {
                HideInfo(sourcePath);
            }
            else
            {
                Console.WriteLine("You didn't want to hide the information in the wav file");
            }
            Console.Write("Would you like to know the information ? ");
            string ans;
            ans = Console.ReadLine();
            String p;
            p = HideInfo(sourcePath);
        }

        private static System.Byte[] bufferInternal_uint8 = null;
        private static System.Int16[] bufferInternal_int16 = null;
        private static byte[] data = null;
        private static System.UInt32 _numberOfSamples;
        public static System.UInt32 NumberOfSamples
        {
            get { return _numberOfSamples; }
            set { _numberOfSamples = value; }
        }

        
        private static byte[] chunk_id = new Byte[4];       
        private static System.UInt32 chunk_size;
        private static byte[] format = new Byte[4];         
        private static byte[] fmtchunk_id = new Byte[4];        
        private static System.UInt32 fmtchunk_size;
        private static System.UInt16 audio_format;
        private static System.UInt16 num_channels;
        private static System.UInt32 sample_rate;
        private static System.UInt32 byte_rate;
        private static System.UInt16 block_align;
        private static System.UInt16 bps;                   
        private static byte[] datachunk_id = new Byte[4];    
        private static System.UInt32 datachunk_size;
        private static String globalFilePath = "";
        static byte[] byteText = Encoding.UTF8.GetBytes("FIEK");
        const int C_HEADER_BYTE_SIZE = 44;


        public enum NUM_CHANNELS
        {
            NOT_DEFINED = 0,
            ONE = 1,
            TWO = 2
        };
        public enum BITS_PER_SAMPLE
        {
            NOT_DEFINED = 0,
            BPS_8_BITS = 8,
            BPS_16_BITS = 16
        };


        static string ReadWavFile(String _Path)
        {
            string file_path = _Path;
            globalFilePath = _Path;
            if (!File.Exists(file_path))
            {
                throw new ApplicationException("ERROR file " + file_path + " doesn't exists. ");
            }
            if ((new FileInfo(file_path)).Length < C_HEADER_BYTE_SIZE)
            {
                throw new ApplicationException("ERROR: File " + file_path + " is smalller then the header of a WAV file");
            }
            using (BinaryReader reader = new BinaryReader(File.Open(file_path, FileMode.Open)))
            {
                // Read WAV file header fields.
                chunk_id = reader.ReadBytes(4);   
                chunk_size = reader.ReadUInt32();
                format = reader.ReadBytes(4);   
                fmtchunk_id = reader.ReadBytes(4);   
                fmtchunk_size = reader.ReadUInt32();
                audio_format = reader.ReadUInt16();
                num_channels = reader.ReadUInt16();
                sample_rate = reader.ReadUInt32();
                byte_rate = reader.ReadUInt32();
                block_align = reader.ReadUInt16();
                bps = reader.ReadUInt16();   
                datachunk_id = reader.ReadBytes(4);   
                datachunk_size = reader.ReadUInt32();

                // File type validations.
                if (System.Text.Encoding.ASCII.GetString(chunk_id) != "RIFF"
                    || System.Text.Encoding.ASCII.GetString(format) != "WAVE")
                {
                    throw new ApplicationException("ERROR: File " + file_path + " is not a WAV file");
                }
                if (audio_format != 1)
                {
                    throw new ApplicationException("ERROR: File " + file_path + " the API only supports PCM format in WAV.");
                }

                switch ((BITS_PER_SAMPLE)bps)
                {
                    case BITS_PER_SAMPLE.BPS_8_BITS:
                        bufferInternal_uint8 = reader.ReadBytes((int)datachunk_size);
                        _numberOfSamples = datachunk_size / num_channels;
                        break;

                    case BITS_PER_SAMPLE.BPS_16_BITS:
                        // Note: To make the following convertion from byte[] to int16[] I could make unsafe code
                        //       and a simple cast but i'm trying to not make unsafe code.
                        int num_int16 = (int)(datachunk_size / sizeof(System.Int16));
                        bufferInternal_int16 = new System.Int16[num_int16];
                        byte[] two_byte_buf_to_int16;
                        for (int i = 0; i < num_int16; i++)
                        {
                            two_byte_buf_to_int16 = reader.ReadBytes(2);
                            bufferInternal_int16[i] = BitConverter.ToInt16(two_byte_buf_to_int16, 0);
                        }
                        _numberOfSamples = (datachunk_size / 2) / num_channels;
                        break;

                    default:
                        throw new ApplicationException("ERROR: Incorret bits per sample in file " + file_path);
                }
            }
            return file_path;
        }
        static string HideInfo(String path)
        {
            var filePath = "";
            var pathAsArray = globalFilePath.Split('\\');
            var index = 0;

            foreach (var pathItem in pathAsArray)
            {
                if (index < pathAsArray.Length - 1)
                {
                    filePath += pathItem + "\\";
                }
                else if (index == pathAsArray.Length - 1)
                {
                    filePath += "\\SteganoCon.wav";
                }
                index++;
            }

            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                writer.Write(chunk_id);
                writer.Write(chunk_size);
                writer.Write(format);
                writer.Write(fmtchunk_id);
                writer.Write(fmtchunk_size);
                writer.Write(audio_format);
                writer.Write(num_channels);
                writer.Write(sample_rate);
                writer.Write(byte_rate);
                writer.Write(block_align);
                writer.Write(bps);
                writer.Write(datachunk_id);
                writer.Write(datachunk_size);

                switch ((BITS_PER_SAMPLE)bps)
                {
                    case BITS_PER_SAMPLE.BPS_8_BITS:
                        if (bufferInternal_uint8 == null)
                        {
                            throw new ApplicationException("ERROR: Data buffer uint8 is NULL!");
                        }
                        // Write WAV data buffer.
                        writer.Write(bufferInternal_uint8);
                        break;

                    case BITS_PER_SAMPLE.BPS_16_BITS:
                        if (bufferInternal_int16 == null)
                        {
                            throw new ApplicationException("ERROR: Data buffer int16 is NULL!");
                        }

                        int num_int16 = (int)(datachunk_size / sizeof(System.Int16));
                        byte[] two_bytes_buf_from_int16;
                        for (int i = 0; i < num_int16; i++)
                        {
                            double byteFromText = 0.0;
                            if (i < byteText.Length)
                            {
                                byteFromText = (double)((int)byteText[i]) / 1000.00;
                                var hiddenValue = bufferInternal_int16[i] + byteFromText;
                                two_bytes_buf_from_int16 = BitConverter.GetBytes(hiddenValue);
                            }
                            else
                            {
                                two_bytes_buf_from_int16 = BitConverter.GetBytes(bufferInternal_int16[i]);
                            }
                            writer.Write(two_bytes_buf_from_int16);
                        }
                        break;

                    default:
                        throw new ApplicationException("ERROR: Incorret bits per sample to write file " + filePath);
                }
            }
            return filePath;
        }

    }
    
}
