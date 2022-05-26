using DirectShowLib;
using System.Runtime.InteropServices;

namespace mediaplayer
{
    public partial class frmPlayer1 : Form
    {
        IGraphBuilder pGraphBuilder = null;
        IMediaControl pMediaControl = null;
        IMediaEvent pMediaEvent = null;

        IVideoWindow pVideoWindow = null;
        bool play = false;
        bool stop = false;

        public frmPlayer1()
        {
            InitializeComponent();

        }

        private void frmPlayer1_Load(object sender, EventArgs e)
        {

        }


        private void btnPlay_Click(object sender, EventArgs e)
        {
            try
            {
                string strRenderFile = @txtFilePath.Text;
                FileInfo fileInfo = new FileInfo(strRenderFile);
                if (!fileInfo.Exists)
                {
                    MessageBox.Show("Not Exists Select File");
                    return;
                }
                stop = false;
                play = true;
                btnStop.Enabled = true;
                MediaInfo();
                pGraphBuilder = (IGraphBuilder)new FilterGraph();
                pMediaControl = (IMediaControl)pGraphBuilder;
                pMediaEvent = (IMediaEvent)pGraphBuilder;
                pVideoWindow = (IVideoWindow)pGraphBuilder;

                //BuildGraph(pGraphBuilder, strRenderFile);
                pMediaControl.RenderFile(strRenderFile);

                this.pVideoWindow.put_Owner(pnScreen.Handle);
                this.pVideoWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipSiblings);
                Rectangle rect = pnScreen.ClientRectangle;
                this.pVideoWindow.SetWindowPosition(0, 0, rect.Right, rect.Bottom);

                int hr = pMediaControl.Run();
                checkHR(hr, "Can't run the graph");
                if (!stop)
                {
                    btnPlay.Enabled = false;
                }
                else
                {
                    btnPlay.Enabled = true;
                }

                while (!stop)
                {
                    System.Threading.Thread.Sleep(500);
                    Console.Write(".");
                    EventCode ev;
                    IntPtr p1, p2;
                    System.Windows.Forms.Application.DoEvents();
                }

            }
            catch (COMException ex)
            {
                Console.WriteLine("COM error: " + ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            if (!stop && play)
            {
                btnStop.Enabled = false;
                btnPlay.Enabled = true;

                stop = true;
                play = false;
                pMediaControl.Stop();
                Marshal.ReleaseComObject(pGraphBuilder);
                pGraphBuilder = null;
                Marshal.ReleaseComObject(pMediaControl);
                pMediaControl = null;
                Marshal.ReleaseComObject(pMediaEvent);
                pMediaEvent = null;
                pnScreen.BackColor = Color.Black;
            }
        }

        static IPin GetPin(IBaseFilter filter, string pinname)
        {
            IEnumPins epins;
            int hr = filter.EnumPins(out epins);
            checkHR(hr, "Can't enumerate pins");
            IntPtr fetched = Marshal.AllocCoTaskMem(4);
            IPin[] pins = new IPin[1];
            while (epins.Next(1, pins, fetched) == 0)
            {
                PinInfo pinfo;
                pins[0].QueryPinInfo(out pinfo);
                bool found = (pinfo.name == pinname);
                DsUtils.FreePinInfo(pinfo);
                if (found)
                    return pins[0];
            }
            checkHR(-1, "Pin not found");
            return null;
        }

        static void checkHR(int hr, string msg)
        {
            if (hr < 0)
            {
                Console.WriteLine(msg);
                DsError.ThrowExceptionForHR(hr);
            }
        }


        #region BuildGraph
        static void BuildGraph(IGraphBuilder pGraph, string srcFile1)
        {
            int hr = 0;

            //graph builder
            ICaptureGraphBuilder2 pBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            hr = pBuilder.SetFiltergraph(pGraph);
            checkHR(hr, "Can't SetFiltergraph");

            Guid CLSID_LAVSplitter = new Guid("{171252A0-8820-4AFE-9DF8-5C92B2D66B04}"); //LAVSplitter.ax
            Guid CLSID_MicrosoftDTVDVDAudioDecoder = new Guid("{E1F1A0B8-BEEE-490D-BA7C-066C40B5E2B9}"); //msmpeg2adec.dll
            Guid CLSID_MicrosoftDTVDVDVideoDecoder = new Guid("{212690FB-83E5-4526-8FD7-74478B7939CD}"); //msmpeg2vdec.dll
            Guid CLSID_AC3Filter = new Guid("{A753A1EC-973E-4718-AF8E-A3F554D45C44}"); //ac3filter64.ax
            Guid CLSID_VideoRenderer = new Guid("{B87BEB7B-8D29-423F-AE4D-6582C10175AC}"); //quartz.dll

            //add File Source (Async.)
            IBaseFilter pFileSourceAsync = (IBaseFilter)new AsyncReader();
            hr = pGraph.AddFilter(pFileSourceAsync, "File Source (Async.)");
            checkHR(hr, "Can't add File Source (Async.) to graph");
            //set source filename
            IFileSourceFilter pFileSourceAsync_src = pFileSourceAsync as IFileSourceFilter;
            if (pFileSourceAsync_src == null)
                checkHR(unchecked((int)0x80004002), "Can't get IFileSourceFilter");
            hr = pFileSourceAsync_src.Load(srcFile1, null);
            checkHR(hr, "Can't load file");

            //add LAV Splitter
            IBaseFilter pLAVSplitter = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_LAVSplitter));
            hr = pGraph.AddFilter(pLAVSplitter, "LAV Splitter");
            checkHR(hr, "Can't add LAV Splitter to graph");

            //connect File Source (Async.) and LAV Splitter
            hr = pGraph.ConnectDirect(GetPin(pFileSourceAsync, "Output"), GetPin(pLAVSplitter, "Input"), null);
            checkHR(hr, "Can't connect File Source (Async.) and LAV Splitter");

            //add Microsoft DTV-DVD Audio Decoder
            IBaseFilter pMicrosoftDTVDVDAudioDecoder = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_MicrosoftDTVDVDAudioDecoder));
            hr = pGraph.AddFilter(pMicrosoftDTVDVDAudioDecoder, "Microsoft DTV-DVD Audio Decoder");
            checkHR(hr, "Can't add Microsoft DTV-DVD Audio Decoder to graph");

            //add Microsoft DTV-DVD Video Decoder
            IBaseFilter pMicrosoftDTVDVDVideoDecoder = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_MicrosoftDTVDVDVideoDecoder));
            hr = pGraph.AddFilter(pMicrosoftDTVDVDVideoDecoder, "Microsoft DTV-DVD Video Decoder");
            checkHR(hr, "Can't add Microsoft DTV-DVD Video Decoder to graph");

            //connect LAV Splitter and Microsoft DTV-DVD Video Decoder
            hr = pGraph.ConnectDirect(GetPin(pLAVSplitter, "Video"), GetPin(pMicrosoftDTVDVDVideoDecoder, "Video Input"), null);
            checkHR(hr, "Can't connect LAV Splitter and Microsoft DTV-DVD Video Decoder");

            //connect LAV Splitter and Microsoft DTV-DVD Audio Decoder
            hr = pGraph.ConnectDirect(GetPin(pLAVSplitter, "Audio"), GetPin(pMicrosoftDTVDVDAudioDecoder, "XForm In"), null);
            checkHR(hr, "Can't connect LAV Splitter and Microsoft DTV-DVD Audio Decoder");

            //add AC3Filter
            IBaseFilter pAC3Filter = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_AC3Filter));
            hr = pGraph.AddFilter(pAC3Filter, "AC3Filter");
            checkHR(hr, "Can't add AC3Filter to graph");

            //add Video Renderer
            IBaseFilter pVideoRenderer = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_VideoRenderer));
            hr = pGraph.AddFilter(pVideoRenderer, "Video Renderer");
            checkHR(hr, "Can't add Video Renderer to graph");

            //connect Microsoft DTV-DVD Audio Decoder and AC3Filter
            hr = pGraph.ConnectDirect(GetPin(pMicrosoftDTVDVDAudioDecoder, "XFrom Out"), GetPin(pAC3Filter, "In"), null);
            checkHR(hr, "Can't connect Microsoft DTV-DVD Audio Decoder and AC3Filter");

            //connect Microsoft DTV-DVD Video Decoder and Video Renderer
            hr = pGraph.ConnectDirect(GetPin(pMicrosoftDTVDVDVideoDecoder, "Video Output 1"), GetPin(pVideoRenderer, "VMR Input0"), null);
            checkHR(hr, "Can't connect Microsoft DTV-DVD Video Decoder and Video Renderer");

            //add Default DirectSound Device
            IBaseFilter pDefaultDirectSoundDevice = (IBaseFilter)new DSoundRender();
            hr = pGraph.AddFilter(pDefaultDirectSoundDevice, "Default DirectSound Device");
            checkHR(hr, "Can't add Default DirectSound Device to graph");

            //connect AC3Filter and Default DirectSound Device
            hr = pGraph.ConnectDirect(GetPin(pAC3Filter, "Out"), GetPin(pDefaultDirectSoundDevice, "Audio Input pin (rendered)"), null);
            checkHR(hr, "Can't connect AC3Filter and Default DirectSound Device");

        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string path = ofd.FileName;
            if (path.Length > 1)
            {
                txtFilePath.Text = ofd.FileName;
                MediaInfo();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MediaInfo();
        }


        private void MediaInfo()
        {
            string Option_Complete;
            string FileName = @txtFilePath.Text;

            FileInfo fileInfo = new FileInfo(FileName);

            if (!fileInfo.Exists)
            {
                MessageBox.Show("Not Exists Select File");
                return;
            }

            string[] msg = new string[50];
            string[] general = new string[9];
            string[] video = new string[23];
            string[] audio = new string[4];

            var tempMediaInfo = new MediaInfo();
            tempMediaInfo.Open(FileName);
            general[0] = "General";
            tempMediaInfo.Option("Inform", "General;%CompleteName%"); general[1] = "- CompleteName : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "General;%Format/String%"); general[2] = "- Format : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "General;%Format_Profile%"); general[3] = "- Format_Profile : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "General;%CodecID/String%"); general[4] = "- CodecID : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "General;%FileSize/String%"); general[5] = "- FileSize : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "General;%Duration/String%"); general[6] = "- Duration : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "General;%OverallBitRate/String%"); general[7] = "- OverallBitRate : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "General;%Encoded_Application/String%"); general[8] = "- Encoded_Application : " + tempMediaInfo.Inform();// 1 h 38 min


            video[0] = "Video";
            tempMediaInfo.Option("Inform", "Video;%Format/String%"); video[1] = "- Format : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%Format/Info%"); video[2] = "- Format/Info : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%Format_Profile%"); video[3] = "- Format_Profile : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%Format_Settings%"); video[4] = "- Format_Settings : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%Format_Settings_RefFrames/String%"); video[5] = "- Format_Settings_RefFrames : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%CodecID%"); video[6] = "- CodecID : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%CodecID/Info%"); video[7] = "- CodecID/Info : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%Duration/String%"); video[8] = "- Duration : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%BitRate/String%"); video[9] = "- BitRate : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%BitRate_Maximum/String%"); video[10] = "- BitRate_Maximum : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%Width/String%"); video[11] = "- Width : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%Height/String%"); video[12] = "- Height : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%DisplayAspectRatio/String%"); video[13] = "- DisplayAspectRatio : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%FrameRate_Mode/String%"); video[14] = "- FrameRate_Mode : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%FrameRate/String%"); video[15] = "- FrameRate : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%ColorSpace%"); video[16] = "- ColorSpace : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%ChromaSubsampling/String%"); video[17] = "- ChromaSubsampling : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%BitDepth/String%"); video[18] = "- BitDepth : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%ScanType/String%"); video[19] = "- ScanType : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%Bits-(Pixel*Frame)%"); video[20] = "- Bits-(Pixel*Frame) : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%StreamSize/String%"); video[21] = "- StreamSize : " + tempMediaInfo.Inform();// 1 h 38 min
            tempMediaInfo.Option("Inform", "Video;%Encoded_Library/String%"); video[22] = "- Encoded_Library : " + tempMediaInfo.Inform();// 1 h 38 min
            //tempMediaInfo.Option("Inform", "Video;%Encoded_Library_Settings%"); video[23] = "- Encoded_Library_Settings : " + tempMediaInfo.Inform();// 1 h 38 min
            //tempMediaInfo.Option("Inform", "Video;%CodecConfigurationBox%"); video[24] = "- CodecConfigurationBox : " + tempMediaInfo.Inform();// 1 h 38 min


            audio[0] = "Audio";
            tempMediaInfo.Option("Inform", "Audio;%SamplingRate/String%"); audio[1] = "- Audio SamplingRate : " + tempMediaInfo.Inform();// 44.1 kHz
            tempMediaInfo.Option("Inform", "Audio;%Language/String%, %Channel(s)% channels, %Codec/String%, %SamplingRate/String%, %BitRate/String%"); audio[2] = "- Audio Language : " + tempMediaInfo.Inform(); // ,2 channels,, 44.1kHz, 160 kb/s
            tempMediaInfo.Option("Inform", "Audio;%BitDepth/String%"); audio[3] = "- Audio BitDepth : " + tempMediaInfo.Inform();// 

            Option_Complete = tempMediaInfo.Option("Complete", "1");                         // end of session
            string result = tempMediaInfo.Inform();
            lblMediaInfo.Text = string.Join(Environment.NewLine, general);
            lblMediaInfo.Text += Environment.NewLine + Environment.NewLine;
            lblMediaInfo.Text += string.Join(Environment.NewLine, video);
            lblMediaInfo.Text += Environment.NewLine + Environment.NewLine;
            lblMediaInfo.Text += string.Join(Environment.NewLine, audio);

            tempMediaInfo.Close();
            tempMediaInfo.delete_pointeur();

        }

        private void frmPlayer1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Stop();
        }
    }


}