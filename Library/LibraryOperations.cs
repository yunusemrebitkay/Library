using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Library
{
    public static class LibraryOperations
    {

        //I created a method for fast search.I set this method to run automatically on instant text change in the textbox.
        public static void FastSearch(LibraryWindow LibraryMain)
        {
            //We check if fast search is selected.
            if (LibraryMain.cmbxForSearchCategory.SelectedIndex == 3)
            {
                //I connect to search the database
                using (LibraryContext context = new LibraryContext())
                {
                    //I'm assigning the information in the textbox to a string variable.
                    string srFastSearch = LibraryMain.txtbxNameSearch.Text;
                    //I search the database with the Where method with the information I get in the textbox.
                    context.TblBooks.Where(pr => pr.Name.Contains(srFastSearch)).OrderByDescending(pr => pr.BookId).Take(1000).Load();
                    //I discard the data in datagrid and reverse the id part
                    LibraryMain.dataGridBooks.ItemsSource = context.TblBooks.Local.ToBindingList().Reverse();
                    //I'm refreshing datagrid
                    LibraryMain.dataGridBooks.Items.Refresh();
                }
            }

        }

        //I created a separate method for searching by category
        public static void Search(LibraryWindow LibraryMain)
        {
            //I'm assigning the information in the textbox to a string variable.
            string srSearch = LibraryMain.txtbxNameSearch.Text;
            //I connect to search the database
            using (LibraryContext context = new LibraryContext())
            {
                //If nothing is written in the textbox, I get the current book list.
                if (srSearch == "")
                {
                    //I am getting information from tblBooks
                    context.TblBooks.OrderBy(pr => pr.Name).OrderByDescending(pr => pr.BookId).Take(1000).Load();
                    //I discard the data in datagrid and reverse the id part
                    LibraryMain.dataGridBooks.ItemsSource = context.TblBooks.Local.ToBindingList().Reverse();
                    //I'm refreshing datagrid
                    LibraryMain.dataGridBooks.Items.Refresh();
                    return;
                }

                //I am checking for more than one condition with the switch method.
                switch (LibraryMain.cmbxForSearchCategory.SelectedIndex)
                {
                    //If selected index = 0 I'm searching by name.
                    case 0:
                        context.TblBooks.Where(pr => pr.Name.Contains(srSearch)).OrderByDescending(pr => pr.Name).Take(1000).Load();
                        break;

                    //If selected index = 1 I'm searching by category.
                    case 1:
                        context.TblBooks.Where(pr => pr.Category.Contains(srSearch)).OrderByDescending(pr => pr.Category).Take(1000).Load();
                        break;

                    //If selected index = 2 I'm searching by author.
                    case 2:
                        context.TblBooks.Where(pr => pr.Author.Contains(srSearch)).OrderByDescending(pr => pr.Author).Take(1000).Load();
                        break;
                    default:
                        break;
                }
                //I'm sending the data from the database to the datagrid
                LibraryMain.dataGridBooks.ItemsSource = context.TblBooks.Local.ToBindingList();
                //I'm refreshing datagrid
                LibraryMain.dataGridBooks.Items.Refresh();

            }
        }

        //I have created a method to filter out books that are not checked when users are checked.
        public static void FilterForAmount(LibraryWindow LibraryMain)
        {
            //I connect to filter the database
            using (LibraryContext context = new LibraryContext())
            {
                //If checked, we run the commands below.
                if (LibraryMain.checkbxFilter.IsChecked == true)
                {
                    //We filter out those with 0 books.
                    context.TblBooks.Where(pr => !pr.Amount.ToString().Contains("0")).OrderByDescending(pr => pr.BookId).Take(1000).Load();
                    //I discard the data in datagrid and reverse
                    LibraryMain.dataGridBooks.ItemsSource = context.TblBooks.Local.ToBindingList().Reverse();
                }
                //If it is not checked, we run the commands below.
                else
                {
                    //Normally we get the information of all the books.
                    context.TblBooks.OrderBy(pr => pr.BookId).Take(1000).Load();
                    //I'm sending the data from the database to the datagrid
                    LibraryMain.dataGridBooks.ItemsSource = context.TblBooks.Local.ToBindingList();
                }
                //I'm refreshing datagrid
                LibraryMain.dataGridBooks.Items.Refresh();
            }
        }

        public static void ListiningTheBooks(LibraryWindow LibraryMain)
        {
            //I connect to list the database
            using (LibraryContext context = new LibraryContext())
            {
                //I am checking for more than one condition with the switch method.
                switch (LibraryMain.cmbxForListing.SelectedIndex)
                {
                    //If selected index = 0 I'm listing by name.
                    case 0:
                        context.TblBooks.OrderBy(pr => pr.Name).OrderByDescending(pr => pr.Name).Take(1000).Reverse().Load();
                        break;
                    //If selected index = 1 I'm listing by category.
                    case 1:
                        context.TblBooks.OrderBy(pr => pr.Category).OrderByDescending(pr => pr.Category).Take(1000).Reverse().Load();
                        break;
                    //If selected index = 2 I'm listing by author.
                    case 2:
                        context.TblBooks.OrderBy(pr => pr.Author).OrderByDescending(pr => pr.Author).Take(1000).Reverse().Load();
                        break;
                    default:
                        break;
                }
                //I'm sending the data from the database to the datagrid
                LibraryMain.dataGridBooks.ItemsSource = context.TblBooks.Local.ToBindingList();
                //I'm refreshing datagrid
                LibraryMain.dataGridBooks.Items.Refresh();
            }
        }

        public static void TakenTheBook(LibraryWindow LibraryMain)
        {
            //I connect to take book the database
            using (LibraryContext context = new LibraryContext())
            {
                //I'm defining it to add information to the TakenBooks database.
                TblTakenBooks TakenBooks = new TblTakenBooks();
                //I define it to make changes to the TakenBooks database.
                TblBooks Books = new TblBooks();

                //We take the user from the login information from the database and assign it to a var variable
                var vrLoggedUser = context.TblUsers.FirstOrDefault(pr => pr.Email == Logging.LoggedUser);
                //We take the maximum number of days and number of books from the Settings database and assign them to var variables.
                var vrMaxTakeTime = context.TblSettings.Where(pr => pr.SettingName == "MaxTakeTime").FirstOrDefault();
                var vrMaxTakeBooks = context.TblSettings.Where(pr => pr.SettingName == "MaxTakeBooks").FirstOrDefault();
                //We define the selected item in the datagrid variable var
                var vrSelectedItem = LibraryMain.dataGridBooks.SelectedItems.Cast<TblBooks>().ToList();
                //We assign the number of books that the user has bought before to the var variable.
                var vrUserTakenBooks = context.TblTakenBooks.Where(pr => pr.Email == Logging.LoggedUser).ToList();

                //We check if the datagrid has made a selection.
                if (LibraryMain.dataGridBooks.SelectedItems.Count > 0)
                {
                    //We will convert the selected item's information into an array with the foreach loop and process it.
                    foreach (var vrBooks in vrSelectedItem)
                    {
                        //We assign the information in the database of the selected book to the var variable
                        var vrSelectedBook = context.TblBooks.FirstOrDefault(pr => pr.Name == vrBooks.Name);

                        //If the selected book quantity is different from zero
                        if (vrBooks.Amount != 0)
                        {
                            //If the user is 1, that is, a student, we take different actions. The student has a certain number of books and time to submit
                            if (vrLoggedUser.UserType == 1)
                            {
                                //If the number of books bought by the student is more than the specified number of books, I show an error message
                                if (vrUserTakenBooks.Count >= vrMaxTakeBooks.Value)
                                {
                                    MessageBox.Show("Error! Students can't take more than " + vrMaxTakeBooks.Value + " books!");
                                    return;
                                }
                                //I add the time the student has to submit to the database.
                                TakenBooks.ReturnDate = DateTime.Now.AddDays(vrMaxTakeTime.Value);
                            }
                            else
                            {
                                //Since he is a teacher, there is no time limit, but I appointed 365 days as I see fit.
                                TakenBooks.ReturnDate = DateTime.Now.AddDays(365);
                            }
                            //I'm throwing the information of the book imported into the database together with the user.
                            TakenBooks.Email = vrLoggedUser.Email;
                            TakenBooks.SchoolNumber = vrLoggedUser.SchoolNumber;
                            TakenBooks.BookName = vrBooks.Name;
                            TakenBooks.TakenDate = DateTime.Now;
                            //I reduce the number of books in the database by one
                            vrSelectedBook.Amount = vrSelectedBook.Amount - 1;
                        }
                        //If the book is not available I show the following error message.
                        else
                        {
                            MessageBox.Show("The selected book is not currently available.");
                            return;
                        }

                        try
                        {
                            //I update book information in database
                            context.TblBooks.Update(vrSelectedBook);
                            //I add the book information imported into the database
                            context.TblTakenBooks.Add(TakenBooks);
                            //I save the changes
                            context.SaveChanges();
                            //I show message that the operation was successful
                            MessageBox.Show("The selected book was successfully imported.");
                            //I am updating datagrid data due to lack of books
                            context.TblBooks.OrderBy(pr => pr.BookId).Take(1000).Load();
                            //I'm sending the data from the database to the datagrid
                            LibraryMain.dataGridBooks.ItemsSource = context.TblBooks.Local.ToBindingList();
                            //I'm refreshing datagrid
                            LibraryMain.dataGridBooks.Items.Refresh();
                        }
                        catch (Exception E)
                        {
                            //If there is an error in saving, I show an error message.
                            MessageBox.Show("An error has occured while registering. Error is: \n" + E.Message.ToString() + "\n\n" + E?.InnerException?.Message);
                            return;
                        }
                    }
                }
                //I show error message if nothing is selected in datagrid
                else
                {
                    MessageBox.Show("Error!You haven't chosen any books");
                }
            }
        }

        public static void ReturnBook(TheBooksOnMe TheMain)
        {
            //I connect to return book the database
            using (LibraryContext context = new LibraryContext())
            {
                //I define it to make changes to the ReturnBooks database.
                TblReturnBooks ReturnBooks = new TblReturnBooks();

                //I take the user's information from the database and assign it to the var variable.
                var vrLoggedUser = context.TblUsers.FirstOrDefault(pr => pr.Email == Logging.LoggedUser);
                //datagrid'den seçili öğrenin verilerini alıp var değişkenine atıyorum
                var selectedItems = TheMain.datagridBooksOnMe.SelectedItems.Cast<TblTakenBooks>().ToList();

                //I am checking whether at least one item is selected in the datagrid.
                if (TheMain.datagridBooksOnMe.SelectedItems.Count > 0)
                {
                    //We will convert the selected item's information into an array with the foreach loop and process it.
                    foreach (var vrBooks in selectedItems)
                    {
                        //In datagrid, I take the information of the selected book from the database and change it to var.
                        var selectedbooks = context.TblBooks.FirstOrDefault(pr => pr.Name == vrBooks.BookName);

                        //I add the returned book information to the database
                        ReturnBooks.Email = vrLoggedUser.Email;
                        ReturnBooks.BookName = vrBooks.BookName;
                        ReturnBooks.ReturnDate = DateTime.Now;
                        //I increase the number of the returned book by 1 and save it in the TblBooks table.
                        selectedbooks.Amount = selectedbooks.Amount + 1;
                        //I show the current status with the message Waiting for confirmation.
                        ReturnBooks.Status = "Waiting for approval";
                    }
                    try
                    {
                        //I add the returned book to the tblReturnBooks database
                        context.TblReturnBooks.Add(ReturnBooks);
                        //I'm deleting the user who received the returned book from the book database
                        context.TblTakenBooks.Local.Remove(context.TblTakenBooks.Where(pr => pr.Email == Logging.LoggedUser).FirstOrDefault());
                        //I save the changes
                        context.SaveChanges();
                        //I show message that the operation was successful
                        MessageBox.Show("The book you selected has been successfully returned after admin approval.");
                        //I'm updating datagrid for taken books
                        context.TblTakenBooks.Where(pr => pr.Email == Logging.LoggedUser).OrderBy(pr => pr.TakenBooksId).Take(1000).Load();
                        //I'm sending the data from the database to the datagrid
                        TheMain.datagridBooksOnMe.ItemsSource = context.TblTakenBooks.Local.ToBindingList();
                        //I'm refreshing datagrid
                        TheMain.datagridBooksOnMe.Items.Refresh();

                    }
                    catch (Exception E)
                    {
                        //If there is an error in saving, I show an error message.
                        MessageBox.Show("An error has occured while registering. Error is: \n" + E.Message.ToString() + "\n\n" + E?.InnerException?.Message);
                        return;
                    }
                }
                //I show error message if nothing is selected in datagrid
                else
                {
                    MessageBox.Show("Error!You haven't chosen any books");
                }

            }

        }

    }


}
