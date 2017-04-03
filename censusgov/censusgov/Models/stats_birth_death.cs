using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace censusgov
{
	/// <summary>
	/// Estimated Components of Resident Population Change,
	/// and Rates of the Components of Resident Population Change
	/// </summary>
	public class stats_birth_death
	{
		/// <summary>
		/// Geography Name
		/// </summary>
		public string GEONAME { get; set; }
		/// <summary>
		/// Births in period
		/// </summary>
		public string BIRTHS { get; set; }
		/// <summary>
		/// Deaths in period
		/// </summary>
		public string DEATHS { get; set; }
		/// <summary>
		/// Natural increase in period
		/// </summary>
		public string NATURALINC { get; set; }
		/// <summary>
		/// Birth rate in period
		/// </summary>
		public string RBIRTH { get; set; }
		/// <summary>
		/// Death rate in period
		/// </summary>
		public string RDEATH { get; set; }
		/// <summary>
		/// Description of PERIOD
		/// </summary>
		public string PERIOD_DESC { get; set; }
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
		public stats_birth_death(string[] jsa)
		{
			try { GEONAME = jsa[0]; }
			catch { GEONAME = ""; }
			try { BIRTHS = jsa[1]; }
			catch { BIRTHS = ""; }
			try { DEATHS = jsa[2]; }
			catch { DEATHS = ""; }
			try { NATURALINC = jsa[3]; }
			catch { NATURALINC = ""; }
			try { RBIRTH = jsa[4]; }
			catch { RBIRTH = ""; }
			try { RDEATH = jsa[5]; }
			catch { RDEATH = ""; }
			try { PERIOD_DESC = jsa[6]; }
			catch { PERIOD_DESC = ""; }
			try { state = jsa[7]; }
			catch { state = ""; }
			try { county = jsa[8]; }
			catch { county = ""; }
		}
		/// <summary>
		/// Create string array from object
		/// </summary>
		public string[] ptoa
		{
			get
			{
				return new string[] {BIRTHS, DEATHS, NATURALINC, RBIRTH, RDEATH, PERIOD_DESC };
			}
		}
	}
	public partial class helper
	{
		/// <summary>
		/// Create list from json content
		/// </summary>
		/// <param name="jsc"></param>
		/// <returns></returns>
		public static List<stats_birth_death> getstatbdlistfromjson(string jsc)
		{
			if (jsc == "")
				return null;
			List<stats_birth_death> statsbd = new List<stats_birth_death>();
			var jss = new JavaScriptSerializer();
			var slist = jss.Deserialize<string[][]>(jsc);
			if (slist.Length > 0)
			{
				for (int i = 1; i < slist.Length; i++)
					statsbd.Add(new stats_birth_death(slist[i]));
			}
			return statsbd;
		}
		/// <summary>
		/// Create single object from json content
		/// </summary>
		/// <param name="jsc"></param>
		public static stats_birth_death getstatbdfromjson(string jsc)
		{
			if (jsc == "")
				return null;
			stats_birth_death statsbd = null;
			var jss = new JavaScriptSerializer();
			var slist = jss.Deserialize<string[][]>(jsc);
			if (slist.Length > 0)
			{
				statsbd = new stats_birth_death(slist[1]);
			}
			return statsbd;
		}
	}
}
