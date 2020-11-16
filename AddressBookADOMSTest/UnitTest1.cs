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
    using static AddressBookADONET.ContactDetails;
    using Newtonsoft.Json.Linq;
    using System.Net;

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
            StreamWriter writer = new StreamWriter(@"C:\Users\kamalakar\Desktop\bridge labs\AddressBookRestSharp\AddressBookRestSharp\Contacts.json");
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
            StreamWriter writer1 = new StreamWriter(@"C:\Users\kamalakar\Desktop\bridge labs\AddressBookRestSharp\AddressBookRestSharp\Contacts1.json");
            JsonTextWriter jsonTextWriter1 = new JsonTextWriter(writer1);
            jsonTextWriter1.WriteStartObject();
            jsonTextWriter1.WritePropertyName("Contacts");
            serializer.Serialize(jsonTextWriter1, contacts);
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

        /// <summary>
        /// UC 23 AddMultipleContacts
        /// </summary>
        [TestMethod]
        public void GivenApiAddMultipleContacts()
        {
            List<ContactDetails> contactList = new List<ContactDetails>();
            ContactDetails contact = new ContactDetails();
            contact.FirstName = "Manish";
            contact.LastName = "Pandey";
            contact.PhoneNumber = "2344746584";
            contact.Email = "mani.com";
            contact.Address = "maruti colony";
            contact.zip.city = "Warangal";
            contact.zip.state = "AndhraPradesh";
            contact.bookNameContactType.Add("MyBook", new List<string> { "Friend" });
            contactList.Add(contact);

            contact = new ContactDetails();
            contact.FirstName = "Vijay";
            contact.LastName = "Sankar";
            contact.PhoneNumber = "4566544566";
            contact.Email = "sankar.com";
            contact.Address = "colony";
            contact.zip.city = "Khammam";
            contact.zip.state = "AndhraPradesh";
            contact.bookNameContactType.Add("MyBook", new List<string> { "Friend" });
            contactList.Add(contact);

            // Get all contacts from json file to display
            
            
            contactList.ForEach(contact =>
            {
                JObject zipObject = new JObject();
                JObject contactTypeObject = new JObject();
                JArray jArray = new JArray();
                JObject jObjectBody = new JObject();
                //Arrange
                zipObject.Add("zip", contact.zip.zip);
                zipObject.Add("city", contact.zip.city);
                zipObject.Add("state", contact.zip.state);
                jObjectBody.Add("zip", zipObject);
                jObjectBody.Add("FirstName", contact.FirstName);
                jObjectBody.Add("LastName", contact.LastName);
                jObjectBody.Add("PhoneNumber", contact.PhoneNumber);
                jObjectBody.Add("Email", contact.Email);
                jObjectBody.Add("Address", contact.Address);
                jObjectBody.Add("city", contact.zip.city);
                jObjectBody.Add("state", contact.zip.state);
                jObjectBody.Add("DateAdded", contact.DateAdded);
                foreach (KeyValuePair<string, List<string>> keyValuePair in contact.bookNameContactType)
                {
                    jObjectBody.Add("BookNameContactType",contactTypeObject);
                    StringBuilder sb = new StringBuilder();
                    foreach (string contactType in keyValuePair.Value)
                    {
                        // sb.Append(contactType);
                        //sb.Append("   ");
                        jArray.Add(contactType);
                    }
                    contactTypeObject.Add(keyValuePair.Key, jArray);


                }

                //Act
                IRestRequest request = new RestRequest("/Contacts", Method.POST);
                request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            });
        }
    }
}
