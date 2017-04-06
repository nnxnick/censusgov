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
	public interface Icensusapi
	{
		/// <summary>
		/// Country population
		/// </summary>
		/// <returns>string content</returns>
		string getpopulations();
		/// <summary>
		/// State population
		/// </summary>
		/// <param name="state">State code</param>
		/// <returns>string content</returns>
		string getpopulations(string state);
		/// <summary>
		/// County population
		/// </summary>
		/// <param name="state">State code</param>
		/// <param name="county">County code (inside State)</param>
		/// <returns>string content</returns>
		string getpopulations(string state, string county);
		/// <summary>
		/// Resident Population Change
		/// </summary>
		/// <returns>string content</returns>
		string getstats_birth_death();
		/// <summary>
		/// State Resident Population Change
		/// </summary>
		/// <param name="state">State code</param>
		/// <returns>string content</returns>
		string getstats_birth_death(string state);
		/// <summary>
		/// County Resident Population Change
		/// </summary>
		/// <param name="state">State code</param>
		/// <param name="county">County code (inside State)</param>
		/// <returns>string content</returns>
		string getstats_birth_death(string state, string county);
		/// <summary>
		/// Detailed Language Spoke
		/// </summary>
		/// <returns>string content</returns>
		string getstats_language();
		/// <summary>
		/// State Detailed Language Spoke
		/// </summary>
		/// <param name="state">State code</param>
		/// <returns>string content</returns>
		string getstats_language(string state);
		/// <summary>
		/// County Detailed Language Spoke
		/// </summary>
		/// <param name="state">State code</param>
		/// <param name="county">County code (inside State)</param>
		/// <returns>string content</returns>
		string getstats_language(string state, string county);
		/// <summary>
		/// State population with age older than 18 years
		/// </summary>
		/// <param name="state">State code</param>
		/// <returns>string content</returns>
		string getpopulations18(string state);
		/// <summary>
		/// State population
		/// </summary>
		/// <param name="state">State code</param>
		/// <returns>string content</returns>
		string getpopulations00(string state);
	}
}
