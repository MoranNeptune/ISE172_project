using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MileStoneClient.Logger;

namespace MileStoneClient.PresentationLayer
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Page
    {
        private ObservableObject obs;
        private ChatRoomWindow chatRoom;
        private string filterChoice, sortChoice, orderChoice;
        private string tFilterChoice, tSortChoice, tOrderChoice;
        private string prevFilter, prevSort, prevOrder;
        private bool isChanged, isLegalData;
        private List<string> users, groups;
        private string userChoice, groupChoice;
        private bool ok;

        public Options(ChatRoomWindow cr, List<string> groups, ObservableObject obs)
        {
            chatRoom = cr;
            this.obs = obs;
            DataContext = obs;
            InitializeComponent();
            isChanged = false;
            isLegalData = true;
            users = new List<string>();
            this.groups = groups;
            ok = false;

            //initialize with ascending sort, none filter and sort by timestamp
            obs.AscendingIsChecked = true;
            obs.F_NoneIsChecked = true;
            obs.S_TimeIsChecked = true;

            //initiate final option choice
            filterChoice = "none";
            sortChoice = "time";
            orderChoice = "ascending";

            //initiate option change
            tFilterChoice = filterChoice;
            tSortChoice = sortChoice;
            tOrderChoice = orderChoice;

            prevFilter = filterChoice;
            prevSort = sortChoice;
            prevOrder = orderChoice;
        }

        /// <summary>
        /// sort by nickname
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortNickname(object sender, RoutedEventArgs e)
        {
            obs.S_NicknameIsChecked = true;
            prevSort = tSortChoice;
            tSortChoice = "name";
        }

        /// <summary>
        /// sort by group id, nickname and timestemp 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortAll(object sender, RoutedEventArgs e)
        {
            obs.S_AllIsChecked = true;
            prevSort = tSortChoice;
            tSortChoice = "all";
        }

        /// <summary>
        /// sort by timestemp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortTime(object sender, RoutedEventArgs e)
        {
            obs.S_TimeIsChecked = true;
            prevSort = tSortChoice;
            tSortChoice = "time";
        }

        /// <summary>
        /// sort messages by ascending order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortAscending(object sender, RoutedEventArgs e)
        {
            obs.AscendingIsChecked = true;
            prevOrder = tOrderChoice;
            tOrderChoice = "ascending";
        }

        /// <summary>
        /// sort messages by decending order 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortDescending(object sender, RoutedEventArgs e)
        {
            obs.DescendingIsChecked = true;
            prevOrder = tOrderChoice;
            tOrderChoice = "decending";
        }

        /// <summary>
        /// no filter - if "None" filter is checked, hide the comboBox for users and groups
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterNone(object sender, RoutedEventArgs e)
        {
            obs.F_NoneIsChecked = true;
            prevFilter = tFilterChoice;
            tFilterChoice = "none";
            obs.IsUserFiltered = "Hidden";
            obs.IsGroupFiltered = "Hidden";
            IsLegalData = true;
        }

        /// <summary>
        /// filter by group id - if "Group Id" is checked, only display comboBox for groups
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterG_Id(object sender, RoutedEventArgs e)
        {
            if (obs.F_GroupIsChecked)
                groupChoice = null;
            if (obs.F_UserIsChecked)
                userChoice = null;

            obs.F_GroupIsChecked = true;
            prevFilter = tFilterChoice;
            tFilterChoice = "group";
            obs.IsUserFiltered = "Hidden";
            obs.IsGroupFiltered = "visible";
            obs.SelectedGroup = 0;
        }

        /// <summary>
        /// filter by user name - if "User" filter is checked, show comboBox for groups
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterUser(object sender, RoutedEventArgs e)
        {
            if (obs.F_GroupIsChecked)
                groupChoice = null;
            if (obs.F_UserIsChecked)
                userChoice = null;

            obs.F_UserIsChecked = true;
            prevFilter = tFilterChoice;
            tFilterChoice = "user";
            obs.IsGroupFiltered = "visible";
            obs.SelectedGroup = 0;
        }

        /// <summary>
        /// set bool values for isChanged (indicating the change of a field) 
        /// and isLegalData (indicating if all data sent is legal) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk(object sender, RoutedEventArgs e)
        {
            //if the order/sort/filter choice was changed
            if (!tOrderChoice.Equals(prevOrder) | !tFilterChoice.Equals(prevFilter) | !tSortChoice.Equals(prevSort))
            {
                isChanged = true;
            }

            //if no filters is clicked - there is a filter
            if (!obs.F_NoneIsChecked)
            {
                //if User filter was clicked and user wasn't picked  
                if (obs.F_UserIsChecked && ((userChoice == null | groupChoice == null) || 
                     (groupChoice.Equals("Groups") | userChoice.Equals("Empty group :("))))
                {
                    MessageBox.Show("Choose user for  filter");
                    isLegalData = false;
                }
                //if group wasn't picked or if picked group is empty
                if (obs.F_GroupIsChecked && (groupChoice == null || groupChoice.Equals("Groups")))
                {
                    MessageBox.Show("Choose group for filter");
                    isLegalData = false;
                }
                else
                {
                    //change sort, order and filter
                    orderChoice = tOrderChoice;
                    sortChoice = tSortChoice;

                    //if filter is by group and a group was picked, or if the filter is by user and both the group and the user were picked
                    if ((obs.F_GroupIsChecked && groupChoice != null) | (obs.F_UserIsChecked && groupChoice != null && UserChoice != null))
                        filterChoice = tFilterChoice;
                }
            }
            //no filter is picked
            else
            {
                orderChoice = tOrderChoice;
                filterChoice = tFilterChoice;
                sortChoice = tSortChoice;
            }

            if (isLegalData)
            {
                obs.IsOptionVisible = null;
                ok = true;
            }
        }

        /// <summary>
        /// initiate list for groups comboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addGroups(object sender, RoutedEventArgs e)
        {
            var groupBox = sender as ComboBox;
            groupBox.ItemsSource = groups;
            groupBox.SelectedIndex = obs.SelectedGroup;
        }

        /// <summary>
        /// display list for groups comboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupList(object sender, SelectionChangedEventArgs e)
        {
            var groupList = sender as ComboBox;

            //if group was picked
            if (groupList.SelectedIndex != 0)
            {
                obs.SelectedGroup = groupList.SelectedIndex;
                string group = groupList.SelectedItem as string;
                groupChoice = group;

                //if Group filter was picked and group was picked
                if (obs.F_GroupIsChecked)
                    isLegalData = true;
                else
                {
                    obs.UserList.Clear();
                    users = chatRoom.getMembersOf(groupChoice);

                    // if there are no members in the group
                    if (users.Count == 0)
                    {
                        obs.UserList.Add("Empty group :(");
                        IsLegalData = false;
                    }
                    else
                        // adds the groups members to the combobox
                        for (int i = 0; i < users.Count; i++)
                            obs.UserList.Add(users[i]);
                    obs.IsUserFiltered = "visible";
                }
            }            
        }

        /// <summary>
        /// initiate list for users comboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addUsers(object sender, RoutedEventArgs e)
        {
            var userBox = sender as ComboBox;
            userBox.ItemsSource = obs.UserList;
            userBox.SelectedIndex = obs.SelectedUser;
        }

        /// <summary>
        /// display list for users comboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userList(object sender, SelectionChangedEventArgs e)
        {
            var userList = sender as ComboBox;
            string name = userList.SelectedItem as string;

            if (userList.SelectedIndex != 0)
                obs.SelectedUser = userList.SelectedIndex;

            if(name!=null && !name.Equals("Empty group :("))
            {
                userChoice = name;
                isLegalData = true;
            }
        }

        public bool IsLegalData
        {
            get { return isLegalData; }
            set { isLegalData = value; }
        }

        public bool Ok
        {
            get { return ok; }
            set { ok = value; }
        }

        public bool IsChanged
        {
            get { return isChanged; }
            set { isChanged = value; }
        }

        public string FilterChoice
        {
            get { return filterChoice; }
        }

        public string SortChoice
        {
            get { return sortChoice; }
        }

        public string OrderChoice
        {
            get { return orderChoice; }
        }

        public string UserChoice
        {
            get { return userChoice; }
        }

        public string GroupChoice
        {
            get { return groupChoice; }
        }
    }
}
