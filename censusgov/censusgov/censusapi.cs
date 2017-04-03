using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace censusgov
{
	/// <summary>
	/// class censusapi contains a wrappers for
	/// Population Estimates APIs (Census Bureau's Population Estimates Program)
	/// https://www.census.gov/data/developers/data-sets/popest-popproj/popest.html
	/// </summary>
	public class censusapi
	{
		/// <summary>
		/// stub for ignore wrong x.509 certificate
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="certification"></param>
		/// <param name="chain"></param>
		/// <param name="sslPolicyErrors"></param>
		/// <returns></returns>
		private static bool AcceptAllCertifications(object sender,
			System.Security.Cryptography.X509Certificates.X509Certificate certification,
			System.Security.Cryptography.X509Certificates.X509Chain chain,
			System.Net.Security.SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}
		static bool _AcceptAllCertificates = false;

		/// <summary>
		/// stub for ignore wrong x.509 certificate
		/// </summary>
		public static void AcceptAllCertificates()
		{
			if (!_AcceptAllCertificates)
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback =
					new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
			}
			System.Net.ServicePointManager.Expect100Continue = false;
			_AcceptAllCertificates = true;
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public censusapi()
		{
			AcceptAllCertificates();
		}
		/// <summary>
		/// Constructor for replace default api key
		/// </summary>
		/// <param name="apikey"></param>
		public censusapi(string apikey)
		{
			_apikey = apikey;
			AcceptAllCertificates();
		}
		/// <summary>
		/// api key for basic authenticate
		/// </summary>
		string _apikey = "00f0c24d84473bf67acb22ee95fbb7e53dd3492e";
		/// <summary>
		/// Any exception message place here
		/// </summary>
		public string code = "";
		static string _url = "http://api.census.gov/";
		static string forrepl = "{forclause}";
		static string inrepl = "{inclause}", apirepl = "{apikey}";
		static string _defforclause = "&for=county:*";
		static string _definclause = "";
		static string[] point =
		{
			"data/2016/pep/population?get=GEONAME,POP,DATE_DESC{forclause}{inclause}&key={apikey}",
			"data/2016/pep/components?get=GEONAME,BIRTHS,DEATHS,NATURALINC,RBIRTH,RDEATH,PERIOD_DESC{forclause}{inclause}&key={apikey}",
			"data/2013/language?get=NAME,EST,LAN7,LANLABEL{forclause}{inclause}&key={apikey}",
			"data/2016/pep/charagegroups?get=GEONAME,POP&AGEGROUP=29{forclause}&key={apikey}",
			"data/2016/pep/charagegroups?get=GEONAME,POP&AGEGROUP=0{forclause}&key={apikey}",
			""
		};
		string _proxyaddr = "207.99.118.74";
		int _proxyport = 8080;
		/// <summary>
		/// if proxy not used - set this field to false 
		/// </summary>
		public bool useproxy = true;
		/// <summary>
		/// Change default proxy settings:
		/// proxyaddr = "207.99.118.74"
		/// proxyport = 8080
		/// </summary>
		/// <param name="proxyaddr"></param>
		/// <param name="proxyport"></param>
		public void setproxy(string proxyaddr, int proxyport)
		{
			_proxyaddr = proxyaddr;
			_proxyport = proxyport;
			if (_proxyaddr == "")
				useproxy = false;
		}
		/// <summary>
		/// call api request
		/// </summary>
		/// <param name="rqst"> url with parameters to api method</param>
		/// <returns>string content</returns>
		string callapi(string rqst)
		{
			// should be return value
			string retv = "";
			HttpClientHandler handler = new HttpClientHandler()
			{
				Proxy = new WebProxy(_proxyaddr, _proxyport),
				UseProxy = true,
			};
			using (HttpClient httpClient = useproxy ? new HttpClient(handler) : new HttpClient())
			{
				try
				{
					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(
						new MediaTypeWithQualityHeaderValue("application/json"));
					var response = httpClient.GetAsync(rqst);

					//get response
					var StatusText = response.Result.Content.ReadAsStringAsync();

					retv = StatusText.Result.ToString();
				}
				catch (Exception ex)
				{
					code += "|" + ex.ToString();
					while (ex.InnerException != null)
					{
						ex = ex.InnerException;
						code += "|" + ex.ToString();
					}
				}
			}
			return retv;
		}
		/// <summary>
		/// Country population
		/// </summary>
		/// <returns>string content</returns>
		public string getpopulations()
		{
			// point[0]
			// should be return value
			string retv = "";
			string rqparams = point[0].
				Replace(apirepl, _apikey).
				Replace(forrepl, _defforclause).
				Replace(inrepl, _definclause);
			string url = _url + rqparams;
			retv = callapi(url);
			return retv;
		}
		/// <summary>
		/// State population
		/// </summary>
		/// <param name="state">State code</param>
		/// <returns>string content</returns>
		public string getpopulations(string state)
		{
			// point[0]
			// should be return value
			string retv = "";
			string rqparams = point[0].
				Replace(apirepl, _apikey).
				Replace(forrepl, _defforclause).
				Replace(inrepl, "&in=state:" + state);
			string url = _url + rqparams;
			retv = callapi(url);
			return retv;
		}
		/// <summary>
		/// County population
		/// </summary>
		/// <param name="state">State code</param>
		/// <param name="county">County code (inside State)</param>
		/// <returns>string content</returns>
		public string getpopulations(string state, string county)
		{
			// point[0]
			// should be return value
			string retv = "";
			string rqparams = point[0].
				Replace(apirepl, _apikey).
				Replace(forrepl, "&for=county:" + county).
				Replace(inrepl, "&in=state:" + state);
			string url = _url + rqparams;
			retv = callapi(url);
			return retv;
		}
		/// <summary>
		/// Resident Population Change
		/// </summary>
		/// <returns>string content</returns>
		public string getstats_birth_death()
		{
			// point[1]
			// should be return value
			string retv = "";
			string rqparams = point[1].
				Replace(apirepl, _apikey).
				Replace(forrepl, _defforclause).
				Replace(inrepl, _definclause);
			string url = _url + rqparams;
			retv = callapi(url);
			return retv;
		}
		/// <summary>
		/// State Resident Population Change
		/// </summary>
		/// <param name="state">State code</param>
		/// <returns>string content</returns>
		public string getstats_birth_death(string state)
		{
			// point[1]
			// should be return value
			string retv = "";
			string rqparams = point[1].
				Replace(apirepl, _apikey).
				Replace(forrepl, _defforclause).
				Replace(inrepl, "&in=state:" + state);
			string url = _url + rqparams;
			retv = callapi(url);
			return retv;
		}
		/// <summary>
		/// County Resident Population Change
		/// </summary>
		/// <param name="state">State code</param>
		/// <param name="county">County code (inside State)</param>
		/// <returns>string content</returns>
		public string getstats_birth_death(string state, string county)
		{
			// point[1]
			// should be return value
			string retv = "";
			string rqparams = point[1].
				Replace(apirepl, _apikey).
				Replace(forrepl, "&for=county:" + county).
				Replace(inrepl, "&in=state:" + state);
			string url = _url + rqparams;
			retv = callapi(url);
			return retv;
		}
		/// <summary>
		/// Detailed Language Spoke
		/// </summary>
		/// <returns>string content</returns>
		public string getstats_language()
		{
			// point[2]
			// should be return value
			string retv = "";
			string rqparams = point[2].
				Replace(apirepl, _apikey).
				Replace(forrepl, _defforclause).
				Replace(inrepl, _definclause);
			string url = _url + rqparams;
			retv = callapi(url);
			return retv;
		}
		/// <summary>
		/// State Detailed Language Spoke
		/// </summary>
		/// <param name="state">State code</param>
		/// <returns>string content</returns>
		public string getstats_language(string state)
		{
			// point[2]
			// should be return value
			string retv = "";
			string rqparams = point[2].
				Replace(apirepl, _apikey).
				Replace(forrepl, _defforclause).
				Replace(inrepl, "&in=state:" + state);
			string url = _url + rqparams;
			retv = callapi(url);
			return retv;
		}
		/// <summary>
		/// County Detailed Language Spoke
		/// </summary>
		/// <param name="state">State code</param>
		/// <param name="county">County code (inside State)</param>
		/// <returns>string content</returns>
		public string getstats_language(string state, string county)
		{
			// point[2]
			// should be return value
			string retv = "";
			string rqparams = point[2].
				Replace(apirepl, _apikey).
				Replace(forrepl, "&for=county:" + county).
				Replace(inrepl, "&in=state:" + state);
			string url = _url + rqparams;
			retv = callapi(url);
			return retv;
		}
		/// <summary>
		/// State population with age older than 18 years
		/// </summary>
		/// <param name="state">State code</param>
		/// <returns>string content</returns>
		public string getpopulations18(string state)
		{
			// point[3]
			// should be return value
			string retv = "";
			string rqparams = point[3].
				Replace(apirepl, _apikey).
				Replace(forrepl, "&for=state:" + state).
				Replace(inrepl, "");
			string url = _url + rqparams;
			retv = callapi(url);
			return retv;
		}
		/// <summary>
		/// State population
		/// </summary>
		/// <param name="state">State code</param>
		/// <returns>string content</returns>
		public string getpopulations00(string state)
		{
			// point[4]
			// should be return value
			string retv = "";
			string rqparams = point[4].
				Replace(apirepl, _apikey).
				Replace(forrepl, "&for=state:" + state).
				Replace(inrepl, "");
			string url = _url + rqparams;
			retv = callapi(url);
			return retv;
		}
	}
}
