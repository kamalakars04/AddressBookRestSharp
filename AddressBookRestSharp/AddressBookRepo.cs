// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileName.cs" company="Bridgelabz">
//   Copyright © 2018 Company
// </copyright>
// <creator Name="Your name"/>
// --------------------------------------------------------------------------------------------------------------------
namespace AddressBookADONET
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Text;
    using System.Threading.Tasks;

    public class AddressBookRepo
    {
        // Create connection string 
        public static string connectionString = @"Server=LAPTOP-CTKSHLKD\SQLEXPRESS; Initial Catalog =Address_BookDB; User ID = sa; Password=kamal@99";
        SqlConnection connection = new SqlConnection(connectionString);

        /// <summary>
        /// UC 16 Gets all contacts.
        /// </summary>
        public List<ContactDetails> GetContacts(string firstName = null, string lastName = null)
        {
            try
            {
                // Open connection
                connection.Open();

                // Declare a command
                SqlCommand command = new SqlCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "dbo.GetAllContacts";
                command.Connection = connection;

                // To get only one contact
                if (firstName != null && lastName != null)
                {
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@lastName", lastName);
                }
                SqlDataReader reader = command.ExecuteReader();
                List<ContactDetails> contactList = new List<ContactDetails>();

                // Read all the details
                contactList = ReadContactDetails(reader);

                // Display all the contacts
                contactList.ForEach(contact => contact.Display());
                reader.Close();
                return contactList;
            }
            catch
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
                return null;
            }
            finally
            {

                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        public List<ContactDetails> ReadContactDetails(SqlDataReader reader)
        {
            try
            {
                List<ContactDetails> contactList = new List<ContactDetails>();
                ContactDetails contact;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // If the contact already exists then add in same contact
                        // Else new contact
                        contact = contactList.Find(con => con.FirstName == reader[0].ToString()
                                                    && con.LastName == reader[1].ToString());

                        // Read into contact details
                        if (contact == null)
                        {
                            contact = new ContactDetails();
                            contact.FirstName = reader[0].ToString();
                            contact.LastName = reader[1].ToString();
                            contact.PhoneNumber = reader[2].ToString();
                            contact.Email = reader[3].ToString();
                            contact.Address = reader[4].ToString();
                            contact.zip.zip = reader[5].ToString();
                            contact.DateAdded = Convert.ToDateTime(reader[6]);
                            contact.zip.city = reader[7].ToString();
                            contact.zip.state = reader[8].ToString();
                            contact.bookNameContactType.Add(reader[9].ToString(), new List<string> { reader[10].ToString() });
                            contactList.Add(contact);
                        }

                        // If contact already exists then add only book name and type
                        else
                        {
                            if (contact.bookNameContactType.ContainsKey(reader[9].ToString()))
                                contact.bookNameContactType[reader[9].ToString()].Add(reader[10].ToString());
                            else
                                contact.bookNameContactType.Add(reader[9].ToString(), new List<string> { reader[10].ToString() });
                        }
                    }
                }
                return contactList;
            }
            catch
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
                return null;
            }
            finally
            {

                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        /// <summary>
        /// UC 17 Updates the contact.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="column">The column.</param>
        /// <param name="newValue">The new value.</param>
        public void UpdateContact(string firstName, string lastName, string column, string newValue)
        {
            try
            {
                // Open connection
                SqlConnection connect = new SqlConnection(connectionString);
                connect.Open();

                // Declare a command and give all its properties
                SqlCommand command = new SqlCommand();
                command.CommandText = "update contactdetails set " + column + " = '" + newValue + "' where firstname = '"
                                        + firstName + "' and lastname = '" + lastName + "'";
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
            catch
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            finally
            {

                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        /// <summary>
        /// UC 18 Gets the contacts added in period.
        /// </summary>
        /// <param name="startdate">The startdate.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public List<ContactDetails> GetContactsAddedInPeriod(DateTime startdate, DateTime endDate)
        {
            try
            {
                // Open connection
                connection.Open();

                // Declare a command and give all its properties
                SqlCommand command = new SqlCommand();
                command.CommandText = "select * from GetContacts() where DateAdded between '"+
                                        startdate.ToString() + "' and '"+ endDate.ToString() + "'";
                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();
                List<ContactDetails> contactList = new List<ContactDetails>();

                // Read all the details
                contactList = ReadContactDetails(reader);

                // Display all the contacts
                contactList.ForEach(contact => contact.Display());
                reader.Close();
                return contactList;
            }
            catch
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
                return null;
            }
            finally
            {

                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        /// <summary>
        /// UC 19 Gets the state of the contacts by city or.
        /// </summary>
        /// <param name="city">The city.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public List<ContactDetails> GetContactsByCityOrState(string city, string state)
        {
            try
            {
                // Open connection
                connection.Open();

                // Declare a command and give all its properties
                SqlCommand command = new SqlCommand();
                command.CommandText = "select * from GetContacts() where city = '" +
                                        city + "' and state =  '" + state + "'";
                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();
                List<ContactDetails> contactList = new List<ContactDetails>();

                // Read all the details
                contactList = ReadContactDetails(reader);

                // Display all the contacts
                contactList.ForEach(contact => contact.Display());
                reader.Close();
                return contactList;
            }
            catch
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
                return null;
            }
            finally
            {

                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        /// <summary>
        /// UC 20 Adds the new contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public void AddNewContact(ContactDetails contact)
        {
            SqlConnection connect = new SqlConnection(connectionString);
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                // Open connection
                connect.Open();

                // Declare a command
                SqlCommand command = new SqlCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "dbo.AddNewContact";
                command.Connection = connect;
                foreach(KeyValuePair<string ,List<string>> bookNameType in contact.bookNameContactType)
                {
                    foreach(string type in bookNameType.Value)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@firstName", contact.FirstName);
                        command.Parameters.AddWithValue("@lastName", contact.LastName);
                        command.Parameters.AddWithValue("@phoneNumber", contact.PhoneNumber);
                        command.Parameters.AddWithValue("@Email", contact.Email);
                        command.Parameters.AddWithValue("@address", contact.Address);
                        command.Parameters.AddWithValue("@zip", contact.zip.zip);
                        command.Parameters.AddWithValue("@city", contact.zip.city);
                        command.Parameters.AddWithValue("@state", contact.zip.state);
                        command.Parameters.AddWithValue("@addressBookName", bookNameType.Key);
                        command.Parameters.AddWithValue("@contactType", type);
                        var result = command.ExecuteNonQuery();
                    }
                }
                stopwatch.Stop();
                Console.WriteLine("Time taken for adding one contact is {0}",stopwatch.ElapsedMilliseconds);
            }
            catch
            {
                if (connect.State == System.Data.ConnectionState.Open)
                    connect.Close();
            }
            finally
            {

                if (connect.State == System.Data.ConnectionState.Open)
                    connect.Close();
            }
        }

        /// <summary>
        /// UC 21 InsertMultipleContactsWithThreads
        /// </summary>
        /// <param name="contactList"></param>
        public void InsertMultipleContactsWithThreads(List<ContactDetails> contactList)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Parallel.ForEach(contactList, contact => AddNewContact(contact));
            stopwatch.Stop();
            Console.WriteLine("Time taken for adding multiple contacts is : {0}",stopwatch.ElapsedMilliseconds);
        }
    }
}
