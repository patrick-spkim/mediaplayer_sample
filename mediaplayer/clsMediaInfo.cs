using System.Runtime.InteropServices;

namespace mediaplayer
{
    public class MediaInfo
    {
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_New();

        [DllImport("MediaInfo.DLL")]
        internal static extern IntPtr MediaInfo_Open(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string FileName);

        [DllImport("MediaInfo.DLL")]
        private static extern IntPtr MediaInfo_Option(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string option, [MarshalAs(UnmanagedType.LPWStr)] string Value);

        [DllImport("MediaInfo.DLL")]
        private static extern IntPtr MediaInfo_Inform(IntPtr Handle, UIntPtr Reserved);

        [DllImport("MediaInfo.DLL")]
        private static extern void MediaInfo_Close(IntPtr Handle);

        [DllImport("MediaInfo.DLL")]
        private static extern void MediaInfo_Delete(IntPtr Handle);

        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_Option(IntPtr Handle, IntPtr Option, IntPtr Value);

        IntPtr Handle;
        public MediaInfo()
        {
            Handle = MediaInfo_New();
        }
        public System.IntPtr Open(string FileName)
        {
            return MediaInfo_Open(Handle, FileName);
        }
        public string Option(string option, string Value = "")
        {
            return Marshal.PtrToStringUni(MediaInfo_Option(Handle, option, Value));
        }
        public string Inform()
        {
            return Marshal.PtrToStringUni(MediaInfo_Inform(Handle, (UIntPtr)0));
        }
        public void Close()
        {
            MediaInfo_Close(Handle);
        }
        public void delete_pointeur()
        {
            MediaInfo_Delete(Handle);
        }

    }
}
