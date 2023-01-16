using Library.Models;
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

        //I created a method to hide/show admin button by user
        public static void AdminPanelButtonVisibility()
        {
            //I hide the button by default
            LibraryMain.btnAdminPanel.Visibility = Visibility.Hidden;
            //I connected to the database to get the user type
            using (LibraryContext context = new LibraryContext())
            {
                //I assigned user data from database to var variable with user information
                var vrUserTypeId = context.TblUsers.Where(pr => pr.Email == Logging.LoggedUser).FirstOrDefault();

                //If there is no required user I made it check
                if (vrUserTypeId != null)
                {
                    //I check if the user type is admin
                    if (vrUserTypeId.UserType == 3)
                    {
                        //if admin i make it visible
                        LibraryMain.btnAdminPanel.Visibility = Visibility.Visible;
                    }
                }
            }
        }



        //https://stackoverflow.com/questions/3243348/how-to-call-a-method-daily-at-specific-time-in-c
        //After the program is opened, I created the following method to run the method I created for checking the books whose return period comes after a certain period of time.
        public static void WaitForMethodRun()
        {
            //Assign to var variable by adding 15 seconds to login time
            var vrRun = Logging.dtTime + TimeSpan.FromSeconds(15);

            //I want the method to run if the time I specified has passed
            if (vrRun <= DateTime.Now)
            {
                GetControlStudentReturnTime();
            }
            //With the delay time I run the method again until the time has elapsed
            else
            {
                var vrDelayTime = vrRun - DateTime.Now;
                Task.Delay(vrDelayTime).ContinueWith(_ => GetControlStudentReturnTime());
            }
        }
        public static void GetControlStudentReturnTime()
        {
            //I am connecting to database for user return notification.
            using (LibraryContext context = new LibraryContext())
            {
                //I'm defining it to add information to the TblNotifications database.
                TblNotifications Notifications = new TblNotifications();

                //I take the information of the books that have expired, together with the user information, and assign them to the var variable.
                var vrReturnTime = context.TblTakenBooks.Where(pr => pr.Email == Logging.LoggedUser).Cast<TblTakenBooks>().ToList();

                //We will convert the selected item's information into an array with the foreach loop and process it.
                foreach (var vrReturn in vrReturnTime)
                {
                    //We check if the return period has expired
                    if (vrReturn.ReturnDate < DateTime.Now)
                    {
                        //I'm putting the information into the database
                        MessageBox.Show("The return period of book '" + vrReturn.BookName + "' has passed. Please return the book without further delay!");
                        Notifications.Email = vrReturn.Email;
                        Notifications.DisplayMessage = "The return period of book '" + vrReturn.BookName + "' has passed. Please return the book without further delay!";
                        Notifications.Time = DateTime.Now;

                        //I check if it is registered in the database so that I do not receive the same notification again
                        var vrSearchTheSame = context.TblNotifications.Where(pr => (pr.DisplayMessage == Notifications.DisplayMessage) && pr.Email == Logging.LoggedUser).FirstOrDefault();

                        try
                        {
                            //If there is, I update the time of that notification.
                            if (vrSearchTheSame != null)
                            {
                                vrSearchTheSame.Time = DateTime.Now;
                                context.TblNotifications.Update(vrSearchTheSame);
                            }
                            else
                            {
                                //I add the notification to the database.
                                context.TblNotifications.Add(Notifications);
                            }
                            //I save the changes
                            context.SaveChanges();
                        }
                        catch (Exception E)
                        {
                            //If there is an error in saving, I show an error message.
                            MessageBox.Show("An error has occured while registering. Error is: \n" + E.Message.ToString() + "\n\n" + E?.InnerException?.Message);
                            return;
                        }
                    }
                }
            }

        }

        public static void GetUserInformation()
        {
            //I connect to database to get user information
            using (LibraryContext context = new LibraryContext())
            {
                //I receive information by e-mail of the user logged in from the database
                var VrUser = context.TblUsers.Where(pr => pr.Email == Logging.LoggedUser).FirstOrDefault();

                if (VrUser != null)
                {
                    //I send content information to the labels I created on the library screen
                    LibraryMain.lblUsername.Content = VrUser.Firstname.ToUpper() + " " + VrUser.Lastname.ToUpper();
                    LibraryMain.lblNumber.Content = VrUser.SchoolNumber;
                }

            }
        }
        public static void GetBooksInformation()
        {
            //I connect to database to get book information
            using (LibraryContext context = new LibraryContext())
            {
                //I am getting information in TblBooks database
                context.TblBooks.OrderBy(pr => pr.BookId).Take(1000).Load();
                //I'm sending the data from the database to the datagrid
                LibraryMain.dataGridBooks.ItemsSource = context.TblBooks.Local.ToBindingList();
            }
        }

        public static void GetBooksOnMe(TheBooksOnMe TheMain)
        {
            //I connect to database to get books on user
            using (LibraryContext context = new LibraryContext())
            {
                //With the e-mail of the logged in user, I receive the books the user bought from the database.
                context.TblTakenBooks.Where(pr => pr.Email == Logging.LoggedUser).OrderBy(pr => pr.TakenBooksId).Take(1000).Load();
                //I'm sending the data from the database to the datagrid
                TheMain.datagridBooksOnMe.ItemsSource = context.TblTakenBooks.Local.ToBindingList();
            }
        }

        public static void GetNotifications(Notifications MainNotification)
        {
            //I connect to database to get notifications
            using (LibraryContext context = new LibraryContext())
            {
                //I receive notifications of the user from the database with the e-mail of the logged in user.
                context.TblNotifications.Where(pr => pr.Email == Logging.LoggedUser).OrderBy(pr => pr.NotificationsId).Take(1000).Load();
                //I'm sending the data from the database to the datagrid
                MainNotification.datagridNotifications.ItemsSource = context.TblNotifications.Local.ToBindingList();

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

        //I created a method to log out
        public static void TryLogout()
        {
            //Reset general user information
            Logging.LoggedUser = "";
            //I closed the LibraryWindow by reopening the Login screen with a success message
            Login LGWindow = new Login();
            MessageBox.Show("You have successfully logged-out");
            LGWindow.Show();
            LibraryMain.Close();
        }

    }
}
