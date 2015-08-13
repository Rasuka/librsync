using System.Runtime.InteropServices;

namespace LibRSync
{
	public class RDiff
	{
		[DllImport("rdiff.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern RSyncResult rdiff_sig(string baseFile, string sigFile);

		[DllImport("rdiff.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern RSyncResult rdiff_delta(string sigFile, string newFile, string deltaFile);

		[DllImport("rdiff.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern RSyncResult rdiff_patch(string baseFile, string deltaFile, string newFile);

		private enum RSyncResult
		{
			RS_DONE = 0, //Completed successfully.
			RS_BLOCKED = 1, //Blocked waiting for more data.
			RS_RUNNING = 2, //Not yet finished or blocked. This value should never be returned to the caller. 
			RS_TEST_SKIPPED = 77, //Test neither passed or failed.
			RS_IO_ERROR = 100, //Error in file or network IO.
			RS_SYNTAX_ERROR = 101, //Command line syntax error.
			RS_MEM_ERROR = 102, //Out of memory.
			RS_INPUT_ENDED = 103, //End of input file, possibly unexpected.
			RS_BAD_MAGIC = 104, //Bad magic number at start of stream.  Probably not a librsync file, or possibly the wrong kind of file or from an incompatible library version.
			RS_UNIMPLEMENTED = 105, //Author is lazy.
			RS_CORRUPT = 106, //Unbelievable value in stream.
			RS_INTERNAL_ERROR = 107, //Probably a library bug.
			RS_PARAM_ERROR = 108 //Bad value passed in to library, probably an application bug.
		}

		public static void Signature(string oldFile, string signatureFile)
		{
			RSyncResult result = rdiff_sig(oldFile, signatureFile);
			switch (result)
			{
				case RSyncResult.RS_DONE:
					break;

				case RSyncResult.RS_IO_ERROR:
					throw new System.IO.IOException("Failed to calculate signature of the file due to an I/O error: " + oldFile + ", signature: " + signatureFile);

				default:
					throw new System.Exception("Failed to calculate signature of the file (" + result + "): " + oldFile + ", signature: " + signatureFile);
			}
		}

		public static void Delta(string signatureFile, string newFile, string deltaFile)
		{
			RSyncResult result = rdiff_delta(signatureFile, newFile, deltaFile);
			switch (result)
			{
				case RSyncResult.RS_DONE:
					break;

				case RSyncResult.RS_IO_ERROR:
					throw new System.IO.IOException("Failed to calculate delta of the file due to an I/O error: " + signatureFile + ", new: " + newFile + ", delta: " + deltaFile);

				default:
					throw new System.Exception("Failed to calculate delta of the file (" + result + "): " + signatureFile + ", new:" + newFile + " delta: " + deltaFile);
			}
		}

		public static void Patch(string oldFile, string deltaFile, string output)
		{
			RSyncResult result = rdiff_patch(oldFile, deltaFile, output);
			switch (result)
			{
				case RSyncResult.RS_DONE:
					break;

				case RSyncResult.RS_IO_ERROR:
					throw new System.IO.IOException("Failed to patch file due to an I/O error: " + oldFile + " + " + deltaFile + " => " + output);

				default:
					throw new System.Exception("Failed to patch file (" + result + "): " + oldFile + " + " + deltaFile + " => " + output);
			}
		}
	}
}
