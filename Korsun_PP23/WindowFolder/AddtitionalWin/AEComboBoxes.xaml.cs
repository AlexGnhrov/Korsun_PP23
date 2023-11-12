using Korsun_PP23.ClassFolder;
using Korsun_PP23.DataFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Korsun_PP23.WindowFolder.AddtitionalWin
{
    /// <summary>
    /// Логика взаимодействия для AEComboBoxes.xaml
    /// </summary>
    public partial class AEComboBoxes : Window
    {

        ComboBox comboBox;



        object EditItem;

        bool isEdit = false;

        string BtnText = "";
        string ItemText = "";
        string RemoveText = "";

        public AEComboBoxes(ComboBox comboBox, object EditItem)
        {
            Owner = GlobalVariablesClass.mainMenu;

            this.EditItem = EditItem;
            this.comboBox = comboBox;

            InitializeComponent();

            isEdit = !(EditItem is null);



            SetDataWindow();

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && AddEditBtn.IsEnabled)
            {
                AddEditBtn_Click(sender, e);
            }
        }

        private void TB_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (isEdit)
            {
                if (string.IsNullOrWhiteSpace(AddEditItemTB.Text))
                {
                    AddEditBtn.Tag = "DeleteBtn";
                    AddEditBtn.Content = "Удалить";
                }
                else
                {
                    AddEditBtn.Tag = "";
                    AddEditBtn.Content = BtnText;
                }

                AddEditBtn.IsEnabled = true;
            }
            else
            {
                AddEditBtn.IsEnabled = !string.IsNullOrWhiteSpace(AddEditItemTB.Text);
            }
        }

        private void AddEditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AddEditBtn.Tag.ToString() != "")
            {
                RemoveItem();;
            }
            else
            {
                AddEditItem();
            }
        }


        private void SetDataWindow()
        {
            switch (comboBox.Name)
            {
                case "OrganizationCB":
                    {
                        if (EditItem == null)
                        {
                            WinLB.Content = Title = "Добавление организации";
                            AddEditBtn.Content = "Добавить организации";
                        }
                        else
                        {
                            AddEditItemTB.Text = ItemText = comboBox.Text;

                            WinLB.Content = Title = "Редактирование организации";
                            AddEditBtn.Content = "Редактировать организации";

                            RemoveText = "\nорганизацию - " + AddEditItemTB.Text;
                        }
                    }
                    break;
                case "RegionCB":
                    {
                        if (EditItem == null)
                        {
                            WinLB.Content = Title = "Добавление региона";
                            AddEditBtn.Content = "Добавить регион";
                        }
                        else
                        {
                            AddEditItemTB.Text = ItemText = comboBox.Text;

                            WinLB.Content = Title = "Редактирование региона";
                            AddEditBtn.Content = "Редактировать регион";

                            RemoveText = "\nрегион - " + AddEditItemTB.Text;
                        }
                    }
                    break;
                case "CityCB":
                    {
                        if (EditItem == null)
                        {
                            WinLB.Content = Title = "Добавление города";
                            AddEditBtn.Content = "Добавить город";
                        }
                        else
                        {
                            AddEditItemTB.Text = ItemText = comboBox.Text;

                            WinLB.Content = Title = "Редактирование города";
                            AddEditBtn.Content = "Редактировать город";

                            RemoveText = "\nгород - " + AddEditItemTB.Text;
                        }
                    }
                    break;
                case "StreetCB":
                    {
                        if (EditItem == null)
                        {
                            WinLB.Content = Title = "Добавление улицы";
                            AddEditBtn.Content = "Добавить улицу";
                        }
                        else
                        {
                            AddEditItemTB.Text = ItemText = comboBox.Text;

                            WinLB.Content = Title = "Редактирование улицы";
                            AddEditBtn.Content = "Редактировать улицу";

                            RemoveText = "\nулицу - " + AddEditItemTB.Text;
                        }
                    }
                    break;
            }


            AddEditBtn.IsEnabled = false;
            BtnText = (string)AddEditBtn.Content;
        }


        private void AddEditItem()
        {
            try
            {
                switch (comboBox.Name)
                {
                    case "OrganizationCB":
                        {
                            Organization organization = EditItem as Organization;


                            var checkRegion = DBEntities.GetContext().Organization.FirstOrDefault(u => u.OrganizationName == AddEditItemTB.Text);

                            if (checkRegion != null && ItemText != AddEditItemTB.Text)
                            {
                                MBClass.ErrorMB("Данная организация уже существует");
                                AddEditItemTB.Focus();
                                return;
                            }

                            if (!isEdit)
                            {
                                organization = new Organization();
                            }

                            organization.OrganizationName = AddEditItemTB.Text.Trim();

                            if (!isEdit)
                            {
                                DBEntities.GetContext().Organization.Add(organization);
                            }

                            DBEntities.GetContext().SaveChanges();




                            if (isEdit)
                                MBClass.InfoMB("Организация успешно отредактирована");
                            else
                            {
                                MBClass.InfoMB("Организация успешно добавлена");

                                comboBox.ItemsSource = DBEntities.GetContext().Organization.ToList();
                                comboBox.SelectedValue = organization.IdOrganization;

                            }


                            Close();

                        }
                        break;
                    case "RegionCB":
                        {
                            Region region = EditItem as Region;


                            var checkOrg = DBEntities.GetContext().Address.FirstOrDefault(u => u.Region.RegionName == AddEditItemTB.Text);

                            if (checkOrg != null && ItemText != AddEditItemTB.Text)
                            {
                                MBClass.ErrorMB("Данный регион уже существует");
                                AddEditItemTB.Focus();
                                return;
                            }

                            if (!isEdit)
                            {
                                region = new Region();
                            }

                            region.RegionName = AddEditItemTB.Text.Trim();

                            if (!isEdit)
                            {
                                DBEntities.GetContext().Region.Add(region);
                            }

                            DBEntities.GetContext().SaveChanges();




                            if (isEdit)
                                MBClass.InfoMB("Регион успешно отредактирован");
                            else
                            {
                                MBClass.InfoMB("Регион успешно добавлен");

                                comboBox.ItemsSource = DBEntities.GetContext().Region.ToList();
                                comboBox.SelectedValue = region.IdRegion;

                            }


                            Close();

                        }
                        break;
                    case "CityCB":
                        {
                            City city = EditItem as City;


                            var checkOrg = DBEntities.GetContext().Address.FirstOrDefault(u => u.City.CityName == AddEditItemTB.Text);

                            if (checkOrg != null && ItemText != AddEditItemTB.Text)
                            {
                                MBClass.ErrorMB("Данный город уже существует");
                                AddEditItemTB.Focus();
                                return;
                            }

                            if (!isEdit)
                            {
                                city = new City();
                            }

                            city.CityName = AddEditItemTB.Text.Trim();

                            if (!isEdit)
                            {
                                DBEntities.GetContext().City.Add(city);
                            }

                            DBEntities.GetContext().SaveChanges();




                            if (isEdit)
                                MBClass.InfoMB("Город успешно отредактирован");
                            else
                            {
                                MBClass.InfoMB("Город успешно добавлен");

                                comboBox.ItemsSource = DBEntities.GetContext().City.ToList();
                                comboBox.SelectedValue = city.IdCity;

                            }


                            Close();

                        }
                        break;
                    case "StreetCB":
                        {
                            Street street = EditItem as Street;


                            var checkOrg = DBEntities.GetContext().Address.FirstOrDefault(u => u.Street.StreetName == AddEditItemTB.Text);

                            if (checkOrg != null && ItemText != AddEditItemTB.Text)
                            {
                                MBClass.ErrorMB("Данная улица уже существует");
                                AddEditItemTB.Focus();
                                return;
                            }

                            if (!isEdit)
                            {
                                street = new Street();
                            }

                            street.StreetName = AddEditItemTB.Text.Trim();

                            if (!isEdit)
                            {
                                DBEntities.GetContext().Street.Add(street);
                            }

                            DBEntities.GetContext().SaveChanges();




                            if (isEdit)
                                MBClass.InfoMB("Улица успешно отредактирована");
                            else
                            {
                                MBClass.InfoMB("Улица успешно добавлена");

                                comboBox.ItemsSource = DBEntities.GetContext().Street.ToList();
                                comboBox.SelectedValue = street.IdStreet;

                            }


                            Close();

                        }
                        break;
                }

            }
            catch (Exception ex)
            {

                MBClass.ErrorMB(ex);
                DBEntities.NullContext();
            }

        }

        private void RemoveItem()
        {
            try
            {
                if (MBClass.QuestionMB("Вы действительно хотите удалить" + RemoveText))
                {
                    switch (comboBox.Name)
                    {
                        case "OrganizationCB":
                            {
                                var CheckOrg = DBEntities.GetContext().Contract.FirstOrDefault(u => u.IdOrganization == (int)comboBox.SelectedValue);

                                if (CheckOrg != null)
                                {
                                    MBClass.ErrorMB("Данная организация используется в контрактах");
                                    AddEditItemTB.Focus();
                                    return;
                                }

                                DBEntities.GetContext().Organization.Remove(comboBox.SelectedItem as Organization);
                                DBEntities.GetContext().SaveChanges();


                                comboBox.SelectedValue = null;

                                Close();
                            }
                            break;

                        case "RegionCB":
                            {
                                var CheckReg = DBEntities.GetContext().Address.FirstOrDefault(u => u.IdRegion == (int)comboBox.SelectedValue);

                                if (CheckReg != null)
                                {
                                    MBClass.ErrorMB("Регион используется в адресе");
                                    AddEditItemTB.Focus();
                                    return;
                                }

                                DBEntities.GetContext().Region.Remove(comboBox.SelectedItem as Region);
                                DBEntities.GetContext().SaveChanges();


                                comboBox.SelectedValue = null;

                                Close();
                            }
                            break;
                        case "CityCB":
                            {
                                var CheckReg = DBEntities.GetContext().Address.FirstOrDefault(u => u.ICity == (int)comboBox.SelectedValue);

                                if (CheckReg != null)
                                {
                                    MBClass.ErrorMB("Город используется в адресе");
                                    AddEditItemTB.Focus();
                                    return;
                                }

                                DBEntities.GetContext().City.Remove(comboBox.SelectedItem as City);
                                DBEntities.GetContext().SaveChanges();


                                comboBox.SelectedValue = null;

                                Close();
                            }
                            break;
                        case "StreetCB":
                            {
                                var CheckReg = DBEntities.GetContext().Address.FirstOrDefault(u => u.IdStreet == (int)comboBox.SelectedValue);

                                if (CheckReg != null)
                                {
                                    MBClass.ErrorMB("Улица используется в адресе");
                                    AddEditItemTB.Focus();
                                    return;
                                }

                                DBEntities.GetContext().Street.Remove(comboBox.SelectedItem as Street);
                                DBEntities.GetContext().SaveChanges();


                                comboBox.SelectedValue = null;

                                Close();
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

                MBClass.ErrorMB(ex);
                DBEntities.NullContext();
            }

        }



        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
