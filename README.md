<div>
<h1>censusgov</h1>

<h2><a href='http://take.ms/CJRyt' target='blank'>Class diagram</a><br/><br/></h2>

<h3>censusgov.censusapi</h3>
Wrapper for <a href='https://www.census.gov/data/developers/data-sets/popest-popproj/popest.html'>Population Estimates APIs</a><br/><br/>

<b><u>Population</u></b><br/>
censusgov.censusapi.getpopulations() +2 overload<br/>
API Call: api.census.gov/data/2016/pep/population<br/><br/>

<b><u>Resident Population Change</u></b><br/><br/>
censusgov.censusapi.getstats_birth_death() +2 overload<br/>
API Call: api.census.gov/data/2016/pep/components<br/><br/>

<b><u>Detailed Language Spoke</u></b><br/>
censusgov.censusapi.getstats_language() +2 overload<br/>
API Call: api.census.gov/data/2013/language.html<br/><br/>


<b><u>Demographic Characteristics Estimates by Age Groups</u></b><br/>
<b>State population with age older than 18 years</b><br/>
censusgov.censusapi.getpopulations18(string state)<br/>
<b>State population</b><br/>
censusgov.censusapi.getpopulations00(string state)><br/>
API Call: api.census.gov/data/2016/pep/charagegroups<br/>

<h3>censusgov.googleapis</h3>
Launch google spreadsheet api<br/><br/>

Note. Please read lines 39-42 in file googleapis.cs<br/>
<div>
// if you have own client_secret.json file<br/>
// you need fill and uncomment next two lines<br/>
// client_secret = "<path to own client_secret.json file>";<br/>
// ApplicationName = "<own application name>";<br/>
</div>
<br/>

<b><u>Create spreadsheet Top-10 of populations county</u></b><br/>
censusgov.googleapis.creategooglesheettop10(ref string spsheedid, ref string spsheeturl)

<a href='https://docs.google.com/spreadsheets/d/1jMJ8IA00O7iiluyb7SJwAdNi5UnxEY6uH0VTWdnlLT8' target='blank'>As example</a>
</div>
