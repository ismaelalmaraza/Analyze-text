using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Azure;
using Azure.AI.TextAnalytics;
// Import namespaces
namespace text_analysis
{    
    class Program    
    {        
        static void Main(string[] args)        
        {            
            try            
            {                
                // Get config settings from AppSettings                
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");                
                IConfigurationRoot configuration = builder.Build();                
                string cogSvcEndpoint = configuration["CognitiveServicesEndpoint"];                
                string cogSvcKey = configuration["CognitiveServiceKey"];     
                          
                // Create client using endpoint and key                
                AzureKeyCredential credentials = new AzureKeyCredential(cogSvcKey);                
                Uri endpoint = new Uri(cogSvcEndpoint);                
                TextAnalyticsClient CogClient = new TextAnalyticsClient(endpoint, credentials);  

                // Analyze each text file in the reviews folder                
                var folderPath = Path.GetFullPath("./reviews");                  
                DirectoryInfo folder = new DirectoryInfo(folderPath);
                var file = folder.GetFiles("review1.txt");
                    
                StreamReader sr = new StreamReader("./reviews/review1.txt");                    
                var text = sr.ReadToEnd();                    
                sr.Close();                    

                bool salir = false;
 
                while (!salir) 
                {

                    Console.WriteLine("Text from your file\n" + text);       
                    Console.WriteLine("1. Get language");
                    Console.WriteLine("2. Get sentiment");
                    Console.WriteLine("3. Get key phrases");
                    Console.WriteLine("4. Get entities");
                    Console.WriteLine("Get linked entities");
                    Console.WriteLine("Select an option");
                    int opcion = Convert.ToInt32(Console.ReadLine());

                    switch (opcion)
                    {
                        case 1:
                            // Get language                    
                            DetectedLanguage detectedLanguage = CogClient.DetectLanguage(text);                    
                            Console.WriteLine($"\nLanguage: {detectedLanguage.Name}"); 
                            break;
            
                        case 2:
                            // Get sentiment                    
                            DocumentSentiment sentimentAnalysis = CogClient.AnalyzeSentiment(text);                    
                            Console.WriteLine($"\nSentiment: {sentimentAnalysis.Sentiment}");
                            break;
            
                        case 3:
                            // Get key phrases                    
                            KeyPhraseCollection phrases = CogClient.ExtractKeyPhrases(text);                    
                            if (phrases.Count > 0)                    
                            {                        
                                Console.WriteLine("\nKey Phrases:");                        
                                foreach(string phrase in phrases)                        
                                {                            
                                    Console.WriteLine($"\t{phrase}");                        
                                }                    
                            }
                            break;

                        case 4:
                            // Get entities                    
                            CategorizedEntityCollection entities = CogClient.RecognizeEntities(text);                    
                            if (entities.Count > 0)                    
                            {                        
                                Console.WriteLine("\nEntities:");                        
                                foreach(CategorizedEntity entity in entities)                        
                                {                            
                                    Console.WriteLine($"\t{entity.Text} ({entity.Category})");                        
                                    }                    
                            } 
                            salir = true;
                            break;
                        case 5:
                            // Get linked entities                    
                            LinkedEntityCollection linkedEntities = CogClient.RecognizeLinkedEntities(text);                    
                            if (linkedEntities.Count > 0)                    
                            {                        
                                Console.WriteLine("\nLinks:");                        
                                foreach(LinkedEntity linkedEntity in linkedEntities)                        
                                {                            
                                    Console.WriteLine($"\t{linkedEntity.Name} ({linkedEntity.Url})");                        
                                }                    
                            } 
                            break;
                        default:
                            Console.WriteLine("Elige una opcion entre 1 y 4");
                            break;
                    }
                }
            }            
            catch (Exception ex)            
            {                
                Console.WriteLine(ex.Message);            
            }        
        }    
    }
}