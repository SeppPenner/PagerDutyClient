#pragma warning disable IDE0065 // Die using-Anweisung wurde falsch platziert.
global using System.Net;
global using System.Text;

global using Microsoft.VisualStudio.TestTools.UnitTesting;

global using Newtonsoft.Json.Linq;

global using PagerDutyClient.Enumeration;
global using PagerDutyClient.Incidents;

global using Serilog;

// The class and the namespace are both called PagerDutyClient, so the REST API client needs an alias.
global using RestClient = PagerDutyClient.PagerDutyClient;
#pragma warning restore IDE0065 // Die using-Anweisung wurde falsch platziert.
