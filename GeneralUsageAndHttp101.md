# Basics of the Internet #

## Brief HTTP Lesson (HTTP 101) ##
"The Hypertext Transfer Protocol (HTTP) is an application-level protocol for distributed, collaborative, hypermedia information systems."[[w3c](http://www.w3.org/Protocols/rfc2616/rfc2616.html)]

When dealing with HTTP you are basically doing what your web browser would ordinarily do for you. The motivation for creating a framework like this one is to remove the web browser from the equation of testing. While different web browsers will still pose a risk to the development of web sites and web applications, it is still very valuable to test those same sites and applications at the HTTP level. A well-engineered site or application will perform functionally regardless of what browser is being used. As the browser serves as an agent to the application under test, so does this API.

An HTTP request consists of the following pieces:
  * A URI:
    * Starting with the protocol, i.e. `http://` or `https://`,
    * And including the port number if the port in use is not standard,
  * And headers, including:
    * Special headers called cookies,
    * The User-Agent string,
    * And several others, such as those defining the acceptable content types, browser compression abilities, timeout limit, and whether the requesting agent wants to keep the connection to the server alive.

A typical HTTP request from one of my favorite browsers looks like this:
<pre>
GET / HTTP/1.1<br>
Host: localhost:6360<br>
User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; rv:2.0b12) Gecko/20100101 Firefox/4.0b12<br>
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8<br>
Accept-Language: en-us,en;q=0.5<br>
Accept-Encoding: gzip, deflate<br>
Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7<br>
Keep-Alive: 115<br>
DNT: 1<br>
Connection: keep-alive<br>
</pre>

Dissecting this a little bit, we find that:
  * The first line indicates the type of request (GET), the path of the request (/), and the protocol type and protocol version (HTTP/1.1),
  * The second line indicates the host and port number that the request was made to (localhost:6360)
  * Next is the User-Agent string (Mozilla/5.0 (Windows NT 6.1; WOW64; rv:2.0b12 Gecko/20100101 Firefox/4.0b12))
  * Then the Accept-headers:
    * Accept: text/html,application/xhtml+xml,application/xml;q=0.9,**/**;q=0.8
    * Accept-Language: en-us,en;q=0.5
    * Accept-Encoding: gzip, deflate
    * Accept-Charset: ISO-8859-1,utf-8;q=0.7,**;q=0.7
  * Then the length of timeout (Keep-Alive: 115)
  * Then here we see Firfox's Do Not Track header (DNT: 1)
  * And finally what type of connection to maintain with the server (Connection: keep-alive)**

These headers, aside from the DNT header are generally very common among browsers. As an example, here are some headers from Microsoft's Internet Explorer:
<pre>
GET / HTTP/1.1<br>
Accept: text/html, application/xhtml+xml, */*<br>
Accept-Language: en-US<br>
User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)<br>
Accept-Encoding: gzip, deflate<br>
Host: localhost:6360<br>
Connection: Keep-Alive<br>
</pre>

## Cookies ##

If a server sets any cookies, then those headers look like the following:
<pre>
Set-Cookie: physicalzip=84081; domain=www.vehix.com; expires=Mon, 07-Mar-2011 20:45:47 GMT; path=/<br>
Set-Cookie: !PreviousPostalCode=84081; domain=www.vehix.com; expires=Mon, 07-Mar-2011 20:45:49 GMT; path=/<br>
Set-Cookie: physicalzip=84081; domain=www.vehix.com; expires=Mon, 07-Mar-2011 20:45:47 GMT; path=/<br>
Set-Cookie: !PreviousPostalCode=84081; domain=www.vehix.com; expires=Mon, 07-Mar-2011 20:45:49 GMT; path=/<br>
Set-Cookie: ASP.NET__!SessionId=s1s5uzvl4s0pes553ibsouea; path=/; !HttpOnly<br>
Set-Cookie: zip=77511; domain=www.vehix.com; expires=Wed, 06-Apr-2011 20:15:49 GMT; path=/<br>
Set-Cookie: market=Houston; domain=www.vehix.com; expires=Mon, 07-Mar-2011 20:45:49 GMT; path=/<br>
Set-Cookie: breadcrumb=Home|/default.aspx:; path=/<br>
Set-Cookie: zip=77511; domain=www.vehix.com; path=/<br>
Set-Cookie: sid=s1s5uzvl4s0pes553ibsouea; path=/<br>
</pre>

These cookies are clearly for a domain named www.vehix.com. Cookies have five components: name, value, domain, path, and expiration. All of the cookies have the same header name, so the cookies are kept in their own collection in the browser. Cookies are contained by the domain that they are set in. Sometimes dependent requests, like those for ad containers on a website will request tracking cookies.

## HTTP Request Method ##
On the Internet there are several different types of requests that can be made according the HTTP 1.1. HTTP 1.1 defines nine standard methods that servers can support. Those methods are OPTIONS, GET, HEAD, POST, PUT, DELETE, TRACE and CONNECT [[w3c](http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html)]. Of these the most common are GET, HEAD, and POST.

### GET Request ###
A GET request may have several headers including cookies, a URL with a port number if necessary, and a query-string. A query-string is usually separated from the URL by a question mark character (?), and then key-value pairs are separated by an ampersand (&) and keys and values are separated by an equals symbol (=). Some modern web sites use other methods of separating the query-string from the rest of the URL. Many MVC (Model-View-Controller) tool-kits don't even have obvious query-strings because their query-strings appear as folders. And example GET request might look something like this:
<pre>
GET /?_a=viewCat&catId=14 HTTP/1.1<br>
</pre>

Here we see the standard query-string in the path of the request (/?_a=viewCat&catId=14). There are two key-value pairs defined here:
  *_a=viewCat
  * catId=14

The key-value pairs are separated by an ampersand, and the query-string is separated from the request path by a question mark. An MVC-like request might look like the following:
<pre>
GET /new-cars/new-car-research/convertible HTTP/1.1<br>
</pre>

Here the path does not indicate an actual path on the server, rather it is a path that is redirected into several indexed arguments. New-cars is probably a controller, while new-car-research is probably a view. Convertible then is obviously data, in this case a vehicle body type or style. Here the URL only indicates a value, but the server interprets the value as that of a specific key, which is indexed in the redirect rules. Thus the server interprets convertible as the value of the body-style or body-type key. If I inject something that does not conform to that pattern, such as ford, then I may get an error or be redirected to a page that I didn't expect. After testing the case, I've learned that the URL is dynamic, thus `GET /new-cars/new-car-research/ford HTTP/1.1` results in a valid page, that I did expect, so there is some dynamic testing going on on the server, which is very sophisticated. Another example of request on the same site looks like this: `GET /car-reviews/2012/ford/focus HTTP/1.1`, which again says that there is an index of query string parameters, as these are not actual folders. Placing the /2012 at the end of the URL results in an error page as expected.

### HEAD Request ###
A HEAD request is used to request just headers from a server. When a server receives a HEAD request it is instructed not to return a body. HEAD requests might be useful for initializing cookies or other important headers, and they are less expensive than GET or POST requests because of these facts.

### POST Request ###
A POST request is composed in a similar manner, except the a POST request may also have a body. The body may be formatted similarly to a GET request's query-string or it may be formatted in a different manner. Some servers may restrict the size of the content, filter content, or block content that does not conform to a specific pattern, but for the most part there is no limit to what a POST request body may contain. This becomes advantageous when using a POST request to upload a file to a server, which is a relatively common task these days.

POST requests are often used when a web form is involved, or when a request to an asynchronous web service is made. In the case of a form, a form may encode its content in different ways. The most common and default encoding for a form is application/x-www-form-urlencoded, but there are two more encodings for forms that are supported by all XHTML DTDs: multipart/form-data (used for uploading files) and text/plain when it is desirable that the browser does not encode the contents of the form [[w3schools](http://www.w3schools.com/tags/att_form_enctype.asp)].

The first encoding, application/x-www-form-urlencoded, as it looks, encodes the contents of the body of a POST request as key-value pairs separated by ampersands, when the keys and values are separated by equals symbols, exactly like the query-string of a standard GET request.

The second encoding, multipart/form-data, chunks the form data and sends the raw bytes of file form fields, so that the server doesn't need to decode them. A file might span several chunks. Special separators are used to attempt to identify when a new field comes up, but this is often problematic for servers, so more common is when separate forms are used for files versus regular form fields. This is, nonetheless important to know.

Finally the third encoding, text/plain, performs no encoding of any data in the form, and it is sent as raw data to the server.

## Why is This At All Important? ##
When I began using Microsoft's web testing tools, I admittedly struggled for some time until I realized what they were doing. I was used to Mercury Quick Test Pro-type web testing tools which utilize the browser and the underlying automation libraries, such as Microsoft COM, to perform the job of automating a web site or web application. But once I realized that the tools were not using a browser, and were simply employing the power of HTTP, then everything made much more sense, and I understood why and how to do things at a much lower level.

In this framework, the tool make multiple web requests in sequence, which is referred to as a session. In every case a request is made, and some kind of response is returned. Though most responses from servers on the Internet are of the HTML, XHTML, or XML sort, there are many other specific response types, such as JSON (JavaScript Object Notation), SOAP (Simple Object Access Protocol), RTSP (Real Time Streaming Protocol), and many others. At the time of this writing the API only supports XML-based responses, but support for others is planned. Please refer to the RoadMap (coming soon) for more details on those plans.

So when using this API to perform web testing, you are actually performing tests on the resultant responses from the server; you are actually performing tests directly on the HTML.

# Basics of the .NET HTTP Web Testing API #

The .NET HTTP Web Testing API conforms to the details described above. It uses HTTP for all of its requests. In the future there might be some other types of requests that can be made such as RTSP requests, which are more complicated in their nature, though the standard is open.

The basics of building an HTTP request are simplified in the API because the API takes care of most of the headers for you by default. While you are allowed to change any header and add your own, the headers described above are set by default to conform to the latest versions of Firefox or Internet Explorer. The differentiation is performed by the User-Agent string.

Building a very basic request is simple:
```
using System;
using System.Collections.Generic;
using Nathandelane.TestingTools.WebTesting;
using Nathandelane.TestingTools.WebTesting.Rules;

namespace ExampleTestProject
{
	public class OreillyHomepage : WebTest
	{
		public override IEnumerator<WebTestRequest> GetRequestEnumerator()
		{
			WebTestRequest request1 = new WebTestRequest("http://oreilly.com/");
			yield return request1;
		}
	}
}
```

As you can see, the simplest request has only a URL. The URL must include either `http://` or `https://` at the beginning of it or it can be or type Uri (which is how the URL is stored in the request). Setting other properties of the request can give you more control over the request. By default this is a GET request. If you set the request body, then it automatically defaults to a POST request. Attempting to set the request body and setting the method to GET will result in an error, since GET requests don't support a request body. Setting a request body can be done like this:
```
// ...
	WebTestRequest request1 = new WebTestRequest("http://somedomain.com/login.aspx");
	request1.ContentType = "application/x-www-form-urlencoded";
	request1.Method = "POST";
	request1.HttpRequestBody = "userName=somebody&amp;password=somepassword123";
// ...
```

## Testing the Response ##
Each request has four events, to which various types of event handlers can be connected. Currently only two events have explicitly base-classed handlers: ValidateResponse and ExtractValues.

ValidateResponse is used to validate the response of a request. This might be useful if you are expected a particular type of response or if you are looking for specific page elements. ValidateResponse is connected to via a ValidationRule.

ExtractValues is used to extract certain values, usually from form fields in a page after the response has been received. If you are testing a client-side workflow, such as a registration form that maintains some data on the client side, or as in Microsoft's Web Forms API, you might need to extract the view state, then the ExtractValues event is of great importance to you. You connect to the ExtractValues event through an ExtractionRule.

The remaining two events, PreExecute and PostExecute, have been added for future compatibility with Microsoft's Web Testing framework. Currently there are no event handler types associated directly with these two events.