using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Library
{
    public static class AdminOperations
    {
        //I have defined the information from the AdminPanel window to the current class under the title "AdminMain"
        public static AdminPanel AdminMain;


        public static void GetBooksInformationForAdmin()
        {
            //I connect to database to get book information
            using (LibraryContext context = new LibraryContext())
            {
                //I am getting information in TblBooks database
                context.TblBooks.OrderBy(pr => pr.BookId).Take(1000).Load();
                //I'm sending the data from the database to the datagrid
                AdminMain.datagridBooksForAdmin.ItemsSource = context.TblBooks.Local.ToBindingList();
            }
        }

        public static void GetTeachersInformationForAdmin()
        {
            //I'm connecting to the database to assign the teachers that need to be approved to the datagrid.
            using (LibraryContext context = new LibraryContext())
            {
                //I am getting information in TblTeacherApprovals database
                context.TblTeacherApprovals.OrderBy(pr => pr.TeacherApprovalsId).Take(1000).Load();
                //I'm sending the data from the database to the datagrid
                AdminMain.datagridTeacherApprovals.ItemsSource = context.TblTeacherApprovals.Local.ToBindingList();
            }
        }

        public static void GetTakenInformationForAdmin()
        {
            //I connect to get taken book information the database
            using (LibraryContext context = new LibraryContext())
            {
                //I am getting information in TblTakenBooks database
                context.TblTakenBooks.OrderBy(pr => pr.TakenBooksId).Take(1000).Load();
                //I'm sending the data from the database to the datagrid
                AdminMain.datagridTakenBooksForAdmin.ItemsSource = context.TblTakenBooks.Local.ToBindingList();
            }
        }

        public static void GetReturnApprovals()
        {
            //I connect to get return approvals the database
            using (LibraryContext context = new LibraryContext())
            {
                //I am getting information in TblReturnBooks database
                context.TblReturnBooks.OrderBy(pr => pr.ReturnBooksId).Take(1000).Load();
                //I'm sending the data from the database to the datagrid
                AdminMain.datagridReturnApprovals.ItemsSource = context.TblReturnBooks.Local.ToBindingList();
            }
        }

        public static void TeacherApproval()
        {
            //I connect to approval teachers the database
            using (LibraryContext context = new LibraryContext())
            {
                //We get selected teacher information from datagrid
                var vrSelectedItem = AdminMain.datagridTeacherApprovals.SelectedItems.Cast<TblTeacherApprovals>().ToList();

                //We check if the datagrid has made a selection.
                if (AdminMain.datagridTeacherApprovals.SelectedItems.Count > 0)
                {
                    //We will convert the selected item's information into an array with the foreach loop and process it.
                    foreach (var vrTeacher in vrSelectedItem)
                    {
                        //We assign the teacher's information from the database to the var variable
                        var vrSelectedTeacher = context.TblUsers.Where(pr => pr.Email == vrTeacher.Email).FirstOrDefault();
                        var vrTeacher_DB = context.TblTeacherApprovals.Where(pr => pr.Email == vrTeacher.Email).FirstOrDefault();
                        //We make the user type of the teacher coming from the database 2.Thus, it will be passed from student to teacher.
                        vrSelectedTeacher.UserType = 2;
                        vrSelectedTeacher.SchoolNumber = vrSelectedTeacher.SchoolNumber + 2000000;

                        try
                        {
                            //I update user information in database
                            context.TblUsers.Update(vrSelectedTeacher);
                            context.TblTeacherApprovals.Local.Remove(vrTeacher_DB);
                            //I save the changes
                            context.SaveChanges();
                            //I show message that the operation was successful
                            MessageBox.Show("The selected teacher has been successfully approved");
                            //I update datagrid data after teacher approval
                            context.TblTeacherApprovals.OrderBy(pr => pr.TeacherApprovalsId).Take(1000).Load();
                            //I'm sending the data from the database to the datagrid
                            AdminMain.datagridTeacherApprovals.ItemsSource = context.TblTeacherApprovals.Local.ToBindingList();
                            //I'm refreshing datagrid
                            AdminMain.datagridTeacherApprovals.Items.Refresh();

                        }
                        catch (Exception E)
                        {
                            //If there is an error in saving, I show an error message.
                            MessageBox.Show("An error has occured while registering. Error is: \n" + E.Message.ToString() + "\n\n" + E?.InnerException?.Message);
                            return;
                        }
                    }
                }
                //If the teacher is not available I show the following error message.
                else
                {
                    MessageBox.Show("Error!You haven't chosen any teacher");
                }
            }
        }


        public static void AddThebook()
        {
            //I connect to add book the database
            using (LibraryContext context = new LibraryContext())
            {
                //I'm defining it to add information to the TblBooks database.
                TblBooks Book = new TblBooks();

                //I discard the information to be saved in the database
                Book.Name = AdminMain.txtbxBookName.Text;
                Book.Category = AdminMain.txtbxCategory.Text;
                Book.Author = AdminMain.txtbxAuthor.Text;
                //Since the information from the textbox is a string, I convert it to an int.
                Book.NumberOfPages = Int32.Parse(AdminMain.txtbxNumberOfPages.Text);
                Book.ReleaseDate = Int32.Parse(AdminMain.txtbxReleaseDate.Text);
                Book.Amount = Int32.Parse(AdminMain.txtbxAmount.Text);

                try
                {
                    //I add the entered book to the database.
                    context.TblBooks.Add(Book);
                    //I save the changes
                    context.SaveChanges();
                    //I show message that the operation was successful
                    MessageBox.Show("The entered book has been successfully saved.");
                    //I update datagrid data after adding book
                    context.TblBooks.OrderBy(pr => pr.BookId).Take(1000).Load();
                    //I'm sending the data from the database to the datagrid
                    AdminMain.datagridBooksForAdmin.ItemsSource = context.TblBooks.Local.ToBindingList();
                    //I'm refreshing datagrid
                    AdminMain.datagridBooksForAdmin.Items.Refresh();

                }
                catch (Exception E)
                {
                    //If there is an error in saving, I show an error message.
                    MessageBox.Show("An error has occured while registering. Error is: \n" + E.Message.ToString() + "\n\n" + E?.InnerException?.Message);
                    return;
                }
            }

        }

        public static void ReturnApproval()
        {
            //I connect to approval return book the database
            using (LibraryContext context = new LibraryContext())
            {
                //We get selected return book information from datagrid
                var vrSelectedReturnBook = AdminMain.datagridReturnApprovals.SelectedItems.Cast<TblReturnBooks>().ToList();

                //We check if the datagrid has made a selection.
                if (AdminMain.datagridReturnApprovals.SelectedItems.Count > 0)
                {
                    //We will convert the selected item's information into an array with the foreach loop and process it.
                    foreach (var vrReturn in vrSelectedReturnBook)
                    {
                        //We assign the return book's information from the database to the var variable
                        var vrSelectedReturn = context.TblReturnBooks.Where(pr => pr.Status != "Approval").FirstOrDefault();

                        //We check if the return of the book has been approved.
                        if (vrReturn.Status == vrSelectedReturn.Status)
                        {
                            //We approve of her book and write "Approved" in the status part.
                            vrSelectedReturn.Status = "Approval";
                            try
                            {
                                //I update book information in database
                                context.TblReturnBooks.Update(vrSelectedReturn);
                                //I save the changes
                                context.SaveChanges();
                                //I show message that the operation was successful
                                MessageBox.Show("The selected return has been successfully approved");
                                //I update the datagrid after returning the book
                                context.TblReturnBooks.OrderBy(pr => pr.ReturnBooksId).Take(1000).Load();
                                //I'm sending the data from the database to the datagrid
                                AdminMain.datagridReturnApprovals.ItemsSource = context.TblReturnBooks.Local.ToBindingList();
                                //I'm refreshing datagrid
                                AdminMain.datagridReturnApprovals.Items.Refresh();

                            }
                            catch (Exception E)
                            {
                                //If there is an error in saving, I show an error message.
                                MessageBox.Show("An error has occured while registering. Error is: \n" + E.Message.ToString() + "\n\n" + E?.InnerException?.Message);
                                return;
                            }
                        }
                        //I show the following error message if the book return has already been approved
                        else
                        {
                            MessageBox.Show("The book you have chosen has already been approved!");
                        }
                    }
                }
                //I show error message if nothing is selected in datagrid
                else
                {
                    MessageBox.Show("Error!You haven't chosen any return");
                }
            }
        }


        public static void SaveSetting()
        {
            //I connect to save setting the database
            using (LibraryContext context = new LibraryContext())
            {
                //We assign the settings in the database to var variables
                var vrMaxTakeBooks = context.TblSettings.Where(pr => pr.SettingName == "MaxTakeBooks").FirstOrDefault();
                var vrMaxTakeTime = context.TblSettings.Where(pr => pr.SettingName == "MaxTakeTime").FirstOrDefault();

                //We determine which setting is from the selected index.
                if (AdminMain.cmboxSettings.SelectedIndex == 0)
                {
                    //Since the information entered in the textbox is a string, I convert it to int.
                    vrMaxTakeBooks.Value = Int32.Parse(AdminMain.txtbxSettingValue.Text);
                }
                //I added the else if for any setting that will be added in the future, although it seems unnecessary.
                else if (AdminMain.cmboxSettings.SelectedIndex == 1)
                {
                    vrMaxTakeTime.Value = Int32.Parse(AdminMain.txtbxSettingValue.Text);
                }

                try
                {
                    //For the selected setting, we get information from the selected index and assign the value entered by the admin.
                    if (AdminMain.cmboxSettings.SelectedIndex == 0)
                    {
                        //I update the selected setting
                        context.TblSettings.Update(vrMaxTakeBooks);
                    }
                    else
                    {
                        context.TblSettings.Update(vrMaxTakeTime);
                    }
                    //I save the changes
                    context.SaveChanges();
                    //I show message that the operation was successful
                    MessageBox.Show("Settings saved successfully");
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
