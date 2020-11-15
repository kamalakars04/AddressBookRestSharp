// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileName.cs" company="Bridgelabz">
//   Copyright © 2018 Company
// </copyright>
// <creator Name="Your name"/>
// --------------------------------------------------------------------------------------------------------------------
namespace AddressBookADOMSTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using AddressBookADONET;
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using RestSharp;
    using RestSharp.Serializers;
    using RestSharp.Deserializers;
    using System.IO;
    using System.Text;

    [TestClass]
    public class UnitTest1
    {
        public AddressBookRepo bookRepo;
        RestClient client;

        [TestInitialize]
        public void setup()
        {
            bookRepo = new AddressBookRepo();
            client = new RestClient("http://localhost:3000");
        }

        
        public void CreateJsonFile()
        {
            // Get all the contacts from database and add them to json
            // First method to create a json file
            List<ContactDetails> contacts = bookRepo.GetContacts();
            StreamWriter writer = new StreamWriter(@"C:\Users\kamalakar\Desktop\bridge labs\AddressBookRestSharp\AddressBookRestSharp\Contacts1.json");
            JsonTextWriter jsonTextWriter = new JsonTextWriter(writer);
            jsonTextWriter.Formatting = Formatting.Indented;
            jsonTextWriter.WriteStartObject();
            jsonTextWriter.WritePropertyName("Contacts");
            jsonTextWriter.WriteStartArray();
            foreach (ContactDetails contact in contacts)
            {
                jsonTextWriter.WriteStartObject();
                jsonTextWriter.WritePropertyName(nameof(contact.FirstName));
                jsonTextWriter.WriteValue(contact.FirstName);
                jsonTextWriter.WritePropertyName(nameof(contact.LastName));
                jsonTextWriter.WriteValue(contact.LastName);
                jsonTextWriter.WritePropertyName(nameof(contact.PhoneNumber));
                jsonTextWriter.WriteValue(contact.PhoneNumber);
                jsonTextWriter.WritePropertyName(nameof(contact.Email));
                jsonTextWriter.WriteValue(contact.Email);
                jsonTextWriter.WritePropertyName(nameof(contact.Address));
                jsonTextWriter.WriteValue(contact.Address);
                jsonTextWriter.WritePropertyName(nameof(contact.DateAdded));
                jsonTextWriter.WriteValue(contact.DateAdded);
                jsonTextWriter.WritePropertyName(nameof(contact.zip.zip));
                jsonTextWriter.WriteValue(contact.zip.zip);
                jsonTextWriter.WritePropertyName(nameof(contact.zip.city));
                jsonTextWriter.WriteValue(contact.zip.city);
                jsonTextWriter.WritePropertyName(nameof(contact.zip.state));
                jsonTextWriter.WriteValue(contact.zip.state);

                // For each addressbook name
                foreach (KeyValuePair<string, List<string>> keyValuePair in contact.bookNameContactType)
                {
                    jsonTextWriter.WritePropertyName("AddressBook Name");
                    jsonTextWriter.WriteValue(keyValuePair.Key);
                    jsonTextWriter.WritePropertyName("contactType");
                    StringBuilder sb = new StringBuilder();
                    foreach (string contactType in keyValuePair.Value)
                    {
                        sb.Append(contactType);
                        sb.Append("   ");
                    }

                    jsonTextWriter.WriteValue(sb.ToString());
                }
                jsonTextWriter.WriteEndObject();
                jsonTextWriter.WriteRaw("\n");
            }
            jsonTextWriter.WriteEndArray();
            jsonTextWriter.WriteEndObject();
            jsonTextWriter.Flush();

            // Second method to create a JSON file
            // Get all the contacts from database and add them to json
            List<ContactDetails> contacts1 = bookRepo.GetContacts();
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            StreamWriter writer1 = new StreamWriter(@"C:\Users\kamalakar\Desktop\bridge labs\AddressBookRestSharp\AddressBookRestSharp\Contacts.json");
            JsonTextWriter jsonTextWriter1 = new JsonTextWriter(writer1);
            jsonTextWriter1.WriteStartObject();
            jsonTextWriter1.WritePropertyName("Contacts");
            serializer.Serialize(jsonTextWriter1, contacts1);
            jsonTextWriter1.WriteEndObject();
            jsonTextWriter1.Flush();
            
        }

        [TestMethod]
        public void GivenApiGetAllContacts()
        {
            // Get all contacts from json file to display
            IRestRequest request = new RestRequest("/Contacts", Method.GET);

            // Execute request
            IRestResponse response = client.Execute(request);

            // Assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }
    }
}
