/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Client;
using HP.Extensibility.Client.Discovery;
using HP.Extensibility.Client.JobStatistics;
using HP.Extensibility.Service.JobStatistics;
using HP.Extensibility.Types.Common;
using OXPd2ExamplesHost.Models;
using System;
using System.Net.Http;
using IHttpClientFactory = OXPd2ExamplesHost.Utilities.IHttpClientFactory;

namespace OXPd2ExamplesHost.Services
{
    /// <summary>
    /// Defines the interface to the service that provides the business logic of the Job Statistics Service examples
    /// </summary>
    public interface IJobStatisticsService
    {
        /// <summary>
        /// Demonstrate SDK use-case of interacting with the JobStatistics Service Capabilities resource
        /// </summary>
        /// <returns>Service.JobStatistics.Capabilities</returns>
        Capabilities GetCapabilities();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the JobStatistics Service JobStatisticsAgents resource
        /// Retrieves the JobStatisticsAgents registered on the device.
        /// </summary>
        /// <returns>Service.JobStatistics.JobStatisticsAgents</returns>
        JobStatisticsAgents EnumerateJobStatisticsAgents();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the JobStatistics Service JobStatisticsAgent resource
        /// Retrieves the JobStatisticsAgent with the specified agentId.
        /// </summary>
        /// <param name="agentId">The agentId of the JobStatisticsAgent</param>
        /// <returns>Service.JobStatistics.JobStatisticsAgent</returns>
        JobStatisticsAgent GetJobStatisticsAgent(string agentId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the JobStatistics Service Jobs resource
        /// </summary>
        /// <param name="agentId">The agentId of the job statistics agent</param>
        /// <returns>Service.JobStatistics.Jobs</returns>
        Jobs EnumerateJobs(string agentId);

        /// <summary>
        /// Demonstrate SDK use-case of modifying the JobStatistics Service Jobs resource
        /// </summary>
        /// <param name="agentId">The agentId of the job statistics agent</param>
        /// <param name="jobs_Modify">the object used to modify the jobs resource</param>
        /// <returns>Service.JobStatistics.Jobs</returns>
        Jobs ModifyJobs(string agentId, Jobs_Modify jobs_Modify);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the JobStatistics Service Job resource
        /// </summary>
        /// <param name="agentId">The agentId of the job statistics agent</param>
        /// <param name="sequnceNumber">The sequenceNumber of the job</param>
        /// <returns>Service.JobStatistics.Job</returns>
        Job GetJob(string agentId, SequenceNumber sequence);
    }


    /// <summary>
    /// Implements the business logic of the JobStatistics examples
    /// </summary>
    public class JobStatisticsService : IJobStatisticsService
    {
        #region Constructor

        private IHttpClientFactory httpClientFactory = null;
        private IDeviceManagementService deviceManagementService;

        public JobStatisticsService(IHttpClientFactory httpClientFactory, IDeviceManagementService deviceManagementService) : this()
        {
            this.httpClientFactory = httpClientFactory;
            this.deviceManagementService = deviceManagementService;
        }

        private JobStatisticsService() { }

        #endregion // Constructor


        #region IJobStatisticsService implementation

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the JobStatistics Service Capabilities resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.JobStatistics.Capabilities</returns>
        public Capabilities GetCapabilities()
        {
            // @StartCodeExample:GetCapabilities
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the JobStatisticsServiceClient
            JobStatisticsServiceClient client = new JobStatisticsServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Capabilities capabilities = client.Capabilities.GetAsync().Result;
            // @EndCodeExample

            return capabilities;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the JobStatistics Service JobStatisticsAgents resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.JobStatistics.JobStatisticsAgents</returns>
        public JobStatisticsAgents EnumerateJobStatisticsAgents()
        {
            // @StartCodeExample:EnumerateJobStatisticsAgents
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the JobStatisticsServiceClient
            JobStatisticsServiceClient client = new JobStatisticsServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            JobStatisticsAgents jobStatisticsAgents = client.JobStatisticsAgents.GetAsync(accessToken).Result;
            // @EndCodeExample

            return jobStatisticsAgents;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Job Statistics Service JobStatisticsAgent Resource
        /// </summary>
        /// <param name="agentId">The agentId of the job statistics agent</param>
        /// <returns>HP.Extensibility.Service.JobStatistics.JobStatisticsAgent</returns>
        public JobStatisticsAgent GetJobStatisticsAgent(string agentId)
        {
            // @StartCodeExample:GetJobStatisticsAgent
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider Owner
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the JobStatisticsServiceClient
            JobStatisticsServiceClient client = new JobStatisticsServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            JobStatisticsAgent jobStatisticsAgent = client.JobStatisticsAgents[agentId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return jobStatisticsAgent;
        }

        public Jobs EnumerateJobs(string agentId)
        {
            // @StartCodeExample:EnumerateJobs
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the JobStatisticsServiceClient
            JobStatisticsServiceClient client = new JobStatisticsServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Jobs jobs = client.JobStatisticsAgents[agentId].Jobs.GetAsync(accessToken).Result;
            // @EndCodeExample

            return jobs;
        }

        public Jobs ModifyJobs(string agentId, Jobs_Modify jobs_Modify)
        {
            // @StartCodeExample:ModifyJobs
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the JobStatisticsServiceClient
            JobStatisticsServiceClient client = new JobStatisticsServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Jobs jobs = client.JobStatisticsAgents[agentId].Jobs.ModifyAsync(accessToken, jobs_Modify).Result;
            // @EndCodeExample

            return jobs;
        }

        public Job GetJob(string agentId, SequenceNumber sequenceNumber) {
            // @StartCodeExample:GetJob
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the JobStatisticsServiceClient
            JobStatisticsServiceClient client = new JobStatisticsServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Job job = client.JobStatisticsAgents[agentId].Jobs[sequenceNumber].GetAsync(accessToken).Result;
            // @EndCodeExample

            return job;
        }

        public void ConstructorExamples()
        {
            string deviceNetworkAddress = string.Empty;
            ServicesDiscovery discoveryTree = null;
            JobStatisticsServiceClient client = null;

            // @StartCodeExample:ConstructorExamples
            // Construct the HttpClientHandler and HttpClient. In this case we are using a handler
            // that will not be performing server certificate validation...
            HttpClientHandler clientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            HttpClient httpClient = new HttpClient(clientHandler, true);

            // Here's the standard constructor using the device's network address and a "discovery tree"
            // instance that's already been fetched from the device (not shown)
            client = new JobStatisticsServiceClient(httpClient, deviceNetworkAddress, discoveryTree);

            // Here's a test/debug constructor that constructs a client that will use the provided service URI
            // as the root endpoint for all resource interactions
            Uri mockServiceUri = new Uri("http://localhost:5000/mockJobStatistics");
            client = new JobStatisticsServiceClient(httpClient, mockServiceUri);

            // @EndCodeExample
        }

        #endregion // IJobStatisticsExamplesService implementation

        public void ExceptionHandlingExample()
        {
            Device currentDevice = deviceManagementService.CurrentDevice;
            string agentId = string.Empty;
            AccessToken accessToken = string.Empty;

            // @StartCodeExample:ExceptionHandlingExample
            try
            {
                // Fetch the discovery tree
                DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
                ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

                // Construct the JobStatisticsServiceClient
                JobStatisticsServiceClient client = new JobStatisticsServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    if (e.InnerException is OXPdHttpRequestException)
                    {
                        var oxpdException = (OXPdHttpRequestException)e.InnerException;
                        if (oxpdException.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            // Can inspect the errors to see what the caused the BadRequest.
                            var errors = oxpdException.ErrorResponse;
                        }
                        else if (oxpdException.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            // Possible issue with a token.
                        }
                        // ... etc.
                    }
                }
            }
            // @EndCodeExample
        }
    }
}
