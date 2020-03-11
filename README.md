# AdGate - Manual Advertisement Bidding
The aim of the system is to allow content publishers to earn more revenue on ad viewings while not decreasing their ability to efficiently search for appropriate ads for their viewers and also allow advertisers to be able to build trust with publishers easily for confidence in the representation of their ads.

# AdGate - Project Details
<details>
  <summary>Objectives</summary>
  
  * To implement a way for publishers to post ad listings that can be viewed by advertisers.
  * To use tags to mark ad listings so that it can reach relevant advertisers much easier.
  * To use tags to mark advertiser profiles so that they can be found by publishers easier.
  * To implement a fixed pricing on ad listings to ensure that publishers will get what they need.
</details>

<details>
  <summary>Deliverables</summary>
  
  #### Advertisers
  * Create a profile with company details
  * Manage profile (Edit).
  * Link tags representing the type of products and services the company provides.
  * Browse ad listings.
  * Purchase ad listings.
  * Communicate with publisher.
  
  #### Publishers
  * Create multiple content profiles containing information about publisher content.
  * Manage content profile (Edit, Delete, Read).
  * Manage own profile (Edit).
  * Link tags representing types of products and services publishers are looking for to content profiles.
  * Browse advertiser profile.
  * Confirm sale of ad listing.
  * Communicate with advertiser.

  #### Administrators
  * View all Publishers and Advertisers.
  * View all Administrators.
  * Deactivate/Activate publisher and advertiser accounts.
  * Deactivate/Activate admin accounts.
  * View publisher or advertiser profiles.

</details>

# Technologies Used

<details>
  <summary>1. Microsoft Azure</summary>

  #### What?
  Azure is a cloud platform providing solutions for Infrastructure as a Service (IaaS), Software as a Service (SaaS) and also Platform as a Service (PaaS) which can be utilized for analytics, storage services, networking options as well as virtual computing. It is very flexible in that it is capable of performing compute changes as needed, it can be used in almost any OS, it has an availability rate of 99.95% as well as a full 24 hours 7 days a week support, it has data centres almost all around the world to provide global data access and finally it relies on the pay-on-the-go structure meaning that developers will only need to pay for what they use by the minute.

  #### Why?
  1. Application Deployment: Azure offers a service called App Services which provisions resources hardware resources remotely to allow users to deploy their web application which can then be accessed through the generated url.  
  2. Performance Analytics: A benefit of using App Services to host the application is that, since it is a fully managed "Platform as a Service (PaaS)", it provides performance analysis such as user load, response time and number of requests. Besides that, there is also performance testing feature where users are allowed to simulate user loads to get an analysis and summary of the application's performance and availability metrics.
  3. Azure SQL Database: Azure also provides an sql database hosting option which allows users to basically store their data on the cloud instead. As expected this would also include analysis of requests, reads and writes and can be horizontally and vertically scaled just as App Services.
  4. Blob Storage: One more thing that needs to be considered when making any online application is the user generated content such as images, documents and more. Blob Storage is one of Azure's storage options which allows developers to store large object data and then provides a uri which can then be used directly to download the object from the browser.

</details>
  
<details>
  <summary>2. ASP.NET Core</summary>
  
  #### What?
  NET Core is an open-source, general-purpose development platform maintained by Microsoft and the .NET community on GitHub. It's cross-platform (supporting Windows, macOS, and Linux) and can be used to build device, cloud, and IoT applications.
  
  #### Why?
  1. Cross-Platform: ASP.NET Core is able to run on all operating systems: Windows; Linus; and MacOS.
  2. Open source: It is open source and hence, there are lots of guides, tutorial and examples online for easy learning.
  3. Integrate well with Azure App Services: Azure has a service called App Services which is able to host ASP.NET Core applications easily and because Visual Studio is used to develop the application, deploying the application to Azure App Services is as easy as publishing the application after configuring it.
  
</details>
  
<details>
  <summary>3. CSS</summary>
  
  When creating the ASP.NET Core application, there is already a bootstrap css used by default. However, to there was a need to customize and create a different look for the application to make it more appealing. 
</details>
  
<details>
  <summary>4. Visual Studio</summary>
</details>


