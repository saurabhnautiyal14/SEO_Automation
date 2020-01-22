##For candidates applying for full-stack roles, we expect to see a front-end component delivered as part of this component, demonstrating knowledge of React and JavaScript best practices.

##SEO optimization: 
===================

##DESING APPROACH: 
===================
- The project is built using .NET CORE 3.1 and React 16.11.0 (Typescript)
- Since requirement says that we can't use any 3rd party lib and Google API search, the method adopted here is via a testing tool Selenium (third party testing tool) 

Backend approch: An api is exposed with url as follows. 
				 https://localhost:5001/api/searchRating
				 Input Params : 1) searchString (comma seperated string to search) 
								2) url : The url which matches the search result based on ranking. 
								
				Using the selenium chrome driver an instance is launched and text is searched. Screen scraping help to capture all the links and form a list of unique links (URL) based on the search.
				The process continues until part/full url is matched or we have reached top 100 unique results. 
				Then the ranking is calculated and final response in serialized in JSON response to send to the front-end.
				
Front-end approch: 
				The UI is kept simple and intutive. 
				The user needs to enter searchKeyword (seperated by Comma) and the url to find the match for ranking. 
				The request is submitted in async form and can accept multiple request in one go. 
				   
				There are notification toasters availble to notify the user of the current status or errors if any. (Any backend or network error is taken care of)
				The results are displayed in a list format below the page and user has the option to clear the results if required. 
				
Current status of Proeject: 
1) The end to end is working fine as of now. However extension (1) and (2) needs some more time to implement. 


PEDNING WORK: 
==================
1) Create an interface in backend to implement generic approach for multiple search engines. (Google.com and Bing.com)
2) Implement a rate limiter in backend to limit the rate of processing as per time. 

2) Optimize the backend to create async task so that performace is faster.
3) Need to cover unit test case for both front-end and backend. 
4) The project can be scalable by creating docker instances and deploying to any cloud. This would help for auto scaling, high availblity and automated deployment.