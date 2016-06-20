using UnityEngine;
using System.IO;
using System.Diagnostics;


public static class Screenrecorder {

	public static Process process;
	public static StreamWriter writer;

	public static bool isActive = false;


	public static void init() {
		isActive = true;
		process = new Process ();

		var absoluteFfmpegExeLoc = Application.dataPath + "/Plugins/other/ffmpeg/bin/ffmpeg.exe";
		var relativeOutFileLoc = "out.mp4"; // TODO a smarter name?
		var ffmpegCommand = "-y -f image2pipe -i - -vf scale=trunc(iw/2)*2:trunc(ih/2)*2 -r 25 -c:v libx264 -pix_fmt yuv420p -crf 18 " + relativeOutFileLoc;

		process.StartInfo.UseShellExecute = false;
		process.StartInfo.RedirectStandardInput = true;
		process.StartInfo.FileName = absoluteFfmpegExeLoc;
		process.StartInfo.Arguments = ffmpegCommand;

		process.Start ();
		writer = process.StandardInput;
		writer.AutoFlush = true;
	}

	public static void writeImg(byte[] img) {
		writer.BaseStream.Write(img, 0, img.Length);
	}

	public static void close() {
		process.Close ();
		isActive = false;
	}

}
