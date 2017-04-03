using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace censusgov
{
	/// <summary>
	/// Detailed Language Spoken
	/// </summary>
	public class languages
	{
		/// <summary>
		/// Geography Name
		/// </summary>
		public string NAME { get; set; }
		/// <summary>
		/// Total number of people who speak the language
		/// </summary>
		public string EST { get; set; }
		/// <summary>
		/// Language families in 7 major categories
		/// </summary>
		public string LAN7 { get; set; }
		/// <summary>
		/// Language category label
		/// </summary>
		public string LANLABEL { get; set; }
		/// <summary>
		/// state code
		/// </summary>
		public string state { get; set; }
		/// <summary>
		/// county code (inside state)
		/// </summary>
		public string county { get; set; }
		/// <summary>
		/// Create object from string array
		/// </summary>
		/// <param name="jsa">string array</param>
		public languages(string[] jsa)
		{
			try { NAME = jsa[0]; }
			catch { NAME = ""; }
			try { EST = jsa[1]; }
			catch { EST = ""; }
			try { LAN7 = jsa[2]; }
			catch { LAN7 = ""; }
			try { LANLABEL = jsa[3]; }
			catch { LANLABEL = ""; }
			try { state = jsa[4]; }
			catch { state = ""; }
			try { county = jsa[5]; }
			catch { county = ""; }
		}
		/// <summary>
		/// Create string array from object
		/// </summary>
		public string[] ptoa
		{
			get
			{
				return new string[] { LANLABEL, EST };
			}
		}
	}
	/// <summary>
	/// class helper
	/// </summary>
	public partial class helper
	{
		/// <summary>
		/// Create list from json content
		/// </summary>
		/// <param name="jsc"></param>
		/// <returns></returns>
		public static List<languages> getlanglistfromjson(string jsc)
		{
			if (jsc == "")
				return null;
			List<languages> langs = new List<languages>();
			var jss = new JavaScriptSerializer();
			var slist = jss.Deserialize<string[][]>(jsc);
			if (slist.Length > 0)
			{
				for (int i = 1; i < slist.Length; i++)
					langs.Add(new languages(slist[i]));
			}
			return langs;
		}
		/// <summary>
		/// Create single object from json content
		/// </summary>
		/// <param name="jsc"></param>
		public static languages getlangfromjson(string jsc)
		{
			if (jsc == "")
				return null;
			languages langs = null;
			var jss = new JavaScriptSerializer();
			var slist = jss.Deserialize<string[][]>(jsc);
			if (slist.Length > 0)
			{
				langs = new languages(slist[1]);
			}
			return langs;
		}
	}
}