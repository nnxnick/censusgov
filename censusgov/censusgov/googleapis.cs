using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Data = Google.Apis.Sheets.v4.Data;
using DData = Google.Apis.Drive.v3.Data;
using System.Threading;

namespace censusgov
{
	/// <summary>
	/// Launch google spreadsheet api
	/// </summary>
	public class googleapis
	{
		static string spreadsheetId = "";
		static string spreadsheetUrl = "";
		static UserCredential credential;
		static string ApplicationName = "apigoogle";
		static string[] Scopes = { SheetsService.Scope.Spreadsheets,
					DriveService.Scope.Drive,
					DriveService.Scope.DriveMetadata,
					DriveService.Scope.DriveAppdata,
					DriveService.Scope.DriveFile,
					SheetsService.Scope.Drive
		};
		/// <summary>
		/// Set Google credential
		/// </summary>
		static void setcredentials()
		{
			// this file nnxnick_client_secret.json linked to test application apigoogle
			// for account nikolayn.n.naumenko@gmail.com
			// file location - in a folder with the executable module, folder .credentials
			string client_secret = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
				".credentials\\nnxnick_client_secret.json");
			// if you have own client_secret.json file
			// you need fill and uncomment next two lines
			// client_secret = "<path to own client_secret.json file>";
			// ApplicationName = "<own application name>";

			/* */
			using (var stream =
				new FileStream(client_secret, FileMode.Open, FileAccess.Read))
			{
				string credPath = System.Environment.GetFolderPath(
					System.Environment.SpecialFolder.Personal);
				credPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
					".credentials\\sheets.googleapis.com-dotnet-quickstart.json");
				credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					Scopes,
					"user",
					CancellationToken.None,
					new FileDataStore(credPath, true)).Result;
			}
			/* */
		}
		/// <summary>
		/// Create spreadsheet Top-10 of populations county
		/// Title contains datetime of creation in format yyyy-MM-ddTHH.mm.ss.fff
		/// </summary>
		/// <param name="spsheedid"></param>
		/// <param name="spsheeturl"></param>
		public static void creategooglesheettop10(Icensusapi xco,ref string spsheedid, ref string spsheeturl)
		{
			//define exist spreadsheet
			spreadsheetId = spsheedid;
			spreadsheetUrl = spsheeturl;

			//get the top 10 most populous county in the united states
			List<population> top10 = null;
			string xfs = xco.getpopulations();
			List<population> xpop = helper.getpoplistfromjson(xfs);
			if (xpop != null)
			{
				top10 = xpop.OrderByDescending(x => x.POP).Take(10).ToList(); //10
			}
			if (top10 != null)
			{
				foreach (var item in top10)
				{
					Console.WriteLine("Get data for " + item.GEONAME);
					// Estimated Components of Resident Population Change
					string statbd = xco.getstats_birth_death(item.state, item.county);
					item.statsbd = helper.getstatbdfromjson(statbd);
					// Detailed Language Spoken (LANG7)
					string langs = xco.getstats_language(item.state, item.county);
					item.languages = helper.getlanglistfromjson(langs);
					// population estimate by agegroup per State
					// agegroup "18 years and over"
					string pop18 = xco.getpopulations18(item.state);
					population18 p18 = helper.getpop18fromjson(pop18);
					if (p18 != null)
					{
						item.POP18 = p18.POP;
					}
					// population estimate by agegroup per State
					// agegroup 0 - all population
					string pop00 = xco.getpopulations00(item.state);
					population18 p00 = helper.getpop18fromjson(pop00);
					if (p00 != null)
					{
						item.POP00 = p00.POP;
					}
				}
				// Columns for general information
				string[] colNames = new[] { "Name", "Population",
					"State's population",
					"State's population\nwith age older\nthan 18 years",
					"Last update" };
				// Columns for Population Change
				string[] colStatsBD = new[] { "Births in period",
					"Deaths in period", "Natural increase\nin period", "Birth rate\nin period","Death rate\nin period","Period" };
				// Columns for Detailed Language
				string[] colLangs = new[] { "Description", "Population" };
				// Define range template
				string range1 = "{0}!A1:E1";
				string range2 = "{0}!A2:E2";
				string range3 = "{0}!A{1}";
				string range2_2 = "{0}!A{1}:F{2}";
				string range4 = "{0}!A{1}:B{2}";

				// Google oauth credentials
				setcredentials();
				var service = new SheetsService(new BaseClientService.Initializer()
				{
					HttpClientInitializer = credential,
					//HttpClientInitializer = sacredential,
					//ApiKey = apikey,
					ApplicationName = ApplicationName,
				});
				// Create Spreadsgeet
				#region
				if (spreadsheetId == "")
				{
					Data.Spreadsheet requestBody = new Data.Spreadsheet();
					requestBody.Properties = new SpreadsheetProperties();
					requestBody.Properties.Title = "Top-10 of populations county " +
						DateTime.Now.ToString("yyyy-MM-ddTHH.mm.ss.fff");
					requestBody.Sheets = new List<Sheet>();

					// Create sheets
					foreach (var item in top10)
					{
						Console.WriteLine("Create sheet for " + item.GEONAME);
						Sheet x = new Sheet();
						x.Properties = new SheetProperties();
						x.Properties.Title = item.GEONAME;
						requestBody.Sheets.Add(x);
					}
					// Execute
					SpreadsheetsResource.CreateRequest request =
							service.Spreadsheets.Create(requestBody);
					for (int xy = 0; xy < 3; xy++)
					{
						try
						{
							Data.Spreadsheet response = request.Execute();
							spreadsheetId = response.SpreadsheetId;
							spreadsheetUrl = response.SpreadsheetUrl;
							spsheedid = spreadsheetId;
							spsheeturl = spreadsheetUrl;
							break;
						}
						catch (Exception ex)
						{
							if (ex.Message.IndexOf("USER-100s") >= 0)
							{
								Thread.Sleep(50000); // Sleep 50 * 3 seconds while clear limit 100s
							}
							else
							{
								Console.WriteLine("An error occurred: " + ex.ToString());
								break;
							}
						}
					}
				}
				#endregion

				// Insert data into spreadsheet
				if (spreadsheetId != "")
				{
					foreach (var item in top10)
					{
						Console.WriteLine("Add data row to sheet " + item.GEONAME);

						// Add general information
						#region
						if (range1 != "")
						{
							string range = String.Format(range1, item.GEONAME);
							ValueRange valueRange = new ValueRange();
							valueRange.MajorDimension = "COLUMNS";
							valueRange.Values = new List<IList<object>>();
							for (int ix = 0; ix < colNames.Length; ix++)
							{
								var oblist = new List<object>() { colNames[ix] };
								valueRange.Values.Add(oblist);
							}
							SpreadsheetsResource.ValuesResource.UpdateRequest update =
								service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
							update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
							for (int xy = 0; xy < 3; xy++)
							{
								try
								{
									UpdateValuesResponse result2 = update.Execute();
									break;
								}
								catch (Exception ex)
								{
									if (ex.Message.IndexOf("USER-100s") >= 0)
									{
										Thread.Sleep(50000); // Sleep 50 * 3 seconds while clear limit 100s
									}
									else
										break;
								}
							}
						}
						if (range2 != "")
						{
							string range = String.Format(range2, item.GEONAME);
							ValueRange valueRange = new ValueRange();
							valueRange.MajorDimension = "COLUMNS";
							valueRange.Values = new List<IList<object>>();
							string[] po = item.ptoa;
							for (int ix = 0; ix < po.Length; ix++)
							{
								var oblist = new List<object>() { po[ix] };
								valueRange.Values.Add(oblist);
							}
							SpreadsheetsResource.ValuesResource.UpdateRequest update =
								service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
							update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
							for (int xy = 0; xy < 3; xy++)
							{
								try
								{
									UpdateValuesResponse result2 = update.Execute();
									break;
								}
								catch (Exception ex)
								{
									if (ex.Message.IndexOf("USER-100s") >= 0)
									{
										Thread.Sleep(50000); // Sleep 50 * 3 seconds while clear limit 100s
									}
									else
										break;
								}
							}
						}
						#endregion

						// Add Population Change information
						#region
						int r = 4;
						if (range3 != "")
						{
							string range = String.Format(range3, item.GEONAME, r);
							ValueRange valueRange = new ValueRange();
							valueRange.MajorDimension = "COLUMNS";
							valueRange.Values = new List<IList<object>> { new List<object>() { "Estimated Resident Population Change, and Rates of Resident Population Change" } };
							SpreadsheetsResource.ValuesResource.UpdateRequest update =
								service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
							update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
							for (int xy = 0; xy < 3; xy++)
							{
								try
								{
									UpdateValuesResponse result2 = update.Execute();
									break;
								}
								catch (Exception ex)
								{
									if (ex.Message.IndexOf("USER-100s") >= 0)
									{
										Thread.Sleep(50000); // Sleep 50 * 3 seconds while clear limit 100s
									}
									else
										break;
								}
							}
							r++;
						}
						if (range2_2 != "")
						{
							string range = String.Format(range2_2, item.GEONAME, r, r);
							ValueRange valueRange = new ValueRange();
							valueRange.MajorDimension = "COLUMNS";
							valueRange.Values = new List<IList<object>>();
							string[] po = colStatsBD;
							for (int ix = 0; ix < po.Length; ix++)
							{
								var oblist = new List<object>() { po[ix] };
								valueRange.Values.Add(oblist);
							}
							SpreadsheetsResource.ValuesResource.UpdateRequest update =
								service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
							update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
							for (int xy = 0; xy < 3; xy++)
							{
								try
								{
									UpdateValuesResponse result2 = update.Execute();
									break;
								}
								catch (Exception ex)
								{
									if (ex.Message.IndexOf("USER-100s") >= 0)
									{
										Thread.Sleep(50000); // Sleep 50 * 3 seconds while clear limit 100s
									}
									else
										break;
								}
							}
							r++;
						}
						if (range2_2 != "")
						{
							string range = String.Format(range2_2, item.GEONAME, r, r);
							ValueRange valueRange = new ValueRange();
							valueRange.MajorDimension = "COLUMNS";
							valueRange.Values = new List<IList<object>>();
							string[] po = item.statsbd.ptoa;
							for (int ix = 0; ix < po.Length; ix++)
							{
								var oblist = new List<object>() { po[ix] };
								valueRange.Values.Add(oblist);
							}
							SpreadsheetsResource.ValuesResource.UpdateRequest update =
								service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
							update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
							r += 2;
							for (int xy = 0; xy < 3; xy++)
							{
								try
								{
									UpdateValuesResponse result2 = update.Execute();
									break;
								}
								catch (Exception ex)
								{
									if (ex.Message.IndexOf("USER-100s") >= 0)
									{
										Thread.Sleep(50000); // Sleep 50 * 3 seconds while clear limit 100s
									}
									else
										break;
								}
							}
						}
						#endregion

						// Add Language Spoken information
						#region
						if (range3 != "")
						{
							string range = String.Format(range3, item.GEONAME, r);
							ValueRange valueRange = new ValueRange();
							valueRange.MajorDimension = "COLUMNS";
							valueRange.Values = new List<IList<object>> { new List<object>() { "Detailed Language Spoken" } };
							SpreadsheetsResource.ValuesResource.UpdateRequest update =
								service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
							update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
							for (int xy = 0; xy < 3; xy++)
							{
								try
								{
									UpdateValuesResponse result2 = update.Execute();
									break;
								}
								catch (Exception ex)
								{
									if (ex.Message.IndexOf("USER-100s") >= 0)
									{
										Thread.Sleep(50000); // Sleep 50 * 3 seconds while clear limit 100s
									}
									else
										break;
								}
							}
							r++;
						}
						if (range4 != "")
						{
							string range = String.Format(range4, item.GEONAME, r, r);
							ValueRange valueRange = new ValueRange();
							valueRange.MajorDimension = "COLUMNS";
							valueRange.Values = new List<IList<object>>();
							string[] po = colLangs;
							for (int ix = 0; ix < po.Length; ix++)
							{
								var oblist = new List<object>() { po[ix] };
								valueRange.Values.Add(oblist);
							}
							SpreadsheetsResource.ValuesResource.UpdateRequest update =
								service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
							update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
							for (int xy = 0; xy < 3; xy++)
							{
								try
								{
									UpdateValuesResponse result2 = update.Execute();
									break;
								}
								catch (Exception ex)
								{
									if (ex.Message.IndexOf("USER-100s") >= 0)
									{
										Thread.Sleep(50000); // Sleep 50 * 3 seconds while clear limit 100s
									}
									else
										break;
								}
							}
							r++;
						}
						if (item.languages != null && item.languages.Count > 0)
						{
							for (int xi = 0; xi < item.languages.Count; xi++)
							{
								string range = String.Format(range4, item.GEONAME, r, r);
								ValueRange valueRange = new ValueRange();
								valueRange.MajorDimension = "COLUMNS";
								valueRange.Values = new List<IList<object>>();
								string[] po = item.languages[xi].ptoa;
								for (int ix = 0; ix < po.Length; ix++)
								{
									var oblist = new List<object>() { po[ix] };
									valueRange.Values.Add(oblist);
								}
								SpreadsheetsResource.ValuesResource.UpdateRequest update =
									service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
								update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
								for (int xy = 0; xy < 3; xy++)
								{
									try
									{
										UpdateValuesResponse result2 = update.Execute();
										break;
									}
									catch (Exception ex)
									{
										if (ex.Message.IndexOf("USER-100s") >= 0)
										{
											Thread.Sleep(50000); // Sleep 50 * 3 seconds while clear limit 100s
										}
										else
											break;
									}
								}
								r++;
							}
						}
						#endregion
					}
				}
			}
			if (spreadsheetId != "")
			{
				var service = new DriveService(new BaseClientService.Initializer()
				{
					HttpClientInitializer = credential,
					ApplicationName = ApplicationName,
				});
				InsertPermission(service, spreadsheetId, null, "anyone", "reader");
			}
		}
		///
		/// Insert a new permission.
		/// service - Drive API service instance. 
		/// fileId - ID of the file to insert permission for.
		/// emailuser - User or group e-mail address, domain name or null for "default" type.
		/// type - The value "user", "group", "domain" or "anyone".
		/// role - The value "owner", "writer" or "reader".
		/// The inserted permission, null is returned if an API error occurred

		public static Permission InsertPermission(DriveService service,
			string fileId,
			string emailuser,
			string type,
			string role)
		{
			Permission newPermission = new Permission();
			if (emailuser != null && emailuser != "")
				newPermission.EmailAddress = emailuser;
			newPermission.Type = type;
			newPermission.Role = role;
			try
			{
				PermissionsResource.CreateRequest request =
					service.Permissions.Create(newPermission, fileId);
				request.Fields = "id";
				return request.Execute();
			}
			catch (Exception e)
			{
				Console.WriteLine("An error occurred: " + e.ToString());
			}
			return null;
		}
	}
}
