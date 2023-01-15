using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Library
{
    public static class Registering
    {
        //I have defined the information from the Register window to the current class under the title "RegisterMain"
        public static Register RegisterMain;
        public static void RegisteringUser()
        {
            //I am checking all the information entered from the registration window by running the Registering class. If it is wrong, I show it with the error message.
            RegisterStatus.runmethods();
            if (RegisterStatus.blControl == false)
            {
                MessageBox.Show(RegisterStatus.srDisplayMessage);
                return;
            }

            //If there is no error, I connect to the database to save the information.
            using (LibraryContext context = new LibraryContext())
            {
                //I have defined the databases that I will modify.
                TblUsers Users = new TblUsers();
                TblTeacherApprovals Teacher = new TblTeacherApprovals();

                //I put the entered information in the necessary places in the User database.
                Users.Firstname = RegisterMain.txtbxFirstname.Text;
                Users.Lastname = RegisterMain.txtbxLastname.Text;
                Users.Email = RegisterMain.txtbxEmail.Text;

                //I use the Guid method to assign a unique string with String+sha256 encryption and use it later. Thus, we can compare with the same string while logging in.
                Guid guid = Guid.NewGuid();
                Users.SaltOfPassword = guid.ToString();

                //We encrypt the password with the sha256 encryption method in the "GlobalMethods" class, together with a unique string expression, and send it to the database.
                Users.Password = GlobalMethods.Encryption(RegisterMain.pwbxPass.Password.ToString(), Users.SaltOfPassword);
                //The reason for assigning Usertype 1 is that teacher approvals are made by the admin. Usertype is automatically assigned as teacher after admin is approved.
                Users.UserType = 1;
                //I created a creative method for the school trick. I get a unique school number by running this method.
                int irSchoolNumber = GlobalMethods.SchoolNumberGenerator();
                Users.SchoolNumber = irSchoolNumber;

                //If the selected user type is a teacher, I add it to the TeacherApprovals database for admin approval.
                if (RegisterMain.cbmbxUserType.SelectedIndex == 1)
                {
                    Teacher.Firstname = RegisterMain.txtbxFirstname.Text;
                    Teacher.Lastname = RegisterMain.txtbxLastname.Text;
                    Teacher.Email = RegisterMain.txtbxEmail.Text;
                    Teacher.DateOfRegistration = DateTime.Now;
                }

                //With try-catch, I prevent it from crashing in case of an error in the changes made to the database.
                try
                {
                    //I add the information to the database and save the changes.
                    context.TblUsers.Add(Users);
                    if (RegisterMain.cbmbxUserType.SelectedIndex == 1)
                    {
                        context.TblTeacherApprovals.Add(Teacher);
                    }
                    context.SaveChanges();
                }
                catch (Exception E)
                {
                    //If there is an error, I show the error with the messagebox. I got this line from our lesson video
                    MessageBox.Show("An error has occured while registering. Error is: \n" + E.Message.ToString() + "\n\n" + E?.InnerException?.Message);
                    return;
                }

                //For the method that should work in the input part, I assign a value to the general datetime again.
                Logging.dtTime = DateTime.Now;

                //I am showing the registration successful message.
                MessageBox.Show("You have successfully registered. You are logging in automatically now.");

                //After successful registration, I open the window of the automatic library
                LibraryWindow LB = new LibraryWindow();
                //I am sending the data of the registered user.
                Logging.LoggedUser = RegisterMain.txtbxEmail.Text;
                LB.lblUsername.Content = RegisterMain.txtbxFirstname.Text.ToUpper() + " " + RegisterMain.txtbxLastname.Text.ToUpper();
                LB.lblNumber.Content = irSchoolNumber;
                LB.Show();
                //I'm closing the current window.
                RegisterMain.Close();
            }

        }
    }
}

