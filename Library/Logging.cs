using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Library
{
    public static class Logging
    {
        //I have defined the information from the Login window to the current class under the title "LoginMain"
        public static Login LoginMain;
        //We define the total number of login attempts
        public static int LoginAttempt = 0;
        //I defined a generic string expression to pass user information to other classes
        public static string LoggedUser = "NA";
        //I have defined an input time datetime for a method that should run at program startup.
        public static DateTime dtTime;

        public static void TryLogin()
        {
            //I assigned the var value for the user mail or number coming from the login screen.
            var vrEmailOrNumber = LoginMain.txtbxEmailorNo.Text;
            //I defined the password from the login screen
            string srPassword = LoginMain.pwbxPassLG.Password.ToString();
            //I have created a method to check if user is logged in with mail or school number. Here I am verifying by running that method.
            bool blStatusInfor = blDigitControl(LoginMain.txtbxEmailorNo.Text);

            //I added the rule that the email field cannot be left blank.
            if (vrEmailOrNumber == "")
            {
                MessageBox.Show("Email or school number field can't be left blank!");
                return;
            }

            //I send information and receive validation to the email control method I created in the "GlobalMethods" class.Also, if it's mail, I use general validation to do this check.
            if (!GlobalMethods.EmailControl(vrEmailOrNumber) & blStatusInfor == false)
            {
                MessageBox.Show("Email you entered is in an invalid format. Please re-enter.");
                return;
            }

            //I added the rule that the password field cannot be left blank.
            if (srPassword == "")
            {
                MessageBox.Show("Password field can't be left blank!");
                return;
            }

            //I connect to database to check login information
            using (LibraryContext context = new LibraryContext())
            {
                //I am comparing number and time using IpAdressFinder method to check login attempts. Thus, I check if the maximum number of entries has been reached within 15 minutes.
                var vrIpTestResult = context.TblTryLogin.Where(pr => pr.TriedIp == GlobalMethods.IpAdressFinder() & pr.DateOfTry.AddMinutes(15) > GlobalMethods.DateTimeHelper.ServerTime).Count();

                //If it's more than five, I want it to give a warning
                if (vrIpTestResult > 5)
                {
                    MessageBox.Show("You have made too many incorrect login attempts within 15 minutes. Please try again later!");
                    return;
                }

                //I can check email-password and school number-password by using general authentication.
                if (blStatusInfor == false)
                {
                    //I get the information of the user logged in from the database by email.
                    var vrLoginEmail = context.TblUsers.FirstOrDefault(pr => pr.Email == vrEmailOrNumber);

                    //I show an error message if such an email is not registered.
                    if (vrLoginEmail == null)
                    {
                        MessageBox.Show("There is no such email in the system!");
                        return;
                    }

                    //Since our password is string + sha256, it has been saved in the database with a fixed string assignment method. After converting our email and password with 256 encryption, we compare it in the database.
                    if (vrLoginEmail.Password != GlobalMethods.Encryption(srPassword, vrLoginEmail.SaltOfPassword))
                    {
                        //If it is wrong, we increase the number of login attempts by running the method.
                        LoginAttemps();
                        MessageBox.Show("Your password does not match your email address!");
                        return;
                    }
                    //If the login is successful, we send an e-mail to the general login information we defined above.
                    LoggedUser = vrLoginEmail.Email.ToString();
                }
                else
                {
                    //I get the information of the user logged in from the database by school number.
                    var vrLoginSchoolNum = context.TblUsers.FirstOrDefault(pr => pr.SchoolNumber == Int32.Parse(vrEmailOrNumber));
                    //I show an error message if such an school number is not registered.
                    if (vrLoginSchoolNum == null)
                    {
                        MessageBox.Show("There is no such school number in the system!");
                        return;
                    }

                    //Since our password is string + sha256, it has been saved in the database with a fixed string assignment method. After converting our school number and password with 256 encryption, we compare it in the database.
                    if (vrLoginSchoolNum.Password != GlobalMethods.Encryption(srPassword, vrLoginSchoolNum.SaltOfPassword))
                    {
                        //If it is wrong, we increase the number of login attempts by running the method.
                        LoginAttemps();
                        MessageBox.Show("Your password does not match your school number!");
                        return;
                    }
                    //If the login is successful, we send an e-mail to the general login information we have defined above by getting information from the school number.
                    LoggedUser = vrLoginSchoolNum.Email.ToString();
                }

                //If the login is successful, we reset the number of login attempts.
                LoginAttempt = 0;
                //we assign current time as value to general input time
                dtTime = DateTime.Now;
                //We show the message that you have successfully logged in
                MessageBox.Show("You have successfully logged in.");
                //We show the library main window and close the current window
                LibraryWindow LbWin = new LibraryWindow();
                LbWin.Show();
                LoginMain.Close();
            }
        }

        //https://stackoverflow.com/questions/463299/how-do-i-make-a-textbox-that-only-accepts-numbers
        //With this method, it is checked that all the information coming from the Textbox contains numbers.
        public static bool blDigitControl(string srLoginText)
        {
            //We convert the incoming string expression to char and check it one by one
            foreach (char CharOfEach in srLoginText)
            {
                if (!Char.IsDigit(CharOfEach))
                    //Return false if it doesn't have a number.
                    return false;
            }
            return true;
        }

        //The method we created for the login attempt. I got it from the tutorial video.
        private static void LoginAttemps()
        {
            //We will open the database and send the ip and time information
            using (LibraryContext context = new LibraryContext())
            {
                //To add information to the TryLogin database, we define it here
                TblTryLogin Attemp = new TblTryLogin();

                //With the ip method we created in the "Global Methods" class, we take the user's ip address and assign it to the ip field in the database.
                Attemp.TriedIp = GlobalMethods.IpAdressFinder();

                //You can get the server time information from the same class with the "DateTimeHelper" method and assign it to the database.
                Attemp.DateOfTry = GlobalMethods.DateTimeHelper.ServerTime;

                //We add the information we receive to the database.
                context.TblTryLogin.Add(Attemp);
                //We save the changes in the database.
                context.SaveChanges();

            }
        }
    }
}
