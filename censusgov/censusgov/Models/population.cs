using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace censusgov
{
	/// <summary>
	/// population estimate per county
	/// </summary>
	public class population
	{
		/// <summary>
		/// Geography Name
		/// </summary>
		public string GEONAME { get; set; }
		/// <summary>
		/// Population
		/// </summary>
		public int POP { get; set; }
		/// <summary>
		/// agegroup 0 - all population (state)
		/// </summary>
		public int POP00 { get; set; }
		/// <summary>
		/// agegroup 29 - population (state) with age 18 years and over
		/// </summary>
		public int POP18 { get; set; }
		/// <summary>
		/// Description of DATE value
		/// </summary>
		public string DATE_DESC { get; set; }
		/// <summary>
		/// state code
		/// </summary>
		public string state { get; set; }
		/// <summary>
		/// county code (inside state)
		/// </summary>
		public string county { get; set; }
		/// <summary>
		/// Resident Population Change
		/// </summary>
		public stats_birth_death statsbd { get; set; }
		/// <summary>
		/// Detailed Language Spoken
		/// </summary>
		public List<languages> languages { get; set; }
		/// <summary>
		/// Create object from string array
		/// </summary>
		/// <param name="jsa">string array</param>
		public population(string[] jsa)
		{
			try { GEONAME = jsa[0]; }
			catch { GEONAME = ""; }
			try { POP = int.Parse(jsa[1]); }
			catch { POP = 0; }
			try { DATE_DESC = jsa[2]; }
			catch { DATE_DESC = ""; }
			try { state = jsa[3]; }
			catch { state = ""; }
			try { county = jsa[4]; }
			catch { county = ""; }
		}
		/// <summary>
		/// Create string array from object
		/// </summary>
		public string [] ptoa {
			get
			{
				return new string[] { GEONAME, POP.ToString(), POP00.ToString(), POP18.ToString(), DATE_DESC };
			}
		}
	}
	/// <summary>
	/// population estimate by agegroup per State
	/// agegroup 0 - all population
	/// agegroup 29 - 18 years and over
	/// </summary>
	public class population18
	{
		/// <summary>
		/// Geography Name
		/// </summary>
		public string GEONAME { get; set; }
		/// <summary>
		/// Population
		/// </summary>
		public int POP { get; set; }
		/// <summary>
		/// state code
		/// </summary>
		public string state { get; set; }

		/// <summary>
		/// Create object from string array
		/// </summary>
		/// <param name="jsa">string array</param>
		public population18(string[] jsa)
		{
			try { GEONAME = jsa[0]; }
			catch { GEONAME = ""; }
			try { POP = int.Parse(jsa[1]); }
			catch { POP = 0; }
			try { state = jsa[2]; }
			catch { state = ""; }
		}
	}

	public partial class helper
	{
		/// <summary>
		/// Create list from json content
		/// </summary>
		/// <param name="jsc"></param>
		/// <returns></returns>
		public static List<population> getpoplistfromjson(string jsc)
		{
			if (jsc == "")
				return null;
			List<population> xpop = new List<population>();
			var jss = new JavaScriptSerializer();
			var slist = jss.Deserialize<string[][]>(jsc);
			if (slist.Length > 0)
			{
				for (int i = 1; i < slist.Length; i++)
					xpop.Add(new population(slist[i]));
			}
			return xpop;
		}
		/// <summary>
		/// Create single object from json content
		/// </summary>
		/// <param name="jsc"></param>
		public static population getpopfromjson(string jsc)
		{
			if (jsc == "")
				return null;
			population xpop = null;
			var jss = new JavaScriptSerializer();
			var slist = jss.Deserialize<string[][]>(jsc);
			if (slist.Length > 0)
			{
				xpop = new population(slist[1]);
			}
			return xpop;
		}
		/// <summary>
		/// Create list from json content
		/// </summary>
		/// <param name="jsc"></param>
		/// <returns></returns>
		public static List<population18> getpop18listfromjson(string jsc)
		{
			if (jsc == "")
				return null;
			List<population18> xpop = new List<population18>();
			var jss = new JavaScriptSerializer();
			var slist = jss.Deserialize<string[][]>(jsc);
			if (slist.Length > 0)
			{
				for (int i = 1; i < slist.Length; i++)
					xpop.Add(new population18(slist[i]));
			}
			return xpop;
		}
		/// <summary>
		/// Create single object from json content
		/// </summary>
		/// <param name="jsc"></param>
		public static population18 getpop18fromjson(string jsc)
		{
			if (jsc == "")
				return null;
			population18 xpop = null;
			var jss = new JavaScriptSerializer();
			var slist = jss.Deserialize<string[][]>(jsc);
			if (slist.Length > 0)
			{
				xpop = new population18(slist[1]);
			}
			return xpop;
		}
	}
}
