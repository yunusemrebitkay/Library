using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace Library
{
    public static class GlobalMethods
    {
        //I have defined the information from the LibraryWindow window to the current class under the title "LibraryMain"
        public static LibraryWindow LibraryMain;

        //https://stackoverflow.com/questions/5342375/regex-email-validation
        //I created a method that checks an email with the codes I got from the website.
        public static bool EmailControl(string srEmail)
        {
            //The string data I get with the regex method is "@" and "." I checked if it contains
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            //I sent back the checked data.
            return regex.IsMatch(srEmail);
        }

        //To encrypt Sha256, I got the following method from the link I specified.
        //https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
        public static string ComputeSha256Hash(this string rawData)
        {
            //I am creating sha256 
            using (SHA256 sha256Hash = SHA256.Create())
            {
                //I am converting the incoming string data to Byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                //I am converting the converted data to string array
                StringBuilder builder = new StringBuilder();

                //With the for loop, I process and assign as much data as the character length of the data.
                for (int i = 0; i < bytes.Length; i++)
                {
                    //We format the string as two uppercase hexadecimal characters with ToString(x2)
                    builder.Append(bytes[i].ToString("x2"));
                }
                //I'm sending back the resulting encrypted value
                return builder.ToString();
            }
        }

        public static string Encryption(string srPassword, string srRandomString)
        {
            //We run the unique string data and password with sha 256 bit encryption method and send back the result.
            return ComputeSha256Hash(srRandomString + srPassword);
        }


        //https://www.tutorialspoint.com/how-to-get-the-ip-address-in-chash
        public static string IpAdressFinder()
        {
            //Do not assign null value before assigning string ip.
            string IPAddress = string.Empty;
            //I throw default the IPHostEntry method, which associates the hostname with an array of aliases and matching IP addresses
            IPHostEntry Host = default(IPHostEntry);
            //I throw the hostname as null
            string Hostname = null;
            //Learning the hostname and assigning it to this variable
            Hostname = System.Environment.MachineName;
            //I assign after doing host query
            Host = Dns.GetHostEntry(Hostname);
            //I read addresses with foreach loop
            foreach (IPAddress IP in Host.AddressList)
            {
                //I'm throwing the matching ip as the user ip.
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    //I convert to "0,0,0,0" format
                    IPAddress = Convert.ToString(IP);
                }
            }
            //I am sending back the ip address
            return IPAddress;
        }

        //The method we created for the utc server time. I learned this method from class.
        static public class DateTimeHelper
        {
            public static DateTime ServerTime
            {
                get { return DateTime.UtcNow; }
            }
        }

        //I created a generator for school number
        public static int SchoolNumberGenerator()
        {
            //I used Random method to generate random numbers.
            Random rndm = new Random();
            //I used month and day in its generator algorithm
            int irMonth = DateTime.Now.Month;
            int irDay = DateTime.Now.Day;

            //I wanted it to create a school number between 10k and 999k
            string Generator = $"{irMonth}{irDay}{rndm.Next(10000, 999999)}";
            //Then I convert this data I created to int and assign it as a school number.
            int SchoolNumber = Int32.Parse(Generator);
            //I am sending back the result.
            return SchoolNumber;
        }
    }
}
