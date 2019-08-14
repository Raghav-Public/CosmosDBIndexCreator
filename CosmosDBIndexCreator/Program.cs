using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace CosmosDBIndexCreator
{
    class Program
    {
        /*ConfigurationManager.AppSettings["DatabaseId"];
        private static string collectionId = ConfigurationManager.AppSettings["CollectionId"];

        private static string endpointUrl = ConfigurationManager.AppSettings["EndPointUrl"];
        private static string authorizationKey = ConfigurationManager.AppSettings["AuthorizationKey"];
        */
        private static string databaseId = ConfigurationManager.AppSettings["DatabaseId"];
        private static string collectionId = ConfigurationManager.AppSettings["CollectionId"];

        private static string endpointUrl = ConfigurationManager.AppSettings["EndPointUrl"];
        private static string authorizationKey = ConfigurationManager.AppSettings["AuthorizationKey"];
        private static readonly ConnectionPolicy connectionPolicy = new ConnectionPolicy { UserAgentSuffix = "Halo queries" };

        private static DocumentClient client;

        static int Main(string[] args)
        {
            try
            {
                if(args.Length != 4)
                {
                    Console.WriteLine("Please provide Endpoint, Authkey, Database, Collection info");
                    return 1;
                }
                ParseArgs(args);
                Console.WriteLine("Using Endpoint: " + endpointUrl);
                Console.WriteLine("Using AuthKey: " + authorizationKey);
                Console.WriteLine("Using DB: " + databaseId);
                Console.WriteLine("Using Collection: " + collectionId);

                using (client = new DocumentClient(new Uri(endpointUrl), authorizationKey))
                {

                    List<string> idPaths = new List<string>();
                    idPaths.Add(ConfigurationManager.AppSettings["Idpath1"]);
                    idPaths.Add(ConfigurationManager.AppSettings["Idpath2"]);
                    idPaths.Add(ConfigurationManager.AppSettings["Idpath3"]);
                    idPaths.Add(ConfigurationManager.AppSettings["Idpath4"]);

                    AddNewIndexTerms(idPaths).Wait();

                }
            }
            catch (Exception e)
            {
                LogException(e);
            }
            finally
            {
                Console.WriteLine("done...");
                //Console.ReadKey();
            }
            return 0;
        }
        private static async Task AddNewIndexTerms(List<string> newPaths)
        {
            var collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);


            Console.WriteLine("\n1. Reading document collection for Database:{0} - Collection:{1}", databaseId, collectionId);
            DocumentCollection collection = await client.ReadDocumentCollectionAsync(collectionUri);

            Console.WriteLine("Collection {0} with index policy \n{1}", collection.Id, collection.IndexingPolicy);

            foreach (string s in newPaths)
            {
                Console.WriteLine("Adding new path {0} to Collection {1}", collection.Id, s);

                collection.IndexingPolicy.IncludedPaths.Add(new IncludedPath { Path = s });
            }
            await client.ReplaceDocumentCollectionAsync(collection);

        }
        private static void LogException(Exception e)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            Exception baseException = e.GetBaseException();
            if (e is DocumentClientException)
            {
                DocumentClientException de = (DocumentClientException)e;
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            else
            {
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            Console.ForegroundColor = color;
        }
        private static void ParseArgs(string[] args)
        {
            if (!string.IsNullOrEmpty(args[0]))
            {
                endpointUrl = args[0];
            }
            if (!string.IsNullOrEmpty(args[1]))
            {
                authorizationKey = args[1];
            }
            if (!string.IsNullOrEmpty(args[2]))
            {
                databaseId = args[2];
            }
            if (!string.IsNullOrEmpty(args[3]))
            {
                collectionId = args[3];
            }
        }
    }
}
