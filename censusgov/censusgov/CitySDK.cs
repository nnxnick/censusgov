using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace autovitals
{
    public class CitySDK
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
		/// Simple constructor
		/// </summary>
		public CitySDK()
		{
			AcceptAllCertificates();
			fillstates();
		}
		/// <summary>
		/// Constructor for replace default api key
		/// </summary>
		/// <param name="apikey"></param>
		public CitySDK(string apikey)
		{
			_apikey = apikey;
			AcceptAllCertificates();
			fillstates();
		}
		/// <summary>
		/// api key for basic authenticate
		/// </summary>
		string _apikey = "00f0c24d84473bf67acb22ee95fbb7e53dd3492e";
		/// <summary>
		/// states contains codes of states
		/// </summary>
		static SortedList<string, string> _states = new SortedList<string, string>();
		public SortedList<string, string> states { get { return _states; } }
		static object tolock = (object)"";
		/// <summary>
		/// Fill collection
		/// </summary>
		void fillstates()
		{
			if (_states.Count <= 0)
			{
				lock(tolock)
				{
					if (_states.Count > 0)
						return;
					_states["01"] = "AL";
					_states["02"] = "AK";
					_states["04"] = "AZ";
					_states["05"] = "AR";
					_states["06"] = "CA";
					_states["08"] = "CO";
					_states["09"] = "CT";
					_states["10"] = "DE";
					_states["12"] = "FL";
					_states["13"] = "GA";
					_states["15"] = "HI";
					_states["16"] = "ID";
					_states["17"] = "IL";
					_states["18"] = "IN";
					_states["19"] = "IA";
					_states["20"] = "KS";
					_states["21"] = "KY";
					_states["22"] = "LA";
					_states["23"] = "ME";
					_states["24"] = "MD";
					_states["25"] = "MA";
					_states["26"] = "MI";
					_states["27"] = "MN";
					_states["28"] = "MS";
					_states["29"] = "MO";
					_states["30"] = "MT";
					_states["31"] = "NE";
					_states["32"] = "NV";
					_states["33"] = "NH";
					_states["34"] = "NJ";
					_states["35"] = "NM";
					_states["36"] = "NY";
					_states["37"] = "NC";
					_states["38"] = "ND";
					_states["39"] = "OH";
					_states["40"] = "OK";
					_states["41"] = "OR";
					_states["42"] = "PA";
					_states["44"] = "RI";
					_states["45"] = "SC";
					_states["46"] = "SD";
					_states["47"] = "TN";
					_states["48"] = "TX";
					_states["49"] = "UT";
					_states["50"] = "VT";
					_states["51"] = "VA";
					_states["53"] = "WA";
					_states["54"] = "WV";
					_states["55"] = "WI";
					_states["56"] = "WY";
					_states["11"] = "DC";
				}
			}
		}
		//    

		static string jsonrequest = @"
{{
    ""level"": ""county"",
    ""state"": ""{0}"",
    ""sublevel"": false,
    ""api"": ""acs5"",
    ""year"": ""2014"",
    ""variables"": [
        ""income"",
        ""population""
    ]
}}
";
		/* *
        ""age"",
        ""median_male_age"",
        ""poverty"",
        ""poverty_female"",
        ""poverty_male"",
		/* */
		public string code = "";
		static string _url = "http://citysdk.commerce.gov/";
		public string callapi(string state)
		{
			// should be return value
			string retv = "";
			string url = _url;
			string data = String.Format(jsonrequest, state);
			/* *
			HttpClientHandler handler = new HttpClientHandler()
			{
				Proxy = new WebProxy("207.99.118.74", 8080),
				UseProxy = true,
			};
			using (var httpClient = new HttpClient(handler))
			/* */
			using (var httpClient = new HttpClient())
			{
				try
				{
					string bat = _apikey + ":";
					httpClient.DefaultRequestHeaders.Authorization =
						new AuthenticationHeaderValue("Basic",
						string.Format("{0}", Convert.ToBase64String(Encoding.Default.GetBytes(bat.ToCharArray())) ) );
					httpClient.DefaultRequestHeaders.Accept.Clear();
					//CURLOPT_HTTPHEADER, array('Accept: application/json'))
					httpClient.DefaultRequestHeaders.Accept.Add(
						new MediaTypeWithQualityHeaderValue("application/json"));
					StringContent stringContent = new StringContent(data);
					stringContent.Headers.ContentType =
						new MediaTypeHeaderValue("application/json"); //application/x-www-form-urlencoded
					var response = httpClient.PostAsync(url, stringContent);

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
	}
}
