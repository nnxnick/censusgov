
# censusgov

A sample project for create the statistic spreadsheet "Top-10 of populations county"
Based on Population Estimates Program and uses current data on births, deaths, and migration to calculate population change since the most recent decennial census and produces a time series of estimates of population, demographic components of change, and housing units. The annual time series of estimates begins with the most recent decennial census data and extends to the vintage year. As each vintage of estimates includes all years since the most recent decennial census, the latest vintage of data available supersedes all previously-produced estimates for those dates

[Class diagram](http://take.ms/CJRyt)

# censusgov.censusapi
Wrapper for [Population Estimates APIs](https://www.census.gov/data/developers/data-sets/popest-popproj/popest.html)

## Population
	censusgov.censusapi.getpopulations() +2 overloads
Annual Population Estimates for the United States; States; Counties
Source: U.S. Census Bureau, Population Division<br/>
API Call: api.census.gov/data/2016/pep/population

## Resident Population Change
	censusgov.censusapi.getstats_birth_death() +2 overloads
Annual Resident Population Estimates, Estimated Components of Resident Population Change, and Rates of the Components of Resident Population Change<br/>
API Call: api.census.gov/data/2016/pep/components

## Detailed Language Spoke
	censusgov.censusapi.getstats_language() +2 overloads
The number of speakers of languages spoken at home and the number of speakers of each language who speak English less than very well<br/>
API Call: api.census.gov/data/2013/language


## Demographic Characteristics Estimates by Age Groups
### State population (age group = 0 (all))
	censusgov.censusapi.getpopulations00(string state) 
### State population with age older than 18 years (age group = 29)
	censusgov.censusapi.getpopulations18(string state) 
API Call: api.census.gov/data/2016/pep/charagegroups

# censusgov.googleapis
Launch google spreadsheet api

Note. Please read lines 43-51 in file googleapis.cs<br/>
	// if you have own client_secret.json file <br/>
	// you need fill and uncomment next two lines<br/>
	// client_secret = "path to own client_secret.json file";<br/>
	// ApplicationName = "own application name";<br/>

## Create spreadsheet Top-10 of populations county
censusgov.googleapis.creategooglesheettop10(ref string spsheedid, ref string spsheeturl) 

[As example spreadsheet](https://docs.google.com/spreadsheets/d/1jMJ8IA00O7iiluyb7SJwAdNi5UnxEY6uH0VTWdnlLT8)

