# HTML Generator Documentation

## Interview Assignment

### Introduction

A short documentation in regard to the features and components added to the pdf generator API. Also an idea on how to create a 
serverless, scalable and maintainable solution based on creating Kubernetes resources dynamically through API request.

**Features added**

    - An action filter that uses fluent validation to validate the input

    - A middleware that will catch any exception thrown and return them in a uniform manner that is easy to use everywhere

    - Generalised the concept of converter options
        
    - Added database support using EF Core

    - Created a docker-compose that will start an instance of the API and connect it with an instance of a SQL server

    - Added Kubernetes support to create a dynamic and serverless solution

    
> ***NOTE*** Please take into consideration that due to the fact that there was a time constrain I was fitted as much material as possible. 

#### Database support

The database was created using Entity Framework Core. I created Convert Job, Converter Options and Converter Margins in order to establish a sense
of reusability and an uniform approach. You can create a template or in our case a Converter Opotions that contains information in how to generate the
output.

#### Docker support

I created a docker-compose that contains two services; the api and a sql server container. The api container will be depended on the sql server
and also connected to the said container. The idea was to create a portable environment that is functinal and resource friendly. Every time
you build and run the compose the api will start with a new database and all the migrations will be migrated to the newly created sql server.

The request that is used to communicate with the api container is http://0.0.0.0:80/{endpoint} (since it not mentioned which port and ip address to be published)

#### Kubernetes support

With the rise of cloud computing the need to depend on Kubernetes infrastructure is becoming more popular. I created the API requests that can
create and deploy e Kubernetes deployment and can be scaled also. The deployment holds the template of the pod that will be used to scale later on.

Also there is the endpoint to delete a specific deployment and watch the readiness probe of a specific pod.

Using the same fashion as in the Kubernetes service you can create pods, volumes, claims, config mapes and so one. You can add limits and request,
add taunts and tolerations, crate multiple container pod and so on. Sky is the limit !

**API Documentation**

### POST Converter

Generate a pdf in base64 encoded of a HTML page

| POST              |                                                           |  
| :---              |    :----                                                 |
| Method            | POST                                                     |
| URL or EndPoint   | /convert            |
| Headers           | API Key                                            |
| Body              | {htmlString, options{pageColorMode, pageOrientation, pagePapaerSize, pageMargins{top, right, bottom, left}}} |                                                        

#### Request body

~~~ json
{
    "htmlString": "PGgxPkhlbGxvITwvaDE+",
    "options": {
        "pageColorMode": "Color",
        "pageOrientation": "InvalidOrientation",
        "pagePaperSize": "A4",
        "pageMargins": {
            "top": 10,
            "right": 10,
            "bottom": 10,
            "left": 10
        }
        
    }
}
~~~

The description of the parameters is as follows:

| Body Parameter Name   | Mandatory | Type         | Example    | Description|
|      :---             |    :---   |  :---        |  :---     |   :---      |
|htmlString          |  Yes      |string | "string" | The base64 representation of the HTML page to be converted    |
|options            |  Yes      |object | | Sends the converting options for the file to be generated |
|pageColorMode            |  Yes       |string   |"string"| Sends the mode of the page color for the file to be generated |
|pageOrientation                 |  Yes      |string   |"string"| Sends the page orientation for the file to be generated  |
|pagePaperSize        |  Yes       |string   |"string"| Sends the page paper size for the file to be generated |
|pageMargins               |  Yes      |object | | Sends the page margins needed to convert the html page |
|top             |  Yes      |int |1 | Sends the top margin value      |
|right           |  Yes      |int |1 | Sends the right margin value      |
|bottom           |  Yes      |int |1 | Sends the bottom margin value      |
|left           |  Yes      |int |1 | Sends the left margin value      |

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~ json
{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": {
    "isSuccess": true,
    "errorMessage": null,
    "pdfDocument": "test",
    "pdfDocumentSize": 1000
  },
  "resultInfo": {
    "totalCount": 0,
    "pageIndex": 0,
    "pageSize": 0,
    "totalPages": 0,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}
~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |array [object]|         | Returns the result of created live job   |
|isSuccess|bool           |true    | Returns whether or not the request was successful   |
|errorMessage|string           |"test"    | Returns the error message in case of failure   |
|pdfDocument|string           |"test"    | Returns the pdf representation of the converted HTML page  |
|pdfDocumentSize|int           |0    | Returns the size of converted pdf in byte   |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |

<h4 class="errors">Errors</h4>

<div class="errorp">
<p>For information about the errors that are common to all actions, see Common Errors:</p>
<ul class="errorlist">
<li>HTTP Status Code: 401 Unauthorized</li>
</ul>
</div>

### POST Converter Job

Create a new convert job

| POST              |                                                           |  
| :---              |    :----                                                 |
| Method            | POST                                                     |
| URL or EndPoint   | /convert/job             |
| Body              | {htmlInput, converterOptionsId} |                                                        


#### Request body

~~~ json
{
  "htmlInput": "test",
  "converterOptionsId": 1,
}
~~~

The description of the parameters is as follows:

| Body Parameter Name   | Mandatory | Type         | Example    | Description|
|      :---             |    :---   |  :---        |  :---     |   :---      |
|htmlInput          |  Yes      |string | "test" | Sends the base64 representation of a HTML page    |
|converterOptionsId            |  Yes      |int | int | Sends the id of the convert options  |

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~ json
{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": {
        "name": C9D2F10F-8556-4D30-B9F0-8F3A2EBA9525
        "htmlinput": "test",
        "converteroptionsid": 0,
        "converteroptions": {
    	    "pageColorMode": "test",
	        "pageorientation": "test",
	        "pagepapersize": "test",
	        "convertermarginsid": 0,
	        "convertermargins":{
	          "top": 1,
	          "right": 1,
	          "bottom": 1,
	          "left": 1
	        }
        },
        "id":1,
        "insertDate": "2023-02-07T19:10:27.7185203",
        "updatedDateTime": "2023-02-07T19:10:27.7185203",
        "insertedBy": "00000000-0000-0000-0000-000000000000",
        "updatedBy": "00000000-0000-0000-0000-000000000000",
        "deleted": false
    }
  },
  "resultInfo": {
    "totalCount": 0,
    "pageIndex": 0,
    "pageSize": 0,
    "totalPages": 0,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}
~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |array [object]|         | Returns the result of created live job   |
|name|string(Guid)        |C9D2F10F-8556-4D30-B9F0-8F3A2EBA9525    | Returns the identifying name of the created job   |
|htmlinput|string | "test"   | Returns the identifing name of the created job   |
|converteroptionsid            |integer($int32) |1 | Sends the id of te convert options to be used    |
|options            |object | | Returns the converting options for the file to be generated |
|pageColorMode               |string   |"string"| Returns the mode of the page color for the file to be generated |
|pageOrientation                  |string   |"string"| Returns the page orientation for the file to be generated  |
|pagePaperSize            |string   |"string"| Returns the page paper size for the file to be generated |
|pageMargins                  |object | | Returns the page margins needed to convert the html page |
|top                 |integer($int32) |1 | Returns the top margin value      |
|right                 |integer($int32) |1 | Returns the right margin value      |
|bottom             |integer($int32) |1 | Returns the bottom margin value      |
|left               |integer($int32) |1 | Returns the left margin value      |
|id | integer($int32)   | 1 | Returns the id of the created entity. |
|insertDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the inserted date time of converting job. |
|updateDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the updated date time of converting job. |
|inseredBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that inserted the converter job. |
|updatedBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that updated the converter job. |
|deleted|bool           |true    | Returns whether the job is deleted  |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |

### GET Converter Job

Get a specific converter job

| GET           |                                                          |  
| :---              |    :----                                             |
| Method            | GET                                                  |
| URL or EndPoint   | /convert/job/{name} |
| Parameters        | name         |
| Body              | Not Applicable|

The description of the URL parameters is as follows:

| URL Parameter Name | Mandatory | Type       | Example | Description|
|      :---          |    :---   |     :---   |  :---       |   :---          |
|{name}          |    Yes    |string      |   1    |Shows the name of the specified job |

#### Request body

The request does not have a request body.

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~json

{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": {
        "name": C9D2F10F-8556-4D30-B9F0-8F3A2EBA9525
        "htmlinput": "test",
        "converteroptionsid": 0,
        "converteroptions": {
    	    "pageColorMode": "test",
	    "pageorientation": "test",
	    "pagepapersize": "test",
	    "convertermarginsid": 0,
	    "convertermargins":{
	      "top": 1,
	      "right": 1,
	      "bottom": 1,
	      "left": 1
	    },
        "id":1,
        "insertDate": "2023-02-07T19:10:27.7185203",
        "updatedDateTime": "2023-02-07T19:10:27.7185203",
        "insertedBy": "00000000-0000-0000-0000-000000000000",
        "updatedBy": "00000000-0000-0000-0000-000000000000",
        "deleted": false
   },
  "resultInfo": {
        "totalCount": 0,
        "pageIndex": 0,
        "pageSize": 0,
        "totalPages": 0,
        "hasNextPage": true,
        "hasPreviousPage": true
  }
}

~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |object|         | Returns the result of created converter job   |
|name|string(Guid)        |C9D2F10F-8556-4D30-B9F0-8F3A2EBA9525    | Returns the identifying name of the created job   |
|htmlInput|string | "test"   | Returns the identifying name of the created job   |
|converterOptionsId            |integer($int32) |1 | Returns the id of the convert options to be used    |
|options                 |object | | Returns the converting options for the file to be generated |
|pageColorMode                 |string   |"string"| Returns the mode of the page color for the file to be generated |
|pageOrientation                   |string   |"string"| Returns the page orientation for the file to be generated  |
|pagePaperSize          |string   |"string"| Returns the page paper size for the file to be generated |
|pageMargins                   |object | | Returns the page margins needed to convert the html page |
|top                |integer($int32) |1 | Returns the top margin value      |
|right               |integer($int32) |1 | Returns the right margin value      |
|bottom             |integer($int32) |1 | Returns the bottom margin value      |
|left               |integer($int32) |1 | Returns the left margin value      |
|id | integer($int32)   | 1 | Returns the id of the specified job. |
|insertDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the inserted date time of converting job. |
|updateDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the updated date time of converting job. |
|inseredBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that inserted the converter job. |
|updatedBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that updated the converter job. |
|deleted|bool           |true    | Returns whether the job is deleted  |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |

### GET All Converter Jobs 

List all converter jobs

| GET           |                                                          |  
| :---              |    :----                                             |
| Method            | GET                                                  |
| URL or EndPoint   | /convert/job |
| Body              | Not Applicable|

#### Request body

The request does not have a request body.

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~ json

{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": [
    {
        "name": C9D2F10F-8556-4D30-B9F0-8F3A2EBA9525
        "htmlinput": "test",
        "converteroptionsid": 0,
        "converteroptions": {
    	    "pageColorMode": "test",
	    "pageorientation": "test",
	    "pagepapersize": "test",
	    "convertermarginsid": 0,
	    "convertermargins":{
	        "top": 1,
	        "right": 1,
	        "bottom": 1,
	        "left": 1
	    },
        "id":1.
        "insertDate": "2023-02-07T19:10:27.7185203",
        "updatedDateTime": "2023-02-07T19:10:27.7185203",
        "insertedBy": "00000000-0000-0000-0000-000000000000",
        "updatedBy": "00000000-0000-0000-0000-000000000000",
        "deleted": false
    }
  ],
  "resultInfo": {
    "totalCount": 0,
    "pageIndex": 0,
    "pageSize": 0,
    "totalPages": 0,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}

~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |array [object]|         | Returns the result of all of the created converter jobs   |
|name|string(Guid)        |C9D2F10F-8556-4D30-B9F0-8F3A2EBA9525    | Returns the identifying name of the created job   |
|htmlInput|string | "test"   | Returns the identifying name of the created job   |
|converterOptionsId               |integer($int32) |1 | Returns the id of te convert options to be used    |
|options                |object | | Returns the converting options for the file to be generated |
|pageColorMode              |string   |"string"| Returns the mode of the page color for the file to be generated |
|pageOrientation                    |string   |"string"| Returns the page orientation for the file to be generated  |
|pagePaperSize          |string   |"string"| Returns the page paper size for the file to be generated |
|pageMargins                 |object | | Returns the page margins needed to convert the html page |
|top                 |integer($int32) |1 | Returns the top margin value      |
|right               |integer($int32) |1 | Returns the right margin value      |
|bottom               |integer($int32) |1 | Returns the bottom margin value      |
|left               |integer($int32) |1 | Returns the left margin value      |
|insertDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the inserted date time of converting job. |
|updateDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the updated date time of converting job. |
|inseredBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that inserted the converter job. |
|updatedBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that updated the converter job. |
|deleted|bool           |true    | Returns whether the job is deleted  |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |

### DELETE Converter job

Delete a specific converter job

| DELETE           |                                                          |  
| :---              |    :----                                                 |
| Method            | DELETE                                                  |
| URL or EndPoint   |/convert/job/{name}        |
| Parameters        | name                |
| Body              | Not Applicable                                           |

The description of the URL parameters is as follows:

| URL Parameter Name | Mandatory | Type       | Example | Description|
|      :---          |    :---   |  :---          | :---        |  :---           |
|{name}          |    Yes    |string($Guid)      |   "00000000-0000-0000-0000-000000000000"    |Shows the name of the job to be deleted |

#### Request Body

The request does not have a request body.

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~ json

{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": true,
  "resultInfo": {
    "totalCount": 0,
    "pageIndex": 0,
    "pageSize": 0,
    "totalPages": 0,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}

~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |bool|   true      | Returns the result of deleting the converter job   |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |

### POST Converter Options

Create a new converter options

| POST              |                                                           |  
| :---              |    :----                                                 |
| Method            | POST                                                     |
| URL or EndPoint   | /convert/options             |
| Body              | {pageColorMode, pageOrientation, pagePaperSize, converterMarginsId} |   

#### Request body

~~~ json
{
  "pageColorMode": "test",
  "pageOrientation": "test",
  "pagePaperSize": "test",
  "converterMarginsId": 1,
}
~~~

The description of the parameters is as follows:

| Body Parameter Name   | Mandatory | Type         | Example    | Description|
|      :---             |    :---   |  :---        |  :---     |   :---      |
|pageColorMode          |  Yes      |string | "test" | Sends the page color mode that will be used to convert   |
|pageOrientation          |  Yes      |string | "test" | Sends the page orientation that will be used to convert   |
|pagePaperSize          |  Yes      |string | "test" | Sends the page paper size that will be used to convert    |
|converterMarginsId            |  Yes      |int | int | Sends the id of the convert margins that will be used to convert  |

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~ json
{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": {
    	"pageColorMode": "test",
	    "pageorientation": "test",
	    "pagepapersize": "test",
	    "convertermarginsid": 0,
	    "convertermargins":{
	      "top": 1,
	      "right": 1,
	      "bottom": 1,
	      "left": 1
	    },
        "id": 1
        "insertDate": "2023-02-07T19:10:27.7185203",
        "updatedDateTime": "2023-02-07T19:10:27.7185203",
        "insertedBy": "00000000-0000-0000-0000-000000000000",
        "updatedBy": "00000000-0000-0000-0000-000000000000",
        "deleted": false
    }
  },
  "resultInfo": {
    "totalCount": 0,
    "pageIndex": 0,
    "pageSize": 0,
    "totalPages": 0,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}
~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |object|         | Returns the result of created converter options   |
|pageColorMode             |string   |"string"| Returns the mode of the page color for the file to be generated |
|pageOrientation                 |string   |"string"| Returns the page orientation for the file to be generated  |
|pagePaperSize         |string   |"string"| Returns the page paper size for the file to be generated |
|pageMargins              |object | | Returns the page margins needed to convert the html page |
|top                |integer($int32) |1 | Returns the top margin value      |
|right             |integer($int32) |1 | Returns the right margin value      |
|bottom           |integer($int32) |1 | Returns the bottom margin value      |
|left            |integer($int32) |1 | Returns the left margin value      |
|id             |integer($int32) |1 |  Returns the id of the entity      |
|insertDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the inserted date time of converting options. |
|updateDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the updated date time of converting options. |
|inseredBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that inserted the converter options. |
|updatedBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that updated the converter options. |
|deleted|bool           |true    | Returns whether the converter options is deleted  |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |

### GET Converter Options

Get a specific converter options

| GET           |                                                          |  
| :---              |    :----                                             |
| Method            | GET                                                  |
| URL or EndPoint   | /convert/options/{id} |
| Parameters        | id         |
| Body              | Not Applicable|

The description of the URL parameters is as follows:

| URL Parameter Name | Mandatory | Type       | Example | Description|
|      :---          |    :---   |     :---   |  :---       |   :---          |
|{id}          |    Yes    |integer($int32)      |   1    |Shows the id of the specified converter options|

#### Request body

The request does not have a request body.

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~ json
{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": {
    	"pageColorMode": "test",
	    "pageorientation": "test",
	    "pagepapersize": "test",
	    "convertermarginsid": 0,
	    "convertermargins":{
	      "top": 1,
	      "right": 1,
	      "bottom": 1,
	      "left": 1
	    },
        "id": 1
        "insertDate": "2023-02-07T19:10:27.7185203",
        "updatedDateTime": "2023-02-07T19:10:27.7185203",
        "insertedBy": "00000000-0000-0000-0000-000000000000",
        "updatedBy": "00000000-0000-0000-0000-000000000000",
        "deleted": false
    }
  },
  "resultInfo": {
    "totalCount": 0,
    "pageIndex": 0,
    "pageSize": 0,
    "totalPages": 0,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}
~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |object|         | Returns the result of created converter options   |
|pageColorMode             |string   |"string"| Returns the mode of the page color for the file to be generated |
|pageOrientation                 |string   |"string"| Returns the page orientation for the file to be generated  |
|pagePaperSize         |string   |"string"| Returns the page paper size for the file to be generated |
|pageMargins              |object | | Returns the page margins needed to convert the html page |
|top                |integer($int32) |1 | Returns the top margin value      |
|right             |integer($int32) |1 | Returns the right margin value      |
|bottom           |integer($int32) |1 | Returns the bottom margin value      |
|left            |integer($int32) |1 | Returns the left margin value      |
|id             |integer($int32) |1 |  Returns the id of the entity      |
|insertDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the inserted date time of converting options. |
|updateDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the updated date time of converting options. |
|inseredBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that inserted the converter options. |
|updatedBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that updated the converter options. |
|deleted|bool           |true    | Returns whether the converter options is deleted  |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |

### GET All Converter Options

List all converter options

| GET           |                                                          |  
| :---              |    :----                                             |
| Method            | GET                                                  |
| URL or EndPoint   | /convert/options |
| Body              | Not Applicable|

#### Request body

The request does not have a request body.

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~ json
{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": [
        {
    	    "pageColorMode": "test",
	        "pageorientation": "test",
	        "pagepapersize": "test",
	        "convertermarginsid": 0,
	        "convertermargins":{
	          "top": 1,
	          "right": 1,
	          "bottom": 1,
	          "left": 1
	        },
            "id": 1
            "insertDate": "2023-02-07T19:10:27.7185203",
            "updatedDateTime": "2023-02-07T19:10:27.7185203",
            "insertedBy": "00000000-0000-0000-0000-000000000000",
            "updatedBy": "00000000-0000-0000-0000-000000000000",
            "deleted": false
        }
      }
  ],
  "resultInfo": {
    "totalCount": 0,
    "pageIndex": 0,
    "pageSize": 0,
    "totalPages": 0,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}
~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |array [object]|         | Returns the result of all created converter options   |
|pageColorMode             |string   |"string"| Returns the mode of the page color for the file to be generated |
|pageOrientation                 |string   |"string"| Returns the page orientation for the file to be generated  |
|pagePaperSize         |string   |"string"| Returns the page paper size for the file to be generated |
|pageMargins              |object | | Returns the page margins needed to convert the html page |
|top                |integer($int32) |1 | Returns the top margin value      |
|right             |integer($int32) |1 | Returns the right margin value      |
|bottom           |integer($int32) |1 | Returns the bottom margin value      |
|left            |integer($int32) |1 | Returns the left margin value      |
|id             |integer($int32) |1 |  Returns the id of the entity      |
|insertDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the inserted date time of converting options. |
|updateDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the updated date time of converting options. |
|inseredBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that inserted the converter options. |
|updatedBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that updated the converter options. |
|deleted|bool           |true    | Returns whether the converter options is deleted  |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |

### DELETE Converter Options

Delete a specific converter options

| DELETE           |                                                          |  
| :---              |    :----                                                 |
| Method            | DELETE                                                  |
| URL or EndPoint   |/convert/options/{id}        |
| Parameters        | id                |
| Body              | Not Applicable                                           |

The description of the URL parameters is as follows:

| URL Parameter Name | Mandatory | Type       | Example | Description|
|      :---          |    :---   |  :---          | :---        |  :---           |
|{id}          |    Yes    |integer($int32)      |   1    |Shows the id of the converter options to be deleted |

#### Request Body

The request does not have a request body.

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~ json

{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": true,
  "resultInfo": {
    "totalCount": 0,
    "pageIndex": 0,
    "pageSize": 0,
    "totalPages": 0,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}

~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |bool|   true      | Returns the result of deleting the converter options   |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |

### POST Converter Margins

Create a converter margins

| POST              |                                                           |  
| :---              |    :----                                                 |
| Method            | POST                                                     |
| URL or EndPoint   | /convert/margins             |
| Body              | {top, right, bottom, left} |   

#### Request body

~~~ json
{
  "top": 1,
  "right": 1,
  "bottom": 1,
  "left": 1
}
~~~

The description of the parameters is as follows:

| Body Parameter Name   | Mandatory | Type         | Example    | Description|
|      :---             |    :---   |  :---        |  :---     |   :---      |
|top          |  Yes      |int($int32) | 1 | Sends the top margin that will be used to convert   |
|right          |  Yes      |int($int32) | 1 | Sends the right margin that will be used to convert   |
|bottom          |  Yes      |int($int32) | 1 | Sends the bottom margin that will be used to convert    |
|left            |  Yes      |int($int32) | 1 | Sends the left margin that will be used to convert  |

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~ json
{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": {
	    "top": 1,
	    "right": 1,
	    "bottom": 1,
	    "left": 1
        "id": 1
        "insertDate": "2023-02-07T19:10:27.7185203",
        "updatedDateTime": "2023-02-07T19:10:27.7185203",
        "insertedBy": "00000000-0000-0000-0000-000000000000",
        "updatedBy": "00000000-0000-0000-0000-000000000000",
        "deleted": false
    }
  },
  "resultInfo": {
    "totalCount": 0,
    "pageIndex": 0,
    "pageSize": 0,
    "totalPages": 0,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}
~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |object|         | Returns the result of created converter margin   |
|top                |integer($int32) |1 | Returns the top margin value      |
|right             |integer($int32) |1 | Returns the right margin value      |
|bottom           |integer($int32) |1 | Returns the bottom margin value      |
|left            |integer($int32) |1 | Returns the left margin value      |
|id             |integer($int32) |1 |  Returns the id of the entity      |
|insertDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the inserted date time of converting margin. |
|updateDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the updated date time of converting margin. |
|inseredBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that inserted the converter margin. |
|updatedBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that updated the converter margin. |
|deleted|bool           |true    | Returns whether the converter options is deleted  |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |

### GET Converter Margins

Get converter margin

| GET           |                                                          |  
| :---              |    :----                                             |
| Method            | GET                                                  |
| URL or EndPoint   | /convert/margins/{id} |
| Parameters        | id         |
| Body              | Not Applicable|

The description of the URL parameters is as follows:

| URL Parameter Name | Mandatory | Type       | Example | Description|
|      :---          |    :---   |     :---   |  :---       |   :---          |
|{id}          |    Yes    |integer($int32)      |   1    |Shows the id of the specified converter margin|

#### Request body

The request does not have a request body.

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~ json
{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": {
	    "top": 1,
	    "right": 1,
	    "bottom": 1,
	    "left": 1
        "id": 1
        "insertDate": "2023-02-07T19:10:27.7185203",
        "updatedDateTime": "2023-02-07T19:10:27.7185203",
        "insertedBy": "00000000-0000-0000-0000-000000000000",
        "updatedBy": "00000000-0000-0000-0000-000000000000",
        "deleted": false
    }
  },
  "resultInfo": {
    "totalCount": 0,
    "pageIndex": 0,
    "pageSize": 0,
    "totalPages": 0,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}
~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |object|         | Returns the result of the requested converter margin   |
|top                |integer($int32) |1 | Returns the top margin value      |
|right             |integer($int32) |1 | Returns the right margin value      |
|bottom           |integer($int32) |1 | Returns the bottom margin value      |
|left            |integer($int32) |1 | Returns the left margin value      |
|id             |integer($int32) |1 |  Returns the id of the entity      |
|insertDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the inserted date time of converting margin. |
|updateDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the updated date time of converting margin. |
|inseredBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that inserted the converter margin. |
|updatedBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that updated the converter margin. |
|deleted|bool           |true    | Returns whether the converter margin is deleted  |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |

### GET All Converter Margins

Get all converter margins

| GET           |                                                          |  
| :---              |    :----                                             |
| Method            | GET                                                  |
| URL or EndPoint   | /convert/margins |
| Body              | Not Applicable|

#### Request body

The request does not have a request body.

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~ json
{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": [
        {
	        "top": 1,
	        "right": 1,
	        "bottom": 1,
	        "left": 1
            "id": 1
            "insertDate": "2023-02-07T19:10:27.7185203",
            "updatedDateTime": "2023-02-07T19:10:27.7185203",
            "insertedBy": "00000000-0000-0000-0000-000000000000",
            "updatedBy": "00000000-0000-0000-0000-000000000000",
            "deleted": false
        }
      }
  ],
  "resultInfo": {
    "totalCount": 0,
    "pageIndex": 0,
    "pageSize": 0,
    "totalPages": 0,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}
~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |array [object]|         | Returns the result of all created converter oprions   |
|top                |integer($int32) |1 | Returns the top margin value      |
|right             |integer($int32) |1 | Returns the right margin value      |
|bottom           |integer($int32) |1 | Returns the bottom margin value      |
|left            |integer($int32) |1 | Returns the left margin value      |
|id             |integer($int32) |1 |  Returns the id of the entity      |
|insertDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the inserted date time of converting options. |
|updateDate | DateTime   | "2021-11-04T15:12:19.222Z" | Returns the updated date time of converting options. |
|inseredBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that inserted the converter options. |
|updatedBy | string($Guid)   | "00000000-0000-0000-0000-000000000000" | Returns the id of the user that updated the converter options. |
|deleted|bool           |true    | Returns whether the converter options is deleted  |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |

### DELETE Converter Margins

Delete a specific converter margins

| DELETE           |                                                          |  
| :---              |    :----                                                 |
| Method            | DELETE                                                  |
| URL or EndPoint   |/convert/margins/{id}        |
| Parameters        | id                |
| Body              | Not Applicable                                           |

The description of the URL parameters is as follows:

| URL Parameter Name | Mandatory | Type       | Example | Description|
|      :---          |    :---   |  :---          | :---        |  :---           |
|{id}          |    Yes    |integer($int32)      |   1    |Shows the id of the converter margins to be deleted |

#### Request Body

The request does not have a request body.

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~ json

{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": true,
  "resultInfo": {
    "totalCount": 0,
    "pageIndex": 0,
    "pageSize": 0,
    "totalPages": 0,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}

~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |bool|   true      | Returns the result of deleting the converter margins   |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |

### POST Deployment

Create and deploy a kubernetes deployment

| POST              |                                                           |  
| :---              |    :----                                                 |
| Method            | POST                                                     |
| URL or EndPoint   | /deploy             |
| Body              | {name, replicas} |   

#### Request body


~~~ json
{
  "name": "test",
  "replicas": 0,
}
~~~

The description of the parameters is as follows:

| Body Parameter Name   | Mandatory | Type         | Example    | Description|
|      :---             |    :---   |  :---        |  :---     |   :---      |
|name          |  Yes      |string | "test" | Sends the name of the deployment to be created    |
|replicas            |  Yes      |int | 0 | Sends the number of replicas to that deployment |

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~ json
{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": {
    "uid": "test",
    "status": 0,
    "containerreadiness": true
  },
  "resultInfo": {
    "totalCount": 0,
    "pageIndex": 0,
    "pageSize": 0,
    "totalPages": 0,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}
~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |array [object]|         | Returns the result of created live job   |
|uid|string        |"true"   | Returns the id of the created deployment   |
|status|integer($int32)        |0   | Returns the enum value for the pod status   |
|containerreadiness|bool        |true   | Returns the readiness probe of the pod  |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |

### DELETE Deployment

Delete a specific deployment

| DELETE           |                                                          |  
| :---              |    :----                                                 |
| Method            | DELETE                                                  |
| URL or EndPoint   |/deploy/{name}        |
| Parameters        | name                |
| Body              | Not Applicable                                           |

The description of the URL parameters is as follows:

| URL Parameter Name | Mandatory | Type       | Example | Description|
|      :---          |    :---   |  :---          | :---        |  :---           |
|{name}          |    Yes    |string      | "test"   |Shows the name of the deployment to be deleted |

#### Request Body

The request does not have a request body.

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~ json
{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": {
    "uid": "test",
    "status": 0,
    "containerreadiness": true
  },
  "resultInfo": {
    "totalCount": 0,
    "pageIndex": 0,
    "pageSize": 0,
    "totalPages": 0,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}
~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |array [object]|         | Returns the result of created live job   |
|uid|string        |"true"   | Returns the id of the created deployment   |
|status|integer($int32)        |0   | Returns the enum value for the pod status   |
|containerreadiness|bool        |true   | Returns the readiness probe of the pod  |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |

### PUT Scale

Scale a kubernetes deployment

| PUT              |                                                           |  
| :---              |    :----                                                 |
| Method            | PUT                                                     |
| URL or EndPoint   | /deploy/scale             |
| Body              | {deployment, replicas} |   

#### Request body


~~~ json
{
  "deployment": "test",
  "replicas": 0,
}
~~~

The description of the parameters is as follows:

| Body Parameter Name   | Mandatory | Type         | Example    | Description|
|      :---             |    :---   |  :---        |  :---     |   :---      |
|deployment          |  Yes      |string | "test" | Sends the name of the deployment to be scaled    |
|replicas            |  Yes      |int | 0 | Sends the number of replicas to scale the deployment |

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~ json
{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": {
    "uid": "test",
    "status": 0,
    "containerreadiness": true
  },
  "resultInfo": {
    "totalCount": 0,
    "pageIndex": 0,
    "pageSize": 0,
    "totalPages": 0,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}
~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |array [object]|         | Returns the result of created live job   |
|uid|string        |"true"   | Returns the id of the created deployment   |
|status|integer($int32)        |0   | Returns the enum value for the pod status   |
|containerreadiness|bool        |true   | Returns the readiness probe of the pod  |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |

### GET Pod Watcher

Get the status of a specific pod

| DELETE           |                                                          |  
| :---              |    :----                                                 |
| Method            | GET                                                  |
| URL or EndPoint   |/deploy/{name}        |
| Parameters        | name                |
| Body              | Not Applicable                                           |

The description of the URL parameters is as follows:

| URL Parameter Name | Mandatory | Type       | Example | Description|
|      :---          |    :---   |  :---          | :---        |  :---           |
|{name}          |    Yes    |string      | "test"   |Shows the name of the name of the pod to observe |

#### Request Body

The request does not have a request body.

#### Responses

If the action is successful, the service sends back an HTTP 200 response.

The following data is returned in json format by the service.

~~~ json
{
  "success": true,
  "errors": [
    "string"
  ],
  "messages": [
    "string"
  ],
  "result": {
    "uid": "test",
    "status": 0,
    "containerreadiness": true
  },
  "resultInfo": {
    "totalCount": 0,
    "pageIndex": 0,
    "pageSize": 0,
    "totalPages": 0,
    "hasNextPage": true,
    "hasPreviousPage": true
  }
}
~~~

| Field Name  | Type         | Example | Description                   |
|      :---   |   :---           |  :---       |      :---                         |
|success    |bool          | true    |If response is success will return true otherwise will return false    |
|errors     |array []    |"string" |Indicate if there was an error                      |
|messages   |array []    |"string" |Returns the response message from back-end      |
|result     |array [object]|         | Returns the result of created live job   |
|uid|string        |"true"   | Returns the id of the created deployment   |
|status|integer($int32)        |0   | Returns the enum value for the pod status   |
|containerreadiness|bool        |true   | Returns the readiness probe of the pod  |
|resultInfo  |object  |        | Returns an object of behavior   |
|totalCount|integer($int32) |   0    | How many records of behavior entity are in database     |
|pageIndex  |integer($int32)|   0    |Returns the page index, from which page you want to see the requested data   |
|pageSize  |integer($int32)|   0    | Returns how many pages you want to list from page index you selected |
|totalPages |integer($int32)|   0    |Returns the total number of pages |
|hasNextPage|bool           |true    | Returns the next page of paginated data   |
|hasPreviousPage|bool       |true    | Returns the previous page of paginated data   |









