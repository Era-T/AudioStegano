using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace Steganography_Wav
{
    public partial class Form1 : Form
    {
        SoundPlayer player;

        private Byte[] bufferInternal_uint8 = null;
        private Int16[] bufferInternal_int16 = null;
        private byte[] data = null;

        private UInt32 _numberOfSamples;
        private byte[] chunk_id = new Byte[4];
        private UInt32 chunk_size;
        private byte[] format = new Byte[4];
        private byte[] fmtchunk_id = new Byte[4];
        private UInt32 fmtchunk_size;
        private UInt16 audio_format;
        private UInt16 num_channels;
        private UInt32 sample_rate;
        private UInt32 byte_rate;
        private UInt16 block_align;
        private UInt16 bps;
        private byte[] datachunk_id = new Byte[4];
        private UInt32 datachunk_size;

        private String globalFilePath = "";

        byte[] byteText = Encoding.UTF8.GetBytes("FIEK");

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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnHide.Enabled = false;
            btnStegano.Enabled = false;
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog.FileName;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            player.Stop();
            btnPlay.Enabled = true;
            btnStop.Enabled = false;
        }


        private void btnPlay_Click(object sender, EventArgs e)
        {

            try
            {
                player = new SoundPlayer();

                player.SoundLocation = textBox1.Text;
                globalFilePath = textBox1.Text;
                var filePath = textBox1.Text;

                using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
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


                    if (System.Text.Encoding.ASCII.GetString(chunk_id) != "RIFF" || System.Text.Encoding.ASCII.GetString(format) != "WAVE")
                    {
                        throw new ApplicationException("ERROR: File " + filePath + " is not a WAV file");
                    }
                    if (audio_format != 1)
                    {
                        throw new ApplicationException("ERROR: File " + filePath + " the API only supports PCM format in WAV.");
                    }


                    switch ((BITS_PER_SAMPLE)bps)
                    {

                        case BITS_PER_SAMPLE.BPS_8_BITS:

                            bufferInternal_uint8 = reader.ReadBytes((int)datachunk_size);
                            data = bufferInternal_uint8;
                            
                            break;


                        case BITS_PER_SAMPLE.BPS_16_BITS:

                            int num_int16 = (int)(datachunk_size / sizeof(System.Int16));

                            bufferInternal_int16 = new System.Int16[num_int16];
                            byte[] two_byte_buf_to_int16;

                            for (int i = 0; i < num_int16; i++)
                            {
                                two_byte_buf_to_int16 = reader.ReadBytes(2);
                                bufferInternal_int16[i] = BitConverter.ToInt16(two_byte_buf_to_int16, 0);
                            }

                            break;

                        default:
                            throw new ApplicationException("ERROR: Incorret bits per sample in file " + filePath);
                    }
                }


                player.Load();
                player.Play();
                btnPlay.Enabled = false;
                btnStop.Enabled = true;
                btnStegano.Enabled = true;
                btnHide.Enabled = true;
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

        private void btnStegano_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                player = new SoundPlayer();

                player.SoundLocation = textBox1.Text;
                var filePath = textBox1.Text;


                using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {

                    var new_chunk_id = reader.ReadBytes(4);
                    var new_chunk_size = reader.ReadUInt32();
                    var new_format = reader.ReadBytes(4);
                    var new_fmtchunk_id = reader.ReadBytes(4);
                    var new_fmtchunk_size = reader.ReadUInt32();
                    var new_audio_format = reader.ReadUInt16();
                    var new_num_channels = reader.ReadUInt16();
                    var new_sample_rate = reader.ReadUInt32();
                    var new_byte_rate = reader.ReadUInt32();
                    var new_block_align = reader.ReadUInt16();
                    var new_bps = reader.ReadUInt16();
                    var new_datachunk_id = reader.ReadBytes(4);
                    var new_datachunk_size = reader.ReadUInt32();


                    if (System.Text.Encoding.ASCII.GetString(chunk_id) != "RIFF" || System.Text.Encoding.ASCII.GetString(format) != "WAVE")
                    {
                        throw new ApplicationException("ERROR: File " + filePath + " is not a WAV file");
                    }
                    if (audio_format != 1)
                    {
                        throw new ApplicationException("ERROR: File " + filePath + " the API only supports PCM format in WAV.");
                    }

                    int num_int16 = (int)(datachunk_size / sizeof(System.Int16));

                    var new_bufferInternal_int16 = new System.Double[num_int16];
                    byte[] two_byte_buf_to_int16;

                    for (int i = 0; i < num_int16; i++)
                    {
                        double byteFromText = 0.0;

                        if (i < byteText.Length)
                        {
                            two_byte_buf_to_int16 = reader.ReadBytes(8);
                            new_bufferInternal_int16[i] = BitConverter.ToDouble(two_byte_buf_to_int16, 0);

                            var hiddenValue = new_bufferInternal_int16[i] - bufferInternal_int16[i];
                            var hiddenValueToByte = Math.Round(hiddenValue, 3) * 1000.00;
                            var word = (char)hiddenValueToByte;

                            sb.Append(word);
                        }
                        else
                        {
                            two_byte_buf_to_int16 = reader.ReadBytes(2);
                            new_bufferInternal_int16[i] = BitConverter.ToInt16(two_byte_buf_to_int16, 0);
                        }
                    }

                    MessageBox.Show("Teksti i lexuar eshte: " + sb.ToString());
                }
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnHide_Click(object sender, EventArgs e)
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
                    filePath += "\\steganoFile.wav";
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
        }
    }
}
    

