using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Data = Google.Apis.Sheets.v4.Data;
using System.Threading;

namespace runtest
{
	class Program
	{
		static string spreadsheetId = "";
		static string spreadsheetUrl = "";
		/// <summary>
		/// Entry point
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			censusgov.googleapis.creategooglesheettop10(ref spreadsheetId, ref spreadsheetUrl);
			if (spreadsheetId!="")
				Console.WriteLine("SpreadsheetId:" + spreadsheetId);
			if (spreadsheetUrl != "")
				Console.WriteLine("SpreadsheetUrl:" + spreadsheetUrl);
			if (spreadsheetId != "" || spreadsheetUrl != "")
			{
				Console.Write("Press key Enter... ");
				Console.ReadLine();
			}
		}
	}
}
