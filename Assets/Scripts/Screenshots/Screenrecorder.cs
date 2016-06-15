using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;


public static class Screenrecorder {

	public static Process process;
	public static BinaryWriter writer;


	public static void init() {
		process = new Process ();


		var absoluteFfmpegExeLoc = Application.dataPath + "/Plugins/other/ffmpeg/bin/ffmpeg.exe";


		var relativeScreenshotFileGenericLoc = "pipe:.png";// "Screenshots\\screenshot%d.png";
		var relativeOutFileLoc = "out.mp4";
		var ffmpegCommand = "-f image2pipe -i " + relativeScreenshotFileGenericLoc + " -vf scale=trunc(iw/2)*2:trunc(ih/2)*2 -r 25 -c:v libx264 -pix_fmt yuv420p -crf 18 " + relativeOutFileLoc; // keep same resolution code from http://stackoverflow.com/a/20848224

		process.StartInfo.UseShellExecute = false;
		process.StartInfo.RedirectStandardInput = true;
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.FileName = absoluteFfmpegExeLoc;
		//process.StartInfo.Arguments = ffmpegCommand;

	

		process.Start ();
		writer = new BinaryWriter(process.StandardInput.BaseStream);


		//process.StandardInput.WriteLine ("ffmpeg");
		//process.WaitForExit ();
	}

	public static void writeImg(byte[] img) {
		writer.Write (img);
		//process.StandardInput.BaseStream.Write (img, 0, 0);
		//System.Drawing.Image img;
		//img.Save(writer.BaseStream, System.Drawing.Imaging.ImageFormat.Bmp);
	}

	public static void close() {
		process.StandardInput.Close ();
	}

}
