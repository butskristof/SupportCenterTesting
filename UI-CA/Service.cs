using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using SC.BL.Domain;

namespace SC.UI.CA
{
	public interface ISupportCenterService
	{
		IEnumerable<TicketResponse> GetTicketResponses(int ticketNumber);
		TicketResponse AddTicketResponse(int ticketNumber, string response, bool isClientResponse);
	}

	internal class Service : ISupportCenterService
	{
		private const string baseUri = "https://localhost:5001/api/";

		private HttpClient GetNewHttpClient()
		{
			HttpClientHandler httpHandler = new HttpClientHandler()
			{
				ServerCertificateCustomValidationCallback = (request, certificate, chain, sslPolicyErrors) => true
			};
			return new HttpClient(httpHandler);
		}
		
		public IEnumerable<TicketResponse> GetTicketResponses(int ticketNumber)
		{
			IEnumerable<TicketResponse> responses = null;
			using (HttpClient http = new HttpClient())
			{
				string uri = baseUri + "TicketResponses?ticketNumber=" + ticketNumber;
				HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);
				//Verwachte content-type van de response meegeven
				httpRequest.Headers.Add("Accept", "application/json");
				
				//Request versturen en wachten op de response
				HttpResponseMessage httpResponse = http.SendAsync(httpRequest).Result;
				if (httpResponse.IsSuccessStatusCode)
				{
					//Body van de response uitlezen als een string
					string responseContentAsString = httpResponse.Content.ReadAsStringAsync().Result;
					//Body-string (in json-formaat) deserializeren (omzetten) naar een verzameling van TicketResponse-objecten
					responses = JsonConvert.DeserializeObject<List<TicketResponse>>(responseContentAsString);
				}
				else
					throw new Exception(httpResponse.StatusCode + " " + httpResponse.ReasonPhrase);
			}
			return responses;
		}
		
		public TicketResponse AddTicketResponse(int ticketNumber, string response, bool isClientResponse)
		{
			TicketResponse tr = null;

			using (HttpClient http = new HttpClient())
			{
				string uri = baseUri + "TicketResponses";
				HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, uri);
				//Request data toevoegen aan body, via anonymous object dat je serialiseert naar json-formaat
				object data = new { TicketNumber = ticketNumber, ResponseText = response, IsClientResponse = isClientResponse };
				string dataAsJsonString = JsonConvert.SerializeObject(data);
				httpRequest.Content = new StringContent(dataAsJsonString, System.Text.Encoding.UTF8, "application/json");
				//Verwachte content-type van de response meegeven
				httpRequest.Headers.Add("Accept", "application/json");
				
				//Request versturen en wachten op de response
				HttpResponseMessage httpResponse = http.SendAsync(httpRequest).Result;
				if (httpResponse.IsSuccessStatusCode)
				{
					//Body van de response uitlezen als een string
					string responseContentAsString = httpResponse.Content.ReadAsStringAsync().Result;
					//Body-string (in json-formaat) deserializeren (omzetten) naar een TicketResponse-object
					tr = JsonConvert.DeserializeObject<TicketResponse>(responseContentAsString);
				}
				else
					throw new Exception(httpResponse.StatusCode + " " + httpResponse.ReasonPhrase);
			}

			return tr;
		}
	}
}