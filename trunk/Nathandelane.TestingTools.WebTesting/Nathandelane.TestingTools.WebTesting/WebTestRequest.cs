/**
 * Web Testing Framework to automate HTTP-based web testing.
 * Copyright (C) 2011 Nathan Lane
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Nathandelane.TestingTools.WebTesting.Events;
using System.IO;
using System.Threading;

namespace Nathandelane.TestingTools.WebTesting
{
	public class WebTestRequest : WebTestItem
	{
		#region Fields

		public delegate void ValidationEventHandler(object o, ValidationEventArgs e);
		public delegate void ExtractionEventHandler(object o, ExtractionEventArgs e);
		public delegate void PreExecuteEventHandler(object o, ExecutionEventArgs e);
		public delegate void PostExecuteEventHandler(object o, ExecutionEventArgs e);

		public event ValidationEventHandler ValidateResponse;
		public event ExtractionEventHandler ExtractValues;
		public event PreExecuteEventHandler PreExecute;
		public event PostExecuteEventHandler PostExecute;

		private HttpWebRequest _webRequest;
		private HttpWebResponse _webResponse;
		private Uri _uri;
		private string _httpResponseBody;
		private WebTestContext _context;
		private int _thinkTime;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets this request's url.
		/// </summary>
		public Uri Uri
		{
			get { return _webRequest.RequestUri; }
			set
			{
				HttpWebRequest newRequest = HttpWebRequest.Create(value) as HttpWebRequest;
			}
		}

		/// <summary>
		/// Gets the HTTP response body.
		/// </summary>
		public string HttpResponseBody
		{
			get { return _httpResponseBody; }
		}

		/// <summary>
		/// Gets the context for this test.
		/// </summary>
		public WebTestContext Context
		{
			get { return _context; }
		}

		/// <summary>
		/// Gets the response URL from the response.
		/// </summary>
		public string ResponseUrl
		{
			get { return _webResponse.ResponseUri.ToString(); }
		}

		/// <summary>
		/// Gets or sets the User-Agent string for this NetTestRequest.
		/// </summary>
		public string UserAgent
		{
			get { return _webRequest.UserAgent; }
			set { _webRequest.UserAgent = value; }
		}

		/// <summary>
		/// Gets or sets a value that indicates whether the request should follow redirection responses.
		/// </summary>
		public bool FollowRedirects
		{
			get { return _webRequest.AllowAutoRedirect; }
			set { _webRequest.AllowAutoRedirect = value; }
		}

		/// <summary>
		/// Gets the outcome of the test.
		/// </summary>
		public WebTestOutcome Outcome
		{
			get { return _context.Outcome; }
		}

		/// <summary>
		/// Gets or sets the method of this request.
		/// </summary>
		public string Method
		{
			get { return _webRequest.Method; }
			set { _webRequest.Method = value; }
		}

		/// <summary>
		/// Gets or sets the timeout for this request.
		/// </summary>
		public int Timeout
		{
			get { return _webRequest.Timeout; }
			set { _webRequest.Timeout = value; }
		}

		/// <summary>
		/// Gets or sets the headers for this request.
		/// </summary>
		public WebHeaderCollection Headers
		{
			get { return _webRequest.Headers; }
			set { _webRequest.Headers = value; }
		}

		/// <summary>
		/// Gets or sets the amount of time to wait after the request has been made.
		/// </summary>
		public int ThinkTime
		{
			get { return _thinkTime; }
			set { _thinkTime = value; }
		}

		/// <summary>
		/// Gets or sets the content-type header for this request.
		/// </summary>
		public string ContentType
		{
			get { return _webRequest.ContentType; }
			set { _webRequest.ContentType = value; }
		}

		/// <summary>
		/// Sets the request body for this request, for example for a POST reequest.
		/// </summary>
		public string HttpRequestBody
		{
			set
			{
				UTF8Encoding encoding = new UTF8Encoding();
				byte[] data = encoding.GetBytes(value);
				
				_webRequest.ContentLength = data.Length;

				if (!_webRequest.Method.Equals("post", StringComparison.InvariantCultureIgnoreCase))
				{
					_webRequest.Method = "post";
				}

				using (Stream postBodyStream = _webRequest.GetRequestStream())
				{
					postBodyStream.Write(data, 0, data.Length);
				}
			}
		}

		/// <summary>
		/// Gets or sets whether to pre-authenticate this request.
		/// </summary>
		public bool PreAuthenticate
		{
			get { return _webRequest.PreAuthenticate; }
			set { _webRequest.PreAuthenticate = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of WebTestRequest using a string URL.
		/// </summary>
		/// <param name="url"></param>
		public WebTestRequest(string url)
		{
			if (!Uri.TryCreate(url, UriKind.Absolute, out _uri))
			{
				throw new ArgumentException("Url must be an absolute address to a resource.");
			}

			InitializeWebRequest();
		}

		/// <summary>
		/// Creates an instance of WebTestRequest using a Uri object.
		/// </summary>
		/// <param name="uri"></param>
		public WebTestRequest(Uri uri)
		{
			_uri = uri;

			InitializeWebRequest();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Executes this WebTestRequest.
		/// </summary>
		public override void Execute()
		{
			OnPreExecute(new ExecutionEventArgs(this));

			_webRequest.CookieContainer = new CookieContainer();

			foreach (Cookie cookie in _context.Cookies)
			{
				_webRequest.CookieContainer.Add(cookie);
			}

			_webResponse = _webRequest.GetResponse() as HttpWebResponse;

			Thread.Sleep(_thinkTime);

			OnPostExecute(new ExecutionEventArgs(this));

			using (StreamReader reader = new StreamReader(_webResponse.GetResponseStream()))
			{
				_httpResponseBody = reader.ReadToEnd();
			}

			_context.Cookies = _webResponse.Cookies;

			OnValidate(new ValidationEventArgs(this));
			OnExtract(new ExtractionEventArgs(this));

			if (_context.Outcome == WebTestOutcome.NotExecuted)
			{
				_context.Outcome = WebTestOutcome.Passed;
			}
		}

		/// <summary>
		/// Fires the PreExecute event.
		/// </summary>
		/// <param name="args"></param>
		protected virtual void OnPreExecute(ExecutionEventArgs args)
		{
			if (PreExecute != null)
			{
				PreExecute(this, args);
			}
		}

		/// <summary>
		/// Fires the PostExecute event.
		/// </summary>
		/// <param name="args"></param>
		protected virtual void OnPostExecute(ExecutionEventArgs args)
		{
			if (PostExecute != null)
			{
				PostExecute(this, args);
			}
		}

		/// <summary>
		/// Fires Validate event.
		/// </summary>
		/// <param name="args"></param>
		protected virtual void OnValidate(ValidationEventArgs args)
		{
			if (ValidateResponse != null)
			{
				ValidateResponse(this, args);
			}
		}

		/// <summary>
		/// Fires Extract event.
		/// </summary>
		/// <param name="args"></param>
		protected virtual void OnExtract(ExtractionEventArgs args)
		{
			if (ExtractValues != null)
			{
				ExtractValues(this, args);
			}
		}

		/// <summary>
		/// Initializes the web request object for this WebTestRequest.
		/// </summary>
		private void InitializeWebRequest()
		{
			_webRequest = WebRequest.Create(_uri) as HttpWebRequest;
			_webRequest.AllowAutoRedirect = true;
			_webRequest.KeepAlive = true;
			_webRequest.Method = "GET";
			_webRequest.Timeout = 60000;
			_webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:2.0b12) Gecko/20100101 Firefox/4.0b12";
			_thinkTime = 0;
			_context = WebTestContext.GetContext();
			_context.Outcome = WebTestOutcome.NotExecuted;
		}

		#endregion
	}
}
