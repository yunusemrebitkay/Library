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
    public static class RegisterStatus
    {
        //I have defined the information from the Register window to the current class under the title "RegisterMain"
        public static Register RegisterMain;
        //I have defined a generic validation checker for the registration.
        public static bool blControl = true;
        //I have defined a generic string message for error messages.
        public static string srDisplayMessage = "NA";

        //I created a generic method to run the required methods.
        public static void runmethods()
        {
            //The purpose of me writing the methods in reverse order is to prevent the program from giving an error by deleting the information that the user has entered before.
            //For example, the firstname information was checked and the correct result was output. The user then made a mistake in the password field and then corrected it. If the user deletes the firstanem part, the program crashes.
            PasswordControl(RegisterMain.pwbxPass.Password, RegisterMain.pwbxAgainPass.Password);
            EmailControl(RegisterMain.txtbxEmail.Text);
            NamesControl(RegisterMain.txtbxFirstname.Text, RegisterMain.txtbxLastname.Text);
        }

        public static void NamesControl(string srFirstname, string srLastname)
        {
            //I defined maximum and minimum numbers for the rule
            int irNamesMaxLength = 64;
            int irNamesMinLength = 3;

            //I added the rule that the firstname field cannot be left blank
            if (srFirstname == "")
            {
                blControl = false;
                srDisplayMessage = "Firstname field can't be left blank!";
                return;
            }
            //I added the rule that the firstname cannot be more than 64 characters, not less than 8 characters
            if (srFirstname.Length > irNamesMaxLength || srFirstname.Length < irNamesMinLength)
            {
                blControl = false;
                srDisplayMessage = "Firstname can't be more than 64 characters and less than 3 characters.";
                return;
            }
            //I added the rule that the lastname field cannot be left blank
            if (srLastname == "")
            {
                blControl = false;
                srDisplayMessage = "Lastname field can't be left blank!";
                return;
            }
            //I added the rule that the lastname cannot be more than 64 characters, not less than 8 characters
            if (srLastname.Length > irNamesMaxLength || srLastname.Length < irNamesMinLength)
            {
                blControl = false;
                srDisplayMessage = "Lastname can't be more than 64 characters and less than 3 characters.";
                return;
            }
        }

        public static void EmailControl(string srEmail)
        {
            //We send and receive information to the method we created for email control in the "GlobaMethods" class.
            bool blEmailInvation = GlobalMethods.EmailControl(srEmail);

            //I added the rule that the email field cannot be left blank
            if (srEmail == "")
            {
                blControl = false;
                srDisplayMessage = "Email field can't be left blank!";
                return;
            }
            //I got it from email check I show error message if correctness is false
            if (blEmailInvation == false)
            {
                blControl = false;
                srDisplayMessage = "Email you entered is in an invalid format. Please re-enter.";
                return;
            }

            //I'm connecting the database
            using (LibraryContext context = new LibraryContext())
            {
                //I am getting data from database that matches user mail. I have assigned information that is unique to users as an email.
                var vrUserEmail = context.TblUsers.FirstOrDefault(pr => pr.Email == srEmail);

                //If there is no such user, I show an error message.
                if (vrUserEmail != null)
                {
                    blControl = false;
                    srDisplayMessage = "The email you entered is in use. Please enter another email.";
                    return;
                }

            }

        }

        public static void PasswordControl(string srPassword, string srAgainPassword)
        {
            //I define the minimum value of the password
            int irPasswordLength = 8;
            //I created a custom character list and defined some elements in it.
            List<char> lstSpecialChars = new List<char> { '_', '-', '@', '#', '$', '%', '*', '?', ';', '&', '!' };

            //I added the rule that the password fields cannot be left blank
            if (srPassword == "" || srAgainPassword == "")
            {
                blControl = false;
                srDisplayMessage = "Password fields can't be left blank!";
                return;
            }
            //I check the length of the entered password
            if (srPassword.Length < irPasswordLength)
            {
                blControl = false;
                srDisplayMessage = "The Password you enter can't be less than 8 characters!";
                return;
            }

            //For the password, I defined the required accuracies for one character, one uppercase letter, one lowercase letter, one number, and one special character rule.
            bool blContainSpecChar = false, blContainUpperCase = false, blContainLowerCase = false, blContainDigit = false, blContainLetter = false;

            //I converted the entered password into a char array.
            foreach (char vrPerChar in srPassword.ToCharArray())
            {
                //I'm checking numbers
                if (char.IsDigit(vrPerChar))
                {
                    blContainDigit = true;
                }

                //I'm checking letter
                if (char.IsLetter(vrPerChar))
                {
                    blContainLetter = true;
                }

                //I'm checking uppercase letter
                if (char.IsUpper(vrPerChar))
                {
                    blContainUpperCase = true;
                }

                //I'm checking lowercase letter
                if (char.IsLower(vrPerChar))
                {
                    blContainLowerCase = true;
                }

                //I'm checking special character
                if (lstSpecialChars.Contains(vrPerChar))
                {
                    blContainSpecChar = true;
                }
            }

            //I show error message if it does not contain special characters.
            if (!blContainSpecChar)
            {
                blControl = false;
                srDisplayMessage = "Your password must contain at least a special character.";
                return;
            }

            //I show error message if it does not contain number.
            if (!blContainDigit)
            {
                blControl = false;
                srDisplayMessage = "Your password must contain at least a number!";
                return;
            }

            //I show error message if it does not contain letter.
            if (!blContainLetter)
            {
                blControl = false;
                srDisplayMessage = "Your password must contain at least a letter!";
                return;
            }

            //I show error message if it does not contain uppercase letter.
            if (!blContainUpperCase)
            {
                blControl = false;
                srDisplayMessage = "Your password must contain at least an upper case character!";
                return;
            }

            //I show error message if it does not contain lowercase letter.
            if (!blContainLowerCase)
            {
                blControl = false;
                srDisplayMessage = "Your password must contain at least an lower case character!";
                return;
            }

            //I check if the two entered passwords are the same.
            if (srPassword != srAgainPassword)
            {
                blControl = false;
                srDisplayMessage = "The passwords you entered don't match! Please re-enter.";
                return;
            }
        }

    }
}
